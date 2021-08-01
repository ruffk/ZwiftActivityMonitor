using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitorV2
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

            if (ZAMsettings.LoggerFactory == null)
                return;

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<LapConfigControl>();
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


            cbDistanceUom.BeginUpdate();
            cbDistanceUom.Items.Clear();
            cbDistanceUom.Items.AddRange(ZAMsettings.Settings.Laps.TriggerDistanceUomItems);
            cbDistanceUom.EndUpdate();

            cbPosition.BeginUpdate();
            cbPosition.Items.Clear();
            cbPosition.Items.AddRange(ZAMsettings.Settings.Laps.TriggerPositionItems);
            cbPosition.EndUpdate();

            cbMeasurementSystem.BeginUpdate();
            cbMeasurementSystem.Items.Clear();
            cbMeasurementSystem.Items.AddRange(ZAMsettings.Settings.Laps.MeasurementSystemItems);
            cbMeasurementSystem.EndUpdate();

            rbManual.Tag = Lap.LapStyleType.Manual;
            rbAutomatic.Tag = Lap.LapStyleType.Automatic;

            rbPosition.Tag = Lap.LapTriggerType.Position;
            rbTime.Tag = Lap.LapTriggerType.Time;
            rbDistance.Tag = Lap.LapTriggerType.Distance;

            // initialize
            EditingSystemSettings = false;

            m_isUserControlLoaded = true;
        }

        public override void ControlGainingFocus(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            base.ControlGainingFocus(sender, e);

            LoadUserControl();

            // Reload each time control is shown
            SystemSettings_LoadFields();

            btnEditSettings.Focus();
        }


        private void SystemSettings_LoadFields()
        {
            if (cbMeasurementSystem.Items.Contains(ZAMsettings.Settings.Laps.MeasurementSystem))
                cbMeasurementSystem.SelectedItem = ZAMsettings.Settings.Laps.MeasurementSystem;

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

            rbPosition.Checked = (Lap.LapTriggerType)rbPosition.Tag == ZAMsettings.Settings.Laps.LapTriggerSetting;
            rbTime.Checked = (Lap.LapTriggerType)rbTime.Tag == ZAMsettings.Settings.Laps.LapTriggerSetting;
            rbDistance.Checked = (Lap.LapTriggerType)rbDistance.Tag == ZAMsettings.Settings.Laps.LapTriggerSetting;
        }


        private bool EditingSystemSettings
        {
            set
            {
                btnEditSettings.Enabled = !value;
                btnSaveSettings.Enabled = value;
                btnCancelSettings.Enabled = value;

                cbMeasurementSystem.Enabled = value;

                rbManual.Enabled = value;
                rbAutomatic.Enabled = value;

                tbDistance.Enabled = value;
                cbDistanceUom.Enabled = value;
                tbHrs.Enabled = value;
                tbMins.Enabled = value;
                tbSecs.Enabled = value;
                cbPosition.Enabled = value;

                rbPosition.Enabled = value;
                rbTime.Enabled = value;
                rbDistance.Enabled = value;

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

            errorOccurred = (errorOccurred || ValidateSystemSettings(this.cbMeasurementSystem));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.rbManual));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.rbAutomatic));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.cbDistanceUom));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.cbPosition));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbDistance));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbHrs));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbMins));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbSecs));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.rbDistance));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.rbTime));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.rbPosition));

            // final validation
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.gbLaps));

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
                case "cbMeasurementSystem":
                    try
                    {
                        if (cbMeasurementSystem.SelectedItem != null)
                        {
                            ZAMsettings.Settings.Laps.MeasurementSystemSetting = (cbMeasurementSystem.SelectedItem as KeyStringPair<MeasurementSystemType>).Key;
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

                case "tbDistance":
                    try
                    {
                        tbDistance.Text = tbDistance.Text.Trim();
                        ZAMsettings.Settings.Laps.TriggerDistance = double.Parse(tbDistance.Text == "" ? "0" : tbDistance.Text);
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

                case "rbDistance":
                case "rbTime":
                case "rbPosition":
                    try
                    {
                        if ((control as RadioButton).Checked)
                            ZAMsettings.Settings.Laps.LapTriggerSetting = (Lap.LapTriggerType)control.Tag;
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "gbLaps": // any final validations
                    try
                    {
                        if (ZAMsettings.Settings.Laps.LapStyleSetting == Lap.LapStyleType.Automatic)
                        {
                            switch (ZAMsettings.Settings.Laps.LapTriggerSetting)
                            {
                                case Lap.LapTriggerType.Distance:
                                    break;

                                case Lap.LapTriggerType.Time:
                                    if (ZAMsettings.Settings.Laps.TriggerTime.TotalSeconds < 60)
                                    {
                                        control = tbHrs;
                                        tbHrs.Focus();

                                        throw new ApplicationException("Trigger time must be a minimum of one minute.");
                                    }
                                    break;

                                case Lap.LapTriggerType.Position:
                                    break;
                            }
                        }
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
                case "cbMeasurementSystem":
                    toolStripStatusLabel.Text = "Select measurement system to use when displaying laps.";
                    break;

                case "tbDistance":
                    toolStripStatusLabel.Text = "Enter distance amount for auto-laps by distance.";
                    break;

                case "cbDistanceUom":
                    toolStripStatusLabel.Text = "Select distance units.";
                    break;

                case "cbPosition":
                    toolStripStatusLabel.Text = "Select position type to use for auto-laps by position.";
                    break;

                case "tbHrs":
                    toolStripStatusLabel.Text = "Enter hours to use for auto-laps by time.";
                    break;

                case "tbMins":
                    toolStripStatusLabel.Text = "Enter minutes to use for auto-laps by time.";
                    break;

                case "tbSecs":
                    toolStripStatusLabel.Text = "Enter seconds to use for auto-laps by time.";
                    break;

                default:
                    //toolStripStatusLabel.Text = control.Name;
                    break;
            }

        }





        #region Base class overrides for event selection
        public override void ControlLosingFocus(object sender, Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventArgs e)
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
        protected override void Parent_BackColorChanged(object sender, EventArgs e)
        {
            base.Parent_BackColorChanged(sender, e);

            this.tbDescSystem.BackColor = this.BackColor;

            MSoffice2010ColorManager colorTable = ZAMappearance.GetColorTable();

            this.gbLaps.BorderColor = colorTable.ActiveFormBorderColor;
            this.gbTriggers.BorderColor = colorTable.ActiveFormBorderColor;

        }

        protected override void Parent_ForeColorChanged(object sender, EventArgs e)
        {
            base.Parent_ForeColorChanged(sender, e);

            this.tbDescSystem.ForeColor = this.ForeColor;
            this.gbLaps.ForeColor = this.ForeColor;
            this.gbTriggers.ForeColor = this.ForeColor;
        }

    }
}
