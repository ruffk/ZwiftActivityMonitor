using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public partial class SplitsConfigControl : UserControlWithStatusBase
    {
        private bool m_editMode;

        public SplitsConfigControl()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            UserControlBase.SetListViewHeaderColor(ref this.lvSplits, SystemColors.Control, Color.Black); 
        }


        private void btnEditSettings_Click(object sender, EventArgs e)
        {
            ZAMsettings.BeginCachedConfiguration();
            EditingSystemSettings = true;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            bool errorOccurred = false;

            errorOccurred = (errorOccurred || ValidateSystemSettings(this.ckbShowSplits));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbSplitDistance));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.cbSplitUom));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.ckbCalculateGoal));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbGoalHrs));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbGoalMins));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbGoalSecs));

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
                case "ckbShowSplits":
                    ZAMsettings.Settings.Splits.ShowSplits = ckbShowSplits.Checked;
                    break;

                case "tbSplitDistance":
                    try
                    {
                        tbSplitDistance.Text = tbSplitDistance.Text.Trim();
                        ZAMsettings.Settings.Splits.SplitDistance = int.Parse(tbSplitDistance.Text == "" ? "0" : tbSplitDistance.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "cbSplitUom":
                    try
                    {
                        cbSplitUom.Text = cbSplitUom.Text.Trim();
                        ZAMsettings.Settings.Splits.SplitUom = cbSplitUom.Text;
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "ckbCalculateGoal":
                    ZAMsettings.Settings.Splits.CalculateGoal = ckbCalculateGoal.Checked;
                    break;

                case "tbGoalHrs":
                    try
                    {
                        tbGoalHrs.Text = tbGoalHrs.Text.Trim();
                        ZAMsettings.Settings.Splits.GoalHours = int.Parse(tbGoalHrs.Text == "" ? "0" : tbGoalHrs.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbGoalMins":
                    try
                    {
                        tbGoalMins.Text = tbGoalMins.Text.Trim();
                        ZAMsettings.Settings.Splits.GoalMinutes = int.Parse(tbGoalMins.Text == "" ? "0" : tbGoalMins.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbGoalSecs":
                    try
                    {
                        tbGoalSecs.Text = tbGoalSecs.Text.Trim();
                        ZAMsettings.Settings.Splits.GoalSeconds = int.Parse(tbGoalSecs.Text == "" ? "0" : tbGoalSecs.Text);
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
                case "ckbShowSplits":
                    break;

                case "tbSplitDistance":
                    toolStripStatusLabel.Text = "";
                    break;

                case "cbSplitUom":
                    break;

                case "ckbCalculateGoal":
                    break;

                case "tbGoalHrs":
                    break;

                case "tbGoalMins":
                    break;

                case "tbGoalSecs":
                    break;
            }

        }


        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsConfigControl>();

            cbSplitUom.BeginUpdate();
            cbSplitUom.Items.Clear();
            cbSplitUom.Items.AddRange(new string[] { "km", "mi" });
            cbSplitUom.EndUpdate();

            // initialize
            EditingSystemSettings = false;

            base.UserControlBase_Load(sender, e);
        }

        private void SystemSettings_LoadFields()
        {
            ckbShowSplits.Checked = ZAMsettings.Settings.Splits.ShowSplits;

            tbSplitDistance.Text = ZAMsettings.Settings.Splits.SplitDistance.ToString();
            cbSplitUom.SelectedItem = ZAMsettings.Settings.Splits.SplitUom;

            ckbCalculateGoal.Checked = ZAMsettings.Settings.Splits.CalculateGoal;

            tbGoalHrs.Text = ZAMsettings.Settings.Splits.GoalHours.ToString();
            tbGoalMins.Text = ZAMsettings.Settings.Splits.GoalMinutes.ToString();
            tbGoalSecs.Text = ZAMsettings.Settings.Splits.GoalSeconds.ToString();
        }

        private bool EditingSystemSettings
        {
            set
            {
                btnEditSettings.Enabled = !value;
                btnSaveSettings.Enabled = value;
                btnCancelSettings.Enabled = value;

                ckbShowSplits.Enabled = value;
                tbSplitDistance.Enabled = value;
                cbSplitUom.Enabled = value;
                ckbCalculateGoal.Enabled = value;
                tbGoalHrs.Enabled = value;
                tbGoalMins.Enabled = value;
                tbGoalSecs.Enabled = value;
                lvSplits.Enabled = value;

                m_editMode = value;
            }

            get { return m_editMode; }
        }



        #region Base class overrides for event selection
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
            // Reload each time control is shown as user profile info may have changed.
            SystemSettings_LoadFields();

            btnEditSettings.Focus();

            base.ControlGainingFocus(sender, e);
        }



        protected override void ListView_ItemSelectionChanged_Disable(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            base.ListView_ItemSelectionChanged_Disable(sender, e);
        }

        protected override void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            base.ListView_DrawItem(sender, e);
        }

        protected override void Listview_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            base.Listview_DrawSubItem(sender, e);
        }

        protected override void SkipControl_Enter(object sender, EventArgs e)
        {
            base.SkipControl_Enter(sender, e);
        }

        #endregion

    }
}
