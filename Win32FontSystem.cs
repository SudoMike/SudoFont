using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace SudoFont
{
	class Win32FontSystem : IFontSystem
	{
		public Win32FontSystem( Control control )
		{
			// Get the list of font families.
			LOGFONT lf = CreateLogFont();

			IntPtr plogFont = Marshal.AllocHGlobal( Marshal.SizeOf( lf ) );
			Marshal.StructureToPtr( lf, plogFont, true );

			using ( Graphics g = control.CreateGraphics() )
			{
				try
				{
					IntPtr hDC = g.GetHdc();

					EnumFontExDelegate callback = 
						delegate( ref ENUMLOGFONTEX lpelfe, ref NEWTEXTMETRICEX lpntme, int FontType, int lParam )
						{
							try
							{
								// For now, just get the western font names..
								if ( lpelfe.elfScript == "Western" )
								{
									_familyLogFonts[ lpelfe.elfFullName ] = lpelfe.elfLogFont;
								}
							}
							catch ( Exception e )
							{
								System.Diagnostics.Trace.WriteLine(e.ToString());
							}
							return 1;
						};

					EnumFontFamiliesEx( hDC, plogFont, callback, IntPtr.Zero, 0 );
					_familyNames = _familyLogFonts.Keys.ToList();

					g.ReleaseHdc( hDC );
				}
				catch
				{
					MessageBox.Show( "Error enumerating Win32 fonts" );
					Debug.Assert( false );
				}
				finally
				{
					Marshal.DestroyStructure( plogFont, typeof( LOGFONT ) );
				}
			}
		}

		public int NumFontFamilies 

		{ 
			get
			{
				return _familyNames.Count;
			}
		}

		public IFontFamily GetFontFamily( int iFamily )
		{
			return new Win32FontFamily( _familyNames[iFamily] );
		}

		public IFontFamily GetFontFamilyByName( string familyName )
		{
			return new Win32FontFamily( familyName );
		}

		public IFont CreateFont( string familyName, int size, FontStyle style )
		{
			// Setup the LOGFONT that represents this font.
			LOGFONT logFont = CopyLogFont( _familyLogFonts[familyName] );

			logFont.lfWeight = Win32FontSystem.FontWeight.FW_NORMAL;
			if ( ( style & FontStyle.Bold ) != 0 )
				logFont.lfWeight = Win32FontSystem.FontWeight.FW_BOLD;

			logFont.lfItalic = ( style & FontStyle.Italic ) != 0;
			logFont.lfUnderline = ( style & FontStyle.Underline ) != 0;
			logFont.lfStrikeOut = ( style & FontStyle.Strikeout ) != 0;
			logFont.lfHeight = size;
			logFont.lfCharSet = FontCharSet.DEFAULT_CHARSET;
			logFont.lfClipPrecision = FontClipPrecision.CLIP_DEFAULT_PRECIS;
			logFont.lfOutPrecision = FontPrecision.OUT_DEFAULT_PRECIS;
			logFont.lfPitchAndFamily = Win32FontSystem.FontPitchAndFamily.DEFAULT_PITCH | Win32FontSystem.FontPitchAndFamily.FF_DONTCARE;

			//IntPtr hFont = CreateFontIndirect( ref logFont );

			IntPtr hFont = Win32FontSystem.CreateFontW( 
				size,	// height
				0,		// width
				0,		// escapement
				0,		// orientation
				(int)logFont.lfWeight,
				BoolToUInt( logFont.lfItalic ),	// italic
				BoolToUInt( logFont.lfUnderline ),	// underline
				BoolToUInt( logFont.lfStrikeOut ),	// strikeout
				(uint)Win32FontSystem.FontCharSet.DEFAULT_CHARSET,
				(uint)Win32FontSystem.FontPrecision.OUT_DEFAULT_PRECIS,
				(uint)Win32FontSystem.FontClipPrecision.CLIP_DEFAULT_PRECIS,
				(uint)Win32FontSystem.FontQuality.DEFAULT_QUALITY,
				(uint)( Win32FontSystem.FontPitchAndFamily.DEFAULT_PITCH | Win32FontSystem.FontPitchAndFamily.FF_DONTCARE ),
				familyName );
			LOGFONT testLogFont = Win32FontSystem.GetLogFont( hFont );

			return new Win32Font( hFont, testLogFont );
		}

		static uint BoolToUInt( bool val )
		{
			return (uint)( val ? 1 : 0 );
		}

		static LOGFONT CopyLogFont( LOGFONT source )
		{
			LOGFONT dest = new LOGFONT()
			{
				lfHeight = source.lfHeight,
				lfWidth = source.lfWidth,
				lfEscapement = source.lfEscapement,
				lfOrientation = source.lfOrientation,
				lfWeight = source.lfWeight,
			
				lfItalic = source.lfItalic,
				lfUnderline = source.lfUnderline,
				lfStrikeOut = source.lfStrikeOut,
				lfCharSet = source.lfCharSet,
				lfOutPrecision = source.lfOutPrecision,
				lfClipPrecision = source.lfClipPrecision,
				lfQuality = source.lfQuality,
				lfPitchAndFamily = source.lfPitchAndFamily,

				lfFaceName = source.lfFaceName
			};

			return dest;
		}



		// Get a LOGFONT for an HFONT.
		public static LOGFONT GetLogFont( IntPtr hFont )
		{
			int sizeofLogFont = Marshal.SizeOf( typeof( LOGFONT ) );

			// Allocate a LOGFONT and call GetObject to fill it in.
			IntPtr plogFont = Marshal.AllocHGlobal( sizeofLogFont );
			int numBytesStored = GetObject( hFont, sizeofLogFont, plogFont );

			// Convert to a .NET LOGFONT
			LOGFONT logfont = CreateLogFont();
			Marshal.PtrToStructure( plogFont, logfont );

			// Cleanup.
			Marshal.DestroyStructure( plogFont, typeof( LOGFONT ) );
			return logfont;
		}

		[DllImport("gdi32.dll")]
		static extern int GetObject( IntPtr hgdiobj, int cbBuffer, IntPtr lpvObject );

		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		static extern int EnumFontFamiliesEx(IntPtr hdc,
										[In] IntPtr pLogfont,
										EnumFontExDelegate lpEnumFontFamExProc,
										IntPtr lParam,
										uint dwFlags);
		[DllImport("gdi32.dll")] static extern IntPtr CreateFontIndirect( [In] ref LOGFONT lplf );


		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			public int lfHeight;
			public int lfWidth;
			public int lfEscapement;
			public int lfOrientation;
			public FontWeight lfWeight;
			
			[MarshalAs(UnmanagedType.U1)]			public bool lfItalic;
			[MarshalAs(UnmanagedType.U1)]			public bool lfUnderline;
			[MarshalAs(UnmanagedType.U1)]			public bool lfStrikeOut;
			public FontCharSet lfCharSet;
			public FontPrecision lfOutPrecision;
			public FontClipPrecision lfClipPrecision;
			public FontQuality lfQuality;
			public FontPitchAndFamily lfPitchAndFamily;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]	public string lfFaceName;
		}


		public enum FontWeight : int
		{
			FW_DONTCARE = 0,
			FW_THIN = 100,
			FW_EXTRALIGHT = 200,
			FW_LIGHT = 300,
			FW_NORMAL = 400,
			FW_MEDIUM = 500,
			FW_SEMIBOLD = 600,
			FW_BOLD = 700,
			FW_EXTRABOLD = 800,
			FW_HEAVY = 900,
		}
		public enum FontCharSet : byte
		{
			ANSI_CHARSET = 0,
			DEFAULT_CHARSET = 1,
			SYMBOL_CHARSET = 2,
			SHIFTJIS_CHARSET = 128,
			HANGEUL_CHARSET = 129,
			HANGUL_CHARSET = 129,
			GB2312_CHARSET = 134,
			CHINESEBIG5_CHARSET = 136,
			OEM_CHARSET = 255,
			JOHAB_CHARSET = 130,
			HEBREW_CHARSET = 177,
			ARABIC_CHARSET = 178,
			GREEK_CHARSET = 161,
			TURKISH_CHARSET = 162,
			VIETNAMESE_CHARSET = 163,
			THAI_CHARSET = 222,
			EASTEUROPE_CHARSET = 238,
			RUSSIAN_CHARSET = 204,
			MAC_CHARSET = 77,
			BALTIC_CHARSET = 186,
		}
		public enum FontPrecision : byte
		{
			OUT_DEFAULT_PRECIS = 0,
			OUT_STRING_PRECIS = 1,
			OUT_CHARACTER_PRECIS = 2,
			OUT_STROKE_PRECIS = 3,
			OUT_TT_PRECIS = 4,
			OUT_DEVICE_PRECIS = 5,
			OUT_RASTER_PRECIS = 6,
			OUT_TT_ONLY_PRECIS = 7,
			OUT_OUTLINE_PRECIS = 8,
			OUT_SCREEN_OUTLINE_PRECIS = 9,
			OUT_PS_ONLY_PRECIS = 10,
		}
		public enum FontClipPrecision : byte
		{
			CLIP_DEFAULT_PRECIS = 0,
			CLIP_CHARACTER_PRECIS = 1,
			CLIP_STROKE_PRECIS = 2,
			CLIP_MASK = 0xf,
			CLIP_LH_ANGLES = (1 << 4),
			CLIP_TT_ALWAYS = (2 << 4),
			CLIP_DFA_DISABLE = (4 << 4),
			CLIP_EMBEDDED = (8 << 4),
		}
		public enum FontQuality : byte
		{
			DEFAULT_QUALITY = 0,
			DRAFT_QUALITY = 1,
			PROOF_QUALITY = 2,
			NONANTIALIASED_QUALITY = 3,
			ANTIALIASED_QUALITY = 4,
			CLEARTYPE_QUALITY = 5,
			CLEARTYPE_NATURAL_QUALITY = 6,
		}
		[Flags]
		public enum FontPitchAndFamily : byte
		{
			DEFAULT_PITCH = 0,
			FIXED_PITCH = 1,
			VARIABLE_PITCH = 2,
			FF_DONTCARE = (0 << 4),
			FF_ROMAN = (1 << 4),
			FF_SWISS = (2 << 4),
			FF_MODERN = (3 << 4),
			FF_SCRIPT = (4 << 4),
			FF_DECORATIVE = (5 << 4),
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct NEWTEXTMETRIC
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public char tmFirstChar;
			public char tmLastChar;
			public char tmDefaultChar;
			public char tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
			int ntmFlags;
			int ntmSizeEM;
			int ntmCellHeight;
			int ntmAvgWidth;
		}

		#pragma warning disable 169
		public struct FONTSIGNATURE
		{
			[MarshalAs(UnmanagedType.ByValArray)]
			int[] fsUsb;
			[MarshalAs(UnmanagedType.ByValArray)]
			int[] fsCsb;
		}
		public struct NEWTEXTMETRICEX
		{
			NEWTEXTMETRIC ntmTm;
			FONTSIGNATURE ntmFontSig;
		}
		#pragma warning restore 169

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct ENUMLOGFONTEX
		{
			public LOGFONT elfLogFont;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string elfFullName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string elfStyle;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string elfScript;
		}

		private const byte DEFAULT_CHARSET = 1;
		private const byte SHIFTJIS_CHARSET = 128;
		private const byte JOHAB_CHARSET = 130;
		private const byte EASTEUROPE_CHARSET = 238;

		private const byte DEFAULT_PITCH = 0;
		private const byte FIXED_PITCH = 1;
		private const byte VARIABLE_PITCH = 2;
		private const byte FF_DONTCARE = (0 << 4);
		private const byte FF_ROMAN = (1 << 4);
		private const byte FF_SWISS = (2 << 4);
		private const byte FF_MODERN = (3 << 4);
		private const byte FF_SCRIPT = (4 << 4);
		private const byte FF_DECORATIVE = (5 << 4);


		delegate int EnumFontExDelegate(ref ENUMLOGFONTEX lpelfe, ref NEWTEXTMETRICEX lpntme, int FontType, int lParam);

		[DllImport("gdi32", EntryPoint="CreateFontW")]
		public static extern IntPtr CreateFontW(
				[In] Int32 nHeight,
				[In] Int32 nWidth,
				[In] Int32 nEscapement,
				[In] Int32 nOrientation,
				[In] Int32 fnWeight,
				[In] UInt32 fdwItalic,
				[In] UInt32 fdwUnderline,
				[In] UInt32 fdwStrikeOut,
				[In] UInt32 fdwCharSet,
				[In] UInt32 fdwOutputPrecision,
				[In] UInt32 fdwClipPrecision,
				[In] UInt32 fdwQuality,
				[In] UInt32 fdwPitchAndFamily,
				[In] [MarshalAs(UnmanagedType.LPStr)] string lpszFace );
		
		[DllImport("gdi32.dll")]	public static extern bool DeleteObject( IntPtr hObject );
		[DllImport("gdi32.dll")]	public static extern bool GetTextExtentPoint( IntPtr hdc, string lpString, int cbString, ref Size lpSize );

		[DllImport("gdi32.dll", EntryPoint="GetCharacterPlacementW")]  public static extern uint GetCharacterPlacementW( IntPtr hdc, [MarshalAs(UnmanagedType.LPWStr)] string lpString, int nCount, int nMaxExtent, ref GCP_RESULTS lpResults, uint dwFlags );
		[DllImport("gdi32.dll")] public static extern IntPtr CreatePen( PenStyle fnPenStyle, int nWidth, uint crColor );
		[DllImport("gdi32.dll")] public static extern uint SetTextColor( IntPtr hdc, uint crColor );

		[DllImport("gdi32.dll")] public static extern bool GetCharABCWidthsFloat( IntPtr hdc, uint iFirstChar, uint iLastChar, [Out] ABCFLOAT [] lpABCF );

		[StructLayout(LayoutKind.Sequential)]
		public struct ABCFLOAT
		{
			 /// <summary>Specifies the A spacing of the character. The A spacing is the distance to add to the current
			 /// position before drawing the character glyph.</summary>
			 public float abcfA;
			 /// <summary>Specifies the B spacing of the character. The B spacing is the width of the drawn portion of
			 /// the character glyph.</summary>
			 public float abcfB;
			 /// <summary>Specifies the C spacing of the character. The C spacing is the distance to add to the current
			 /// position to provide white space to the right of the character glyph.</summary>
			 public float abcfC;
		}

		public enum PenStyle : int
		{
			PS_SOLID    = 0, //The pen is solid.
			PS_DASH     = 1, //The pen is dashed.
			PS_DOT      = 2, //The pen is dotted.
			PS_DASHDOT      = 3, //The pen has alternating dashes and dots.
			PS_DASHDOTDOT       = 4, //The pen has alternating dashes and double dots.
			PS_NULL     = 5, //The pen is invisible.
			PS_INSIDEFRAME      = 6,// Normally when the edge is drawn, it’s centred on the outer edge meaning that half the width of the pen is drawn
				// outside the shape’s edge, half is inside the shape’s edge. When PS_INSIDEFRAME is specified the edge is drawn 
				//completely inside the outer edge of the shape.
			PS_USERSTYLE    = 7,
			PS_ALTERNATE    = 8,
			PS_STYLE_MASK       = 0x0000000F,

			PS_ENDCAP_ROUND     = 0x00000000,
			PS_ENDCAP_SQUARE    = 0x00000100,
			PS_ENDCAP_FLAT      = 0x00000200,
			PS_ENDCAP_MASK      = 0x00000F00,

			PS_JOIN_ROUND       = 0x00000000,
			PS_JOIN_BEVEL       = 0x00001000,
			PS_JOIN_MITER       = 0x00002000,
			PS_JOIN_MASK    = 0x0000F000,

			PS_COSMETIC     = 0x00000000,
			PS_GEOMETRIC    = 0x00010000,
			PS_TYPE_MASK    = 0x000F0000
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct GCP_RESULTS
		{
			public int StructSize;
			[MarshalAs(UnmanagedType.LPTStr)]
			public string OutString;
			public IntPtr Order;
			public IntPtr Dx;
			public IntPtr CaretPos;
			public IntPtr Class;
			public IntPtr Glyphs;
			public int GlyphCount;
			public int MaxFit;
		}

		[Flags]
		public enum GCPFlags : uint
		{
			GCP_DBCS = 0x0001,
			GCP_REORDER = 0x0002,
			GCP_USEKERNING = 0x0008,
			GCP_GLYPHSHAPE = 0x0010,
			GCP_LIGATE = 0x0020,
			GCP_DIACRITIC = 0x0100,
			GCP_KASHIDA = 0x0400,
			GCP_ERROR = 0x8000,
			GCP_JUSTIFY = 0x00010000,
			GCP_CLASSIN = 0x00080000,
			GCP_MAXEXTENT = 0x00100000,
			GCP_JUSTIFYIN = 0x00200000,
			GCP_DISPLAYZWG = 0x00400000,
			GCP_SYMSWAPOFF = 0x00800000,
			GCP_NUMERICOVERRIDE = 0x01000000,
			GCP_NEUTRALOVERRIDE = 0x02000000,
			GCP_NUMERICSLATIN = 0x04000000,
			GCP_NUMERICSLOCAL = 0x08000000,
		}
	
		public static LOGFONT CreateLogFont()
		{
			LOGFONT lf = new LOGFONT();
			lf.lfHeight = 0;
			lf.lfWidth = 0;
			lf.lfEscapement = 0;
			lf.lfOrientation = 0;
			lf.lfWeight = 0;
			lf.lfItalic = false;
			lf.lfUnderline = false;
			lf.lfStrikeOut = false;
			lf.lfCharSet = FontCharSet.DEFAULT_CHARSET;
			lf.lfOutPrecision = 0;
			lf.lfClipPrecision = 0;
			lf.lfQuality = 0;
			lf.lfPitchAndFamily =  FontPitchAndFamily.FF_DONTCARE;
			lf.lfFaceName = "";

			return lf;
		}	

		[DllImport("gdi32.dll", ExactSpelling=true, PreserveSig=true, SetLastError=true)] public static extern IntPtr SelectObject( IntPtr hdc, IntPtr hgdiobj );
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)] public static extern bool TextOut( IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString );
		
		[DllImport("gdi32.dll")] public static extern int SetBkMode( IntPtr hdc, int iBkMode );
		public const int BKMODE_TRANSPARENT = 1;
		public const int BKMODE_OPAQUE = 2;
	
		
		Dictionary< string, LOGFONT > _familyLogFonts = new Dictionary<string,LOGFONT>();
		List< string > _familyNames = new List<string>();
	}

	public class GdiObjectSelector : IDisposable
	{
		public GdiObjectSelector( Graphics g, IntPtr hObject )
		{
			_graphics = g;
			_hDC = g.GetHdc();
			_prevObject = Win32FontSystem.SelectObject( _hDC, hObject );
		}

		public GdiObjectSelector( IntPtr hDC, IntPtr hObject )
		{
			_hDC = hDC;
			_prevObject = Win32FontSystem.SelectObject( _hDC, hObject );
		}

		public void Dispose()
		{
			Win32FontSystem.SelectObject( _hDC, _prevObject );
			
			if ( _graphics != null )
				_graphics.ReleaseHdc( _hDC );

			_hDC = _prevObject = IntPtr.Zero;
			_graphics = null;
		}

		public IntPtr HDC
		{
			get { return _hDC; }
		}

		Graphics _graphics;
		IntPtr _hDC;
		IntPtr _prevObject;
	}


	class Win32FontFamily : IFontFamily
	{
		public Win32FontFamily( string familyName )
		{
			_familyName = familyName;
		}

		public string Name 
		{ 
			get
			{
				return _familyName;
			}
		}

		public bool IsStyleAvailable( FontStyle style )
		{
			return true; // TODO
		}

		string _familyName;
	}


	class Win32Font : IFont
	{
		public Win32Font( IntPtr hFont, Win32FontSystem.LOGFONT logFont )
		{
			_font = hFont;
			
			if ( _font != IntPtr.Zero )
			{
				//_logFont = Win32FontSystem.GetLogFont( hFont );
				_logFont = logFont;
			}
		}

		public string Name
		{
			get
			{
				return _logFont.lfFaceName;
			}
		}

		public SizeF MeasureString( Graphics g, string str )
		{
			using ( GdiObjectSelector selector = new GdiObjectSelector( g, _font ) )
			{
				Size size = new Size( 0, 0 );
				Win32FontSystem.GetTextExtentPoint( selector.HDC, str, str.Length, ref size );
				return new SizeF( size.Width, size.Height );
			}
		}

		public void DrawString( Graphics g, string str, Brush brush, Point location )
		{
			SolidBrush solidBrush = brush as SolidBrush;
			if ( solidBrush == null )
				throw new Exception( "Win32Font.DrawString only supports SolidBrush" );

			using ( GdiObjectSelector selector = new GdiObjectSelector( g, _font ) )
			{
				// Make a pen to represent the color.
				uint crColor = (uint)( solidBrush.Color.R << 0 ) | (uint)( solidBrush.Color.G << 8 ) | (uint)( solidBrush.Color.B << 16 );
				uint prevTextColor = Win32FontSystem.SetTextColor( selector.HDC, crColor );

				// Set background mode to transparent.
				int oldBKMode = Win32FontSystem.SetBkMode( selector.HDC, Win32FontSystem.BKMODE_TRANSPARENT );

				Win32FontSystem.TextOut( selector.HDC, 0, 0, str, str.Length );
				
				Win32FontSystem.SetBkMode( selector.HDC, oldBKMode );
				Win32FontSystem.SetTextColor( selector.HDC, prevTextColor );
			}
		}

		public float[] GetCharacterXPositions( Graphics g, string str )
		{
#if true
			// Make sure our _charWidths list has all the necessary characters.
			using ( GdiObjectSelector selector = new GdiObjectSelector( g, _font ) )
			{
				UpdateCharWidths( selector.HDC, str );
			}

			float[] xCoords = new float[ str.Length ];
			float curXOffset = 0;
			for ( int i=0; i < str.Length; i++ )
			{
				xCoords[i] = curXOffset;
				Win32FontSystem.ABCFLOAT abc = _charWidths[ (uint)str[i] ];
				curXOffset += abc.abcfA + abc.abcfB + abc.abcfC;
			}

			return xCoords;
#else
			// This method gives us spacing between character cells, but it doesn't tell us what the cells are!
			Win32FontSystem.GCP_RESULTS results = new Win32FontSystem.GCP_RESULTS();
			results.StructSize = Marshal.SizeOf( typeof( Win32FontSystem.GCP_RESULTS ) );

			// Setup the int array for them to write results into.
			int[] dx = new int[ str.Length ];
			GCHandle handle = GCHandle.Alloc( dx, GCHandleType.Pinned );
			results.Dx = Marshal.UnsafeAddrOfPinnedArrayElement( dx, 0 );

			using ( GdiObjectSelector selector = new GdiObjectSelector( g, _font ) )
			{
				// Call GetCharacterPlacement
				Win32FontSystem.GetCharacterPlacementW( 
					selector.HDC, 
					str, 
					str.Length, 
					0,				// max extent (ignored)
					ref results,
					(uint)Win32FontSystem.GCPFlags.GCP_USEKERNING );
			}

			// Unpin the array.
			handle.Free();

			// Convert to floats for output.
			return dx.Select( x => (float)x ).ToArray();
#endif
		}

		public float GetHeightInPixels( Control control )
		{
			//lfHeight = -MulDiv(PointSize, GetDeviceCaps(hDC, LOGPIXELSY), 72);
			return _logFont.lfHeight;
		}

		public float GetBaselinePos( FontStyle style )
		{
			return 3;
		}

		public IFontFamily FontFamily
		{
			get
			{
				return new Win32FontFamily( _logFont.lfFaceName );
			}
		}

		~Win32Font()
		{
			Win32FontSystem.DeleteObject( _font );
			_font = IntPtr.Zero;
		}

		void UpdateCharWidths( IntPtr hDC, string str )
		{
			if ( str.Length == 0 )
				return;

			// Figure out the character range for this string.
			uint rangeMin = uint.MaxValue;
			uint rangeMax = uint.MinValue;
			for ( int i=0; i < str.Length; i++ )
			{
				uint ch = (uint)str[i];
				rangeMin = Math.Min( rangeMin, ch );
				rangeMax = Math.Max( rangeMax, ch );
			}

			if ( _charWidthsNumChars == 0 )
			{
				_charWidthsFirstChar = rangeMin;
				_charWidthsNumChars = rangeMax - rangeMin + 1;
				CalculateCharWidthRange( hDC, _charWidthsFirstChar, _charWidthsNumChars );
			}
			else
			{
				// Does this string's character range exceed the one we've calculated so far?
				if ( rangeMin < _charWidthsFirstChar )
				{
					CalculateCharWidthRange( hDC, rangeMin, _charWidthsFirstChar-rangeMin );
					_charWidthsFirstChar = rangeMin;
				}
				
				if ( rangeMax >= _charWidthsFirstChar+_charWidthsNumChars )
				{
					CalculateCharWidthRange( hDC, _charWidthsFirstChar+_charWidthsNumChars, rangeMax - ( _charWidthsFirstChar + _charWidthsNumChars ) + 1 );
					_charWidthsNumChars = rangeMax - _charWidthsFirstChar + 1;
				}
			}
		}

		void CalculateCharWidthRange( IntPtr hDC, uint firstChar, uint numChars )
		{
			Win32FontSystem.ABCFLOAT[] values = new Win32FontSystem.ABCFLOAT[numChars];
			Win32FontSystem.GetCharABCWidthsFloat( hDC, firstChar, firstChar + numChars - 1, values );

			for ( uint i=0; i < numChars; i++ )
			{
				Debug.Assert( !_charWidths.ContainsKey( i + firstChar ) );
				_charWidths[i + firstChar] = values[i];
			}
		}

		// Range of characters being used.
		uint _charWidthsFirstChar = 0;
		uint _charWidthsNumChars = 0;
		Dictionary< uint, Win32FontSystem.ABCFLOAT > _charWidths = new Dictionary<uint,Win32FontSystem.ABCFLOAT>();

		Win32FontSystem.LOGFONT _logFont;
		IntPtr _font;
	}
}

