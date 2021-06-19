﻿using System;
using System.Windows.Threading;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using ZwiftPacketMonitor;

namespace ZwiftActivityMonitorV2
{
    public partial class AdvancedOptions : Form
    {
        private readonly ILogger<AdvancedOptions> Logger;
        private Dispatcher m_dispatcher;

        public AdvancedOptions()
        {
            Logger = ZAMsettings.LoggerFactory.CreateLogger<AdvancedOptions>();

            InitializeComponent();

            this.Icon = Properties.Resources.ZAMicon;
        }

        private delegate void ProcessedRiderStateEventHandlerDelegate(object sender, RiderStateEventArgs e);

        private void ProcessedRiderStateEventHandler(object sender, RiderStateEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new ProcessedRiderStateEventHandlerDelegate(ProcessedRiderStateEventHandler), new object[] { sender, e });
                return;
            }

            lblEventsProcessed.Text = ZAMsettings.ZPMonitorService.EventsProcessed.ToString();

            string[] row = { e.Id.ToString(), e.Power.ToString(), e.Heartrate.ToString(), DateTime.Now.ToString() };

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

            try
            {
                ZAMsettings.ZPMonitorService.StartMonitor(debugMode, targetHr, targetPower);

                OnMonitorStatusChanged();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"{ex.Message}.\r\r1) Run command IpConfig /all at a windows CMD prompt to find your network.\r2) Update the ZwiftPacketMonitor:Network key in appsettings.json", "ZwiftPacketMonitor Not Started", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred: " + ex.ToString(), "ZwiftPacketMonitor Not Started", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            ZAMsettings.ZPMonitorService.StopMonitor();

            OnMonitorStatusChanged();
        }

        private void AdvancedOptions_Load(object sender, EventArgs e)
        {
            Logger.LogInformation($"Load event");
            m_dispatcher = Dispatcher.CurrentDispatcher;
        }

        private void AdvancedOptions_VisibleChanged(object sender, EventArgs e)
        {
            Logger.LogInformation($"Visible event");
        }

        private void AdvancedOptions_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.LogInformation($"FormClosed event");

            ZAMsettings.ZPMonitorService.RiderStateEvent -= ProcessedRiderStateEventHandler;
        }

        private void AdvancedOptions_Shown(object sender, EventArgs e)
        {
            Logger.LogInformation($"Shown event");

            lblEventsProcessed.Text = ZAMsettings.ZPMonitorService.EventsProcessed.ToString();
            lblEthernetDevice.Text = ZAMsettings.Settings.Network;

            rbFindByMetrics.Checked = true;

            // Set controls based on started/stopped
            OnMonitorStatusChanged();

            ZAMsettings.ZPMonitorService.RiderStateEvent += ProcessedRiderStateEventHandler;

            //if (m_zpMonitorService.IsAutoStart)
            //{
            //    btnStart.PerformClick();
            //}

        }

        private void OnMonitorStatusChanged()
        {
            if (ZAMsettings.ZPMonitorService.IsZPMonitorStarted)
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

                if (ZAMsettings.ZPMonitorService.IsDebugMode)
                {
                    cbMonitorOthers.Checked = true;
                    if (ZAMsettings.ZPMonitorService.TargetPower > 0 || ZAMsettings.ZPMonitorService.TargetHeartrate > 0)
                    {
                        tbTargetHeartrate.Text = ZAMsettings.ZPMonitorService.TargetHeartrate.ToString();
                        tbTargetPower.Text = ZAMsettings.ZPMonitorService.TargetPower.ToString();
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