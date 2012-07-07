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

		void Recalculate()
		{
			if ( _fontsList.SelectedItem == null || _sizeCombo.Text == "" )
				return;

			// Get the font family.
			string fontFamily = CurrentSelectedFontFamilyName;
			FontFamily family = new FontFamily( fontFamily );

			// Setup the FontStyle.
			FontStyle style = 0;

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

		[DebuggerDisplay( "Char: {Character}, {Width} x {Height}, Packed: ({PackedX}, {PackedY})" )]
		class CharacterInfo
		{
			public Char Character;
			
			// If you print the character into a Graphics at (0,0), this will be where the actual nonzero data is.
			public int XOffset;
			public int YOffset;
			public int Width;
			public int Height;

			// Where this character is in the packed texture.
			public int PackedX;
			public int PackedY;
			
			// This is the original image data after Graphics.DrawString.
			// Dimensions are Width x Height.
			public UInt32[] Image;
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
						c.Width = extents.Width;
						c.Height = extents.Height;

						g.FillRectangle( blackBrush, new Rectangle( 0, 0, extents.X + extents.Width, extents.Y + extents.Height ) );
					}
				}
			}

			// Figure out the max character size.
			Size maxSize = new System.Drawing.Size( 0, 0 );
			foreach ( var c in infos )
			{
				maxSize.Width  = Math.Max( maxSize.Width, c.Width );
				maxSize.Height = Math.Max( maxSize.Height, c.Height );
			}

			int maxImageDimension = 1024;

			// Now, pack all the characters in the output.
			// First, sort by height.
			CharacterInfo[] sorted = infos.OrderBy( x => -x.Height ).ToArray();
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

		static void RenderPackedImage( Bitmap bitmap, CharacterInfo[] infos, bool alphaOnly )
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
				CopyImageData( c.Image, c.Width, c.Height, bm, c.PackedX, c.PackedY, alphaOnly );

			bitmap.UnlockBits( bm );
		}

		static void CopyImageData( uint[] imageData, int imageWidth, int imageHeight, BitmapData dest, int destX, int destY, bool alphaOnly )
		{
			unsafe
			{
				uint *pDestBase = (uint*)dest.Scan0;

				for ( int y=0; y < imageHeight; y++ )
				{
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
				int lineWidth = infos[ firstChar ].Width;
				int lastChar;
				for ( lastChar=firstChar+1; lastChar < infos.Length; lastChar++ )
				{
					if ( lastChar != firstChar )
					{
						if ( ( lineWidth + extraPadX + infos[lastChar].Width ) > width )
							break;

						lineWidth += extraPadX;
					}
					
					lineWidth += infos[lastChar].Width;
				}

				int numCharsInLine = lastChar - firstChar;

				// Figure out the height of the line.
				int maxHeight = 0;
				for ( int i=0; i < numCharsInLine; i++ )
					maxHeight = Math.Max( maxHeight, infos[ firstChar + i ].Height );

				// Now just add each character in there.
				int curX = 0;
				for ( int i=0; i < numCharsInLine; i++ )
				{
					infos[ firstChar + i ].PackedX = curX;
					infos[ firstChar + i ].PackedY = curY;
					curX += infos[ firstChar + i ].Width + extraPadX;
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
			for ( int i=32; i <= 127; i++ )
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
			OpenFileDialog ofd = new OpenFileDialog();

			ofd.InitialDirectory = Environment.CurrentDirectory;
			ofd.Filter = "Configuration Files (*.sfc)|*.sfc|Font Files (*.sf)|*.sf|All files (*.*)|*.*" ;
			ofd.FilterIndex = 0;

			if ( ofd.ShowDialog() == DialogResult.OK )
			{
			}
		}

		private void saveMenuItem_Click( object sender, EventArgs e )
		{
			if ( _prevFontFilename == null )
				FontFile_SaveAs();
			else
				FontFile_Save();
		}

		private void saveAsMenuItem_Click( object sender, EventArgs e )
		{
			FontFile_SaveAs();
		}

		private void importConfigurationMenuItem_Click( object sender, EventArgs e )
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.InitialDirectory = Environment.CurrentDirectory;
			dlg.Filter = "Config Files (*.sfc)|*.sfc|All Files (*.*)|*.*";
			dlg.FilterIndex = 0;

			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				ImportConfiguration( dlg.FileName );
			}
		}

		bool ImportConfiguration( string filename )
		{
			using ( StreamReader reader = new StreamReader( filename ) )
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

			_prevConfigFilename = filename;
			return true;
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

			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				using ( StreamWriter writer = new StreamWriter( dlg.FileName ) )
				{
					writer.WriteLine( ConfigFilenameHeader );
					WriteOption( writer, ConfigFileKey_FontFamily, CurrentSelectedFontFamilyName );
					WriteOption( writer, ConfigFileKey_FontSize, CurrentComboBoxFontSize );

					foreach ( var ctl in _fontStyleControls )
						WriteOption( writer, ctl.ConfigFileKey, ctl.Control.Checked );

					WriteOption( writer, ConfigFileKey_AlphaOnly, _alphaOnlyControl.Checked );
					WriteOption( writer, ConfigFileKey_EmbedConfigInFontFile, _embedConfigurationOption.Checked );
				}

				_prevConfigFilename = dlg.FileName;
			}
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
		}

		void FontFile_SaveAs()
		{
			// Find out how they want to save the font file.
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.InitialDirectory = Environment.CurrentDirectory;
			dlg.Filter = "Font Files (*.sfn)|*.sfn|All Files (*.*)|*.*";
			dlg.FilterIndex = 0;

			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				_prevConfigFilename = dlg.FileName;
				FontFile_Save();
			}
		}


		static readonly string ConfigFilenameHeader = "SudoFont Font Configuration File v1.0";
		static readonly string ConfigFileKey_FontFamily = "FontFamily";
		static readonly string ConfigFileKey_FontSize = "FontSize";
		static readonly string ConfigFileKey_AlphaOnly = "IsAlphaOnly";
		static readonly string ConfigFileKey_EmbedConfigInFontFile = "EmbedConfigInFontFile";

		string _prevFontFilename = null;
		string _prevConfigFilename = null;

		string _previewText = "0123456789 _*+- ()[]#@\nABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz";

		struct FontStyleControl
		{
			public CheckBox Control;
			public FontStyle Style;
			public string ConfigFileKey;
		}

		FontStyleControl[] _fontStyleControls;
		Bitmap _packedImage;

		Font _currentFont;
		InstalledFontCollection _allFonts;
	}
}
