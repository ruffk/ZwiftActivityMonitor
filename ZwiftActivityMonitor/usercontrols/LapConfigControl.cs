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
    public partial class LapConfigControl : UserControlWithStatusBase
    {
        private bool m_editMode;
        private bool m_isUserControlLoaded;

        public LapConfigControl()
        {
            InitializeComponent();

            if (DesignMode)
                return;

        }

        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            LoadUserControl();

            base.UserControlBase_Load(sender, e);
        }

        /// <summary>
        /// Called by UserControlBase_Load or ControlGainingFocus, whichever occurs first.
        /// </summary>
        private void LoadUserControl()
        {
            if (m_isUserControlLoaded)
                return;

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<LapConfigControl>();

            cbDistanceUom.BeginUpdate();
            cbDistanceUom.Items.Clear();
            //cbDistanceUom.Items.AddRange(new string[] { "km", "mi" });
            cbDistanceUom.Items.AddRange(ZAMsettings.Settings.Laps.TriggerDistanceUomItems);
            cbDistanceUom.EndUpdate();

            cbPosition.BeginUpdate();
            cbPosition.Items.Clear();
            //cbPosition.Items.AddRange(new string[] { "Start and Lap Button", "Lap Button Only" });
            cbPosition.Items.AddRange(ZAMsettings.Settings.Laps.TriggerPositionItems);
            cbPosition.EndUpdate();

            rbManual.Tag = Lap.LapStyleType.Manual;
            rbAutomatic.Tag = Lap.LapStyleType.Automatic;

            // initialize
            EditingSystemSettings = false;

            m_isUserControlLoaded = true;
        }

        public override void ControlGainingFocus(object sender, CancelEventArgs e)
        {
            if (DesignMode)
                return;

            LoadUserControl();

            // Reload each time control is shown
            SystemSettings_LoadFields();

            btnEditSettings.Focus();

            base.ControlGainingFocus(sender, e);
        }


        private void SystemSettings_LoadFields()
        {
            tbDistance.Text = ZAMsettings.Settings.Laps.TriggerDistance.ToString();

            if (cbDistanceUom.Items.Contains(ZAMsettings.Settings.Laps.TriggerDistanceUom))
                cbDistanceUom.SelectedItem = ZAMsettings.Settings.Laps.TriggerDistanceUom;

            tbHrs.Text = ZAMsettings.Settings.Laps.TriggerHours.ToString();
            tbMins.Text = ZAMsettings.Settings.Laps.TriggerMinutes.ToString();
            tbSecs.Text = ZAMsettings.Settings.Laps.TriggerSeconds.ToString();

            if (cbPosition.Items.Contains(ZAMsettings.Settings.Laps.TriggerPosition))
                cbPosition.SelectedItem = ZAMsettings.Settings.Laps.TriggerPosition;

            rbManual.Checked = (Lap.LapStyleType)rbManual.Tag == ZAMsettings.Settings.Laps.LapStyleSetting;
            rbAutomatic.Checked = (Lap.LapStyleType)rbAutomatic.Tag == ZAMsettings.Settings.Laps.LapStyleSetting;
        }


        private bool EditingSystemSettings
        {
            set
            {
                btnEditSettings.Enabled = !value;
                btnSaveSettings.Enabled = value;
                btnCancelSettings.Enabled = value;

                rbManual.Enabled = value;
                rbAutomatic.Enabled = value;

                tbDistance.Enabled = value;
                cbDistanceUom.Enabled = value;
                tbHrs.Enabled = value;
                tbMins.Enabled = value;
                tbSecs.Enabled = value;
                cbPosition.Enabled = value;

                m_editMode = value;
            }

            get { return m_editMode; }
        }

        private void btnEditSettings_Click(object sender, EventArgs e)
        {
            ZAMsettings.BeginCachedConfiguration();
            EditingSystemSettings = true;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            bool errorOccurred = false;

            errorOccurred = (errorOccurred || ValidateSystemSettings(this.rbManual));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.rbAutomatic));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.cbDistanceUom));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.cbPosition));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbDistance));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbHrs));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbMins));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbSecs));

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
                case "tbDistance":
                    try
                    {
                        tbDistance.Text = tbDistance.Text.Trim();
                        ZAMsettings.Settings.Laps.TriggerDistance = int.Parse(tbDistance.Text == "" ? "0" : tbDistance.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "cbDistanceUom":
                    try
                    {
                        if (cbDistanceUom.SelectedItem != null)
                        {
                            ZAMsettings.Settings.Laps.TriggerDistanceUomSetting = (cbDistanceUom.SelectedItem as KeyStringPair<Lap.DistanceUomType>).Key;
                        }
                        else
                        {
                            throw new ApplicationException("Please select a distance unit of measure.");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "cbPosition":
                    try
                    {
                        if (cbPosition.SelectedItem != null)
                        {
                            ZAMsettings.Settings.Laps.TriggerPositionSetting = (cbPosition.SelectedItem as KeyStringPair<Lap.TriggerPositionType>).Key;
                        }
                        else
                        {
                            throw new ApplicationException("Please select a distance unit of measure.");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbHrs":
                    try
                    {
                        tbHrs.Text = tbHrs.Text.Trim();
                        ZAMsettings.Settings.Laps.TriggerHours = int.Parse(tbHrs.Text == "" ? "0" : tbHrs.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbMins":
                    try
                    {
                        tbMins.Text = tbMins.Text.Trim();
                        ZAMsettings.Settings.Laps.TriggerMinutes = int.Parse(tbMins.Text == "" ? "0" : tbMins.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbSecs":
                    try
                    {
                        tbSecs.Text = tbSecs.Text.Trim();
                        ZAMsettings.Settings.Laps.TriggerSeconds = int.Parse(tbSecs.Text == "" ? "0" : tbSecs.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "rbManual":
                case "rbAutomatic":
                    try
                    {
                        if ((control as RadioButton).Checked)
                            ZAMsettings.Settings.Laps.LapStyleSetting = (Lap.LapStyleType)control.Tag;
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
                //case "ckbShowSplits":
                //    toolStripStatusLabel.Text = "Select to be shown split times at distance intervals.";
                //    break;

                //case "tbSplitDistance":
                //    toolStripStatusLabel.Text = "Enter the distance to travel for each split time.";
                //    break;

                //case "cbSplitUom":
                //    toolStripStatusLabel.Text = "Select kilometers or miles.";
                //    break;

                //case "ckbCalculateGoal":
                //    toolStripStatusLabel.Text = "Select to be shown goal times at distance intervals.";
                //    break;

                //case "tbGoalHrs":
                //    toolStripStatusLabel.Text = "Enter goal hours.";
                //    break;

                //case "tbGoalMins":
                //    toolStripStatusLabel.Text = "Enter goal minutes.";
                //    break;

                //case "tbGoalSecs":
                //    toolStripStatusLabel.Text = "Enter goal seconds.";
                //    break;

                //case "tbGoalDistance":
                //    toolStripStatusLabel.Text = "Enter the total goal distance.";
                //    break;
                default:
                    //toolStripStatusLabel.Text = control.Name;
                    break;
            }

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

        private void cbDistanceUom_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //lblGoalDistanceUom.Text = cbDistanceUom.Text;
        }

        private void rbManual_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbAutomatic_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
