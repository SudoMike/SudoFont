
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace SudoFont
{
	// This draws a string to a Bitmap.
	public class SudoFontTest
	{
		// This is a test that creates a bitmap using a SudoFont.
		// This can be used as reference for code that uses SudoFonts to rasterize text.
		// If you set comparisonFont, it'll render the same string into the bitmap under the SudoFont output.
		// This is useful for verifying/checking how close SudoFonts compare to .NET Graphics text output.
		public static Bitmap CreateBitmapFromString( string fontFilename, string testString, int startX, int startY, Font comparisonFont=null )
		{
			try
			{
				// Open the font file.
				using ( BinaryReader reader = new BinaryReader( File.OpenRead( fontFilename ) ) )
				{
					// Load the font.
					SudoFont font = new SudoFont();
					if ( !font.Load( reader ) )
						return null;

					// Load the associated PNG file.
					Bitmap pngBitmap = new Bitmap( Path.ChangeExtension( fontFilename, "png" ) );

					// Get the character positions.
					SudoFontLayout.CharacterLayout[] layouts = SudoFontLayout.LayoutCharacters( font, testString, 0, 0 );

					// Draw the character.
					int width = layouts[ layouts.Length - 1 ].XOffset + font.LineHeight * 2;
					int height = layouts[ layouts.Length - 1 ].YOffset + font.LineHeight;
					if ( comparisonFont != null )
						height += font.LineHeight + 5;

					Bitmap bitmap = new Bitmap( width, height );
					using ( Graphics g = Graphics.FromImage( bitmap ) )
					{
						g.Clear( Color.Black );

						foreach ( SudoFontLayout.CharacterLayout layout in layouts )
						{
							int sfi = layout.SudoFontIndex;
							Rectangle srcRect = new Rectangle( font.Characters[sfi].PackedX, font.Characters[sfi].PackedY, font.Characters[sfi].PackedWidth, font.Characters[sfi].PackedHeight );
							Rectangle destRect = new Rectangle( layout.XOffset, layout.YOffset, srcRect.Width, srcRect.Height );
							g.DrawImage( pngBitmap, destRect, srcRect, GraphicsUnit.Pixel );
						}

						if ( comparisonFont != null )
							g.DrawString( testString, comparisonFont, new SolidBrush( Color.White ), new Point( 0, startY + font.LineHeight + 5 ) );
					}

					return bitmap;
				}
			}
			catch ( Exception )
			{
				return null;
			}
		}
	}
}

