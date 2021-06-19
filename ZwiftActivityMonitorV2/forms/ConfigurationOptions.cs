using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;


namespace ZwiftActivityMonitorV2
{
    public partial class ConfigurationOptions : Syncfusion.Windows.Forms.Office2010Form
    {
        private readonly ILogger<ConfigurationOptions> m_logger;

        public ConfigurationOptions(Point ZAMWindowPos)
        {
            m_logger = ZAMsettings.LoggerFactory.CreateLogger<ConfigurationOptions>();

            InitializeComponent();

            ucStatistics.Logger = ZAMsettings.LoggerFactory.CreateLogger<StatisticsControl>();
            ucUserProfiles.Logger = ZAMsettings.LoggerFactory.CreateLogger<UserProfileControl>();
            ucSystem.Logger = ZAMsettings.LoggerFactory.CreateLogger<SystemControl>();
            //SystemControl.PacketMonitor = ZAMsettings.ZPMonitorService;
            SystemControl.ZAMWindowPos = ZAMWindowPos;

            this.Icon = Properties.Resources.ZAMicon;

            ZAMappearance.ApplyColorScheme(this);

            tabOptions.TabPanelBackColor = this.ColorTable.ActiveFormBorderColor;
            tabOptions.ActiveTabColor = this.ColorTable.ActiveFormBorderColor;
            tabOptions.ActiveTabForeColor = this.ColorTable.FormTextColor;

            tabOptions.InactiveTabColor = this.ColorTable.InactiveFormBorderColor; ;
            tabOptions.InActiveTabForeColor = this.ColorTable.FormTextColor;

            // the user controls will receive notifications that their parent's (the tabpages) colors have changed
            this.tpSystem.BackColor = this.ColorTable.FormBackground;
            this.tpSystem.ForeColor = this.ColorTable.FormTextColor;

            this.tpCollectors.BackColor = this.ColorTable.FormBackground;
            this.tpCollectors.ForeColor = this.ColorTable.FormTextColor;

            this.tpLaps.BackColor = this.ColorTable.FormBackground;
            this.tpLaps.ForeColor = this.ColorTable.FormTextColor;

            this.tpSplits.BackColor = this.ColorTable.FormBackground;
            this.tpSplits.ForeColor = this.ColorTable.FormTextColor;

            this.tpUserProfiles.BackColor = this.ColorTable.FormBackground;
            this.tpUserProfiles.ForeColor = this.ColorTable.FormTextColor;
        }

        private void ConfigurationOptions_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            // toggle the tabpage selection to get the Selecting / Selected events to fire for the initial tabpage
            tabOptions.SelectedIndex = 1;
            tabOptions.SelectedIndex = 0;
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

            ucStatistics.ControlLosingFocus(sender, e);
            args.Cancel = e.Cancel;
            if (e.Cancel)
                return;

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

        //private void tabOptions_Selecting(object sender, TabControlCancelEventArgs e)
        //{
        //    if (DesignMode)
        //        return;

        //    if (e.TabPageIndex == -1)
        //        return;

        //    m_logger.LogInformation($"TabPageName: {e.TabPage.Name} Action: {e.Action.ToString()}");

        //    if (e.Action == TabControlAction.Selecting)
        //    {
        //        switch (e.TabPage.Name)
        //        {
        //            case "tpSystem":
        //                ucSystem.ControlGainingFocus(sender, e);
        //                break;

        //            case "tpUserProfiles":
        //                ucUserProfiles.ControlGainingFocus(sender, e);
        //                break;

        //            case "tpCollectors":
        //                ucStatistics.ControlGainingFocus(sender, e);
        //                break;

        //            case "tpSplits":
        //                ucSplits.ControlGainingFocus(sender, e);
        //                break;

        //            case "tpLaps":
        //                ucLaps.ControlGainingFocus(sender, e);
        //                break;

        //            case "tpTest":
        //                break;

        //        }
        //    }

        //    if (e.Action == TabControlAction.Deselecting)
        //    {
        //        switch (e.TabPage.Name)
        //        {
        //            case "tpSystem":
        //                ucSystem.ControlLosingFocus(sender, e);
        //                break;

        //            case "tpUserProfiles":
        //                ucUserProfiles.ControlLosingFocus(sender, e);
        //                break;

        //            case "tpCollectors":
        //                ucStatistics.ControlLosingFocus(sender, e);
        //                break;

        //            case "tpSplits":
        //                ucSplits.ControlLosingFocus(sender, e);
        //                break;

        //            case "tpLaps":
        //                ucLaps.ControlLosingFocus(sender, e);
        //                break;

        //            case "tpTest":
        //                break;
        //        }
        //    }
        //}



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

            m_logger.LogInformation($"TabPageName: {e.TabPage.Name} Action: {e.Action.ToString()}");

            // we're only interested in Selected events
            if (e.Action != TabControlAction.Selected)
                return;

            switch (e.TabPage.Name)
            {
                case "tpSystem":
                    break;

                case "tpUserProfiles":
                    break;

                case "tpCollectors":
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

            Debug.WriteLine($"tabOptions_SelectedIndexChanging - TabPageName: {this.tabOptions.SelectedTab.Name}");

            switch (this.tabOptions.SelectedTab.Name)
            {
                case "tpSystem":
                    this.ucSystem.ControlLosingFocus(sender, args);
                    break;

                case "tpUserProfiles":
                    this.ucUserProfiles.ControlLosingFocus(sender, args);
                    break;

                case "tpCollectors":
                    this.ucStatistics.ControlLosingFocus(sender, args);
                    break;

                case "tpSplits":
                    this.ucSplits.ControlLosingFocus(sender, args);
                    break;

                case "tpLaps":
                    this.ucLaps.ControlLosingFocus(sender, args);
                    break;
            }

        }
        private void tabOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabOptions.SelectedTab == null)
                return;

            Debug.WriteLine($"tabOptions_SelectedIndexChanged - TabPageName: {this.tabOptions.SelectedTab.Name}");

            switch (this.tabOptions.SelectedTab.Name)
            {
                case "tpSystem":
                    this.ucSystem.ControlGainingFocus(sender, e);
                    break;

                case "tpUserProfiles":
                    this.ucUserProfiles.ControlGainingFocus(sender, e);
                    break;

                case "tpCollectors":
                    this.ucStatistics.ControlGainingFocus(sender, e);
                    break;

                case "tpSplits":
                    this.ucSplits.ControlGainingFocus(sender, e);
                    break;

                case "tpLaps":
                    this.ucLaps.ControlGainingFocus(sender, e);
                    break;
            }

        }

    }
}
