
using System;
using System.Drawing;
using System.Runtime.InteropServices;


namespace SudoFont
{
	public class FontServices
	{
		[ StructLayout( LayoutKind.Sequential ) ]
		public struct ABC
		{
			// Returns A + B + C
			public int Total
			{
				get { return (int)( this.A + this.B + this.C ); }
			}

			public int A;
			public uint B;
			public int C;
		}
		
		public static ABC GetCharWidthABC( Char ch, Font font, Graphics gr )
		{
			ABC[] _temp = new ABC[1];
			IntPtr hDC = gr.GetHdc();
			
			Font ft = (Font)font.Clone();
			IntPtr hFt = ft.ToHfont();

			SelectObject(hDC, hFt);
			GetCharABCWidthsW(hDC, ch, ch, _temp);

			DeleteObject(hFt);

			gr.ReleaseHdc();
			return _temp[0];
		}

		public static TEXTMETRIC GetTextMetrics( Graphics graphics, Font font ) 
		{ 
			IntPtr hDC = graphics.GetHdc(); 
			TEXTMETRIC textMetric; 
			IntPtr hFont = font.ToHfont(); 

			try 
			{ 
				IntPtr hFontPreviouse = SelectObject(hDC, hFont); 
				bool result = GetTextMetrics(hDC, out textMetric); 
				SelectObject(hDC, hFontPreviouse); 
			} 
			finally 
			{  
				DeleteObject(hFont); 
				graphics.ReleaseHdc(hDC); 
			} 

			return textMetric; 
		}
		
		public static FontToHDCBinder BindFontToHDC( Graphics g, Font font )
		{
			return new FontToHDCBinder( g, font );
		}

		public static void TextOut( Graphics graphics, Font font, Color color, int x, int y, string str )
		{
			using ( FontToHDCBinder binder = FontServices.BindFontToHDC( graphics, font ) )
			{
				UInt32 prevColor = GetTextColor( binder.HDC );
				SetTextColor( binder.HDC, ToColorRef( color ) );

				int prevBkMode = SetBkMode( binder.HDC, TRANSPARENT );

				TextOut( binder.HDC, x, y, str, str.Length );

				SetBkMode( binder.HDC, prevBkMode );
				SetTextColor( binder.HDC, prevColor );
			}
		}

		// This isn't used anymore. It's left here as reference for how to get kerning data in C#.
		public static KerningPair[] GetKerningPairs( Font font, Graphics graphics )
		{
			// Select the HFONT into the HDC.
			IntPtr hDC = graphics.GetHdc();
			Font fontClone = (Font)font.Clone();
			IntPtr hFont = fontClone.ToHfont();
			SelectObject( hDC, hFont );

			// Find out how many pairs there are and allocate them.
			int numKerningPairs = GetKerningPairs( hDC.ToInt32(), 0, null );
			KerningPair[] kerningPairs = new KerningPair[ numKerningPairs ];

			// Get the pairs.
			GetKerningPairs( hDC.ToInt32(), kerningPairs.Length, kerningPairs );

			DeleteObject( hFont );
			graphics.ReleaseHdc();

			return kerningPairs;
		}

		// Convert a Color to a COLORREF.
		public static UInt32 ToColorRef( Color clr )
		{
			return ((UInt32)clr.R << 0) | ((UInt32)clr.G << 8) | ((UInt32)clr.B << 16);
		}

		// Simplifies binding a font to an HDC for use with native Win32 functions.
		public class FontToHDCBinder : IDisposable
		{
			public FontToHDCBinder( Graphics g, Font font )
			{
				_graphics = g;
				_hDC = g.GetHdc();
				_hFont = font.ToHfont();
				_hPrevFont = FontServices.SelectObject( _hDC, _hFont );
			}

			public IntPtr HDC
			{
				get { return _hDC; }
			}

			public void Dispose()
			{
				FontServices.DeleteObject( _hFont );
				_graphics.ReleaseHdc( _hDC );

				_hFont = _hDC = IntPtr.Zero;
			}

			Graphics _graphics;
			IntPtr _hDC;
			IntPtr _hFont;
			IntPtr _hPrevFont;
		}

	
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct TEXTMETRIC
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
		}

		[ StructLayout(LayoutKind.Sequential) ]
		public struct KerningPair
		{
			public Int16 wFirst;
			public Int16 wSecond;
			public Int32 iKernAmount;
		}

		[ DllImport("Gdi32.dll", EntryPoint="GetKerningPairs", SetLastError = true) ]
		static extern int GetKerningPairs( int hdc, int nNumPairs, [ In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1) ] KerningPair[] kerningPairs );

		[DllImport("Gdi32.dll", CharSet=CharSet.Unicode)] 
		static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj); 

		[DllImport("Gdi32.dll", CharSet=CharSet.Unicode)] 
		static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm); 

		[DllImport("Gdi32.dll", CharSet=CharSet.Unicode)] 
		static extern bool DeleteObject(IntPtr hdc); 

		[DllImport("Gdi32.dll", CharSet=CharSet.Unicode)] 
		static extern System.UInt32 GetTextColor( IntPtr hdc ); 

		[DllImport("Gdi32.dll", CharSet=CharSet.Unicode)] 
		static extern UInt32 SetTextColor( IntPtr hdc, UInt32 color ); 

		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		static extern bool TextOut(IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString);

		[DllImport("gdi32.dll")]
		static extern int SetBkMode(IntPtr hdc, int iBkMode);

		// Constants for SetBkMode
		const int TRANSPARENT = 1;
		const int OPAQUE = 2;	

		//
		// Internal support calls..
		//

		[ DllImport("gdi32.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true) ]
		static extern bool GetCharABCWidthsW(IntPtr hdc, uint uFirstChar, uint uLastChar, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeConst=1)] ABC[] lpabc);
	}
}


