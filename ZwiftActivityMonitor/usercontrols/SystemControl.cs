using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;

namespace ZwiftActivityMonitor
{
    public partial class SystemControl : UserControlBase
    {
        public static ZPMonitorService PacketMonitor { get; set; }
        public static Point ZAMWindowPos { get;  set; }

        private Dispatcher m_dispatcher;
        private bool m_editMode;

        public SystemControl()
        {
            InitializeComponent();
        }

        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            base.UserControlBase_Load(sender, e);

            if (DesignMode)
                return;

            m_dispatcher = Dispatcher.CurrentDispatcher;

            // Load available network devices
            try
            {
                List<NetworkListItem> list = ZAMsettings.Networks;
                cbNetwork.Items.AddRange(list.ToArray());
            }
            catch (Exception ex)
            {
                // Display error regarding need for elevated permissions
                MessageBox.Show(ex.Message, "Error accessing system network devices", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Load values from configuration into fields
            //SystemSettings_LoadFields();

            //SetMonitorButtonStatus();

            PacketMonitor.RiderStateEvent += ProcessedRiderStateEventHandler;

            // initialize
            EditingSystemSettings = false;
        }

        //protected override void SkipControl_Enter(object sender, EventArgs e)
        //{
        //    base.SkipControl_Enter(sender, e);
        //}

        public override void ControlLosingFocus(object sender, CancelEventArgs e)
        {
            base.ControlLosingFocus(sender, e);

            if (EditingSystemSettings)
            {
                MessageBox.Show("Please either Save or Cancel current work before proceeding.", "Pending Changes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
        public override void ControlGainingFocus(object sender, CancelEventArgs e)
        {
            base.ControlGainingFocus(sender, e);

            // Reload each time control is shown as user profile info may have changed.
            SystemSettings_LoadFields();

            //ActiveControl = this;
            btnEditSettings.Focus();
        }


        #region SystemSettings

        private void SystemSettings_LoadFields()
        {
            foreach (NetworkListItem i in cbNetwork.Items)
                if (i.Network == ZAMsettings.Settings.Network)
                {
                    cbNetwork.SelectedItem = i;
                    break;
                }


            // Load the available users
            cbCurrentUser.BeginUpdate();
            cbCurrentUser.Items.Clear();
            cbCurrentUser.Items.AddRange(ZAMsettings.Settings.GetUsers.ToArray());
            foreach (UserProfile user in cbCurrentUser.Items)
                if (user.UniqueId == ZAMsettings.Settings.CurrentUserProfile)
                {
                    cbCurrentUser.SelectedItem = user;
                    break;
                }
            cbCurrentUser.EndUpdate();

            ckbAutoStart.Checked = ZAMsettings.Settings.AutoStart;

            if (ZAMsettings.Settings.WindowPositionX > 0)
                tbWindowPosX.Text = ZAMsettings.Settings.WindowPositionX.ToString();

            if (ZAMsettings.Settings.WindowPositionY > 0)
                tbWindowPosY.Text = ZAMsettings.Settings.WindowPositionY.ToString();

            // Current position of Zwift Activity Monitor window (for user reference)
            tbCurWindowPosX.Text = ZAMWindowPos.X.ToString();
            tbCurWindowPosY.Text = ZAMWindowPos.Y.ToString();

            SetMonitorButtonStatus();
        }

        private void btnEditSettings_Click(object sender, EventArgs e)
        {
            ZAMsettings.BeginCachedConfiguration();
            EditingSystemSettings = true;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            bool errorOccurred = false;

            errorOccurred = (errorOccurred || ValidateSystemSettings(tbWindowPosX));
            errorOccurred = (errorOccurred || ValidateSystemSettings(tbWindowPosY));
            errorOccurred = (errorOccurred || ValidateSystemSettings(cbNetwork));
            errorOccurred = (errorOccurred || ValidateSystemSettings(cbCurrentUser));
            errorOccurred = (errorOccurred || ValidateSystemSettings(ckbAutoStart));

            if (!errorOccurred)
            {
                ZAMsettings.CommitCachedConfiguration();
                EditingSystemSettings = false;
            }
        }

        private void btnCancelSettings_Click(object sender, EventArgs e)
        {
            ZAMsettings.RollbackCachedConfiguration();

            errorProvider.Clear();

            // Reload values from configuration into fields since cancel was pressed
            SystemSettings_LoadFields();

            EditingSystemSettings = false;
        }


        private bool EditingSystemSettings
        {
            set
            {
                btnEditSettings.Enabled = !value;
                btnSaveSettings.Enabled = value;
                btnCancelSettings.Enabled = value;

                cbCurrentUser.Enabled = value;
                cbNetwork.Enabled = value;
                ckbAutoStart.Enabled = value;
                tbWindowPosX.Enabled = value;
                tbWindowPosY.Enabled = value;
                tbCurWindowPosX.Enabled = value;
                tbCurWindowPosY.Enabled = value;

                m_editMode = value;
            }

            get { return m_editMode; }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                PacketMonitor.StartMonitor();
                SetMonitorButtonStatus();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"{ex.Message}.\r\rRun command IpConfig /all at a windows CMD prompt to find your network.", "Packet Monitor Failed to Start", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred: " + ex.ToString(), "Packet Monitor Failed to Start", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            PacketMonitor.StopMonitor();
            SetMonitorButtonStatus();
        }

        private void SetMonitorButtonStatus()
        {
            if (PacketMonitor.IsStarted)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                ckbRunning.Checked = true;
            }
            else
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                ckbRunning.Checked = false;
            }
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

            lblEventsProcessed.Text = PacketMonitor.EventsProcessed.ToString();

            string[] row = { e.Id.ToString(), e.Power.ToString(), e.Heartrate.ToString(), DateTime.Now.ToString() };

            lvTrace.Items.Insert(0, new ListViewItem(row));

            if (lvTrace.Items.Count > 10)
            {
                lvTrace.Items.RemoveAt(10);
            }
        }


        private void SystemSettings_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = ValidateSystemSettings(sender as Control);
        }

        private bool ValidateSystemSettings(Control control)
        {
            bool errorOccurred = false;

            errorProvider.SetError(control, "");

            switch (control.Name)
            {
                case "tbWindowPosX":
                    try
                    {
                        tbWindowPosX.Text = tbWindowPosX.Text.Trim();
                        ZAMsettings.Settings.WindowPositionX = int.Parse(tbWindowPosX.Text == "" ? "0" : tbWindowPosX.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbWindowPosY":
                    try
                    {
                        tbWindowPosY.Text = tbWindowPosY.Text.Trim();
                        ZAMsettings.Settings.WindowPositionY = int.Parse(tbWindowPosY.Text == "" ? "0" : tbWindowPosY.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "cbNetwork":
                    try
                    {
                        if (cbNetwork.SelectedItem != null)
                        {
                            ZAMsettings.Settings.Network = (cbNetwork.SelectedItem as NetworkListItem).Network;
                        }
                        else
                        {
                            throw new ApplicationException("Please select a network device.");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "cbCurrentUser":
                    try
                    {
                        if (cbCurrentUser.SelectedItem != null)
                        {
                            ZAMsettings.Settings.CurrentUserProfile = (cbCurrentUser.SelectedItem as UserProfile).UniqueId;
                        }
                        else
                        {
                            throw new ApplicationException("Please select a user profile.");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "ckbAutoStart":
                    try
                    {
                        ZAMsettings.Settings.AutoStart = ckbAutoStart.Checked;
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                default:
                    Debug.Assert(1 == 0, $"Unknown control {control.Name} passed to validate method.");
                    break;
            }

            return errorOccurred;
        }

        public void SystemSettings_TooltipOnEnter(object sender, EventArgs e)
        {
            HandleTooltipsSystemSettings(sender as Control, true);
        }
        public void SystemSettings_TooltipOnLeave(object sender, EventArgs e)
        {
            HandleTooltipsSystemSettings(sender as Control, false);
        }

        public void HandleTooltipsSystemSettings(Control control, bool isEntering)
        {
            if (!isEntering)
            {
                toolStripStatusLabel.Text = "";
                return;
            }

            switch (control.Name)
            {
                case "tbWindowPosX":
                    toolStripStatusLabel.Text = "Location where the Zwift Activity Monitor window will appear on startup.  Set the (X,Y) coordinate values.";
                    break;

                case "tbWindowPosY":
                    toolStripStatusLabel.Text = "Location where the Zwift Activity Monitor window will appear on startup.  Set the (X,Y) coordinate values.";
                    break;

                case "cbNetwork":
                    toolStripStatusLabel.Text = "Choose the network that you use when running Zwift on this computer.";
                    break;

                case "cbCurrentUser":
                    toolStripStatusLabel.Text = "Choose the user profile to use during activity monitoring.";
                    break;

                case "ckbAutoStart":
                    toolStripStatusLabel.Text = "If checked, the Zwift Packet Monitor will run automatically on startup using the configured Network setting.";
                    break;
            }

        }

        #endregion

    }
}
