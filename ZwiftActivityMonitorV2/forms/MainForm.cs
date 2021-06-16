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
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Grid;
using System.Drawing.Drawing2D;

namespace ZwiftActivityMonitorV2
{

    public partial class MainForm : Syncfusion.Windows.Forms.Office2010Form, Dapplo.Microsoft.Extensions.Hosting.WinForms.IWinFormsShell
    {
        private const string HomeTitle = "Activity Monitor";

        public MainForm()
        {
            InitializeComponent();

            //toolStrip.Renderer = new ToolStripProfessionalRendererEx();
            ucColorView.ColorsAndFontChanged += ucColorView_ColorsAndFontChanged;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            SetControlColors();

            // toggle the tabs so the first tab gets initialized
            tabControl.SelectedIndex = 1;
            tabControl.SelectedIndex = 0;

            this.Size = ZAMsettings.Settings.Appearance.WindowSize;
        }
        private void ucColorView_ColorsAndFontChanged(object sender, ColorsAndFontChangedEventArgs e)
        {
            SetControlColors();
        }

        private void SetControlColors()
        {
            this.UseOffice2010SchemeBackColor = true;

            ZAMappearance settings = ZAMsettings.Settings.Appearance;

            if (settings.ThemeSetting != ThemeType.Custom)
            {
                this.ColorScheme = settings.GetOfficeColorScheme(settings.ThemeSetting, out Color? managedColor);

                if (this.ColorScheme == Office2010Theme.Managed)
                {
                    Office2010Colors.ApplyManagedColors(this, managedColor.Value);
                }
            }
            else
            {
                this.ColorScheme = Office2010Theme.Managed;
                Office2010Colors.ApplyManagedColors(this, settings.ManagedColor);
            }

            Color foreColor = this.ColorTable.FormTextColor;
            Color backColor = this.ColorTable.FormBackground;

            if (settings.TransparencySetting != TransparencyType.NotTransparent)
            {
                foreColor = (settings.TransparencySetting == TransparencyType.TransparentBlackText ? Color.Black : Color.White);
                
                backColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
                this.TransparencyKey = backColor;
            }
            else
            {
                this.TransparencyKey = Color.Empty;
            }

            this.BackColor = backColor;

            tpActivity.BackColor = backColor;
            tpSplit.BackColor = backColor;
            tpLap.BackColor = backColor;
            tpColor.BackColor = this.ColorTable.FormBackground;

            tabControl.TabPanelBackColor = this.ColorTable.ActiveFormBorderColor;
            tabControl.InactiveTabColor = this.ColorTable.ActiveFormBorderColor;
            tabControl.ActiveTabColor = this.ColorTable.ActiveFormBorderColor;

            pnBottom.BackColor = this.ColorTable.ActiveFormBorderColor;
            statusStrip.BackColor = this.ColorTable.ActiveFormBorderColor;
            tssbMenu.ForeColor = this.ColorTable.FormTextColor;
            statusLabel.ForeColor = this.ColorTable.FormTextColor;

            tpActivity.ForeColor = foreColor;
            tpSplit.ForeColor = foreColor;
            tpLap.ForeColor = foreColor;
            tpColor.ForeColor = this.ColorTable.FormTextColor;

            ucActivityView.HeaderGradientBeginColor = this.ColorTable.ActiveTitleGradientBegin;
            ucActivityView.HeaderGradientEndColor = this.ColorTable.ActiveTitleGradientEnd;
            ucActivityView.HeaderForeColor = this.ColorTable.FormTextColor;
            ucActivityView.RowBackColor = backColor;
            ucActivityView.RowForeColor = foreColor;

            ucSplitView.HeaderGradientBeginColor = this.ColorTable.ActiveTitleGradientBegin;
            ucSplitView.HeaderGradientEndColor = this.ColorTable.ActiveTitleGradientEnd;
            ucSplitView.HeaderForeColor = this.ColorTable.FormTextColor;
            ucSplitView.RowBackColor = backColor;
            ucSplitView.RowForeColor = foreColor;

            ucLapView.HeaderGradientBeginColor = this.ColorTable.ActiveTitleGradientBegin;
            ucLapView.HeaderGradientEndColor = this.ColorTable.ActiveTitleGradientEnd;
            ucLapView.HeaderForeColor = this.ColorTable.FormTextColor;
            ucLapView.RowBackColor = backColor;
            ucLapView.RowForeColor = foreColor;

            ucColorView.HeaderGradientBeginColor = this.ColorTable.ActiveTitleGradientBegin;
            ucColorView.HeaderGradientEndColor = this.ColorTable.ActiveTitleGradientEnd;
            ucColorView.HeaderForeColor = this.ColorTable.FormTextColor;
            ucColorView.RowBackColor = backColor;
            ucColorView.RowForeColor = foreColor;

            FontStyle style = 0;

            style |= settings.IsFontBold ? FontStyle.Bold : 0;
            style |= settings.IsFontItalic ? FontStyle.Italic : 0;

            Font font = new Font(settings.FontFamily, settings.FontSize, style);

            ucActivityView.RowFont  = font;
            ucSplitView.RowFont     = font;
            ucLapView.RowFont       = font;
            ucColorView.RowFont     = font;

        }

        private void tabControl_SelectedIndexChanging(object sender, SelectedIndexChangingEventArgs e)
        {
            if (this.tabControl.SelectedTab == null)
                return;

            //Debug.WriteLine($"tabControl_SelectedIndexChanging - TabPageName: {this.tabControl.SelectedTab.Name}");

            switch (this.tabControl.SelectedTab.Name)
            {
                case "tpActivity":
                    this.ucActivityView.ControlLosingFocus(sender, e);
                    break;

                case "tpSplit":
                    this.ucSplitView.ControlLosingFocus(sender, e);
                    break;

                case "tpLap":
                    this.ucLapView.ControlLosingFocus(sender, e);
                    break;

                case "tpColor":
                    // When leaving the tpColor tab, show the transparent background if set.
                    if (ZAMsettings.Settings.Appearance.TransparencySetting != TransparencyType.NotTransparent)
                    {
                        this.TransparencyKey = this.BackColor;
                    }
                    this.ucColorView.ControlLosingFocus(sender, e);
                    break;
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab == null)
                return;

            //Debug.WriteLine($"tabControl_SelectedIndexChanged - TabPageName: {this.tabControl.SelectedTab.Name}");

            switch (this.tabControl.SelectedTab.Name)
            {
                case "tpActivity":
                    this.Text = HomeTitle + "";
                    this.ucActivityView.ControlGainingFocus(sender, e);
                    break;

                case "tpSplit":
                    this.Text = HomeTitle + " (Splits)";
                    this.ucSplitView.ControlGainingFocus(sender, e);
                    break;

                case "tpLap":
                    this.Text = HomeTitle + " (Laps)";
                    this.ucLapView.ControlGainingFocus(sender, e);
                    break;

                case "tpColor":
                    this.Text = HomeTitle + " (Colors)";

                    // When changing to the tpColor tab, eliminate the transparent background if showing.
                    if (ZAMsettings.Settings.Appearance.TransparencySetting != TransparencyType.NotTransparent)
                    {
                        this.TransparencyKey = Color.Empty;
                    }
                    this.ucColorView.ControlGainingFocus(sender, e);
                    break;
            }
        }

        /// <summary>
        /// Allow user to change tab alignment position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine($"tabControl_MouseClick - Location: {e.Location}, Button: {e.Button}");

            if (e.Button != MouseButtons.Right)
                return;

            ContextMenuStrip menuStrip = new ContextMenuStrip();
            ToolStripMenuItem item;

            item = (ToolStripMenuItem)menuStrip.Items.Add(TabAlignment.Top.ToString());
            item = (ToolStripMenuItem)menuStrip.Items.Add(TabAlignment.Bottom.ToString());
            item = (ToolStripMenuItem)menuStrip.Items.Add(TabAlignment.Left.ToString());
            item = (ToolStripMenuItem)menuStrip.Items.Add(TabAlignment.Right.ToString());

            foreach (ToolStripMenuItem mi in menuStrip.Items)
            {
                TabAlignment itemAlignment = Enum.Parse<TabAlignment>(mi.Text);
                mi.CheckOnClick = true;
                mi.Checked = (tabControl.Alignment == itemAlignment);
                mi.Tag = new KeyValuePair<string, TabAlignment>("TabAlignment", itemAlignment);
                mi.ToolTipText = item.Text;
                mi.CheckedChanged += tabContextMenu_CheckStateChanged;
            }
            menuStrip.Show(Cursor.Position);


        }

        /// <summary>
        /// Set the user selected tab alignment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabContextMenu_CheckStateChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            KeyValuePair<string, TabAlignment> itemTag = (KeyValuePair<string, TabAlignment>)item.Tag;

            tabControl.Alignment = itemTag.Value;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //Debug.WriteLine($"MainForm_Resize - Size: {this.Size}");
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (this.Size != ZAMsettings.Settings.Appearance.WindowSize)
            {
                ZAMsettings.BeginCachedConfiguration();
                ZAMsettings.Settings.Appearance.WindowSize = this.Size;
                ZAMsettings.CommitCachedConfiguration();

                Debug.WriteLine($"MainForm_ResizeEnd - New window size saved, Size: {this.Size}");
            }
        }
    }
}
