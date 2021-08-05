using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;


namespace ZwiftActivityMonitorV2
{
    public partial class ConfigurationOptions : Syncfusion.WinForms.Controls.SfForm //Syncfusion.Windows.Forms.Office2010Form
    {
        private readonly ILogger<ConfigurationOptions> Logger;

        public ConfigurationOptions(Point ZAMWindowPos)
        {
            InitializeComponent();

            if (DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<ConfigurationOptions>();

            SystemControl.ZAMWindowPos = ZAMWindowPos;

            MSoffice2010ColorManager colorTable = ZAMappearance.ApplyColorTable(this);
            this.Icon = Properties.Resources.ZAMicon;

            //ZAMappearance.ApplyColorScheme(this);

            tabOptions.TabPanelBackColor = colorTable.ActiveFormBorderColor;
            tabOptions.ActiveTabColor = colorTable.ActiveFormBorderColor;
            tabOptions.ActiveTabForeColor = colorTable.FormTextColor;

            tabOptions.InactiveTabColor = colorTable.InactiveFormBorderColor; ;
            tabOptions.InActiveTabForeColor = colorTable.FormTextColor;

            // the user controls will receive notifications that their parent's (the tabpages) colors have changed
            this.tpSystem.BackColor = Color.AliceBlue;
            this.tpSystem.ForeColor = Color.AliceBlue;
            this.tpLaps.BackColor = Color.AliceBlue;
            this.tpLaps.ForeColor = Color.AliceBlue;
            this.tpSplits.BackColor = Color.AliceBlue;
            this.tpSplits.ForeColor = Color.AliceBlue;
            this.tpUserProfiles.BackColor = Color.AliceBlue;
            this.tpUserProfiles.ForeColor = Color.AliceBlue;
            this.tpGeneral.BackColor = Color.AliceBlue;
            this.tpGeneral.ForeColor = Color.AliceBlue;

            this.tpSystem.BackColor = colorTable.FormBackground;
            this.tpSystem.ForeColor = colorTable.FormTextColor;

            this.tpLaps.BackColor = colorTable.FormBackground;
            this.tpLaps.ForeColor = colorTable.FormTextColor;

            this.tpSplits.BackColor = colorTable.FormBackground;
            this.tpSplits.ForeColor = colorTable.FormTextColor;

            this.tpUserProfiles.BackColor = colorTable.FormBackground;
            this.tpUserProfiles.ForeColor = colorTable.FormTextColor;

            this.tpGeneral.BackColor = colorTable.FormBackground;
            this.tpGeneral.ForeColor = colorTable.FormTextColor;
        }

        private void ConfigurationOptions_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            // toggle the tabpage selection to get the Selecting / Selected events to fire for the initial tabpage
            tabOptions.SelectedIndex = 1;
            tabOptions.SelectedIndex = 0;

            if (string.IsNullOrEmpty(ZAMsettings.Settings.Network))
            {
                string msgText = "Quick start instructions:\n\n"
                    + "1) System tab - Select your network, save, and then start the Zwift Packet Monitor.\n\n"
                    + "2) User Profiles tab - Set the correct weight, FTP, and email address for the default user profile.\n\n"
                    + "3) Close this configuration window, select Menu->Start, and start Zwifting!\n\n\n"
                    + "Once your comfortable with ZAM, come back and set up Splits and Laps.";

                MessageBox.Show(this, msgText, "Initial Setup Guidance", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void ConfigurationOptions_FormClosing(object sender, FormClosingEventArgs args)
        {
            if (DesignMode)
                return;

            Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventArgs e = new(-1);

            ucSystem.ControlLosingFocus(sender, e);
            args.Cancel = e.Cancel;
            if (e.Cancel)
                return;

            //ucStatistics.ControlLosingFocus(sender, e);
            //args.Cancel = e.Cancel;
            //if (e.Cancel)
            //    return;

            ucUserProfiles.ControlLosingFocus(sender, e);
            args.Cancel = e.Cancel;
            if (e.Cancel)
                return;

            ucSplits.ControlLosingFocus(sender, e);
            args.Cancel = e.Cancel;
            if (e.Cancel)
                return;

            ucLaps.ControlLosingFocus(sender, e);
            args.Cancel = e.Cancel;
            if (e.Cancel)
                return;

            ucSplits.ControlLosingFocus(sender, e);
            args.Cancel = e.Cancel;
            if (e.Cancel)
                return;
        }

        private void SkipControl_Enter(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c != null && c.Parent != null)
            {
                c.Parent.SelectNextControl(c, true, true, true, true);
            }
        }

        /// <summary>
        /// This event handles TabPage Selected / Deselected events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabOptions_Selected(object sender, TabControlEventArgs e)
        {
            if (DesignMode)
                return;

            if (e.TabPageIndex == -1)
                return;

            Logger.LogDebug($"TabPageName: {e.TabPage.Name} Action: {e.Action.ToString()}");

            // we're only interested in Selected events
            if (e.Action != TabControlAction.Selected)
                return;

            switch (e.TabPage.Name)
            {
                case "tpSystem":
                    break;

                case "tpUserProfiles":
                    break;

                case "tpGeneral":
                    break;

                case "tpSplits":
                    break;

                case "tpLaps":
                    break;
            }
        }


        private void tabOptions_SelectedIndexChanging(object sender, Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventArgs args)
        {
            if (this.tabOptions.SelectedTab == null)
                return;

            Logger.LogDebug($"tabOptions_SelectedIndexChanging - TabPageName: {this.tabOptions.SelectedTab.Name}");

            switch (this.tabOptions.SelectedTab.Name)
            {
                case "tpSystem":
                    this.ucSystem.ControlLosingFocus(sender, args);
                    break;

                case "tpUserProfiles":
                    this.ucUserProfiles.ControlLosingFocus(sender, args);
                    break;

                case "tpSplits":
                    this.ucSplits.ControlLosingFocus(sender, args);
                    break;

                case "tpLaps":
                    this.ucLaps.ControlLosingFocus(sender, args);
                    break;

                case "tpGeneral":
                    this.ucGeneral.ControlLosingFocus(sender, args);
                    break;
            }

        }
        private void tabOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabOptions.SelectedTab == null)
                return;

            Logger.LogDebug($"tabOptions_SelectedIndexChanged - TabPageName: {this.tabOptions.SelectedTab.Name}");

            switch (this.tabOptions.SelectedTab.Name)
            {
                case "tpSystem":
                    this.ucSystem.ControlGainingFocus(sender, e);
                    break;

                case "tpUserProfiles":
                    this.ucUserProfiles.ControlGainingFocus(sender, e);
                    break;

                case "tpSplits":
                    this.ucSplits.ControlGainingFocus(sender, e);
                    break;

                case "tpLaps":
                    this.ucLaps.ControlGainingFocus(sender, e);
                    break;

                case "tpGeneral":
                    this.ucGeneral.ControlGainingFocus(sender, e);
                    break;
            }
        }
    }
}
