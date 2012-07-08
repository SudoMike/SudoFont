
using System;
using System.IO;


namespace SudoFont
{
	// You can use this to load a binary font file at runtime.
	// This serves as a specification for the binary file.
	public class SudoFont
	{
		public bool Load( BinaryReader file )
		{
			try
			{
				// Make sure it's the right version.
				if ( file.ReadString() != FontFileHeader )
					return false;

				while ( true )
				{
					short sectionID = file.ReadInt16();

					// Finished?
					if ( sectionID == FontFile_Section_Finished )
						break;

					// Read the font info block.
					if ( sectionID == FontFile_Section_FontInfo )
					{
						if ( !ReadFontInfo( file ) )
							return false;
					}
					else if ( sectionID == FontFile_Section_Characters )
					{
						// Characters block.
					}
					else if ( sectionID == FontFile_Section_Kerning )
					{
						// Kerning.
					}
					else if ( sectionID == FontFile_Section_Config )
					{
						// Configuration for the SudoFont program.
					}
				}

				return true;
			}
			catch ( Exception )
			{
				return false;
			}
		}

		bool ReadFontInfo( BinaryReader file )
		{
			this.LineHeight = file.ReadInt16();
			return ( this.LineHeight > 0 && this.LineHeight < 9999 );
		}



		public struct Character
		{
			// Find out how much extra padding to add (or subtract) between this character and the next character.
			public int GetKerning( Char nextChar )
			{
				for ( int i=0; i < Kerning.Length; i++ )
				{
					if ( this.Kerning[i].SecondChar == nextChar )
						return this.Kerning[i].KernAmount;
				}

				return 0;
			}

			public Char Char;
			
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

		public struct KerningInfo
		{
			public Char SecondChar;	// The first character is the SudoFont.Character that owns this KerningInfo.
			public short KernAmount;
		}

		// Font file keys.
		public static readonly string FontFileHeader = "SudoFont1.0";
		public static readonly short FontFile_Section_FontInfo = 0;		// This is a bunch of info like font height, name, etc.
		public static readonly short FontFile_Section_Characters = 1;
		public static readonly short FontFile_Section_Kerning = 2;
		public static readonly short FontFile_Section_Config = 3;			// This is the configuration for SudoFont.
		public static readonly short FontFile_Section_Finished = 999;

		// All the characters in this font.
		public Character[] Characters;
		public int LineHeight;
	}
}

