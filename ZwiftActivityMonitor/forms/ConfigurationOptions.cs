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

        /*

        internal class UserProfileListViewItem : ListViewItem
        {
            private UserProfile m_userProfile;

            public UserProfileListViewItem() : base(new string[] { "*** New User Profile ***" })
            {
                m_userProfile = new UserProfile();

                m_userProfile.Weight = 165;
                m_userProfile.PowerThreshold = 100;
                m_userProfile.DefaultCollectors.Add("5 sec", true);
                m_userProfile.DefaultCollectors.Add("5 min", true);
                m_userProfile.DefaultCollectors.Add("20 min", true);
            }
            public UserProfileListViewItem(UserProfile userProfile) : base(SubItemStrings(userProfile))
            {
                m_userProfile = (UserProfile)userProfile.Clone();
            }

            private static string[] SubItemStrings(UserProfile user)
            {
                return (new string[]
                {
                    user.Name,
                    user.Default ? "Yes" : "No",
                    user.Weight.ToString() + " " + (user.WeightInKgs ? "kgs" : "lbs"),
                    user.PowerThreshold.ToString()
                });
            }

            public void Refresh()
            {
                // Set each SubItem individually.  Cannot Clear and AddRange as it messes up.
                string[] text = SubItemStrings(m_userProfile);

                for (int i = 0; i < base.SubItems.Count; i++)
                    base.SubItems[i] = new ListViewSubItem(this, text[i]);
            }

            public UserProfile UserProfile
            {
                get
                {
                    return m_userProfile;
                }
                set
                {
                    m_userProfile = (UserProfile)value.Clone();
                }
            }
        }

        */


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
            }
        }


        /*
        #region UserProfiles

        /// <summary>
        /// Occurs once on load of form
        /// </summary>
        private void UserProfiles_Load()
        {
            EditingUserProfiles = false; // initialize

            List<UserProfile> users = ZAMsettings.Settings.GetUsers;

            lvUserProfiles.Items.Clear();
            users.ForEach(user => 
            { 
                lvUserProfiles.Items.Add(new UserProfileListViewItem(user)); 
            });
            
            if (lvUserProfiles.Items.Count > 0)
            {
                lvUserProfiles.Items[0].Selected = true;
            }

          

            // Get collectors and add names only to the checked list box
            List<Collector> collectors = ZAMsettings.Settings.GetCollectors;

            collectors.ForEach(collector => clbCollectors.Items.Add(collector.Name));

        }

        /// <summary>
        /// Used to update display fields when selection changes
        /// </summary>
        /// <param name="user"></param>
        private void UserProfiles_LoadFields(UserProfile user)
        {
            if (user != null)
            {
                tbName.Text = user.Name;
                ckbDefault.Checked = user.Default;
                tbWeight.Text = user.Weight.ToString();
                rbKgs.Checked = user.WeightInKgs;
                rbLbs.Checked = !user.WeightInKgs;
                nPowerThreshold.Value = user.PowerThreshold;

                for (int i = 0; i<clbCollectors.Items.Count; i++)
                {
                    if (!user.DefaultCollectors.TryGetValue(clbCollectors.Items[i].ToString(), out bool setting))
                        setting = false;

                    clbCollectors.SetItemChecked(i, setting);
                }
            }
        }

        private bool EditingUserProfiles
        {
            set
            {
                btnAddProfile.Enabled = !value;
                btnCancelProfile.Enabled = value;
                btnSaveProfile.Enabled = value;

                // some things you can only do if an item is selected
                if (lvUserProfiles.SelectedItems.Count > 0)
                {
                    btnEditProfile.Enabled = !value;
                    btnRemoveProfile.Enabled = !value;
                }
                else
                {
                    btnEditProfile.Enabled = false;
                    btnRemoveProfile.Enabled = false;
                }

                // when editing, you can't change the selection
                lvUserProfiles.Enabled = !value;

                tbName.Enabled = value;
                ckbDefault.Enabled = value;
                tbWeight.Enabled = value;
                rbKgs.Enabled = value;
                rbLbs.Enabled = value;
                nPowerThreshold.Enabled = value;
                clbCollectors.Enabled = value;

                m_logger.LogInformation($"EditingUserProfiles: {value}, SelectedItemsCount: {lvUserProfiles.SelectedItems.Count}");
            }

            get { return btnSaveProfile.Enabled; }
        }

        private void lvUserProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbName.Text = "";
            tbWeight.Text = "";
            ckbDefault.Checked = false;
            nPowerThreshold.Value = nPowerThreshold.Minimum;

            if (lvUserProfiles.SelectedItems.Count > 0)
            {
                UserProfile user = ((UserProfileListViewItem)lvUserProfiles.SelectedItems[0]).UserProfile;

                UserProfiles_LoadFields(user);

                m_logger.LogInformation($"SelectedIndexChanged {user.Name} selected.");
            }
            else
            {
                m_logger.LogInformation($"SelectedIndexChanged nothing selected.");
            }

            EditingUserProfiles = false;
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            if (lvUserProfiles.SelectedItems.Count > 0)
            {
                UserProfileListViewItem itemBeingEdited = (UserProfileListViewItem)lvUserProfiles.SelectedItems[0];
                UserProfile userBeingEdited = itemBeingEdited.UserProfile;

                //userBeingEdited.BeginEdit(); // save current values
            }
            
            ZAMsettings.BeginCachedConfiguration();
            EditingUserProfiles = true;
        }

        private void btnAddProfile_Click(object sender, EventArgs e)
        {
            UserProfileListViewItem item = new UserProfileListViewItem();
            lvUserProfiles.Items.Add(item);
            item.Selected = true;

            ZAMsettings.BeginCachedConfiguration();
            EditingUserProfiles = true;
        }

        private void btnRemoveProfile_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            bool errorOccurred = false;

            // this should not happen
            if (lvUserProfiles.SelectedItems.Count < 1)
            {
                ZAMsettings.RollbackCachedConfiguration();
                EditingSystemSettings = false;
                return;
            }

            UserProfileListViewItem itemBeingEdited = (UserProfileListViewItem)lvUserProfiles.SelectedItems[0];
            UserProfile userBeingEdited = itemBeingEdited.UserProfile;

            errorOccurred = (errorOccurred || ValidateUserProfiles(tbName));
            errorOccurred = (errorOccurred || ValidateUserProfiles(ckbDefault));
            errorOccurred = (errorOccurred || ValidateUserProfiles(tbWeight));
            errorOccurred = (errorOccurred || ValidateUserProfiles(rbLbs));
            errorOccurred = (errorOccurred || ValidateUserProfiles(rbKgs));
            errorOccurred = (errorOccurred || ValidateUserProfiles(nPowerThreshold));
            errorOccurred = (errorOccurred || ValidateUserProfiles(clbCollectors));

            if (!errorOccurred)
            {
                try
                {

                    // Add or Update the UserProfile dictionary in the configuration.  
                    ZAMsettings.Settings.UpsertUserProfile(userBeingEdited);

                    ZAMsettings.CommitCachedConfiguration();

                    //UserProfile newItem = ZAMsettings.Settings.UserProfiles[userBeingEdited.UniqueId];
                    UserProfileListViewItem lvi = new UserProfileListViewItem(userBeingEdited);
                    lvUserProfiles.Items.Add(lvi);
                    lvi.Selected = true;

                    lvUserProfiles.Items.Remove(itemBeingEdited);

                    foreach (UserProfileListViewItem item in lvUserProfiles.Items)
                    {
                        bool isDefault = (item.UserProfile.UniqueId == ZAMsettings.Settings.DefaultUserProfile);

                        if (item.UserProfile.Default != isDefault)
                        {
                            item.UserProfile.Default = isDefault;
                            item.Refresh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception occurred: " + ex.ToString(), "Error saving User Profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    errorOccurred = true;
                }
                finally
                {
                    EditingSystemSettings = false;
                }
            }
        }

        private void btnCancelProfile_Click(object sender, EventArgs e)
        {
            ZAMsettings.RollbackCachedConfiguration();

            errorProvider.Clear();

            if (lvUserProfiles.SelectedItems.Count > 0)
            {
                UserProfileListViewItem itemBeingEdited = (UserProfileListViewItem)lvUserProfiles.SelectedItems[0];
                UserProfile userBeingEdited = itemBeingEdited.UserProfile;

                // If a newly added row, remove it and select the first one in list
                if (userBeingEdited.UniqueId.Length == 0)
                {
                    lvUserProfiles.Items.Remove(itemBeingEdited);
                    if (lvUserProfiles.Items.Count > 0)
                    {
                        lvUserProfiles.Items[0].Selected = true;
                    }
                }
                else
                {
                    // Refresh user from rolled back configuration
                    UserProfile refreshUser = ZAMsettings.Settings.UserProfiles[userBeingEdited.UniqueId];
                    UserProfiles_LoadFields(refreshUser);

                    // This will clone the UserProfile so that the listview doesn't have a direct link to the configuration
                    itemBeingEdited.UserProfile = refreshUser;
                }
            }

            EditingUserProfiles = false;
        }


        private void UserProfiles_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = ValidateUserProfiles(sender as Control);
        }

        private bool ValidateUserProfiles(Control control)
        {
            bool errorOccurred = false;

            errorProvider.SetError(control, "");

            if (lvUserProfiles.SelectedItems.Count < 1)
            {
                errorProvider.SetError(lvUserProfiles, "Internal error occurred.  No item in list view is selected.");
                return false;
            }

            //if (this.m_userMightBeCanceling)
            //    return false;

            UserProfile user = ((UserProfileListViewItem)lvUserProfiles.SelectedItems[0]).UserProfile;

            switch (control.Name)
            {
                case "tbName":
                    try
                    {
                        tbName.Text = tbName.Text.Trim();
                        user.Name = tbName.Text;
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "ckbDefault":
                    try
                    {
                        user.Default = ckbDefault.Checked;
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbWeight":
                    try
                    {
                        tbWeight.Text = tbWeight.Text.Trim();
                        if (tbWeight.Text == "")
                            throw new FormatException("Weight cannot be blank.");

                        user.Weight = decimal.Parse(tbWeight.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "rbLbs":
                case "rbKgs":
                    try
                    {
                        user.WeightInKgs = rbKgs.Checked;
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "nPowerThreshold":
                    try
                    {
                        user.PowerThreshold = (int)nPowerThreshold.Value;
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "clbCollectors":
                    try
                    {
                        if (clbCollectors.CheckedItems.Count < 1)
                        {
                            throw new ApplicationException($"Please select at least one default collector.");
                        }

                        if (clbCollectors.CheckedItems.Count > 3)
                        {
                            throw new ApplicationException($"A maximum of three default collectors may be selected.");
                        }

                        user.DefaultCollectors.Clear();

                        for(int i=0; i<clbCollectors.Items.Count; i++)
                        {
                            if (clbCollectors.GetItemChecked(i))
                                user.DefaultCollectors.Add(clbCollectors.Items[i].ToString(), true);
                        }

                        // clear the highlight bar
                        clbCollectors.SelectedItem = null;
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

        public void UserProfiles_TooltipOnEnter(object sender, EventArgs e)
        {
            HandleTooltipsUserProfiles(sender as Control, true);
        }
        public void UserProfiles_TooltipOnLeave(object sender, EventArgs e)
        {
            HandleTooltipsUserProfiles(sender as Control, false);
        }

        public void HandleTooltipsUserProfiles(Control control, bool isEntering)
        {
            if (!isEntering)
            {
                tsslStatus.Text = "";
                return;
            }

            switch (control.Name)
            {
                case "tbName":
                    tsslStatus.Text = "";
                    break;

                case "ckbDefault":
                    tsslStatus.Text = "If checked, this user will be selected by default when the system starts.";
                    break;

                case "tbWeight":
                    tsslStatus.Text = "Enter the same weight used in Zwift for accurate w/kg calculations.";
                    break;

                case "rbLbs":
                case "rbKgs":
                    tsslStatus.Text = "Weight unit of measure.";
                    break;

                case "nPowerThreshold":
                    tsslStatus.Text = "Functional Power Threshold (FTP).  Enter your best 20 min power x 0.95";
                    break;

                case "clbCollectors":
                    tsslStatus.Text = "Choose up to three default collectors.  These can be changed at runtime.";
                    break;
            }
        }

        #endregion
        */

        /*
        #region SystemSettings

        private void SystemSettings_Load()
        {
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

                m_logger.LogError(ex, "Error while retrieving networks.");
                this.Close();
                return;
            }


            // Load values from configuration into fields
            SystemSettings_LoadFields();

            SetMonitorButtonStatus();

            m_zpMonitorService.PlayerStateEvent += ProcessedPlayerStateEventHandler;
        }


        /// <summary>
        /// Called when tab page is selected
        /// </summary>
        private void SystemSettings_TabSelected()
        {
            SystemSettings_LoadFields();
        }



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
            tbCurWindowPosX.Text = m_zamWindowXpos.ToString();
            tbCurWindowPosY.Text = m_zamWindowYpos.ToString();

            EditingSystemSettings = false;
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
            }

            get { return btnSaveSettings.Enabled; }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                m_zpMonitorService.StartMonitor();
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
            m_zpMonitorService.StopMonitor();
            SetMonitorButtonStatus();
        }

        private void SetMonitorButtonStatus()
        {
            if (m_zpMonitorService.IsStarted)
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
                            throw new ArgumentException("Please select a network device.");
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
                            throw new ArgumentException("Please select a user profile.");
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
                tsslStatus.Text = "";
                return;
            }

            switch (control.Name)
            {
                case "tbWindowPosX":
                    tsslStatus.Text = "Location where the Zwift Activity Monitor window will appear on startup.  Set the (X,Y) coordinate values.";
                    break;

                case "tbWindowPosY":
                    tsslStatus.Text = "Location where the Zwift Activity Monitor window will appear on startup.  Set the (X,Y) coordinate values.";
                    break;

                case "cbNetwork":
                    tsslStatus.Text = "Choose the network that you use when running Zwift on this computer.";
                    break;

                case "cbCurrentUser":
                    tsslStatus.Text = "Choose the user profile to use during activity monitoring.";
                    break;

                case "ckbAutoStart":
                    tsslStatus.Text = "If checked, the Zwift Packet Monitor will run automatically on startup using the configured Network setting.";
                    break;
            }

        }

        #endregion
        */


    }
}
