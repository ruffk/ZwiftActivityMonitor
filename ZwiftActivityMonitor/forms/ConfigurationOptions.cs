using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


namespace ZwiftActivityMonitor
{
    public partial class ConfigurationOptions : Form
    {
        private readonly ILogger<ConfigurationOptions> m_logger;

        public ConfigurationOptions(IServiceProvider serviceProvider, Point ZAMWindowPos)
        {
            m_logger = ZAMsettings.LoggerFactory.CreateLogger<ConfigurationOptions>();

            InitializeComponent();

            ucStatistics.Logger = ZAMsettings.LoggerFactory.CreateLogger<StatisticsControl>();
            ucUserProfiles.Logger = ZAMsettings.LoggerFactory.CreateLogger<UserProfileControl>();
            ucSystem.Logger = ZAMsettings.LoggerFactory.CreateLogger<SystemControl>();
            SystemControl.PacketMonitor = ZAMsettings.ZPMonitorService;
            SystemControl.ZAMWindowPos = ZAMWindowPos;

            this.Icon = Properties.Resources.cycling1;
        }

        private void ConfigurationOptions_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            // toggle the tabpage selection to get the Selecting / Selected events to fire for the initial tabpage
            tabOptions.SelectedIndex = -1;
            tabOptions.SelectedIndex = 0;
        }

        private void ConfigurationOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DesignMode)
                return;

            ucSystem.ControlLosingFocus(sender, e);
            if (e.Cancel)
                return;

            ucStatistics.ControlLosingFocus(sender, e);
            if (e.Cancel)
                return;

            ucUserProfiles.ControlLosingFocus(sender, e);
            if (e.Cancel)
                return;

            ucSplits.ControlLosingFocus(sender, e);
            if (e.Cancel)
                return;

            ucLaps.ControlLosingFocus(sender, e);
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

        private void tabOptions_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (DesignMode)
                return;

            if (e.TabPageIndex == -1)
                return;

            m_logger.LogInformation($"TabPageName: {e.TabPage.Name} Action: {e.Action.ToString()}");

            if (e.Action == TabControlAction.Selecting)
            {
                switch (e.TabPage.Name)
                {
                    case "tpSystem":
                        ucSystem.ControlGainingFocus(sender, e);
                        break;

                    case "tpUserProfiles":
                        ucUserProfiles.ControlGainingFocus(sender, e);
                        break;

                    case "tpCollectors":
                        ucStatistics.ControlGainingFocus(sender, e);
                        break;

                    case "tpSplits":
                        ucSplits.ControlGainingFocus(sender, e);
                        break;

                    case "tpLaps":
                        ucLaps.ControlGainingFocus(sender, e);
                        break;

                }
            }

            if (e.Action == TabControlAction.Deselecting)
            {
                switch (e.TabPage.Name)
                {
                    case "tpSystem":
                        ucSystem.ControlLosingFocus(sender, e);
                        break;

                    case "tpUserProfiles":
                        ucUserProfiles.ControlLosingFocus(sender, e);
                        break;

                    case "tpCollectors":
                        ucStatistics.ControlLosingFocus(sender, e);
                        break;

                    case "tpSplits":
                        ucSplits.ControlLosingFocus(sender, e);
                        break;

                    case "tpLaps":
                        ucLaps.ControlLosingFocus(sender, e);
                        break;
                }
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
    }
}
