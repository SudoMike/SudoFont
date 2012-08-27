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


namespace SudoFont
{
	class DotNetFontSystem : IFontSystem
	{
		public DotNetFontSystem()
		{
			_installedFontCollection = new InstalledFontCollection();
			foreach ( FontFamily family in _installedFontCollection.Families )
			{
				_fontFamilies.Add( new DotNetFontFamily( family ) );
			}
		}

		public int NumFontFamilies 
		{ 
			get { return _fontFamilies.Count; }
		}

		public IFontFamily GetFontFamily( int iFamily )
		{
			return _fontFamilies[iFamily];
		}

		public IFontFamily GetFontFamilyByName( string name )
		{
			foreach ( DotNetFontFamily family in _fontFamilies )
			{
				if ( family.Name == name )
					return family;
			}
			return null;
		}

		public IFont CreateFont( string familyName, int size, FontStyle style, TextRenderingHint renderingHint )
		{
			Font font = new Font( familyName, size, style );
			return new DotNetFont( font, renderingHint );
		}

		InstalledFontCollection _installedFontCollection;
		List< DotNetFontFamily > _fontFamilies = new List<DotNetFontFamily>();
	}

	class DotNetFontFamily : IFontFamily
	{
		public DotNetFontFamily( FontFamily fontFamily )
		{
			_fontFamily = fontFamily;
		}

		public string Name
		{
			get
			{
				return _fontFamily.Name;
			}
		}

		public bool IsStyleAvailable( FontStyle style )
		{
			return _fontFamily.IsStyleAvailable( style );
		}

		FontFamily _fontFamily;
	}

	class DotNetFont : IFont
	{
		public DotNetFont( Font font, TextRenderingHint textRenderingHint )
		{
			_font = font;
			_textRenderingHint = textRenderingHint;
		}

		public string Name
		{
			get
			{
				return _font.Name;
			}
		}

		public void DrawString( Graphics g, string str, Brush brush, Point location )
		{
			TextRenderingHint prevTextRenderingHint = g.TextRenderingHint;
			g.TextRenderingHint = _textRenderingHint;

			g.DrawString( str, _font, brush, location );

			g.TextRenderingHint = prevTextRenderingHint;
		}
		
		public float[] GetCharacterXPositions( Graphics g, string str )
		{
			// Setup the StringFormat with proper CharacterRange references.
			StringFormat testFormat = new StringFormat();
			CharacterRange[] ranges = new CharacterRange[ str.Length ];
			for ( int i=0; i < str.Length; i++ )
				ranges[i] = new CharacterRange( i, 1 );

			testFormat.SetMeasurableCharacterRanges( ranges );

			// Measure into Regions
			Region[] regions = g.MeasureCharacterRanges( str, _font, new Rectangle( 0, 0, 1000, 1000 ), testFormat );

			// Convert Regions to Rects, then X coords.
			float[] xCoords = regions.Select( region => region.GetBounds( g ).X ).ToArray();
			return xCoords;
		}

		public SizeF MeasureString( Graphics g, string str )
		{
			return g.MeasureString( str, _font );
		}

		public float GetHeightInPixels( Control control )
		{
			float heightInPixels;

			if ( _font.Unit == GraphicsUnit.Point )
			{
				float heightInPoints = _font.GetHeight();
				float heightInInches = heightInPoints / 72.0f;
			
				using ( Graphics g = control.CreateGraphics() )
				{
					heightInPixels = heightInInches * g.DpiY;
				}
			}
			else if ( _font.Unit == GraphicsUnit.Pixel )
			{
				heightInPixels = _font.GetHeight();
			}
			else
			{
				Debug.Assert( false );
				heightInPixels = 5;
			}

			return heightInPixels;
		}

		public float GetBaselinePos( Graphics g, FontStyle style )
		{
			return ( ( (float)_font.FontFamily.GetCellAscent( style ) / _font.FontFamily.GetEmHeight( style ) ) * _font.Size );
		}

		public IFontFamily FontFamily 
		{ 
			get
			{
				if ( _fontFamily == null )
					_fontFamily = new DotNetFontFamily( _font.FontFamily );

				return _fontFamily;
			}
		}

		IFontFamily _fontFamily;
		Font _font;
		TextRenderingHint _textRenderingHint;
	}
}

