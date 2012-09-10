namespace SudoFont
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.Label _sizeLabel;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.GroupBox groupBox3;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Button embedHelp;
			System.Windows.Forms.Label label7;
			System.Windows.Forms.Button atomicPixelsHelp;
			this._bottomOffsetTextbox = new System.Windows.Forms.TextBox();
			this._topOffsetTextbox = new System.Windows.Forms.TextBox();
			this._hintCombo = new System.Windows.Forms.ComboBox();
			this._bottomColorDisplay = new System.Windows.Forms.Panel();
			this._topColorDisplay = new System.Windows.Forms.Panel();
			this._strikeoutOption = new System.Windows.Forms.CheckBox();
			this._underlineOption = new System.Windows.Forms.CheckBox();
			this._italicOption = new System.Windows.Forms.CheckBox();
			this._boldOption = new System.Windows.Forms.CheckBox();
			this._sizeCombo = new System.Windows.Forms.ComboBox();
			this._savePNGButton = new System.Windows.Forms.Button();
			this._alphaPreviewCheckbox = new System.Windows.Forms.CheckBox();
			this._copyToClipboardButton = new System.Windows.Forms.Button();
			this._previewTextEntry = new System.Windows.Forms.TextBox();
			this._fontsList = new System.Windows.Forms.ListBox();
			this._fontPreview = new System.Windows.Forms.Panel();
			this._outputPreview = new System.Windows.Forms.Panel();
			this._outputImageSizeLabel = new System.Windows.Forms.Label();
			this._characterSetControl = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._resetCharacterSetButton = new System.Windows.Forms.Button();
			this._alphaOnlyControl = new System.Windows.Forms.CheckBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.loadConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportFontImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fontSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._fontSystemDotNetControl = new System.Windows.Forms.ToolStripMenuItem();
			this._fontSystemWin32Control = new System.Windows.Forms.ToolStripMenuItem();
			this._embedConfigurationOption = new System.Windows.Forms.CheckBox();
			this._atomicPixelsControl = new System.Windows.Forms.TextBox();
			this._handleSpecialSuffix = new System.Windows.Forms.CheckBox();
			_sizeLabel = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			groupBox1 = new System.Windows.Forms.GroupBox();
			label6 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			groupBox3 = new System.Windows.Forms.GroupBox();
			label5 = new System.Windows.Forms.Label();
			embedHelp = new System.Windows.Forms.Button();
			label7 = new System.Windows.Forms.Label();
			atomicPixelsHelp = new System.Windows.Forms.Button();
			groupBox1.SuspendLayout();
			groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _sizeLabel
			// 
			_sizeLabel.AutoSize = true;
			_sizeLabel.Location = new System.Drawing.Point(10, 34);
			_sizeLabel.Name = "_sizeLabel";
			_sizeLabel.Size = new System.Drawing.Size(27, 13);
			_sizeLabel.TabIndex = 2;
			_sizeLabel.Text = "Size";
			_sizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(464, 19);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(45, 13);
			label1.TabIndex = 4;
			label1.Text = "Preview";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(6, 100);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(80, 13);
			label2.TabIndex = 6;
			label2.Text = "Output Preview";
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(this._bottomOffsetTextbox);
			groupBox1.Controls.Add(this._topOffsetTextbox);
			groupBox1.Controls.Add(label6);
			groupBox1.Controls.Add(this._hintCombo);
			groupBox1.Controls.Add(this._bottomColorDisplay);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(this._topColorDisplay);
			groupBox1.Controls.Add(this._strikeoutOption);
			groupBox1.Controls.Add(this._underlineOption);
			groupBox1.Controls.Add(this._italicOption);
			groupBox1.Controls.Add(this._boldOption);
			groupBox1.Controls.Add(this._sizeCombo);
			groupBox1.Controls.Add(_sizeLabel);
			groupBox1.Location = new System.Drawing.Point(272, 19);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(177, 277);
			groupBox1.TabIndex = 9;
			groupBox1.TabStop = false;
			groupBox1.Text = "Style";
			// 
			// _bottomOffsetTextbox
			// 
			this._bottomOffsetTextbox.Location = new System.Drawing.Point(146, 189);
			this._bottomOffsetTextbox.Name = "_bottomOffsetTextbox";
			this._bottomOffsetTextbox.Size = new System.Drawing.Size(25, 20);
			this._bottomOffsetTextbox.TabIndex = 14;
			this._bottomOffsetTextbox.TextChanged += new System.EventHandler(this._bottomOffsetTextbox_TextChanged);
			// 
			// _topOffsetTextbox
			// 
			this._topOffsetTextbox.Location = new System.Drawing.Point(146, 166);
			this._topOffsetTextbox.Name = "_topOffsetTextbox";
			this._topOffsetTextbox.Size = new System.Drawing.Size(25, 20);
			this._topOffsetTextbox.TabIndex = 13;
			this._topOffsetTextbox.TextChanged += new System.EventHandler(this._topOffsetTextbox_TextChanged);
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(12, 182);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(96, 13);
			label6.TabIndex = 12;
			label6.Text = "Top/Bottom Colors";
			// 
			// _hintCombo
			// 
			this._hintCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._hintCombo.FormattingEnabled = true;
			this._hintCombo.Location = new System.Drawing.Point(13, 241);
			this._hintCombo.Name = "_hintCombo";
			this._hintCombo.Size = new System.Drawing.Size(151, 21);
			this._hintCombo.TabIndex = 7;
			this._hintCombo.SelectedIndexChanged += new System.EventHandler(this.hintCombo_SelectedIndexChanged);
			// 
			// _bottomColorDisplay
			// 
			this._bottomColorDisplay.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this._bottomColorDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._bottomColorDisplay.Location = new System.Drawing.Point(118, 189);
			this._bottomColorDisplay.Name = "_bottomColorDisplay";
			this._bottomColorDisplay.Size = new System.Drawing.Size(22, 20);
			this._bottomColorDisplay.TabIndex = 9;
			this._bottomColorDisplay.Click += new System.EventHandler(this._bottomColorDisplay_Click);
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(10, 225);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(102, 13);
			label3.TabIndex = 11;
			label3.Text = "Text Rendering Hint";
			// 
			// _topColorDisplay
			// 
			this._topColorDisplay.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this._topColorDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._topColorDisplay.Location = new System.Drawing.Point(118, 166);
			this._topColorDisplay.Name = "_topColorDisplay";
			this._topColorDisplay.Size = new System.Drawing.Size(22, 20);
			this._topColorDisplay.TabIndex = 7;
			this._topColorDisplay.Click += new System.EventHandler(this._topColorDisplay_Click);
			// 
			// _strikeoutOption
			// 
			this._strikeoutOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._strikeoutOption.Location = new System.Drawing.Point(46, 137);
			this._strikeoutOption.Name = "_strikeoutOption";
			this._strikeoutOption.Size = new System.Drawing.Size(73, 20);
			this._strikeoutOption.TabIndex = 6;
			this._strikeoutOption.Text = "Strikeout";
			this._strikeoutOption.UseVisualStyleBackColor = true;
			this._strikeoutOption.CheckedChanged += new System.EventHandler(this._strikeoutOption_CheckedChanged);
			// 
			// _underlineOption
			// 
			this._underlineOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._underlineOption.Location = new System.Drawing.Point(46, 114);
			this._underlineOption.Name = "_underlineOption";
			this._underlineOption.Size = new System.Drawing.Size(73, 20);
			this._underlineOption.TabIndex = 5;
			this._underlineOption.Text = "Underline";
			this._underlineOption.UseVisualStyleBackColor = true;
			this._underlineOption.CheckedChanged += new System.EventHandler(this._underlineOption_CheckedChanged);
			// 
			// _italicOption
			// 
			this._italicOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._italicOption.Location = new System.Drawing.Point(47, 91);
			this._italicOption.Name = "_italicOption";
			this._italicOption.Size = new System.Drawing.Size(73, 20);
			this._italicOption.TabIndex = 4;
			this._italicOption.Text = "Italic";
			this._italicOption.UseVisualStyleBackColor = true;
			this._italicOption.CheckedChanged += new System.EventHandler(this._italicOption_CheckedChanged);
			// 
			// _boldOption
			// 
			this._boldOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._boldOption.Location = new System.Drawing.Point(47, 68);
			this._boldOption.Name = "_boldOption";
			this._boldOption.Size = new System.Drawing.Size(73, 20);
			this._boldOption.TabIndex = 3;
			this._boldOption.Text = "Bold";
			this._boldOption.UseVisualStyleBackColor = true;
			this._boldOption.CheckedChanged += new System.EventHandler(this._boldOption_CheckedChanged);
			// 
			// _sizeCombo
			// 
			this._sizeCombo.FormattingEnabled = true;
			this._sizeCombo.Location = new System.Drawing.Point(43, 31);
			this._sizeCombo.Name = "_sizeCombo";
			this._sizeCombo.Size = new System.Drawing.Size(121, 21);
			this._sizeCombo.TabIndex = 2;
			this._sizeCombo.SelectedIndexChanged += new System.EventHandler(this._sizeCombo_SelectedIndexChanged);
			this._sizeCombo.TextUpdate += new System.EventHandler(this._sizeCombo_TextUpdate);
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(6, 66);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(72, 13);
			label4.TabIndex = 10;
			label4.Text = "Character Set";
			// 
			// groupBox3
			// 
			groupBox3.Controls.Add(this._savePNGButton);
			groupBox3.Controls.Add(this._alphaPreviewCheckbox);
			groupBox3.Controls.Add(this._copyToClipboardButton);
			groupBox3.Controls.Add(this._previewTextEntry);
			groupBox3.Controls.Add(this._fontsList);
			groupBox3.Controls.Add(label5);
			groupBox3.Controls.Add(this._fontPreview);
			groupBox3.Controls.Add(groupBox1);
			groupBox3.Controls.Add(label1);
			groupBox3.Location = new System.Drawing.Point(12, 27);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(809, 301);
			groupBox3.TabIndex = 13;
			groupBox3.TabStop = false;
			groupBox3.Text = "Font";
			// 
			// _savePNGButton
			// 
			this._savePNGButton.Location = new System.Drawing.Point(573, 84);
			this._savePNGButton.Name = "_savePNGButton";
			this._savePNGButton.Size = new System.Drawing.Size(103, 20);
			this._savePNGButton.TabIndex = 15;
			this._savePNGButton.Text = "Save PNG";
			this._savePNGButton.UseVisualStyleBackColor = true;
			this._savePNGButton.Click += new System.EventHandler(this._savePNGButton_Click);
			// 
			// _alphaPreviewCheckbox
			// 
			this._alphaPreviewCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._alphaPreviewCheckbox.Location = new System.Drawing.Point(703, 84);
			this._alphaPreviewCheckbox.Name = "_alphaPreviewCheckbox";
			this._alphaPreviewCheckbox.Size = new System.Drawing.Size(98, 20);
			this._alphaPreviewCheckbox.TabIndex = 14;
			this._alphaPreviewCheckbox.Text = "Alpha Preview";
			this._alphaPreviewCheckbox.UseVisualStyleBackColor = true;
			this._alphaPreviewCheckbox.CheckedChanged += new System.EventHandler(this._alphaPreviewCheckbox_CheckedChanged);
			// 
			// _copyToClipboardButton
			// 
			this._copyToClipboardButton.Location = new System.Drawing.Point(464, 84);
			this._copyToClipboardButton.Name = "_copyToClipboardButton";
			this._copyToClipboardButton.Size = new System.Drawing.Size(103, 20);
			this._copyToClipboardButton.TabIndex = 12;
			this._copyToClipboardButton.Text = "Copy to Clipboard";
			this._copyToClipboardButton.UseVisualStyleBackColor = true;
			this._copyToClipboardButton.Click += new System.EventHandler(this._copyToClipboardButton_Click);
			// 
			// _previewTextEntry
			// 
			this._previewTextEntry.Location = new System.Drawing.Point(542, 47);
			this._previewTextEntry.Name = "_previewTextEntry";
			this._previewTextEntry.Size = new System.Drawing.Size(261, 20);
			this._previewTextEntry.TabIndex = 12;
			this._previewTextEntry.TextChanged += new System.EventHandler(this._previewTextEntry_TextChanged);
			// 
			// _fontsList
			// 
			this._fontsList.FormattingEnabled = true;
			this._fontsList.Location = new System.Drawing.Point(6, 19);
			this._fontsList.Name = "_fontsList";
			this._fontsList.Size = new System.Drawing.Size(260, 277);
			this._fontsList.TabIndex = 1;
			this._fontsList.SelectedIndexChanged += new System.EventHandler(this._fontsList_SelectedIndexChanged);
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(464, 50);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(69, 13);
			label5.TabIndex = 13;
			label5.Text = "Preview Text";
			// 
			// _fontPreview
			// 
			this._fontPreview.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this._fontPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._fontPreview.Location = new System.Drawing.Point(464, 110);
			this._fontPreview.Name = "_fontPreview";
			this._fontPreview.Size = new System.Drawing.Size(339, 185);
			this._fontPreview.TabIndex = 0;
			this._fontPreview.Paint += new System.Windows.Forms.PaintEventHandler(this._fontPreview_Paint);
			// 
			// embedHelp
			// 
			embedHelp.Location = new System.Drawing.Point(311, 31);
			embedHelp.Name = "embedHelp";
			embedHelp.Size = new System.Drawing.Size(19, 21);
			embedHelp.TabIndex = 11;
			embedHelp.Text = "?";
			embedHelp.UseVisualStyleBackColor = true;
			embedHelp.Click += new System.EventHandler(this.embedHelp_Click);
			// 
			// _outputPreview
			// 
			this._outputPreview.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this._outputPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._outputPreview.Location = new System.Drawing.Point(6, 116);
			this._outputPreview.Name = "_outputPreview";
			this._outputPreview.Size = new System.Drawing.Size(797, 392);
			this._outputPreview.TabIndex = 5;
			this._outputPreview.Paint += new System.Windows.Forms.PaintEventHandler(this._outputPreview_Paint);
			// 
			// _outputImageSizeLabel
			// 
			this._outputImageSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._outputImageSizeLabel.Location = new System.Drawing.Point(9, 100);
			this._outputImageSizeLabel.Name = "_outputImageSizeLabel";
			this._outputImageSizeLabel.Size = new System.Drawing.Size(794, 13);
			this._outputImageSizeLabel.TabIndex = 8;
			this._outputImageSizeLabel.Text = "[dimensions]";
			this._outputImageSizeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// _characterSetControl
			// 
			this._characterSetControl.Location = new System.Drawing.Point(84, 63);
			this._characterSetControl.Name = "_characterSetControl";
			this._characterSetControl.Size = new System.Drawing.Size(638, 20);
			this._characterSetControl.TabIndex = 9;
			this._characterSetControl.TextChanged += new System.EventHandler(this._characterSetControl_TextChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this._handleSpecialSuffix);
			this.groupBox2.Controls.Add(atomicPixelsHelp);
			this.groupBox2.Controls.Add(this._atomicPixelsControl);
			this.groupBox2.Controls.Add(label7);
			this.groupBox2.Controls.Add(embedHelp);
			this.groupBox2.Controls.Add(this._resetCharacterSetButton);
			this.groupBox2.Controls.Add(this._alphaOnlyControl);
			this.groupBox2.Controls.Add(this._characterSetControl);
			this.groupBox2.Controls.Add(label4);
			this.groupBox2.Controls.Add(this._outputPreview);
			this.groupBox2.Controls.Add(label2);
			this.groupBox2.Controls.Add(this._outputImageSizeLabel);
			this.groupBox2.Location = new System.Drawing.Point(12, 346);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(809, 514);
			this.groupBox2.TabIndex = 12;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Export";
			// 
			// _resetCharacterSetButton
			// 
			this._resetCharacterSetButton.Location = new System.Drawing.Point(728, 63);
			this._resetCharacterSetButton.Name = "_resetCharacterSetButton";
			this._resetCharacterSetButton.Size = new System.Drawing.Size(75, 20);
			this._resetCharacterSetButton.TabIndex = 10;
			this._resetCharacterSetButton.Text = "Reset";
			this._resetCharacterSetButton.UseVisualStyleBackColor = true;
			this._resetCharacterSetButton.Click += new System.EventHandler(this._resetCharacterSetButton_Click);
			// 
			// _alphaOnlyControl
			// 
			this._alphaOnlyControl.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._alphaOnlyControl.Location = new System.Drawing.Point(6, 32);
			this._alphaOnlyControl.Name = "_alphaOnlyControl";
			this._alphaOnlyControl.Size = new System.Drawing.Size(90, 20);
			this._alphaOnlyControl.TabIndex = 7;
			this._alphaOnlyControl.Text = "Alpha Only";
			this._alphaOnlyControl.UseVisualStyleBackColor = true;
			this._alphaOnlyControl.CheckedChanged += new System.EventHandler(this._alphaOnlyControl_CheckedChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.fontSystemToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(833, 24);
			this.menuStrip1.TabIndex = 14;
			this.menuStrip1.Text = "_menu";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.loadConfigurationToolStripMenuItem,
            this.exportFontImageToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
			// 
			// loadConfigurationToolStripMenuItem
			// 
			this.loadConfigurationToolStripMenuItem.Name = "loadConfigurationToolStripMenuItem";
			this.loadConfigurationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
			this.loadConfigurationToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.loadConfigurationToolStripMenuItem.Text = "&Import Configuration";
			this.loadConfigurationToolStripMenuItem.Click += new System.EventHandler(this.importConfigurationMenuItem_Click);
			// 
			// exportFontImageToolStripMenuItem
			// 
			this.exportFontImageToolStripMenuItem.Name = "exportFontImageToolStripMenuItem";
			this.exportFontImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.exportFontImageToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.exportFontImageToolStripMenuItem.Text = "Export &Configuration";
			this.exportFontImageToolStripMenuItem.Click += new System.EventHandler(this.exportConfigurationMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(221, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// fontSystemToolStripMenuItem
			// 
			this.fontSystemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fontSystemDotNetControl,
            this._fontSystemWin32Control});
			this.fontSystemToolStripMenuItem.Name = "fontSystemToolStripMenuItem";
			this.fontSystemToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
			this.fontSystemToolStripMenuItem.Text = "Font &System";
			// 
			// _fontSystemDotNetControl
			// 
			this._fontSystemDotNetControl.Name = "_fontSystemDotNetControl";
			this._fontSystemDotNetControl.Size = new System.Drawing.Size(107, 22);
			this._fontSystemDotNetControl.Text = ".NET";
			this._fontSystemDotNetControl.Click += new System.EventHandler(this._fontSystemDotNetControl_Click);
			// 
			// _fontSystemWin32Control
			// 
			this._fontSystemWin32Control.Name = "_fontSystemWin32Control";
			this._fontSystemWin32Control.Size = new System.Drawing.Size(107, 22);
			this._fontSystemWin32Control.Text = "Win32";
			this._fontSystemWin32Control.Click += new System.EventHandler(this._fontSystemWin32Control_Click);
			// 
			// _embedConfigurationOption
			// 
			this._embedConfigurationOption.AutoSize = true;
			this._embedConfigurationOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._embedConfigurationOption.Checked = true;
			this._embedConfigurationOption.CheckState = System.Windows.Forms.CheckState.Checked;
			this._embedConfigurationOption.Location = new System.Drawing.Point(139, 380);
			this._embedConfigurationOption.Name = "_embedConfigurationOption";
			this._embedConfigurationOption.Size = new System.Drawing.Size(178, 17);
			this._embedConfigurationOption.TabIndex = 8;
			this._embedConfigurationOption.Text = "Embed Configuration in Font File";
			this._embedConfigurationOption.UseVisualStyleBackColor = true;
			// 
			// _atomicPixelsControl
			// 
			this._atomicPixelsControl.Location = new System.Drawing.Point(442, 31);
			this._atomicPixelsControl.Name = "_atomicPixelsControl";
			this._atomicPixelsControl.Size = new System.Drawing.Size(40, 20);
			this._atomicPixelsControl.TabIndex = 14;
			this._atomicPixelsControl.TextChanged += new System.EventHandler(this._atomicPixelsControl_TextChanged);
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(357, 34);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(79, 13);
			label7.TabIndex = 15;
			label7.Text = "# Atomic Pixels";
			// 
			// atomicPixelsHelp
			// 
			atomicPixelsHelp.Location = new System.Drawing.Point(488, 31);
			atomicPixelsHelp.Name = "atomicPixelsHelp";
			atomicPixelsHelp.Size = new System.Drawing.Size(19, 21);
			atomicPixelsHelp.TabIndex = 16;
			atomicPixelsHelp.Text = "?";
			atomicPixelsHelp.UseVisualStyleBackColor = true;
			atomicPixelsHelp.Click += new System.EventHandler(this.atomicPixelsHelp_Click);
			// 
			// _handleSpecialSuffix
			// 
			this._handleSpecialSuffix.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._handleSpecialSuffix.Location = new System.Drawing.Point(552, 32);
			this._handleSpecialSuffix.Name = "_handleSpecialSuffix";
			this._handleSpecialSuffix.Size = new System.Drawing.Size(134, 20);
			this._handleSpecialSuffix.TabIndex = 17;
			this._handleSpecialSuffix.Text = "Handle Special Suffix";
			this._handleSpecialSuffix.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(833, 864);
			this.Controls.Add(this._embedConfigurationOption);
			this.Controls.Add(groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "Sudo Font";
			this.Activated += new System.EventHandler(this.MainForm_Activated);
			this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.Move += new System.EventHandler(this.MainForm_Move);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel _fontPreview;
		private System.Windows.Forms.ListBox _fontsList;
		private System.Windows.Forms.ComboBox _sizeCombo;
		private System.Windows.Forms.Panel _outputPreview;
		private System.Windows.Forms.Label _outputImageSizeLabel;
		private System.Windows.Forms.CheckBox _strikeoutOption;
		private System.Windows.Forms.CheckBox _underlineOption;
		private System.Windows.Forms.CheckBox _italicOption;
		private System.Windows.Forms.CheckBox _boldOption;
		private System.Windows.Forms.TextBox _characterSetControl;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox _alphaOnlyControl;
		private System.Windows.Forms.Button _resetCharacterSetButton;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportFontImageToolStripMenuItem;
		private System.Windows.Forms.CheckBox _embedConfigurationOption;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem loadConfigurationToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ComboBox _hintCombo;
		private System.Windows.Forms.Panel _bottomColorDisplay;
		private System.Windows.Forms.Panel _topColorDisplay;
		private System.Windows.Forms.TextBox _previewTextEntry;
		private System.Windows.Forms.Button _copyToClipboardButton;
		private System.Windows.Forms.ToolStripMenuItem fontSystemToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _fontSystemDotNetControl;
		private System.Windows.Forms.ToolStripMenuItem _fontSystemWin32Control;
		private System.Windows.Forms.Button _savePNGButton;
		private System.Windows.Forms.CheckBox _alphaPreviewCheckbox;
		private System.Windows.Forms.TextBox _bottomOffsetTextbox;
		private System.Windows.Forms.TextBox _topOffsetTextbox;
		private System.Windows.Forms.TextBox _atomicPixelsControl;
		private System.Windows.Forms.CheckBox _handleSpecialSuffix;
	}
}

