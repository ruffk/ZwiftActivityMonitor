using System;
using System.Windows.Threading;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using ZwiftPacketMonitor;

namespace ZwiftActivityMonitor
{
    public partial class AdvancedOptions : Form, IWinFormsShell
    {
        private readonly ZPMonitorService m_zpMonitorService;
        private readonly ILogger<AdvancedOptions> m_logger;
        private Dispatcher m_dispatcher;

        public AdvancedOptions(ZPMonitorService zPMonitorService, ILogger<AdvancedOptions> logger)
        {
            m_zpMonitorService = zPMonitorService;
            m_logger = logger;

            InitializeComponent();

            m_logger.LogInformation($"Class {this.GetType()} initialized.");
        }

        private delegate void ProcessedPlayerStateEventHandlerDelegate(object sender, PlayerStateEventArgs e);

        private void ProcessedPlayerStateEventHandler(object sender, PlayerStateEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new ProcessedPlayerStateEventHandlerDelegate(ProcessedPlayerStateEventHandler), new object[] { sender, e });
                return;
            }

            lblEventsProcessed.Text = m_zpMonitorService.EventsProcessed.ToString();

            string[] row = { e.PlayerState.Id.ToString(), e.PlayerState.Power.ToString(), e.PlayerState.Heartrate.ToString(), DateTime.Now.ToString() };

            lvTrace.Items.Insert(0, new ListViewItem(row));

            if (lvTrace.Items.Count > 10)
            {
                lvTrace.Items.RemoveAt(10);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int targetHr = 0;
            int targetPower = 0;

            bool debugMode = cbMonitorOthers.Checked;

            if (rbFindByMetrics.Checked)
            {
                if (!Int32.TryParse(tbTargetHeartrate.Text, out targetHr) || targetHr == 0)
                {
                    tbTargetHeartrate.Text = "0";
                    targetHr = 0;
                }
                if (!Int32.TryParse(tbTargetPower.Text, out targetPower) || targetPower == 0)
                {
                    tbTargetPower.Text = "0";
                    targetPower = 0;
                }
            }

            m_zpMonitorService.StartMonitor(debugMode, targetHr, targetPower);

            OnMonitorStatusChanged();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_zpMonitorService.StopMonitor();

            OnMonitorStatusChanged();
        }

        private void AdvancedOptions_Load(object sender, EventArgs e)
        {
            m_logger.LogInformation($"Load event");
            m_dispatcher = Dispatcher.CurrentDispatcher;
        }

        private void AdvancedOptions_VisibleChanged(object sender, EventArgs e)
        {
            m_logger.LogInformation($"Visible event");
        }

        private void AdvancedOptions_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_logger.LogInformation($"FormClosed event");

            m_zpMonitorService.PlayerStateEvent -= ProcessedPlayerStateEventHandler;
        }

        private void AdvancedOptions_Shown(object sender, EventArgs e)
        {
            m_logger.LogInformation($"Shown event");

            lblEventsProcessed.Text = m_zpMonitorService.EventsProcessed.ToString();
            lblEthernetDevice.Text = m_zpMonitorService.Network;

            rbFindByMetrics.Checked = true;

            // Set controls based on started/stopped
            OnMonitorStatusChanged();

            m_zpMonitorService.PlayerStateEvent += ProcessedPlayerStateEventHandler;

        }

        private void OnMonitorStatusChanged()
        {
            if (m_zpMonitorService.IsStarted)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                cbMonitorOthers.Enabled = false;
                rbFindByMetrics.Enabled = false;
                rbRandomlyChoose.Enabled = false;
                tbTargetHeartrate.Enabled = false;
                tbTargetPower.Enabled = false;
                lblHeartrate.Enabled = false;
                lblBpm.Enabled = false;
                lblPower.Enabled = false;
                lblWatts.Enabled = false;


                lblStatus.Text = "Running";

                if (m_zpMonitorService.IsDebugMode)
                {
                    cbMonitorOthers.Checked = true;
                    if (m_zpMonitorService.TargetPower > 0 || m_zpMonitorService.TargetHeartrate > 0)
                    {
                        tbTargetHeartrate.Text = m_zpMonitorService.TargetHeartrate.ToString();
                        tbTargetPower.Text = m_zpMonitorService.TargetPower.ToString();
                        rbFindByMetrics.Checked = true;
                    }
                }
            }
            else
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;

                cbMonitorOthers.Enabled = true;
                rbFindByMetrics.Enabled = true;
                rbRandomlyChoose.Enabled = true;
                tbTargetHeartrate.Enabled = true;
                tbTargetPower.Enabled = true;
                lblHeartrate.Enabled = true;
                lblBpm.Enabled = true;
                lblPower.Enabled = true;
                lblWatts.Enabled = true;

                lblStatus.Text = "Not Running";
            }

        }
    }
}
