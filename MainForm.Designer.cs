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
			System.Windows.Forms.Label label3;
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.Label label4;
			this._fontPreview = new System.Windows.Forms.Panel();
			this._fontsList = new System.Windows.Forms.ListBox();
			this._sizeCombo = new System.Windows.Forms.ComboBox();
			this._outputPreview = new System.Windows.Forms.Panel();
			this._outputImageSizeLabel = new System.Windows.Forms.Label();
			this._boldOption = new System.Windows.Forms.CheckBox();
			this._italicOption = new System.Windows.Forms.CheckBox();
			this._underlineOption = new System.Windows.Forms.CheckBox();
			this._strikeoutOption = new System.Windows.Forms.CheckBox();
			this._characterSetControl = new System.Windows.Forms.TextBox();
			_sizeLabel = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			groupBox1 = new System.Windows.Forms.GroupBox();
			label4 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _fontPreview
			// 
			this._fontPreview.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this._fontPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._fontPreview.Location = new System.Drawing.Point(12, 344);
			this._fontPreview.Name = "_fontPreview";
			this._fontPreview.Size = new System.Drawing.Size(469, 174);
			this._fontPreview.TabIndex = 0;
			this._fontPreview.Paint += new System.Windows.Forms.PaintEventHandler(this._fontPreview_Paint);
			// 
			// _fontsList
			// 
			this._fontsList.FormattingEnabled = true;
			this._fontsList.Location = new System.Drawing.Point(12, 12);
			this._fontsList.Name = "_fontsList";
			this._fontsList.Size = new System.Drawing.Size(260, 264);
			this._fontsList.TabIndex = 1;
			this._fontsList.SelectedIndexChanged += new System.EventHandler(this._fontsList_SelectedIndexChanged);
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
			// _sizeCombo
			// 
			this._sizeCombo.FormattingEnabled = true;
			this._sizeCombo.Location = new System.Drawing.Point(43, 31);
			this._sizeCombo.Name = "_sizeCombo";
			this._sizeCombo.Size = new System.Drawing.Size(121, 21);
			this._sizeCombo.TabIndex = 3;
			this._sizeCombo.SelectedIndexChanged += new System.EventHandler(this._sizeCombo_SelectedIndexChanged);
			this._sizeCombo.TextUpdate += new System.EventHandler(this._sizeCombo_TextUpdate);
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(12, 328);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(69, 13);
			label1.TabIndex = 4;
			label1.Text = "Font Preview";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(12, 542);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(80, 13);
			label2.TabIndex = 6;
			label2.Text = "Output Preview";
			// 
			// _outputPreview
			// 
			this._outputPreview.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this._outputPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._outputPreview.Location = new System.Drawing.Point(12, 558);
			this._outputPreview.Name = "_outputPreview";
			this._outputPreview.Size = new System.Drawing.Size(469, 174);
			this._outputPreview.TabIndex = 5;
			this._outputPreview.Paint += new System.Windows.Forms.PaintEventHandler(this._outputPreview_Paint);
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(12, 735);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(100, 13);
			label3.TabIndex = 7;
			label3.Text = "Output Image Size: ";
			// 
			// _outputImageSizeLabel
			// 
			this._outputImageSizeLabel.AutoSize = true;
			this._outputImageSizeLabel.Location = new System.Drawing.Point(109, 735);
			this._outputImageSizeLabel.Name = "_outputImageSizeLabel";
			this._outputImageSizeLabel.Size = new System.Drawing.Size(0, 13);
			this._outputImageSizeLabel.TabIndex = 8;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(this._strikeoutOption);
			groupBox1.Controls.Add(this._underlineOption);
			groupBox1.Controls.Add(this._italicOption);
			groupBox1.Controls.Add(this._boldOption);
			groupBox1.Controls.Add(this._sizeCombo);
			groupBox1.Controls.Add(_sizeLabel);
			groupBox1.Location = new System.Drawing.Point(278, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(177, 172);
			groupBox1.TabIndex = 9;
			groupBox1.TabStop = false;
			groupBox1.Text = "Style";
			// 
			// _boldOption
			// 
			this._boldOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._boldOption.Location = new System.Drawing.Point(47, 68);
			this._boldOption.Name = "_boldOption";
			this._boldOption.Size = new System.Drawing.Size(73, 20);
			this._boldOption.TabIndex = 11;
			this._boldOption.Text = "Bold";
			this._boldOption.UseVisualStyleBackColor = true;
			this._boldOption.CheckedChanged += new System.EventHandler(this._boldOption_CheckedChanged);
			// 
			// _italicOption
			// 
			this._italicOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._italicOption.Location = new System.Drawing.Point(47, 91);
			this._italicOption.Name = "_italicOption";
			this._italicOption.Size = new System.Drawing.Size(73, 20);
			this._italicOption.TabIndex = 12;
			this._italicOption.Text = "Italic";
			this._italicOption.UseVisualStyleBackColor = true;
			this._italicOption.CheckedChanged += new System.EventHandler(this._italicOption_CheckedChanged);
			// 
			// _underlineOption
			// 
			this._underlineOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._underlineOption.Location = new System.Drawing.Point(46, 114);
			this._underlineOption.Name = "_underlineOption";
			this._underlineOption.Size = new System.Drawing.Size(73, 20);
			this._underlineOption.TabIndex = 13;
			this._underlineOption.Text = "Underline";
			this._underlineOption.UseVisualStyleBackColor = true;
			this._underlineOption.CheckedChanged += new System.EventHandler(this._underlineOption_CheckedChanged);
			// 
			// _strikeoutOption
			// 
			this._strikeoutOption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._strikeoutOption.Location = new System.Drawing.Point(46, 137);
			this._strikeoutOption.Name = "_strikeoutOption";
			this._strikeoutOption.Size = new System.Drawing.Size(73, 20);
			this._strikeoutOption.TabIndex = 14;
			this._strikeoutOption.Text = "Strikeout";
			this._strikeoutOption.UseVisualStyleBackColor = true;
			this._strikeoutOption.CheckedChanged += new System.EventHandler(this._strikeoutOption_CheckedChanged);
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(12, 299);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(72, 13);
			label4.TabIndex = 10;
			label4.Text = "Character Set";
			// 
			// _characterSetControl
			// 
			this._characterSetControl.Location = new System.Drawing.Point(90, 296);
			this._characterSetControl.Name = "_characterSetControl";
			this._characterSetControl.Size = new System.Drawing.Size(391, 20);
			this._characterSetControl.TabIndex = 11;
			this._characterSetControl.TextChanged += new System.EventHandler(this._characterSetControl_TextChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(726, 763);
			this.Controls.Add(this._characterSetControl);
			this.Controls.Add(label4);
			this.Controls.Add(groupBox1);
			this.Controls.Add(this._outputImageSizeLabel);
			this.Controls.Add(label3);
			this.Controls.Add(label2);
			this.Controls.Add(this._outputPreview);
			this.Controls.Add(label1);
			this.Controls.Add(this._fontsList);
			this.Controls.Add(this._fontPreview);
			this.Name = "MainForm";
			this.Text = "Sudo Font";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
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
	}
}

