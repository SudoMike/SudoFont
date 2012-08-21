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
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			_fontPreview.BackColor = Color.Black;
			_outputPreview.BackColor = Color.Black;

			_alphaOnlyControl.Checked = true;

			// Setup a default character set.
			ResetCharacterSet();

			// Fill up the fonts listbox.
			_allFonts = new InstalledFontCollection();
			foreach ( FontFamily family in _allFonts.Families )
			{
				if ( family.IsStyleAvailable( FontStyle.Regular ) )
					_fontsList.Items.Add( family.Name );
			}

			// Add sizes.
			_sizeCombo.Items.Add( 6 );
			_sizeCombo.Items.Add( 8 );
			_sizeCombo.Items.Add( 9 );
			_sizeCombo.Items.Add( 10 );
			_sizeCombo.Items.Add( 11 );
			_sizeCombo.Items.Add( 12 );
			_sizeCombo.Items.Add( 14 );
			_sizeCombo.Items.Add( 18 );
			_sizeCombo.Items.Add( 24 );
			_sizeCombo.Items.Add( 30 );
			_sizeCombo.Items.Add( 36 );
			_sizeCombo.Items.Add( 48 );
			_sizeCombo.Items.Add( 60 );
			_sizeCombo.Items.Add( 72 );

			_fontStyleControls = new FontStyleControl[4];
			_fontStyleControls[0] = new FontStyleControl() { Control = _boldOption, Style = FontStyle.Bold, ConfigFileKey = "IsBold" };
			_fontStyleControls[1] = new FontStyleControl() { Control = _italicOption, Style = FontStyle.Italic, ConfigFileKey = "IsItalic" };
			_fontStyleControls[2] = new FontStyleControl() { Control = _underlineOption, Style = FontStyle.Underline, ConfigFileKey = "IsUnderline" };
			_fontStyleControls[3] = new FontStyleControl() { Control = _strikeoutOption, Style = FontStyle.Strikeout, ConfigFileKey = "IsStrikeout" };

			_fontsList.SelectedItem = "Arial";
			_sizeCombo.SelectedItem = 12;

			_hintCombo.Items.Add( "ClearTypeGridFit" );
			_hintCombo.Items.Add( "SystemDefault" );
			_hintCombo.Items.Add( "SingleBitPerPixelGridFit" );
			_hintCombo.Items.Add( "SingleBitPerPixel" );
			_hintCombo.Items.Add( "AntiAliasGridFit" );
			_hintCombo.Items.Add( "AntiAlias" );
			_hintCombo.SelectedIndex = 0;
			
			UpdateColorDisplays();
			Recalculate();
		}

		string CurrentSelectedFontFamilyName
		{
			get
			{
				return _fontsList.SelectedItem.ToString();
			}

			set
			{
				_fontsList.SelectedItem = value;
			}
		}

		int CurrentComboBoxFontSize
		{
			get
			{
				try
				{
					return Convert.ToInt32( _sizeCombo.Text );
				}
				catch ( Exception )
				{
					return 12;
				}
			}

			set
			{
				_sizeCombo.Text = value.ToString();
			}
		}

		FontStyle GetFontStyleForFamily( FontFamily family )
		{
			FontStyle style = 0;

			if ( family.IsStyleAvailable( FontStyle.Regular ) )
				style = FontStyle.Regular;

			foreach ( var ctl in _fontStyleControls )
			{
				if ( ctl.Control.Checked && family.IsStyleAvailable( style & ctl.Style ) )
					style |= ctl.Style;
			}

			return style;
		}

		void Recalculate()
		{
			if ( _fontsList.SelectedItem == null || _sizeCombo.Text == "" )
				return;

			// Get the font family.
			string fontFamily = CurrentSelectedFontFamilyName;
			FontFamily family = new FontFamily( fontFamily );

			// Setup the FontStyle.
			FontStyle style = GetFontStyleForFamily( family );

			if ( family.IsStyleAvailable( FontStyle.Regular ) )
				style = FontStyle.Regular;

			foreach ( var ctl in _fontStyleControls )
			{
				if ( ctl.Control.Checked && family.IsStyleAvailable( style & ctl.Style ) )
					style |= ctl.Style;
			}

			// Figure out the size.
			int size = CurrentComboBoxFontSize;

			_currentFont = new Font( fontFamily, size, style );

			BuildPackedImage();

			if ( _packedImage != null )
			{
				int numRawBytes = ( _packedImage.Width * _packedImage.Height * 4 );
				int pngSize;
				using ( MemoryStream tempStream = new MemoryStream() )
				{
					_packedImage.Save( tempStream, ImageFormat.Png );
					pngSize = (int)tempStream.Length;
				}

				_outputImageSizeLabel.Text = string.Format( "{0} x {1}, RGBA size: {2}, PNG size: {3}", _packedImage.Width, _packedImage.Height, FormatSizeString( numRawBytes ), FormatSizeString( pngSize ) );
			}
			else
			{
				_outputImageSizeLabel.Text = "( error )";
			}

			_fontPreview.Invalidate();
		}

		string FormatSizeString( int numBytes )
		{
			if ( numBytes >= 1024*1024 )
				return string.Format( "{0:F2}M", numBytes / ( 1024.0 * 1024.0 ) );
			else
				return string.Format( "{0}k", numBytes / 1024 );
		}

		void SetTextRenderingHint( Graphics g )
		{
			string val = _hintCombo.Text;
			if ( val == "SystemDefault" )
				g.TextRenderingHint = TextRenderingHint.SystemDefault;
			else if ( val == "SingleBitPerPixelGridFit" )
				g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
			else if ( val == "SingleBitPerPixel" )
				g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
			else if ( val == "AntiAliasGridFit" )
				g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			else if ( val == "AntiAlias" )
				g.TextRenderingHint = TextRenderingHint.AntiAlias;
			else
				g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
		}

		void BuildPackedImage()
		{
			_packedImage = null;
			_outputPreview.Invalidate();

			// Remove any duplicates from the character set.
			string characterSet = new string( _characterSetControl.Text.Union( _characterSetControl.Text ).ToArray() );

			CharacterInfo[] infos = new CharacterInfo[ characterSet.Length ];

			Brush whiteBrush = new SolidBrush( Color.White );
			Brush blackBrush = new SolidBrush( Color.Black );
			
			// Get all the bitmap data and extents for the characters.
			using ( Bitmap tempBitmap = new Bitmap( 512, 512, PixelFormat.Format32bppArgb ) )
			{
				using ( Graphics g = Graphics.FromImage( tempBitmap ) )
				{
					SetTextRenderingHint( g );

					// Initialize it to black to start.
					g.Clear( Color.Black );

					// Initialize each CharacterInfo.
					for ( int i=0; i < characterSet.Length; i++ )
					{
						CharacterInfo c = new CharacterInfo();
						infos[i] = c;

						c.Character = characterSet[i];

						// Draw this character into tempBitmap.
						g.DrawString( c.Character.ToString(), _currentFont, whiteBrush, new Point( 0, 0 ) );

						// Scan to find the extents.
						Rectangle? startingExtentsNullable = GetMaximumCharacterExtents( g, _currentFont, c.Character );
						if ( !startingExtentsNullable.HasValue )
							return;

						// Make sure we're not accessing outside the bitmap itself. If this letter is larger than 512x512,
						// then we've got bigger problems elsewhere!
						Rectangle startingExtents = startingExtentsNullable.Value;
						startingExtents.Width = Math.Min( startingExtents.Width, tempBitmap.Width );
						startingExtents.Height = Math.Min( startingExtents.Height, tempBitmap.Height );

						Rectangle extents;
						ExtractImageData( tempBitmap, startingExtents, out c.Image, out extents );
						c.XOffset = extents.X;
						c.YOffset = extents.Y;
						c.PackedWidth = extents.Width;
						c.PackedHeight = extents.Height;

						g.FillRectangle( blackBrush, new Rectangle( 0, 0, extents.X + extents.Width, extents.Y + extents.Height ) );
					}
				}
			}

			// Figure out the max character size.
			Size maxSize = new System.Drawing.Size( 0, 0 );
			foreach ( var c in infos )
			{
				maxSize.Width  = Math.Max( maxSize.Width, c.PackedWidth );
				maxSize.Height = Math.Max( maxSize.Height, c.PackedHeight );
			}

			int maxImageDimension = 1024;

			// Now, pack all the characters in the output.
			// First, sort by height.
			CharacterInfo[] sorted = infos.OrderBy( x => -x.PackedHeight ).ToArray();
			_finalCharacterSet = sorted;

			// Now we can write the final index of each character.
			for ( int i=0; i < _finalCharacterSet.Length; i++ )
				_finalCharacterSet[i].SudoFontIndex = i;

			int packedWidth = 16;
			int packedHeight = 0;
			while ( packedWidth < maxImageDimension )
			{
				// We started out with a tiny width, so the height was super likely to be larger than the width.
				// If we finally get it where the width and height are equal, or where width is just one power of 
				// two larger, then we're happy.
				packedHeight = PackCharacters( sorted, packedWidth );
				if ( NextPowerOfTwo( packedWidth ) >= packedHeight )
					break;

				packedWidth <<= 1;
			}

			// Now render the final bitmap.
			_packedImage = new Bitmap( packedWidth, NextPowerOfTwo( packedHeight ) );
			RenderPackedImage( _packedImage, infos, _alphaOnlyControl.Checked );
		}

		void RenderPackedImage( Bitmap bitmap, CharacterInfo[] infos, bool alphaOnly )
		{
			using ( Graphics g = Graphics.FromImage( bitmap ) )
			{
				if ( alphaOnly )
					g.Clear( Color.FromArgb( 0 ) );
				else
					g.Clear( Color.Black );
			}

			BitmapData bm = bitmap.LockBits( new Rectangle( 0, 0, bitmap.Width, bitmap.Height ), ImageLockMode.WriteOnly, bitmap.PixelFormat );
			
			foreach ( CharacterInfo c in infos )
				CopyImageData( c.Image, c.PackedWidth, c.PackedHeight, bm, c.PackedX, c.PackedY, alphaOnly, c.YOffset );

			bitmap.UnlockBits( bm );
		}

		void CopyImageData( uint[] imageData, int imageWidth, int imageHeight, BitmapData dest, int destX, int destY, bool alphaOnly, int shadingYOffset )
		{
			unsafe
			{
				FontStyle style = GetFontStyleForFamily( _currentFont.FontFamily );
				int bottomPos = (int)( ( (float)_currentFont.FontFamily.GetCellAscent( style ) / _currentFont.FontFamily.GetEmHeight( style ) ) * _currentFont.Size );
				uint *pDestBase = (uint*)dest.Scan0;

				for ( int y=0; y < imageHeight; y++ )
				{
					// Figure out shading.
					int shadingY = y + shadingYOffset;
					int shadingR, shadingG, shadingB;
					
					if ( shadingY >= bottomPos )
					{
						shadingR = _bottomColor.R;
						shadingG = _bottomColor.G;
						shadingB = _bottomColor.B;
					}
					else
					{
						shadingR = _topColor.R + ( ( _bottomColor.R - _topColor.R ) * shadingY ) / bottomPos;
						shadingG = _topColor.G + ( ( _bottomColor.G - _topColor.G ) * shadingY ) / bottomPos;
						shadingB = _topColor.B + ( ( _bottomColor.B - _topColor.B ) * shadingY ) / bottomPos;
					}

					int outY = destY + y;
					if ( outY < 0 || outY > dest.Height )
						continue;

					for ( int x=0; x < imageWidth; x++ )
					{
						int outX = destX + x;
						if ( outX < 0 || outX > dest.Width )
							continue;

						UInt32 color = imageData[ y * imageWidth + x ];
						
						if ( alphaOnly )
						{
							// We have to guess at an alpha value here. We'll do that based on R, G, and B.
							uint r = ( color >> 16 ) & 0xFF;
							uint g = ( color >>  8 ) & 0xFF;
							uint b = ( color >>  0 ) & 0xFF;
							uint alpha = ( r + g + b ) / 3;
							color = 0xFFFFFF | ( alpha << 24 );
						}

						// Shading...
						if ( true )
						{
							uint sr = ( color >> 16 ) & 0xFF;
							uint sg = ( color >>  8 ) & 0xFF;
							uint sb = ( color >>  0 ) & 0xFF;
							uint sa = ( color & 0xFF000000 );

							sr = (uint)( ( (uint)sr * shadingR ) >> 8 );
							sg = (uint)( ( (uint)sg * shadingG ) >> 8 );
							sb = (uint)( ( (uint)sb * shadingB ) >> 8 );

							color = ( sr << 16 ) | ( sg << 8 ) | ( sb ) | sa;
						}

						pDestBase[ outY * ( dest.Stride >> 2 ) + outX ] = color;
					}
				}
			}
		}

		// We're assuming that the infos are sorted by height.
		// Returns the max Y value of any character.
		static int PackCharacters( CharacterInfo[] infos, int width, int extraPadX = 0, int extraPadY = 0 )
		{
			// Find runs..
			int firstChar = 0;
			int curY = 0;
			for ( int iRun=0; iRun < int.MaxValue; iRun++ )
			{
				if ( firstChar == infos.Length )
					break;

				// Figure out how many characters we can fit horizontally on this line.
				int lineWidth = infos[ firstChar ].PackedWidth;
				int lastChar;
				for ( lastChar=firstChar+1; lastChar < infos.Length; lastChar++ )
				{
					if ( lastChar != firstChar )
					{
						if ( ( lineWidth + extraPadX + infos[lastChar].PackedWidth ) > width )
							break;

						lineWidth += extraPadX;
					}
					
					lineWidth += infos[lastChar].PackedWidth;
				}

				int numCharsInLine = lastChar - firstChar;

				// Figure out the height of the line.
				int maxHeight = 0;
				for ( int i=0; i < numCharsInLine; i++ )
					maxHeight = Math.Max( maxHeight, infos[ firstChar + i ].PackedHeight );

				// Now just add each character in there.
				int curX = 0;
				for ( int i=0; i < numCharsInLine; i++ )
				{
					infos[ firstChar + i ].PackedX = curX;
					infos[ firstChar + i ].PackedY = curY;
					curX += infos[ firstChar + i ].PackedWidth + extraPadX;
				}

				firstChar += numCharsInLine;
				curY += maxHeight + extraPadY;
			}
			
			return curY;
		}

		static int NextPowerOfTwo( int i )
		{
			int ret = 1;
			
			while ( ret < i )
				ret <<= 1;

			return ret;
		}

		static void ExtractImageData( Bitmap bitmap, Rectangle startingExtents, out UInt32[] data, out Rectangle extents )
		{
			BitmapData bm = bitmap.LockBits( new Rectangle( 0, 0, bitmap.Width, bitmap.Height ), ImageLockMode.ReadOnly, bitmap.PixelFormat );

			// Scan for Y extents.
			int minY = int.MaxValue;
			int maxY = int.MinValue;
			for ( int y = startingExtents.Y; y < startingExtents.Y + startingExtents.Height; y++ )
			{
				if ( !IsRowEmpty( bm, y, startingExtents.X, startingExtents.Width ) )
				{
					minY = Math.Min( y, minY );
					maxY = Math.Max( y, maxY );
				}
			}
			
			// Scan for X extents.
			int minX = int.MaxValue;
			int maxX = int.MinValue;
			for ( int x = startingExtents.X; x < startingExtents.X + startingExtents.Width; x++ )
			{
				if ( !IsColumnEmpty( bm, x, startingExtents.Y, startingExtents.Height ) )
				{
					minX = Math.Min( x, minX );
					maxX = Math.Max( x, maxX );
				}
			}

			// Setup the extents rect.
			if ( minX == int.MaxValue || minY == int.MaxValue || maxX == int.MinValue || maxY == int.MinValue )
				extents = new Rectangle( startingExtents.X, startingExtents.Y, 0, 0 );
			else
				extents = new Rectangle( minX, minY, maxX - minX + 1, maxY - minY + 1 );

			// Copy the data to an output array.
			data = new uint[ extents.Width * extents.Height ];

			unsafe
			{
				uint *pSrc = (uint*)bm.Scan0;
				int uintStride = bm.Stride >> 2;

				for ( int y=0; y < extents.Height; y++ )
				{
					for ( int x=0; x < extents.Width; x++ )
					{
						data[ y * extents.Width + x ] = pSrc[ ( y + extents.Y ) * uintStride + ( x + extents.X ) ];
					}
				}
			}

			bitmap.UnlockBits( bm );
		}

		static bool IsRowEmpty( BitmapData bm, int row, int startX, int width )
		{
			unsafe
			{
				UInt32 *pCur = (UInt32*)bm.Scan0 + ( row * (bm.Stride>>2) ) + startX;
				UInt32 *pEnd = pCur + width;
				while ( pCur < pEnd )
				{
					if ( *pCur != 0xFF000000 )
						return false;

					++pCur;
				}
			}

			return true;
		}

		static bool IsColumnEmpty( BitmapData bm, int column, int startY, int height )
		{
			int strideInPixels = bm.Stride >> 2;

			unsafe
			{
				UInt32 *pCur = (UInt32*)bm.Scan0 + column;
				UInt32 *pEnd = pCur + height * strideInPixels;
				while ( pCur < pEnd )
				{
					if ( *pCur != 0xFF000000 )
						return false;

					pCur += strideInPixels;
				}
			}

			return true;
		}

		static Rectangle? GetMaximumCharacterExtents( Graphics g, Font font, Char ch )
		{
			SizeF size = g.MeasureString( ch.ToString(), font );
			return new Rectangle( 0, 0, (int)Math.Ceiling( size.Width ), (int)Math.Ceiling( size.Height ) );
		}

		private void _fontPreview_Paint( object sender, PaintEventArgs e )
		{
			if ( _currentFont == null )
				return;

			Graphics g = e.Graphics;
			SetTextRenderingHint( g );

			int curY = 0;
			foreach ( string line in _previewText.Split( new [] { '\n' } ) )
			{
				g.DrawString( line, _currentFont, new SolidBrush( Color.White ), new Point( 0, curY ) );
				curY += _currentFont.Height;
			}
		}

		private void _outputPreview_Paint( object sender, PaintEventArgs e )
		{
			if ( _packedImage == null )
				return;

			// Center it.
			Point pt = new Point()
			{
				X = ( _outputPreview.Width - _packedImage.Width ) / 2,
				Y = ( _outputPreview.Height - _packedImage.Height ) / 2
			};

			// Draw the preview image.
			Graphics g = e.Graphics;
			g.DrawImage( _packedImage, pt );

			// Draw a bounding rectangle.
			Rectangle outlineRect = new Rectangle( pt.X, pt.Y, _packedImage.Width, _packedImage.Height );
			outlineRect.Inflate( 2, 2 );
			g.DrawRectangle( new Pen( Color.Gray ), outlineRect );

			// _testBitmap can be set to draw test stuff on the display.
			// Remember to invalidate _outputPreview if you set it!
			if ( _testBitmap != null )
				g.DrawImage( _testBitmap, new Point( 5,5 ) );
		}

		private void _fontsList_SelectedIndexChanged( object sender, EventArgs e )
		{
			// Get the font family.
			string fontFamily = _fontsList.SelectedItem.ToString();
			FontFamily family = new FontFamily( fontFamily );

			// Figure out which styles are available.
			foreach ( var ctl in _fontStyleControls )
			{
				ctl.Control.Enabled = family.IsStyleAvailable( ctl.Style );
				ctl.Control.Checked &= ctl.Control.Enabled;	// Restrict the current settings to what's available.
			}
			
			Recalculate();
		}

		private void _boldOption_CheckedChanged( object sender, EventArgs e )
		{
			Recalculate();
		}

		private void _italicOption_CheckedChanged( object sender, EventArgs e )
		{
			Recalculate();
		}

		private void _underlineOption_CheckedChanged( object sender, EventArgs e )
		{
			Recalculate();
		}

		private void _strikeoutOption_CheckedChanged( object sender, EventArgs e )
		{
			Recalculate();
		}

		private void _sizeCombo_TextUpdate( object sender, EventArgs e )
		{
			Recalculate();
		}

		private void _sizeCombo_SelectedIndexChanged( object sender, EventArgs e )
		{
			Recalculate();
		}

		private void _characterSetControl_TextChanged( object sender, EventArgs e )
		{
			Recalculate();
		}

		private void _alphaOnlyControl_CheckedChanged( object sender, EventArgs e )
		{
			Recalculate();
		}

		private void _resetCharacterSetButton_Click( object sender, EventArgs e )
		{
			ResetCharacterSet();
		}

		void ResetCharacterSet()
		{
			string characterSetText = "";
			for ( int i=32; i < 127; i++ )
			{
				characterSetText += ( (Char)i ).ToString();
			}
			_characterSetControl.Text = characterSetText;
		}

		private void MainForm_Shown( object sender, EventArgs e )
		{
			// Default focus control is the fonts list.
			_fontsList.Focus();
		}

		private void embedHelp_Click( object sender, EventArgs e )
		{
			MessageBox.Show( "If you embed the configuration in the font file, then you don't need a separate configuration file. You can just open the (exported) font file in this application to load the configuration." );
		}

		private void openMenuItem_Click( object sender, EventArgs e )
		{
			OpenFileDialog dlg = new OpenFileDialog();

			dlg.InitialDirectory = Environment.CurrentDirectory;
			dlg.Filter = "Font Files (*.sfn)|*.sfn|All files (*.*)|*.*" ;
			dlg.FilterIndex = 0;
			dlg.InitialDirectory = _dialogsInitialDirectory;

			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				_dialogsInitialDirectory = Path.GetDirectoryName( dlg.FileName );

				try
				{
					// First, load the SudoFont.
					RuntimeFont loadedFont = new RuntimeFont();
					using ( Stream stream = File.OpenRead( dlg.FileName ) )
					{
						if ( !loadedFont.Load( new BinaryReader( stream ), keepConfigBlock: true ) )
							MessageBox.Show( "Invalid font" );

						// Then read the configuration out of it.
						MemoryStream configBlock = loadedFont.ConfigurationBlockStream;
						if ( ReadConfigurationFromStream( configBlock ) )
							_prevFontFilename = dlg.FileName;
						else
							MessageBox.Show( "Unable to read configuration block" );

						UpdateColorDisplays();
						Recalculate();
					}
				}
				catch ( Exception )
				{
					MessageBox.Show( "Error loading font" );
				}
			}
		}

		private void saveMenuItem_Click( object sender, EventArgs e )
		{
			if ( _prevFontFilename == null )
				saveAsMenuItem_Click( sender, e );	// Treat it as a Save-As
			else
				FontFile_Save();
		}

		private void saveAsMenuItem_Click( object sender, EventArgs e )
		{
			FontFile_SaveAs( ( _currentFont.Name + "-" + this.CurrentComboBoxFontSize.ToString() ).Replace( " ", "-" ) );
		}

		private void importConfigurationMenuItem_Click( object sender, EventArgs e )
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.InitialDirectory = Environment.CurrentDirectory;
			dlg.Filter = "Config Files (*.sfc)|*.sfc|All Files (*.*)|*.*";
			dlg.FilterIndex = 0;
			dlg.InitialDirectory = _dialogsInitialDirectory;

			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				_dialogsInitialDirectory = Path.GetDirectoryName( dlg.FileName );
				ImportConfiguration( dlg.FileName );
			}
		}

		bool ReadConfigurationFromStream( Stream stream )
		{
			using ( StreamReader reader = new StreamReader( stream ) )
			{
				if ( reader.ReadLine() != ConfigFilenameHeader )
					return OnLoadError( "Missing header" );

				Dictionary< string, string > options = new Dictionary<string,string>();
				int curLine = 0;
				while ( true )
				{
					++curLine;
					string line = reader.ReadLine();
					if ( line == null )
						break;

					int i = line.IndexOf( '=' );
					if ( i == -1 )
						return OnLoadError( "Invalid key/value format on line {0}", i );

					string key = line.Substring( 0, i - 1 );
					string value = line.Substring( i + 2 );
					options[key] = value;
				}

				try
				{
					// Read top and bottom colors.
					int topR = 255, topG = 255, topB = 255;
					if ( options.ContainsKey( ConfigFileKey_TopColorR ) ) topR = Convert.ToInt32( options[ ConfigFileKey_TopColorR ] );
					if ( options.ContainsKey( ConfigFileKey_TopColorG ) ) topG = Convert.ToInt32( options[ ConfigFileKey_TopColorG ] );
					if ( options.ContainsKey( ConfigFileKey_TopColorB ) ) topB = Convert.ToInt32( options[ ConfigFileKey_TopColorB ] );
					_topColor = Color.FromArgb( topR, topG, topB );

					int bottomR = 255, bottomG = 255, bottomB = 255;
					if ( options.ContainsKey( ConfigFileKey_BottomColorR ) ) bottomR = Convert.ToInt32( options[ ConfigFileKey_BottomColorR ] );
					if ( options.ContainsKey( ConfigFileKey_BottomColorG ) ) bottomG = Convert.ToInt32( options[ ConfigFileKey_BottomColorG ] );
					if ( options.ContainsKey( ConfigFileKey_BottomColorB ) ) bottomB = Convert.ToInt32( options[ ConfigFileKey_BottomColorB ] );
					_bottomColor = Color.FromArgb( bottomR, bottomG, bottomB );

					CurrentSelectedFontFamilyName = GetOption( options, ConfigFileKey_FontFamily );
					CurrentComboBoxFontSize = Convert.ToInt32( GetOption( options, ConfigFileKey_FontSize ) );

					foreach ( var ctl in _fontStyleControls )
						ctl.Control.Checked = Convert.ToBoolean( GetOption( options, ctl.ConfigFileKey ) );

					_alphaOnlyControl.Checked = Convert.ToBoolean( GetOption( options, ConfigFileKey_AlphaOnly ) );
					_embedConfigurationOption.Checked = Convert.ToBoolean( GetOption( options, ConfigFileKey_EmbedConfigInFontFile ) );
				}
				catch ( Exception )
				{
					return OnLoadError( "Error parsing configuration file" );
				}
			}

			return true;
		}

		bool ImportConfiguration( string filename )
		{
			using ( Stream stream = File.OpenRead( filename ) )
			{
				return ReadConfigurationFromStream( stream );
			}
		}

		string GetOption( Dictionary< string, string > options, string key )
		{
			return options[key];
		}

		bool OnLoadError( string error, params object[] args )
		{
			string str = string.Format( error, args );
			MessageBox.Show( str );
			return false;
		}

		private void exportConfigurationMenuItem_Click( object sender, EventArgs e )
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.InitialDirectory = Environment.CurrentDirectory;
			dlg.Filter = "Config Files (*.sfc)|*.sfc|All Files (*.*)|*.*";
			dlg.FilterIndex = 0;
			dlg.InitialDirectory = _dialogsInitialDirectory;

			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				_dialogsInitialDirectory = Path.GetDirectoryName( dlg.FileName );

				using ( StreamWriter writer = new StreamWriter( dlg.FileName ) )
				{
					ExportConfiguration( writer );
				}
			}
		}

		void ExportConfiguration( StreamWriter writer )
		{
			writer.WriteLine( ConfigFilenameHeader );
			WriteOption( writer, ConfigFileKey_FontFamily, CurrentSelectedFontFamilyName );
			WriteOption( writer, ConfigFileKey_FontSize, CurrentComboBoxFontSize );

			foreach ( var ctl in _fontStyleControls )
				WriteOption( writer, ctl.ConfigFileKey, ctl.Control.Checked );

			WriteOption( writer, ConfigFileKey_AlphaOnly, _alphaOnlyControl.Checked );
			WriteOption( writer, ConfigFileKey_EmbedConfigInFontFile, _embedConfigurationOption.Checked );
			
			WriteOption( writer, ConfigFileKey_TopColorR, _topColor.R );
			WriteOption( writer, ConfigFileKey_TopColorG, _topColor.G );
			WriteOption( writer, ConfigFileKey_TopColorB, _topColor.B );

			WriteOption( writer, ConfigFileKey_BottomColorR, _bottomColor.R );
			WriteOption( writer, ConfigFileKey_BottomColorG, _bottomColor.G );
			WriteOption( writer, ConfigFileKey_BottomColorB, _bottomColor.B );
		}

		void WriteOption( StreamWriter writer, string optionName, string value )
		{
			writer.WriteLine( string.Format( "{0} = {1}", optionName, value ) );
		}

		void WriteOption( StreamWriter writer, string optionName, int value )
		{
			writer.WriteLine( string.Format( "{0} = {1}", optionName, value ) );
		}

		void WriteOption( StreamWriter writer, string optionName, bool value )
		{
			writer.WriteLine( string.Format( "{0} = {1}", optionName, value ) );
		}

		private void exitMenuItem_Click( object sender, EventArgs e )
		{
			Close();
		}

		void FontFile_Save()
		{
			if ( _prevFontFilename == null )
			{
				FontFile_SaveAs();
				return;
			}

			// Calculate kernings. This is expensive.
			List< CharacterKerningInfo > kernings = new List<CharacterKerningInfo>();
			using ( Graphics g = CreateGraphics() )
			{
				for ( int i=0; i < _finalCharacterSet.Length; i++ )
				{
					CalculateSpacingInfo( g, _finalCharacterSet[i], kernings );
				}
			}

			// Save out the .sfn file.
			using ( Stream outStream = File.Open( _prevFontFilename, FileMode.Create, FileAccess.Write ) )
			{
				using ( BinaryWriter writer = new BinaryWriter( outStream ) )
				{
					writer.Write( RuntimeFont.FontFileHeader );

					WriteFontInfoSection( writer );
					WriteFontCharactersSection( writer );
					WriteFontKerningSection( writer, kernings );
					WriteFontConfigSection( writer );

					writer.Write( RuntimeFont.FontFile_Section_Finished );
				}
			}
			
			// Save out the corresponding PNG file.
			_packedImage.Save( Path.ChangeExtension( _prevFontFilename, null ) + "-texture.png" );
			
			// Useful for verifying the saving, loading, and rendering/spacing.
			//SetTestBitmap( SudoFontTest.CreateBitmapFromString( _prevFontFilename, "This is a test string. !@#$%^&*", 0, 0, _currentFont, win32APITest: true ) );
		}

		void SetTestBitmap( Bitmap bitmap )
		{
			_testBitmap = bitmap;
			_outputPreview.Invalidate();
		}

		// Return the RectangleF for each character in the string.
		static RectangleF[] MeasureAllCharacterRanges( Graphics g, string str, Font font )
		{
			StringFormat testFormat = new StringFormat();

			CharacterRange[] ranges = new CharacterRange[ str.Length ];
			for ( int i=0; i < str.Length; i++ )
				ranges[i] = new CharacterRange( i, 1 );

			testFormat.SetMeasurableCharacterRanges( ranges );
			Region[] regions = g.MeasureCharacterRanges( str, font, new Rectangle( 0, 0, 1000, 1000 ), testFormat );
			RectangleF[] rects = regions.Select( x => x.GetBounds(g) ).ToArray();
			return rects;
		}

		void WriteFontInfoSection( BinaryWriter writer )
		{
			using ( SectionWriter sectionWriter = new SectionWriter( writer, RuntimeFont.FontFile_Section_FontInfo ) )
			{
				using ( Graphics g = CreateGraphics() )
				{
					float heightInPixels;

					if ( _currentFont.Unit == GraphicsUnit.Point )
					{
						float heightInPoints = _currentFont.GetHeight();
						float heightInInches = heightInPoints / 72.0f;
						heightInPixels = heightInInches * g.DpiY;
						writer.Write( (short)heightInPixels ); // Font height.
					}
					else if ( _currentFont.Unit == GraphicsUnit.Pixel )
					{
						heightInPixels = _currentFont.GetHeight();
						writer.Write( (short)heightInPixels ); // Font height.
					}
					else
					{
						Debug.Assert( false );
						writer.Write( (short)5 ); // Font height.
					}
				}
			}
		}

		// This is how we calculate XAdvance and find kerning. Normally, we'd use GetCharABCWidths and GetKerningPairs,
		// but we weren't able to get the spacing to match Graphics.DrawString properly using that data.
		//
		// This method is inefficient, but it's highly accurate because it deduces the correct spacing information 
		// directly from GraphicsMeasureCharacterRanges.
		//
		// It also happens to generate a lot less kerning pairs than GetKerningPairs does (even after reducing down to
		// our limited character set here).
		void CalculateSpacingInfo( Graphics g, CharacterInfo c, List< CharacterKerningInfo > kernings )
		{
			int[] distances = new int[ _finalCharacterSet.Length ];
			int minDist = int.MaxValue;
			int maxDist = int.MinValue;

			int averageDist = 0;
			for ( int i=0; i < _finalCharacterSet.Length; i++ )
			{
				// We pad with spaces on the sides because this returns different spacing numbers for characters on the edges of the string. Go figure..
				RectangleF[] rects = MeasureAllCharacterRanges( g, " " + c.Character.ToString() + _finalCharacterSet[i].Character + " ", _currentFont );
				int dist = (int)( rects[2].X - rects[1].X );
				minDist = Math.Min( dist, minDist );
				maxDist = Math.Max( dist, maxDist );
				
				distances[i] = dist;
				averageDist += dist;
			}

			// By setting XAdvance to the (rounded) average distance to the other characters, we're intending to 
			// reduce the # of kerning pairs necessary. This tends to work well, usually returning 1 kerning pair 
			// for most characters (that have kerning at all) in a font.
			averageDist = (int)( (float)averageDist / _finalCharacterSet.Length + 0.5f );
			c.XAdvance = averageDist;

			if ( minDist != maxDist )
			{
				CharacterKerningInfo info = new CharacterKerningInfo();
				info.Character = c;
				info.Kernings = new List<CharacterKerningInfo.KerningPair>();
				for ( int i=0; i < distances.Length; i++ )
				{
					if ( distances[i] != c.XAdvance )
					{
						info.Kernings.Add(
							new CharacterKerningInfo.KerningPair()
							{
								SecondCharacter = _finalCharacterSet[i],
								KernAmount = distances[i] - c.XAdvance
							}
						);
					}
				}

				kernings.Add( info );
			}
		}


		void WriteFontCharactersSection( BinaryWriter writer )
		{
			using ( SectionWriter sectionWriter = new SectionWriter( writer, RuntimeFont.FontFile_Section_Characters ) )
			{
				// Write the # of characters.
				writer.Write( (short)_finalCharacterSet.Length );

				for ( int i=0; i < _finalCharacterSet.Length; i++ )
				{
					CharacterInfo c = _finalCharacterSet[i];

					writer.Write( (short)c.Character );

					// Write its location in the packed image.
					writer.Write( (short)c.PackedX );
					writer.Write( (short)c.PackedY );
					writer.Write( (short)c.PackedWidth );
					writer.Write( (short)c.PackedHeight );

					// Write its placement offset (i.e. if you were to print this packed letter at (0,0), how offsetted would it be?)
					writer.Write( (short)c.XOffset );
					writer.Write( (short)c.YOffset );

					// How far we advance X to get to the next character.
					writer.Write( (short)c.XAdvance );
				}
			}
		}

		void WriteFontKerningSection( BinaryWriter writer, List< CharacterKerningInfo > kernings )
		{
			using ( SectionWriter sectionWriter = new SectionWriter( writer, RuntimeFont.FontFile_Section_Kerning ) )
			{
				using ( Graphics g = CreateGraphics() )
				{
					FontServices.KerningPair[] kerningPairs = FontServices.GetKerningPairs( _currentFont, g );

					// First, figure out how many characters have kerning.
					// Write the # of characters that have kerning.
					writer.Write( (Int16)kernings.Count );

					// For each one, write its kerning list.
					foreach ( CharacterKerningInfo info in kernings )
					{
						// Write which character this is.
						writer.Write( (Int16)info.Character.SudoFontIndex );

						// Write its list of kernings.
						writer.Write( (Int16)info.Kernings.Count );
						for ( int i=0; i < info.Kernings.Count; i++ )
						{
							writer.Write( (Int16)info.Kernings[i].SecondCharacter.SudoFontIndex );
							writer.Write( (Int16)info.Kernings[i].KernAmount );
						}
					}
				}
			}
		}

		void WriteFontConfigSection( BinaryWriter writer )
		{
			byte[] bytes;

			// First, write the configuration into a MemoryStream, then into our byte[] array.
			using ( MemoryStream memStream = new MemoryStream() )
			{
				using ( StreamWriter memStreamWriter = new StreamWriter( memStream ) )
				{
					ExportConfiguration( memStreamWriter );
					memStreamWriter.Flush();

					memStream.Position = 0;
					bytes = new byte[ memStream.Length ];
					memStream.Read( bytes, 0, bytes.Length );
				}
			}

			// Now write it to the section.
			using ( SectionWriter sectionWriter = new SectionWriter( writer, RuntimeFont.FontFile_Section_Config ) )
			{
				writer.Write( bytes );
			}
		}
		

		int CountKerningsWithFirstChar( FontServices.KerningPair[] kerningPairs, Char ch )
		{
			int total = 0;
			
			for ( int i=0; i < kerningPairs.Length; i++ )
			{
				if ( kerningPairs[i].wFirst == (Int16)ch )
					++total;
			}

			return total;
		}

		void FontFile_SaveAs( string defaultName=null )
		{
			// Find out how they want to save the font file.
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.InitialDirectory = Environment.CurrentDirectory;
			dlg.Filter = "Font Files (*.sfn)|*.sfn|All Files (*.*)|*.*";
			dlg.FilterIndex = 0;
			dlg.FileName = defaultName;
			dlg.InitialDirectory = _dialogsInitialDirectory;

			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				_dialogsInitialDirectory = Path.GetDirectoryName( dlg.FileName );
				_prevFontFilename = dlg.FileName;

				FontFile_Save();
			}
		}
		
		private void hintCombo_SelectedIndexChanged( object sender, EventArgs e )
		{
			Recalculate();
		}

		// This is our main data about each character in the _packedImage font.
		[DebuggerDisplay( "Char: {Character}, {Width} x {Height}, Packed: ({PackedX}, {PackedY})" )]
		class CharacterInfo
		{
			public Char Character;
			
			// If you print the character into a Graphics at (0,0), this will be where the actual nonzero data is.
			public int XOffset;
			public int YOffset;
			public int PackedWidth;
			public int PackedHeight;

			// Where this character is in the packed texture.
			public int PackedX;
			public int PackedY;

			// How far to advance on X when printing this char.
			// The total amount to advance for each char is (XAdvance + Font.SpacingHorz + Kerning( thisChar, nextChar )).
			public int XAdvance;
			
			// This is the original image data after Graphics.DrawString.
			// Dimensions are Width x Height.
			public UInt32[] Image;

			// This is the index into the font's character array.
			// This is not written to the font file.
			public int SudoFontIndex;
		}

		// We have one instance of this (in _fontStyleControls) for each bold/italic/strikeout/underline control.
		struct FontStyleControl
		{
			public CheckBox Control;
			public FontStyle Style;
			public string ConfigFileKey;
		}

		// This makes it easy to write a section with a length at the beginning.
		// When you dispose of it, it'll go back and write the length in.
		// A section looks like:
		//		[int16] section_ID
		//		[int32] section_length
		//		(section_length's worth of data)
		class SectionWriter : IDisposable
		{
			public SectionWriter( BinaryWriter writer, short sectionID )
			{
				_writer = writer;

				_writer.Write( sectionID );
				_writer.Flush();

				_sectionStartPosition = _writer.BaseStream.Position;
				_writer.Write( (int)0 );	// Write a placeholder for section length.
				_writer = writer;
			}

			public void Dispose()
			{
				_writer.Flush();
				
				long numSectionBytes = _writer.BaseStream.Position - _sectionStartPosition - sizeof( int );
				Debug.Assert( numSectionBytes > 0 );

				_writer.BaseStream.Position = _sectionStartPosition;
				_writer.Write( (int)numSectionBytes );
				_writer.Flush();
				_writer.BaseStream.Position += numSectionBytes;
			}

			BinaryWriter _writer;
			long _sectionStartPosition;
		}

		// We use this to figure out the kerning for a single CharacterInfo, based on the Win32 KerningPairs.
		[ DebuggerDisplay( "Char: {Character.Character}, Count: {Kernings.Count}" ) ]
		class CharacterKerningInfo
		{
			public CharacterKerningInfo()
			{
			}

			//
			// NOTE: This is currently unused. This would seem to be the right way to do it,
			//       but the results that we get back don't match what you get if you call Graphics.DrawString.
			//       Instead, see CalculateSpacingInfo.
			//
			public CharacterKerningInfo( CharacterInfo character, FontServices.KerningPair[] kerningPairs, CharacterInfo[] validCharacters )
			{
				this.Character = character;

				// Add a kerning pair for anything that we're the first character in (and has a valid second character).
				for ( int i=0; i < kerningPairs.Length; i++ )
				{
					if ( kerningPairs[i].wFirst == (short)this.Character.Character )
					{
						CharacterInfo second = FindCharacter( (Char)kerningPairs[i].wSecond, validCharacters );	// Find the second kerning character in validCharacters.
						if ( second != null )
						{
							KerningPair pair = new KerningPair()
							{
								SecondCharacter = second,
								KernAmount = kerningPairs[i].iKernAmount
							};

							this.Kernings.Add( pair );
						}
					}
				}
			}

			CharacterInfo FindCharacter( Char ch, CharacterInfo[] validCharacters )
			{
				foreach ( var c in validCharacters )
				{
					if ( c.Character == ch )
						return c;
				}
				
				return null;
			}

			[ DebuggerDisplay( "SecondChar: {SecondCharacter.Character}, KernAmount: {KernAmount}" ) ]
			public struct KerningPair
			{
				public CharacterInfo SecondCharacter;
				public int KernAmount;
			}

			public CharacterInfo Character;
			public List< KerningPair > Kernings = new List<KerningPair>();
		}


		private void _topColorDisplay_Click( object sender, EventArgs e )
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _topColor;

			if ( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{
				_topColor = dlg.Color;
				UpdateColorDisplays();
				Recalculate();
			}
		}

		private void _bottomColorDisplay_Click( object sender, EventArgs e )
		{
			ColorDialog dlg = new ColorDialog();
			dlg.Color = _bottomColor;

			if ( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{
				_bottomColor = dlg.Color;
				UpdateColorDisplays();
				Recalculate();
			}
		}

		void UpdateColorDisplays()
		{
			_topColorDisplay.BackColor = _topColor;
			_bottomColorDisplay.BackColor = _bottomColor;
		}

	
		// Config file keys.
		static readonly string ConfigFilenameHeader = "SudoFont Font Configuration File v1.0";
		static readonly string ConfigFileKey_FontFamily = "FontFamily";
		static readonly string ConfigFileKey_FontSize = "FontSize";
		static readonly string ConfigFileKey_AlphaOnly = "IsAlphaOnly";
		static readonly string ConfigFileKey_EmbedConfigInFontFile = "EmbedConfigInFontFile";

		static readonly string ConfigFileKey_TopColorR = "TopR";
		static readonly string ConfigFileKey_TopColorG = "TopG";
		static readonly string ConfigFileKey_TopColorB = "TopB";

		static readonly string ConfigFileKey_BottomColorR = "BtmR";
		static readonly string ConfigFileKey_BottomColorG = "BtmG";
		static readonly string ConfigFileKey_BottomColorB = "BtmB";

		string _prevFontFilename = null;

		string _previewText = "0123456789 _*+- ()[]#@\nABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz";

		FontStyleControl[] _fontStyleControls;
		Bitmap _packedImage;

		// This is what's baked into the current packedImage texture.
		CharacterInfo[] _finalCharacterSet;

		Font _currentFont;
		InstalledFontCollection _allFonts;

		// This preserves the same directory that we'll create open and save dialogs at.
		string _dialogsInitialDirectory;

		// Used for testing. If you set this, it'll render the bitmap into _outputPreview.
		// Use SetTestBitmap to set this so it'll invalidate _outputPreview!
		Bitmap _testBitmap;

		Color _topColor = Color.White;
		Color _bottomColor = Color.White;
	}
}
