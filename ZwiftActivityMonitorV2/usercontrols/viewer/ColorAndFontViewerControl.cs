using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Extensions.Logging;


using Syncfusion.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Linq;

namespace ZwiftActivityMonitorV2
{
    public partial class ColorAndFontViewerControl : ViewerControlEx
    {
        private bool InitializingControls { get; set; } = false;
        private readonly ILogger<ColorAndFontViewerControl> Logger;


        public event EventHandler<ColorsAndFontChangedEventArgs> ColorsAndFontChanged;

        public ColorAndFontViewerControl()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<ColorAndFontViewerControl>();


            this.cbFonts.Fill();

            this.btnColor.ColorGroups = ((Syncfusion.Windows.Forms.ColorUIGroups)
                (Syncfusion.Windows.Forms.ColorUIGroups.StandardColors | Syncfusion.Windows.Forms.ColorUIGroups.CustomColors)
                );
        }

        private void ColorAndFontViewControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            Logger.LogDebug($"{this.GetType()}.ViewControl_Load");

            ZAMappearance settings = ZAMsettings.Settings.Appearance;

            this.BeginInitializingControls();

            cbTheme.DataSource = new BindingSource(settings.ThemeItems, null);

            cbTransparency.DataSource = new BindingSource(settings.TransparencyItems, null);

            this.EndInitializingControls();

            sfToolTip.SetToolTip(this.btnAccept, "Save and apply selections.");
        }

        /// <summary>
        /// This control behaves a bit differently from the configuration tabs.  It always initializes itself to whatever the current settings are.
        /// If the user switches tabs without saving, there is no warning and selections are lost.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ControlGainingFocus(object sender, EventArgs e)
        {
            base.ControlGainingFocus(sender, e);

            ZAMappearance settings = ZAMsettings.Settings.Appearance;

            this.BeginInitializingControls();

            nudFontSize.Value = nudFontSize.Minimum;
            nudFontSize.Value = (int)settings.FontSize;

            if (settings.Theme.HasValue)
                cbTheme.SelectedValue = settings.Theme.Value.Value;

            if (settings.Transparency.HasValue)
                cbTransparency.SelectedValue = settings.Transparency.Value.Value;

            this.btnColor.SelectedColor = settings.ManagedColor;

            this.cbFonts.SelectedIndex = cbFonts.Items.IndexOf(settings.FontFamily);
            //this.cbFonts.SelectedItem = settings.FontFamily;

            cbBold.Checked = settings.IsFontBold;
            cbItalic.Checked = settings.IsFontItalic;

            this.EndInitializingControls();
        }

        /// <summary>
        /// While initializing, the controls won't update.  This is used during initial setup of the control.
        /// </summary>
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

            FontStyle style = 0;

            style |= cbBold.Checked ? FontStyle.Bold : 0;
            style |= cbItalic.Checked ? FontStyle.Italic : 0;

            Font font = new Font(this.cbFonts.SelectedItem.ToString(), (float)this.nudFontSize.Value, style);
            lblSample.Font = font;


            var selection = (KeyValuePair<ThemeType, string>)cbTheme.SelectedItem;

            MSoffice2010ColorManager colorTable;

            if (selection.Key != ThemeType.Custom)
            {
                MSoffice2010Theme theme = ZAMsettings.Settings.Appearance.GetMSoffice2010Theme(selection.Key, out Color? managedColor);

                if (theme == MSoffice2010Theme.Managed)
                {
                    // store managed colors in the static object
                    MSoffice2010ColorManager.ApplyManagedColors(managedColor.Value);
                    colorTable = MSoffice2010ColorManager.GetColorTable(MSoffice2010Theme.Managed);
                }
                else
                {
                    colorTable = MSoffice2010ColorManager.GetColorTable(theme);
                }
            }
            else
            {
                // store managed colors in the static object
                MSoffice2010ColorManager.ApplyManagedColors(btnColor.SelectedColor);
                colorTable = MSoffice2010ColorManager.GetColorTable(MSoffice2010Theme.Managed);
            }

            this.panel1.BackColor = colorTable.FormBackground;
            this.panel1.ForeColor = colorTable.FormTextColor;
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

            //Logger.LogDebug($"cbTheme_SelectedIndexChanged - cbTheme.SelectedItem: {this.cbTheme.SelectedItem}");

            var selection = (KeyValuePair<ThemeType, string>)cbTheme.SelectedItem;

            btnColor.Enabled = (selection.Key == ThemeType.Custom);

            AdjustFont();
        }

        private void cbTransparency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbTransparency.SelectedItem == null)
                return;

            //Logger.LogDebug($"cbTransparency_SelectedIndexChanged - cbTransparency.SelectedItem: {this.cbTransparency.SelectedItem}");

            AdjustFont();
        }

        private void btnColor_ColorSelected(object sender, EventArgs e)
        {
            //Logger.LogDebug($"btnColor_ColorSelected - {btnColor.SelectedColor}");

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
            if (cbTheme.SelectedItem == null || cbTransparency.SelectedItem == null || cbFonts.SelectedItem == null)
            {
                MessageBox.Show(this.ParentForm, "Please make sure to select a valid theme, transparency, and font before saving.", "Unable to save", MessageBoxButtons.OK);
                return;
            }

            Logger.LogDebug($"Theme: {cbTheme.SelectedValue}, ManagedColor: {btnColor.SelectedColor}, Font: {cbFonts.SelectedItem} Transparency: {cbTransparency.SelectedValue}");

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

            this.sfToolTip.Show("Configuration saved.", Cursor.Position, 2000);
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
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnColorsAndFontChanged)");
                }
            }
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down)
                e.SuppressKeyPress = true;
        }

    }
}
