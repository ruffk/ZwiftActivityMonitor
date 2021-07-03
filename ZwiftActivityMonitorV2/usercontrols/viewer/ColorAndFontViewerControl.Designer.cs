
namespace ZwiftActivityMonitorV2
{
    partial class ColorAndFontViewerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cbBold = new System.Windows.Forms.CheckBox();
            this.cbItalic = new System.Windows.Forms.CheckBox();
            this.nudFontSize = new Syncfusion.Windows.Forms.Tools.NumericUpDownExt();
            this.cbTheme = new System.Windows.Forms.ComboBox();
            this.cbFonts = new ZwiftActivityMonitorV2.FontComboBox();
            this.btnColor = new Syncfusion.Windows.Forms.ColorPickerButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSample = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAccept = new System.Windows.Forms.Button();
            this.cbTransparency = new System.Windows.Forms.ComboBox();
            this.sfToolTip = new Syncfusion.Windows.Forms.SfToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbBold
            // 
            this.cbBold.AutoSize = true;
            this.cbBold.BackColor = System.Drawing.Color.Transparent;
            this.cbBold.Location = new System.Drawing.Point(76, 50);
            this.cbBold.Name = "cbBold";
            this.cbBold.Size = new System.Drawing.Size(50, 19);
            this.cbBold.TabIndex = 30;
            this.cbBold.Text = "Bold";
            this.cbBold.UseVisualStyleBackColor = false;
            this.cbBold.CheckedChanged += new System.EventHandler(this.fontStyleModifiers_CheckedChanged);
            // 
            // cbItalic
            // 
            this.cbItalic.AutoSize = true;
            this.cbItalic.BackColor = System.Drawing.Color.Transparent;
            this.cbItalic.Location = new System.Drawing.Point(127, 50);
            this.cbItalic.Name = "cbItalic";
            this.cbItalic.Size = new System.Drawing.Size(51, 19);
            this.cbItalic.TabIndex = 40;
            this.cbItalic.Text = "Italic";
            this.cbItalic.UseVisualStyleBackColor = false;
            this.cbItalic.CheckedChanged += new System.EventHandler(this.fontStyleModifiers_CheckedChanged);
            // 
            // nudFontSize
            // 
            this.nudFontSize.BeforeTouchSize = new System.Drawing.Size(53, 23);
            this.nudFontSize.CanOverrideStyle = true;
            this.nudFontSize.ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Managed;
            this.nudFontSize.Location = new System.Drawing.Point(17, 47);
            this.nudFontSize.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudFontSize.MaxLength = 2;
            this.nudFontSize.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudFontSize.Name = "nudFontSize";
            this.nudFontSize.Size = new System.Drawing.Size(53, 23);
            this.nudFontSize.TabIndex = 20;
            this.nudFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudFontSize.ThemeName = "Office2010";
            this.nudFontSize.ThemesEnabled = true;
            this.nudFontSize.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudFontSize.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.Office2010;
            this.nudFontSize.ValueChanged += new System.EventHandler(this.nudFontSize_ValueChanged);
            // 
            // cbTheme
            // 
            this.cbTheme.DisplayMember = "Value";
            this.cbTheme.FormattingEnabled = true;
            this.cbTheme.Location = new System.Drawing.Point(17, 96);
            this.cbTheme.Name = "cbTheme";
            this.cbTheme.Size = new System.Drawing.Size(153, 23);
            this.cbTheme.TabIndex = 50;
            this.cbTheme.ValueMember = "Value";
            this.cbTheme.SelectedIndexChanged += new System.EventHandler(this.cbTheme_SelectedIndexChanged);
            // 
            // cbFonts
            // 
            this.cbFonts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbFonts.DropDownBackColor = System.Drawing.SystemColors.Window;
            this.cbFonts.DropDownBorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cbFonts.FormattingEnabled = true;
            this.cbFonts.Location = new System.Drawing.Point(17, 20);
            this.cbFonts.Name = "cbFonts";
            this.cbFonts.Size = new System.Drawing.Size(207, 24);
            this.cbFonts.TabIndex = 10;
            this.cbFonts.SelectedIndexChanged += new System.EventHandler(this.cbFonts_SelectedIndexChanged);
            // 
            // btnColor
            // 
            this.btnColor.Appearance = Syncfusion.Windows.Forms.ButtonAppearance.Office2010;
            this.btnColor.BackColor = System.Drawing.Color.Transparent;
            this.btnColor.BeforeTouchSize = new System.Drawing.Size(90, 23);
            this.btnColor.CanOverrideStyle = true;
            this.btnColor.CustomTabName = "Theme";
            this.btnColor.Location = new System.Drawing.Point(202, 96);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(90, 23);
            this.btnColor.StandardTabName = "Standard";
            this.btnColor.TabIndex = 60;
            this.btnColor.Text = "Choose Color";
            this.btnColor.ThemeName = "Office2010";
            this.btnColor.ColorSelected += new System.EventHandler(this.btnColor_ColorSelected);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 35;
            this.label1.Text = "Choose Font:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 15);
            this.label2.TabIndex = 36;
            this.label2.Text = "Choose Theme/Transparency:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 15);
            this.label3.TabIndex = 37;
            this.label3.Text = "Or";
            // 
            // lblSample
            // 
            this.lblSample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSample.Location = new System.Drawing.Point(0, 0);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(108, 38);
            this.lblSample.TabIndex = 37;
            this.lblSample.Text = "Sample Text";
            this.lblSample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblSample);
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(202, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(110, 40);
            this.panel1.TabIndex = 42;
            // 
            // btnAccept
            // 
            this.btnAccept.BackColor = System.Drawing.Color.Transparent;
            this.btnAccept.BackgroundImage = global::ZwiftActivityMonitorV2.Properties.Resources._checked;
            this.btnAccept.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAccept.FlatAppearance.BorderSize = 0;
            this.btnAccept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccept.Location = new System.Drawing.Point(290, 121);
            this.btnAccept.Margin = new System.Windows.Forms.Padding(0);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(25, 25);
            this.btnAccept.TabIndex = 80;
            this.btnAccept.TabStop = false;
            this.btnAccept.UseVisualStyleBackColor = false;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // cbTransparency
            // 
            this.cbTransparency.DisplayMember = "Value";
            this.cbTransparency.FormattingEnabled = true;
            this.cbTransparency.Location = new System.Drawing.Point(17, 122);
            this.cbTransparency.Name = "cbTransparency";
            this.cbTransparency.Size = new System.Drawing.Size(153, 23);
            this.cbTransparency.TabIndex = 70;
            this.cbTransparency.ValueMember = "Value";
            this.cbTransparency.SelectedIndexChanged += new System.EventHandler(this.cbTransparency_SelectedIndexChanged);
            // 
            // ColorAndFontViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.cbTransparency);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnColor);
            this.Controls.Add(this.cbFonts);
            this.Controls.Add(this.cbTheme);
            this.Controls.Add(this.cbBold);
            this.Controls.Add(this.cbItalic);
            this.Controls.Add(this.nudFontSize);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ColorAndFontViewerControl";
            this.Size = new System.Drawing.Size(315, 147);
            this.Load += new System.EventHandler(this.ColorAndFontViewControl_Load);
            this.BackColorChanged += new System.EventHandler(this.ColorAndFontViewControl_BackColorChanged);
            this.ForeColorChanged += new System.EventHandler(this.ColorAndFontViewControl_ForeColorChanged);
            this.ParentChanged += new System.EventHandler(this.ColorAndFontViewControl_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.nudFontSize)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox cbBold;
        private System.Windows.Forms.CheckBox cbItalic;
        private Syncfusion.Windows.Forms.Tools.NumericUpDownExt nudFontSize;
        private System.Windows.Forms.ComboBox cbTheme;
        private FontComboBox cbFonts;
        private Syncfusion.Windows.Forms.ColorPickerButton btnColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSample;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.ComboBox cbTransparency;
        private Syncfusion.Windows.Forms.SfToolTip sfToolTip;
    }
}
