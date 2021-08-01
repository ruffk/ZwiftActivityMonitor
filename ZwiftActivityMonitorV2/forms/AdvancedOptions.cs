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
using System.Diagnostics;
using System.Drawing;

namespace ZwiftActivityMonitorV2
{
    public partial class AdvancedOptions : Syncfusion.WinForms.Controls.SfForm
    {
        private readonly ILogger<AdvancedOptions> Logger;
        private Dispatcher m_dispatcher;

        public AdvancedOptions()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<AdvancedOptions>();

            this.BackColor = Color.AliceBlue;
            this.ForeColor = Color.AliceBlue;

            MSoffice2010ColorManager colorTable = ZAMappearance.ApplyColorTable(this);

            // SfForm is not firing the property changed events so doing it manually here.
            this.AdvancedOptions_BackColorChanged(this, null);
            this.AdvancedOptions_ForeColorChanged(this, null);

            this.Icon = Properties.Resources.ZAMicon;

            UserControlBase.SetListViewHeaderColor(ref this.lvTrace, SystemColors.Control, SystemColors.ControlText);
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
            int targetPlayerId = 0;
            bool debugMode = false;
            bool simulationMode = false;

            if (cbMonitorOthers.Checked)
            {
                debugMode = rbFindByHeartRate.Checked || rbFindByPlayerId.Checked;
                simulationMode = rbSimulation.Checked;

                if (rbFindByHeartRate.Checked)
                {
                    if (!Int32.TryParse(tbTargetHeartrate.Text, out targetHr) || targetHr == 0)
                    {
                        tbTargetHeartrate.Text = "0";
                        targetHr = 0;
                    }
                }
                else if (rbFindByPlayerId.Checked)
                {
                    if (!Int32.TryParse(tbPlayerId.Text, out targetPlayerId) || targetPlayerId == 0)
                    {
                        tbPlayerId.Text = "0";
                        targetPlayerId = 0;
                    }
                }
            }

            try
            {
                ZAMsettings.ZPMonitorService.StartMonitor(debugMode, targetHr, targetPlayerId, simulationMode);

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
            Logger.LogDebug($"Load event");
            m_dispatcher = Dispatcher.CurrentDispatcher;
        }

        private void AdvancedOptions_VisibleChanged(object sender, EventArgs e)
        {
            Logger.LogDebug($"Visible event");
        }

        private void AdvancedOptions_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.LogDebug($"FormClosed event");

            ZAMsettings.ZPMonitorService.RiderStateEvent -= ProcessedRiderStateEventHandler;
        }

        private void AdvancedOptions_Shown(object sender, EventArgs e)
        {
            Logger.LogDebug($"Shown event");

            lblEventsProcessed.Text = ZAMsettings.ZPMonitorService.EventsProcessed.ToString();
            lblEthernetDevice.Text = ZAMsettings.Settings.Network;

            rbFindByHeartRate.Checked = true;

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
                if (ZAMsettings.ZPMonitorService.IsDebugMode)
                {
                    cbMonitorOthers.Checked = true;
                    if (ZAMsettings.ZPMonitorService.TargetHeartrate > 0)
                    {
                        tbTargetHeartrate.Text = ZAMsettings.ZPMonitorService.TargetHeartrate.ToString();
                        rbFindByHeartRate.Checked = true;
                    }

                    if (ZAMsettings.ZPMonitorService.TargetPlayerId > 0)
                    {
                        tbPlayerId.Text = ZAMsettings.ZPMonitorService.TargetPlayerId.ToString();
                        rbFindByPlayerId.Checked = true;
                    }
                }
                else if (ZAMsettings.ZPMonitorService.SimulateRiderActivity)
                {
                    cbMonitorOthers.Checked = true;
                    rbSimulation.Checked = true;
                }

                btnStart.Enabled = false;
                btnStop.Enabled = true;

                cbMonitorOthers.Enabled = false;
                rbFindByHeartRate.Enabled = false;
                rbFindByPlayerId.Enabled = false;
                rbSimulation.Enabled = false;
                tbTargetHeartrate.Enabled = false;
                tbPlayerId.Enabled = false;
                lblHeartrate.Enabled = false;
                lblBpm.Enabled = false;
                lblPlayerId.Enabled = false;
                tbTargetHeartrate.Enabled = false;
                tbPlayerId.Enabled = false;
                //lblWatts.Enabled = false;


                lblStatus.Text = "Running";
            }
            else
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;

                cbMonitorOthers.Enabled = true;
                rbFindByHeartRate.Enabled = true;
                rbFindByPlayerId.Enabled = true;
                rbSimulation.Enabled = true;
                tbTargetHeartrate.Enabled = true;
                tbPlayerId.Enabled = true;
                lblHeartrate.Enabled = true;
                lblBpm.Enabled = true;
                lblPlayerId.Enabled = true;
                //lblWatts.Enabled = true;

                rbFindByHeartRate.Enabled = cbMonitorOthers.Checked;
                rbFindByPlayerId.Enabled = cbMonitorOthers.Checked;
                rbSimulation.Enabled = cbMonitorOthers.Checked;
                tbTargetHeartrate.Enabled = cbMonitorOthers.Checked;
                tbPlayerId.Enabled = cbMonitorOthers.Checked;

                lblStatus.Text = "Not Running";
            }

        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            //Debug.WriteLine($"{this.GetType()}::OnBackColorChanged");
            //base.OnBackColorChanged(e);

            //this.btnClose.BackColor = this.BackColor;
            //this.btnStart.BackColor = this.BackColor;
            //this.btnStop.BackColor = this.BackColor;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            //Debug.WriteLine($"{this.GetType()}::OnForeColorChanged");
            //base.OnForeColorChanged(e);

            //this.gbZwiftPacketMonitor.ForeColor = this.ForeColor;
            //this.btnClose.ForeColor = this.ForeColor;
            //this.btnStart.ForeColor = this.ForeColor;
            //this.btnStop.ForeColor = this.ForeColor;
        }

        private void AdvancedOptions_BackColorChanged(object sender, EventArgs e)
        {
            this.btnClose.BackColor = this.BackColor;
            this.btnStart.BackColor = this.BackColor;
            this.btnStop.BackColor = this.BackColor;

            MSoffice2010ColorManager colorTable = ZAMappearance.GetColorTable();

            this.gbZwiftPacketMonitor.BorderColor = colorTable.ActiveFormBorderColor;
        }

        private void AdvancedOptions_ForeColorChanged(object sender, EventArgs e)
        {
            this.gbZwiftPacketMonitor.ForeColor = this.ForeColor;
            this.btnClose.ForeColor = this.ForeColor;
            this.btnStart.ForeColor = this.ForeColor;
            this.btnStop.ForeColor = this.ForeColor;
        }

        private void cbMonitorOthers_CheckedChanged(object sender, EventArgs e)
        {
            rbFindByHeartRate.Enabled = cbMonitorOthers.Checked;
            rbFindByPlayerId.Enabled = cbMonitorOthers.Checked;
            rbSimulation.Enabled = cbMonitorOthers.Checked;
            tbTargetHeartrate.Enabled = cbMonitorOthers.Checked;
            tbPlayerId.Enabled = cbMonitorOthers.Checked;
        }

        private void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void Listview_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

    }
}
