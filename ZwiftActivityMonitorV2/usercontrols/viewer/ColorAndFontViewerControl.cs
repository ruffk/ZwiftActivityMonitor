using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Syncfusion.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Linq;

namespace ZwiftActivityMonitorV2
{
    public partial class ColorAndFontViewerControl : ViewerUserControlEx
    {

        // temp
        //public ZAMColorScheme ZAMColors { get; } = new ZAMColorScheme();

        private bool InitializingControls { get; set; } = false;

        public event EventHandler<ColorsAndFontChangedEventArgs> ColorsAndFontChanged;


        public ColorAndFontViewerControl()
        {
            InitializeComponent();

            //Appearance.InitializeDefaultValues();

            this.cbFonts.Fill();

            this.btnColor.ColorGroups = ((Syncfusion.Windows.Forms.ColorUIGroups)
                (Syncfusion.Windows.Forms.ColorUIGroups.StandardColors | Syncfusion.Windows.Forms.ColorUIGroups.CustomColors)
                );
        }

        private void ColorAndFontViewControl_Load(object sender, EventArgs e)
        {
            this.BeginInitializingControls();

            ZAMappearance settings = ZAMsettings.Settings.Appearance;

            nudFontSize.Value = (int)settings.FontSize;

            cbTheme.DataSource = new BindingSource(settings.ThemeItems, null);
            if (settings.Theme.HasValue)
                cbTheme.SelectedValue = settings.Theme.Value.Value;

            cbTransparency.DataSource = new BindingSource(settings.TransparencyItems, null);
            if (settings.Transparency.HasValue)
                cbTransparency.SelectedValue = settings.Transparency.Value.Value;

            this.btnColor.SelectedColor = settings.ManagedColor;

            this.cbFonts.SelectedItem = settings.FontFamily;
            cbBold.Checked = settings.IsFontBold;
            cbItalic.Checked = settings.IsFontItalic;

            this.EndInitializingControls();
        }

        private void BeginInitializingControls()
        {
            this.InitializingControls = true;
        }

        private void EndInitializingControls()
        {
            this.InitializingControls = false;

            this.AdjustFont();
        }

        private void cbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustFont();
        }

        private void AdjustFont()
        {
            if (this.InitializingControls)
                return;

            if (this.cbFonts.SelectedItem == null)
                return;

            if (this.cbTheme.SelectedItem == null)
                return;

            //Debug.WriteLine($"AdjustFont - cbFonts.SelectedItem: {this.cbFonts.SelectedItem}");

            //Debug.WriteLine($"AdjustFont - cbTheme.SelectedItem: {this.cbTheme.SelectedItem}");

            FontStyle style = 0;

            style |= cbBold.Checked ? FontStyle.Bold : 0;
            style |= cbItalic.Checked ? FontStyle.Italic : 0;

            Font font = new Font(this.cbFonts.SelectedItem.ToString(), (float)this.nudFontSize.Value, style);
            lblSample.Font = font;


            var selection = (KeyValuePair<ThemeType, string>)cbTheme.SelectedItem;

            Office2010FormEx hidden = new Office2010FormEx();
            hidden.UseOffice2010SchemeBackColor = true;

            if (selection.Key != ThemeType.Custom)
            {
                hidden.ColorScheme = ZAMsettings.Settings.Appearance.GetOfficeColorScheme(selection.Key, out Color? managedColor);

                if (hidden.ColorScheme == Office2010Theme.Managed)
                {
                    Office2010Colors.ApplyManagedColors(hidden, managedColor.Value);
                }
            }
            else
            {
                hidden.ColorScheme = Office2010Theme.Managed;
                Office2010Colors.ApplyManagedColors(hidden, btnColor.SelectedColor);
            }

            Office2010Colors colors = hidden.GetColorTable();
            this.panel1.BackColor = colors.FormBackground;
            this.panel1.ForeColor = colors.FormTextColor;
        }

        private void nudFontSize_ValueChanged(object sender, EventArgs e)
        {
            AdjustFont();
        }

        private void fontStyleModifiers_CheckedChanged(object sender, EventArgs e)
        {
            AdjustFont();
        }

        private void cbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbTheme.SelectedItem == null)
                return;

            //Debug.WriteLine($"cbTheme_SelectedIndexChanged - cbTheme.SelectedItem: {this.cbTheme.SelectedItem}");

            var selection = (KeyValuePair<ThemeType, string>)cbTheme.SelectedItem;

            btnColor.Enabled = (selection.Key == ThemeType.Custom);

            AdjustFont();
        }

        private void cbTransparency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbTransparency.SelectedItem == null)
                return;

            //Debug.WriteLine($"cbTransparency_SelectedIndexChanged - cbTransparency.SelectedItem: {this.cbTransparency.SelectedItem}");

            AdjustFont();
        }

        private void btnColor_ColorSelected(object sender, EventArgs e)
        {
            //Debug.WriteLine($"btnColor_ColorSelected - {btnColor.SelectedColor}");

            if (btnColor.SelectedColor == Color.Transparent)
            {
                btnColor.SelectedColor = SystemColors.Control;
            }

            AdjustFont();
        }



        private void ColorAndFontViewControl_BackColorChanged(object sender, EventArgs e)
        {
            //foreach (Control control in this.Controls)
            //{
            //    if (control is Label || control is CheckBox)// || control is ColorPickerButton || control is Panel)
            //    {
            //        control.BackColor = this.BackColor;
            //    }
            //}

        }

        private void ColorAndFontViewControl_ForeColorChanged(object sender, EventArgs e)
        {
            //foreach(Control control in this.Controls)
            //{
            //    if (control is Label || control is CheckBox)// || control is ColorPickerButton || control is Panel)
            //    {
            //        control.ForeColor = this.ForeColor;
            //    }
            //}
        }


        private void Parent_ForeColorChanged(object sender, EventArgs e)
        {
            this.ForeColor = this.Parent.ForeColor;
        }

        private void Parent_BackColorChanged(object sender, EventArgs e)
        {
            this.BackColor = this.Parent.BackColor;
        }


        private void ColorAndFontViewControl_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.BackColorChanged += Parent_BackColorChanged;
                this.Parent.ForeColorChanged += Parent_ForeColorChanged;
            }
        }


        private void btnAccept_Click(object sender, EventArgs e)
        {
            Debug.WriteLine($"Theme: {cbTheme.SelectedValue}, ManagedColor: {btnColor.SelectedColor}, Font: {cbFonts.SelectedItem} Transparency: {cbTransparency.SelectedValue}");

            ZAMsettings.BeginCachedConfiguration();

            ZAMsettings.Settings.Appearance.ThemeSetting = ((KeyValuePair<ThemeType, string>)cbTheme.SelectedItem).Key;
            ZAMsettings.Settings.Appearance.TransparencySetting = ((KeyValuePair<TransparencyType, string>)cbTransparency.SelectedItem).Key;
            ZAMsettings.Settings.Appearance.ManagedColor = btnColor.SelectedColor;
            ZAMsettings.Settings.Appearance.FontFamily = cbFonts.SelectedItem.ToString();
            ZAMsettings.Settings.Appearance.FontSize = (float)nudFontSize.Value;
            ZAMsettings.Settings.Appearance.IsFontBold = cbBold.Checked;
            ZAMsettings.Settings.Appearance.IsFontItalic = cbItalic.Checked;

            ZAMsettings.CommitCachedConfiguration();

            ZAMappearance cs = ZAMsettings.Settings.Appearance;

            ColorsAndFontChangedEventArgs args = new(cs.ThemeSetting, cs.TransparencySetting, cs.ManagedColor, cs.FontFamily, cs.FontSize, cs.IsFontBold, cs.IsFontItalic);
            
            // The MainForm listens for this event so it can refresh all colors
            this.OnColorsAndFontChanged(args);

            MessageBoxAdv.Office2010Theme = Office2010Theme.Blue;
            MessageBoxAdv.Show("Configuration saved successfully.", "Appearance Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnColorsAndFontChanged(ColorsAndFontChangedEventArgs e)
        {
            EventHandler<ColorsAndFontChangedEventArgs> handler = ColorsAndFontChanged;
            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch
                {
                    // Don't let downstream exceptions bubble up
                }
            }
        }


    }
}
