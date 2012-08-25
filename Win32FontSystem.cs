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
	class Blah
	{
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		static extern int EnumFontFamiliesEx(IntPtr hdc,
										[In] IntPtr pLogfont,
										EnumFontExDelegate lpEnumFontFamExProc,
										IntPtr lParam,
										uint dwFlags);


		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{

			public int lfHeight;
			public int lfWidth;
			public int lfEscapement;
			public int lfOrientation;
			public FontWeight lfWeight;
			[MarshalAs(UnmanagedType.U1)]
			public bool lfItalic;
			[MarshalAs(UnmanagedType.U1)]
			public bool lfUnderline;
			[MarshalAs(UnmanagedType.U1)]
			public bool lfStrikeOut;
			public FontCharSet lfCharSet;
			public FontPrecision lfOutPrecision;
			public FontClipPrecision lfClipPrecision;
			public FontQuality lfQuality;
			public FontPitchAndFamily lfPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
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


		public void RunTest( Graphics G )
		{
			LOGFONT lf = CreateLogFont("");

			IntPtr plogFont = Marshal.AllocHGlobal(Marshal.SizeOf(lf));
			Marshal.StructureToPtr(lf, plogFont, true);

			int ret = 0;
			try
			{
				IntPtr P = G.GetHdc();

				del1 = new EnumFontExDelegate(callback1);
				ret = EnumFontFamiliesEx(P, plogFont, del1, IntPtr.Zero, 0);

				System.Diagnostics.Trace.WriteLine("EnumFontFamiliesEx = " + ret.ToString());

				G.ReleaseHdc(P);
			}
			catch
			{
				System.Diagnostics.Trace.WriteLine("Error!");
			}
			finally
			{
				Marshal.DestroyStructure(plogFont, typeof(LOGFONT));

			}
			
			
			// Try calling CreateFont on each font.
			int numCreated = 0;
			foreach ( string faceName in this.FontNames )
			{
				IntPtr hFont = Blah.CreateFontW( 
					12,
					0,		// width
					0,		// escapement
					0,		// orientation
					(int)Blah.FontWeight.FW_NORMAL,
					0,	// italic
					0,	// underline
					0,	// strikeout
					(uint)Blah.FontCharSet.DEFAULT_CHARSET,
					(uint)Blah.FontPrecision.OUT_DEFAULT_PRECIS,
					(uint)Blah.FontClipPrecision.CLIP_DEFAULT_PRECIS,
					(uint)Blah.FontQuality.DEFAULT_QUALITY,
					(uint)( Blah.FontPitchAndFamily.DEFAULT_PITCH | Blah.FontPitchAndFamily.FF_DONTCARE ),
					faceName );

				if ( hFont != IntPtr.Zero )
				{
					Debug.WriteLine( "Created " + faceName );
					++numCreated;

					Blah.DeleteObject( hFont );
				}
			}
		}

		public delegate int EnumFontExDelegate(ref ENUMLOGFONTEX lpelfe, ref NEWTEXTMETRICEX lpntme, int FontType, int lParam);
			public EnumFontExDelegate del1;


		public int callback1(ref ENUMLOGFONTEX lpelfe, ref NEWTEXTMETRICEX lpntme, int FontType, int lParam)
		{
			try
			{ 
				if ( lpelfe.elfScript == "Western" )
				{
					Debug.WriteLine( "Font: {0}, Style: {1}", lpelfe.elfFullName, lpelfe.elfStyle );
					FontNames.Add( lpelfe.elfFullName );
				}
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(e.ToString());
			}
			return 1;
		}

		public List< string > FontNames = new List<string>();

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
		
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject( IntPtr hObject );

		public static LOGFONT CreateLogFont(string fontname)
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
	}
}

