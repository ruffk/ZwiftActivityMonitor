using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public partial class UserProfileControl : UserControlBase
    {
        #region Internal Classes

        internal class UserProfileListViewItem : ListViewItem
        {
            private UserProfile m_userProfile;

            public UserProfileListViewItem() : base(new string[] { "*** New User Profile ***" })
            {
                m_userProfile = new UserProfile();

                this.SubItems.Add(new ListViewSubItem());
                this.SubItems.Add(new ListViewSubItem());
                this.SubItems.Add(new ListViewSubItem());
                this.SubItems.Add(new ListViewSubItem());

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
                string[] text = SubItemStrings(m_userProfile);

                for (int i = 0; i < 4; i++)
                    base.SubItems[i].Text = text[i];

                // Set each SubItem individually.  Cannot Clear and AddRange as it messes up.
                //for (int i = 0; i < base.SubItems.Count; i++)
                //    base.SubItems[i] = new ListViewSubItem(this, text[i]);
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

        #endregion

        public UserProfileControl()
        {
            InitializeComponent();
        }

        internal override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            base.UserControlBase_Load(sender, e);

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

            EditingUserProfiles = false; // initialize
        }

        internal override void SkipControl_Enter(object sender, EventArgs e)
        {
            base.SkipControl_Enter(sender, e);
        }
        public override void ControlLosingFocus(object sender, CancelEventArgs e)
        {
            base.ControlLosingFocus(sender, e);

            if (EditingUserProfiles)
            {
                MessageBox.Show("Please either Save or Cancel current work before proceeding.", "Pending Changes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }


        #region UserProfiles

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

                for (int i = 0; i < clbCollectors.Items.Count; i++)
                {
                    if (!user.DefaultCollectors.TryGetValue(clbCollectors.Items[i].ToString(), out bool setting))
                        setting = false;

                    clbCollectors.SetItemChecked(i, setting);
                }
            }
            else
            {
                tbName.Text = "";
                tbWeight.Text = "";
                ckbDefault.Checked = false;
                rbLbs.Checked = true;
                nPowerThreshold.Value = nPowerThreshold.Minimum;

                for (int i = 0; i < clbCollectors.Items.Count; i++)
                {
                    clbCollectors.SetItemChecked(i, false);
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
                    UserProfile user = ((UserProfileListViewItem)lvUserProfiles.SelectedItems[0]).UserProfile;

                    btnEditProfile.Enabled = !value;

                    // Don't enable the Remove button if default user is selected.
                    btnRemoveProfile.Enabled = (user.Default ? false : !value);
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

                Logger.LogInformation($"EditingUserProfiles: {value}, SelectedItemsCount: {lvUserProfiles.SelectedItems.Count}");
            }

            get { return btnSaveProfile.Enabled; }
        }

        private void lvUserProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvUserProfiles.SelectedItems.Count > 0)
            {
                UserProfile user = ((UserProfileListViewItem)lvUserProfiles.SelectedItems[0]).UserProfile;

                UserProfiles_LoadFields(user);

                Logger.LogInformation($"SelectedIndexChanged {user.Name} selected.");
            }
            else
            {
                UserProfiles_LoadFields(null);
                Logger.LogInformation($"SelectedIndexChanged nothing selected.");
            }

            EditingUserProfiles = false;
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
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
            if (lvUserProfiles.SelectedItems.Count > 0)
            {
                UserProfileListViewItem itemBeingEdited = (UserProfileListViewItem)lvUserProfiles.SelectedItems[0];
                UserProfile userBeingEdited = itemBeingEdited.UserProfile;

                if (MessageBox.Show(this, $"Delete user ({userBeingEdited.Name}), are you sure?", "Deletion Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        ZAMsettings.BeginCachedConfiguration();

                        ZAMsettings.Settings.DeleteUserProfile(userBeingEdited);

                        ZAMsettings.CommitCachedConfiguration();

                        lvUserProfiles.BeginUpdate();
                        lvUserProfiles.Items.Remove(itemBeingEdited);

                        if (lvUserProfiles.Items.Count > 0)
                        {
                            lvUserProfiles.Items[0].Selected = true;
                        }
                        lvUserProfiles.EndUpdate();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Exception occurred: " + ex.ToString(), "Error deleting User Profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            bool errorOccurred = false;

            // this should not happen
            if (lvUserProfiles.SelectedItems.Count < 1)
            {
                ZAMsettings.RollbackCachedConfiguration();
                EditingUserProfiles = false;
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

                    lvUserProfiles.BeginUpdate();

                    // Refresh the fields and the list view
                    UserProfiles_LoadFields(userBeingEdited); 
                    itemBeingEdited.Refresh();

                    //UserProfileListViewItem lvi = new UserProfileListViewItem(userBeingEdited);
                    //lvUserProfiles.Items.Add(lvi);
                    //lvi.Selected = true;

                    //lvUserProfiles.Items.Remove(itemBeingEdited);

                    foreach (UserProfileListViewItem item in lvUserProfiles.Items)
                    {
                        bool isDefault = (item.UserProfile.UniqueId == ZAMsettings.Settings.DefaultUserProfile);

                        if (item.UserProfile.Default != isDefault)
                        {
                            item.UserProfile.Default = isDefault;
                            item.Refresh();
                        }
                    }

                    lvUserProfiles.EndUpdate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception occurred: " + ex.ToString(), "Error saving User Profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    errorOccurred = true;
                }
                finally
                {
                    EditingUserProfiles = false;
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

                        for (int i = 0; i < clbCollectors.Items.Count; i++)
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
                toolStripStatusLabel.Text = "";
                return;
            }

            switch (control.Name)
            {
                case "tbName":
                    toolStripStatusLabel.Text = "";
                    break;

                case "ckbDefault":
                    toolStripStatusLabel.Text = "If checked, this user will be selected by default when the system starts.";
                    break;

                case "tbWeight":
                    toolStripStatusLabel.Text = "Enter the same weight used in Zwift for accurate w/kg calculations.";
                    break;

                case "rbLbs":
                case "rbKgs":
                    toolStripStatusLabel.Text = "Weight unit of measure.";
                    break;

                case "nPowerThreshold":
                    toolStripStatusLabel.Text = "Functional Power Threshold (FTP).  Enter your best 20 min power x 0.95";
                    break;

                case "clbCollectors":
                    toolStripStatusLabel.Text = "Choose up to three default collectors.  These can be changed at runtime.";
                    break;
            }
        }

        #endregion


    }
}
