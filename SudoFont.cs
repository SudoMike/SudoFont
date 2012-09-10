
using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;


namespace SudoFont
{
	// You can use this to load a binary font file at runtime.
	// This serves as a specification for the binary file.
	public class RuntimeFont
	{
		// If keepConfigBlock is true, we'll save the whole block in a MemoryStream
		// that you can access with SudoFont.ConfigurationBlockStream.
		public bool Load( BinaryReader file, bool keepConfigBlock=false )
		{
			try
			{
				// Make sure it's the right version.
				string fileHeader = file.ReadString();

				// Check the version number.
				bool version1_0 = false;
				if ( fileHeader == "SudoFont1.0" )
					version1_0 = true;
				else if ( fileHeader != FontFileHeader )
					return false;

				bool didReadCharacters = false;
				bool didReadKerning = false;
				bool didReadFontInfo = false;

				while ( true )
				{
					short sectionID = file.ReadInt16();

					// Finished?
					if ( sectionID == FontFile_Section_Finished )
						break;

					int sectionLength = file.ReadInt32();

					// Read the font info section.
					if ( sectionID == FontFile_Section_FontInfo )
					{
						if ( !ReadFontInfo( file, version1_0 ) )
							return false;

						didReadFontInfo = true;
					}
					else if ( sectionID == FontFile_Section_Characters )
					{
						// Characters section.
						if ( !ReadCharacters( file ) )
							return false;

						didReadCharacters = true;
					}
					else if ( sectionID == FontFile_Section_Kerning )
					{
						// We must have read the characters before we can read kerning.
						if ( !didReadCharacters )
							return false;

						// Kerning.
						if ( !ReadKerning( file ) )
							return false;

						didReadKerning = true;
					}
					else if ( sectionID == FontFile_Section_Config )
					{
						// Configuration for the SudoFont program.
						if ( keepConfigBlock )
						{
							byte[] bytes = new byte[ sectionLength ];
							file.Read( bytes, 0, sectionLength );

							_configurationBlockStream = new MemoryStream();
							_configurationBlockStream.Write( bytes, 0, bytes.Length );
							_configurationBlockStream.Position = 0;
						}
						else
						{
							file.BaseStream.Seek( sectionLength, SeekOrigin.Current );
						}
					}
				}

				InitCharactersLookupTable();
				return ( didReadCharacters && didReadFontInfo && didReadKerning );
			}
			catch ( Exception )
			{
				return false;
			}
		}

		// Use this if you mipmap the texture that you load. This will adjust all the
		// relevant variables (like character offsets
		public void AdjustForTextureSize( int width, int height )
		{
			this.RuntimeScale = (float)width / this.OriginalTextureWidth;
		}

		public MemoryStream ConfigurationBlockStream
		{
			get { return _configurationBlockStream; }
		}

		// Returns -1 if the character isn't found.
		public int FindCharacter( Char ch )
		{
			// Can we do a fast lookup?
			if ( ch >= CharactersLookupTable_FirstASCIIValue && ch <= CharactersLookupTable_LastASCIIValue )
			{
				return _charactersLookupTable[ (int)ch - CharactersLookupTable_FirstASCIIValue ];
			}
			else
			{
				// Doh, we must do a full search.
				for ( int i=0; i < this.Characters.Length; i++ )
				{
					if ( this.Characters[i].CharacterCode == ch )
						return i;
				}

				return -1;
			}
		}

		// Get the line height, accounting for the runtime scaling.
		public int LineHeight
		{
			get { return (int)( this.OriginalLineHeight * this.RuntimeScale ); }
		}

		// Initialize our 
		void InitCharactersLookupTable()
		{
			_charactersLookupTable = new short[ CharactersLookupTable_LastASCIIValue + CharactersLookupTable_FirstASCIIValue + 1 ];

			for ( int i=0; i < _charactersLookupTable.Length; i++ )
				_charactersLookupTable[i] = -1;

			for ( int i=0; i < this.Characters.Length; i++ )
			{
				int charCode = (int)this.Characters[i].CharacterCode;
				if ( charCode >= CharactersLookupTable_FirstASCIIValue && charCode <= CharactersLookupTable_LastASCIIValue )
					_charactersLookupTable[ charCode - CharactersLookupTable_FirstASCIIValue ] = (short)i;
			}
		}

		bool ReadFontInfo( BinaryReader file, bool version1_0 )
		{
			this.OriginalLineHeight = file.ReadInt16();

			if ( !version1_0 )
			{
				this.OriginalTextureWidth = file.ReadInt16();
				this.OriginalTextureHeight = file.ReadInt16();
			}

			return ( this.OriginalLineHeight > 0 && this.OriginalLineHeight < 9999 );
		}

		bool ReadCharacters( BinaryReader file )
		{
			this.Characters = new Character[ file.ReadInt16() ];

			for ( int i=0; i < this.Characters.Length; i++ )
			{
				this.Characters[i].CharacterCode = (Char)file.ReadInt16();

				this.Characters[i].PackedX = file.ReadInt16();
				this.Characters[i].PackedY = file.ReadInt16();
				this.Characters[i].PackedWidth = file.ReadInt16();
				this.Characters[i].PackedHeight = file.ReadInt16();

				this.Characters[i].XOffset = file.ReadInt16();
				this.Characters[i].YOffset = file.ReadInt16();

				this.Characters[i].XAdvance = file.ReadInt16();
			}

			return true;
		}

		bool ReadKerning( BinaryReader file )
		{
			int numCharactersWithKerning = file.ReadInt16();

			for ( int iCharacter=0; iCharacter < numCharactersWithKerning; iCharacter++ )
			{
				// Which character is this kerning information for?
				int whichCharacter = file.ReadInt16();
				if ( whichCharacter < 0 || whichCharacter >= this.Characters.Length )
					return false;

				// Read the kerning info for it.
				if ( !ReadKerningForCharacter( ref this.Characters[whichCharacter], file ) )
					return false;
			}

			return true;
		}

		bool ReadKerningForCharacter( ref Character c, BinaryReader file )
		{
			c.Kerning = new KerningInfo[ file.ReadInt16() ];

			for ( int i=0; i < c.Kerning.Length; i++ )
			{
				c.Kerning[i].SecondChar = (Char)file.ReadInt16();
				c.Kerning[i].KernAmount = file.ReadInt16();
			}

			return true;
		}

		[ DebuggerDisplay( "Char: {Char}, Packed: ({PackedX}, {PackedY}, {PackedWidth}, {PackedHeight}), XAdvance: {XAdvance}, Offset: ({XOffset}, {YOffset})" ) ]
		public struct Character
		{
			// Find out how much extra padding to add (or subtract) between this character and the next character.
			public int GetKerning( Char nextChar )
			{
				if ( this.Kerning == null )
					return 0;

				for ( int i=0; i < Kerning.Length; i++ )
				{
					if ( this.Kerning[i].SecondChar == nextChar )
						return this.Kerning[i].KernAmount;
				}

				return 0;
			}

			// Which character this Character references.
			public Char CharacterCode;
			
			// Location of this character in the packed image.
			public short PackedX;
			public short PackedY;
			public short PackedWidth;
			public short PackedHeight;

			// Where to draw this character on the target.
			public short XOffset;
			public short YOffset;

			// How much to advance your X position after drawing this character.
			// The total amount to advance for each char is ( XAdvance + GetKerning( nextChar ) ).
			public short XAdvance;

			public KerningInfo[] Kerning;
		}

		[ DebuggerDisplay( "SecondChar: {SecondChar}, KernAmount: {KernAmount}" ) ]
		public struct KerningInfo
		{
			public Char SecondChar;	// The first character is the SudoFont.Character that owns this KerningInfo.
			public short KernAmount;
		}
		
		public float RuntimeScale = 1;

		// This is a lookup table that is used for some really common ASCII characters (values 32 - 128).
		const int CharactersLookupTable_FirstASCIIValue = 32;
		const int CharactersLookupTable_LastASCIIValue = 126;
		short[] _charactersLookupTable;

		// Font file keys.
		public static readonly string FontFileHeader = "SudoFont1.1";
		public static readonly short FontFile_Section_FontInfo = 0;		// This is a bunch of info like font height, name, etc.
		public static readonly short FontFile_Section_Characters = 1;
		public static readonly short FontFile_Section_Kerning = 2;
		public static readonly short FontFile_Section_Config = 3;			// This is the configuration for SudoFont.
		public static readonly short FontFile_Section_Finished = 999;

		// All the characters in this font.
		public Character[] Characters;
		public int OriginalLineHeight;

		// Character.PackedX and PackedY are in terms of this. If you've downsampled your font texture,
		// these will allow you to still get the right texture coordinates.
		public int OriginalTextureWidth;
		public int OriginalTextureHeight;

		// Only valid if keepConfigBlock is true in Load.
		MemoryStream _configurationBlockStream;
	}


	// This is a reference class to show how the letters should be spaced.
	public class FontLayout
	{
		[ DebuggerDisplay( "Offset: ({XOffset}, {YOffset}), Index: {SudoFontIndex}" ) ]
		public struct CharacterLayout
		{
			public int SudoFontIndex;	// The index into SudoFont.Characters
			public int XOffset;
			public int YOffset;
		}

		public static int MeasureStringWidth( RuntimeFont font, string str )
		{
			int width = 0;
			for ( int i=0; i < str.Length; i++ )
			{
				Char ch = str[i];
				int ic = font.FindCharacter( ch );
				if ( ic != -1 )
				{
					float totalXAdvance = font.Characters[ic].XAdvance * font.RuntimeScale;
					
					if ( i < str.Length-1 )
						totalXAdvance += font.Characters[ic].GetKerning( str[i+1] ) * font.RuntimeScale;
					
					width += (int)Math.Ceiling( totalXAdvance );
				}
			}

			return width;
		}

		public static CharacterLayout[] LayoutCharacters( RuntimeFont font, string str, int startX, int startY )
		{
			CharacterLayout[] layout = new CharacterLayout[ str.Length ];

			int curX = startX;
			int curY = startY;

			for ( int i=0; i < str.Length; i++ )
			{
				int whichCharacter = font.FindCharacter( str[i] );
				if ( whichCharacter == -1 )
					continue;

				layout[i].SudoFontIndex = whichCharacter;
				layout[i].XOffset = (int)( curX + font.Characters[whichCharacter].XOffset * font.RuntimeScale );
				layout[i].YOffset = (int)( curY + font.Characters[whichCharacter].YOffset * font.RuntimeScale );

				float totalXAdvance = font.Characters[whichCharacter].XAdvance * font.RuntimeScale;
				
				if ( i+1 < str.Length )
					totalXAdvance += font.Characters[whichCharacter].GetKerning( str[i+1] ) * font.RuntimeScale;
				
				curX += (int)Math.Ceiling( totalXAdvance );
			}

			return layout;
		}
	}
}

