
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
		public static Bitmap CreateBitmapFromString( string fontFilename, string testString, int startX, int startY, Font comparisonFont=null, bool win32APITest=false )
		{
			try
			{
				// Open the font file.
				using ( BinaryReader reader = new BinaryReader( File.OpenRead( fontFilename ) ) )
				{
					// Load the font.
					RuntimeFont font = new RuntimeFont();
					if ( !font.Load( reader ) )
						return null;

					// Load the associated PNG file.
					Bitmap pngBitmap = new Bitmap( Path.ChangeExtension( fontFilename, "png" ) );

					// Get the character positions.
					FontLayout.CharacterLayout[] layouts = FontLayout.LayoutCharacters( font, testString, 0, 0 );

					// Draw the character.
					int extraLineSpacing = 5;

					int width = layouts[ layouts.Length - 1 ].XOffset + font.LineHeight * 2;
					int height = layouts[ layouts.Length - 1 ].YOffset + font.LineHeight;
					if ( comparisonFont != null )
					{
						height += font.LineHeight + extraLineSpacing;

						if ( win32APITest )
							height += font.LineHeight + extraLineSpacing;
					}

					Color textColor = Color.White;

					Bitmap bitmap = new Bitmap( width, height );
					using ( Graphics g = Graphics.FromImage( bitmap ) )
					{
						g.Clear( Color.Black );

						foreach ( FontLayout.CharacterLayout layout in layouts )
						{
							int sfi = layout.SudoFontIndex;
							Rectangle srcRect = new Rectangle( font.Characters[sfi].PackedX, font.Characters[sfi].PackedY, font.Characters[sfi].PackedWidth, font.Characters[sfi].PackedHeight );
							Rectangle destRect = new Rectangle( layout.XOffset, layout.YOffset, srcRect.Width, srcRect.Height );
							g.DrawImage( pngBitmap, destRect, srcRect, GraphicsUnit.Pixel );
						}

						int curY = startY + font.LineHeight + extraLineSpacing;
						if ( comparisonFont != null )
						{
							g.DrawString( testString, comparisonFont, new SolidBrush( textColor ), new Point( 0, curY ) );
							curY += font.LineHeight + extraLineSpacing;

							if ( win32APITest )
								FontServices.TextOut( g, comparisonFont, textColor, 3, curY, testString );
						}
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

