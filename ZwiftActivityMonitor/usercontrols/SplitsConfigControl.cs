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
        #region Internal Classes
        internal class SplitItem
        {
            public string Distance { get; set; } = "";
            public string Time { get; set; } = "";
            public string TotalDistance { get; set; } = "";
            public string TotalTime { get; set; } = "";

            public SplitItem()
            {
            }

            public void ClearDataFields()
            {
                this.Distance = "";
                this.Time = "";
                this.TotalDistance = "";
                this.TotalTime = "";
            }
        }

        internal class SplitListViewItem : ListViewItem
        {
            public SplitItem SplitItem { get; }

            public SplitListViewItem(SplitItem item) : base(SubItemStrings(item))
            {
                this.SplitItem = item;
            }

            private static string[] SubItemStrings(SplitItem item)
            {
                return (new string[]
                {
                    item.Distance,
                    item.Time,
                    item.TotalDistance,
                    item.TotalTime
                });
            }

            public void Refresh()
            {
                string[] text = SubItemStrings(SplitItem);

                for (int i = 0; i < text.Length; i++)
                    this.SubItems[i].Text = text[i];
            }
        }

        #endregion

        private bool m_editMode;
        private bool m_isUserControlLoaded;

        public SplitsConfigControl()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            UserControlBase.SetListViewHeaderColor(ref this.lvSplits, SystemColors.Control, Color.Black); 
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

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsConfigControl>();

            cbSplitUom.BeginUpdate();
            cbSplitUom.Items.Clear();
            cbSplitUom.Items.AddRange(new string[] { "km", "mi" });
            cbSplitUom.EndUpdate();

            // initialize
            EditingSystemSettings = false;

            m_isUserControlLoaded = true;
        }
        private void ListView_Resize(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            UserControlBase.HideHorizontalScrollBar(sender as ListView);
        }

        //private void HideHorizontalScrollBar(ListView listView)
        //{
        //    listView.Scrollable = true;

        //    ZAMsettings.ShowScrollBar(listView.Handle, 0, false);
        //}

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
            ckbShowSplits.Checked = ZAMsettings.Settings.Splits.ShowSplits;

            tbSplitDistance.Text = ZAMsettings.Settings.Splits.SplitDistance.ToString();
            
            foreach (string item in cbSplitUom.Items)
                if (item == ZAMsettings.Settings.Splits.SplitUom)
                {
                    cbSplitUom.SelectedItem = item;
                    break;
                }

            lblGoalDistanceUom.Text = ZAMsettings.Settings.Splits.SplitUom;

            ckbCalculateGoal.Checked = ZAMsettings.Settings.Splits.CalculateGoal;

            tbGoalHrs.Text = ZAMsettings.Settings.Splits.GoalHours.ToString();
            tbGoalMins.Text = ZAMsettings.Settings.Splits.GoalMinutes.ToString();
            tbGoalSecs.Text = ZAMsettings.Settings.Splits.GoalSeconds.ToString();
            tbGoalDistance.Text = ZAMsettings.Settings.Splits.GoalDistance.ToString();

            this.LoadSplitChart();

        }

        private void LoadSplitChart()
        {
            lvSplits.Items.Clear();
            lblGoalSpeed.Text = "";

            if (!ckbShowSplits.Checked || !ckbCalculateGoal.Checked)
                return;

            Splits splits = ZAMsettings.Settings.Splits;

            double numSplits = splits.GoalDistance / splits.SplitDistance;
            if (numSplits < 1)
                return;

            TimeSpan goalTime = new TimeSpan(splits.GoalHours, splits.GoalMinutes, splits.GoalSeconds);
            if (goalTime.TotalSeconds < 1)
                return;

            TimeSpan splitTime = new TimeSpan(0, 0, (int)Math.Round(goalTime.TotalSeconds / numSplits, 0));
            string splitTimeStr = splitTime.Hours.ToString("0#") + ":" + splitTime.Minutes.ToString("0#") + ":" + splitTime.Seconds.ToString("0#");

            int curDistance = 0;
            TimeSpan curTime = new TimeSpan();

            for (int i=0; i<(int)numSplits; i++)
            {
                if (lvSplits.Items.Count >= 100)
                    break;

                int totalDistance = curDistance + splits.SplitDistance;
                TimeSpan totalTime = curTime.Add(splitTime);

                SplitItem item = new SplitItem() 
                { 
                    Distance = $"{splits.SplitDistance} {splits.SplitUom}",
                    Time = splitTimeStr,
                    TotalDistance = $"{totalDistance} {splits.SplitUom}",
                    TotalTime = $"{totalTime.Hours.ToString("0#") + ":" + totalTime.Minutes.ToString("0#") + ":" + totalTime.Seconds.ToString("0#")}"
                };
                lvSplits.Items.Add(new SplitListViewItem(item));

                curDistance = totalDistance;
                curTime = totalTime;
            }

            if (numSplits != (int)numSplits)
            {
                double lastSplitDistance = Math.Round(splits.GoalDistance - curDistance, 1);
                TimeSpan lastSplitTime = goalTime.Subtract(curTime);

                SplitItem item = new SplitItem()
                {
                    Distance = $"{lastSplitDistance} {splits.SplitUom}",
                    Time = $"{lastSplitTime.Hours.ToString("0#") + ":" + lastSplitTime.Minutes.ToString("0#") + ":" + lastSplitTime.Seconds.ToString("0#")}",
                    TotalDistance = $"{Math.Round(splits.GoalDistance, 1)} {splits.SplitUom}",
                    TotalTime = $"{goalTime.Hours.ToString("0#") + ":" + goalTime.Minutes.ToString("0#") + ":" + goalTime.Seconds.ToString("0#")}"
                };
                lvSplits.Items.Add(new SplitListViewItem(item));
            }

            double goalSpeed = Math.Round((splits.GoalDistance / goalTime.TotalSeconds) * 3600, 1);
            lblGoalSpeed.Text = $"{goalSpeed.ToString("#.#")} {(splits.SplitUom == "km" ? "kph" : "mph")}";
        }

        private bool EditingSystemSettings
        {
            set
            {
                btnEditSettings.Enabled = !value;
                btnSaveSettings.Enabled = value;
                btnCancelSettings.Enabled = value;

                ckbShowSplits.Enabled = value;
                lblSplitsEvery.Enabled = value;
                tbSplitDistance.Enabled = value;
                cbSplitUom.Enabled = value;

                ckbCalculateGoal.Enabled = value;
                lblGoalTime.Enabled = value;
                tbGoalHrs.Enabled = value;
                tbGoalMins.Enabled = value;
                tbGoalSecs.Enabled = value;
                lblGoalDistance.Enabled = value;
                tbGoalDistance.Enabled = value;
                //lvSplits.Enabled = value;

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

            errorOccurred = (errorOccurred || ValidateSystemSettings(this.ckbShowSplits));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbSplitDistance));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.cbSplitUom));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.ckbCalculateGoal));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbGoalHrs));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbGoalMins));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbGoalSecs));
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbGoalDistance));

            // Check to make sure selections / values all make sense
            errorOccurred = (errorOccurred || ValidateSystemSettings(this.lvSplits));

            if (!errorOccurred)
            {
                ZAMsettings.CommitCachedConfiguration();
                EditingSystemSettings = false;

                this.LoadSplitChart();
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
                        if (cbSplitUom.SelectedItem != null)
                        {
                            ZAMsettings.Settings.Splits.SplitUom = (string)cbSplitUom.SelectedItem;
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

                case "tbGoalDistance":
                    try
                    {
                        tbGoalDistance.Text = tbGoalDistance.Text.Trim();
                        ZAMsettings.Settings.Splits.GoalDistance = double.Parse(tbGoalDistance.Text == "" ? "0" : tbGoalDistance.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "lvSplits": // business logic using multiple fields
                    try
                    {
                        if (ZAMsettings.Settings.Splits.CalculateGoal && ZAMsettings.Settings.Splits.GoalDistance < ZAMsettings.Settings.Splits.SplitDistance)
                        {
                            control = this.tbGoalDistance;
                            control.Focus();

                            throw new ApplicationException("Goal distance must be >= split distance to calculate goals.");
                        }

                        if (ZAMsettings.Settings.Splits.CalculateGoal && ZAMsettings.Settings.Splits.GoalTimeSpan.TotalSeconds < 1)
                        {
                            control = this.tbGoalHrs;
                            control.Focus();

                            throw new ApplicationException("A goal time must be entered to calculate goals.");
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
                case "ckbShowSplits":
                    toolStripStatusLabel.Text = "Select to be shown split times at distance intervals.";
                    break;

                case "tbSplitDistance":
                    toolStripStatusLabel.Text = "Enter the distance to travel for each split time.";
                    break;

                case "cbSplitUom":
                    toolStripStatusLabel.Text = "Select kilometers or miles.";
                    break;

                case "ckbCalculateGoal":
                    toolStripStatusLabel.Text = "Select to be shown goal times at distance intervals.";
                    break;

                case "tbGoalHrs":
                    toolStripStatusLabel.Text = "Enter goal hours.";
                    break;

                case "tbGoalMins":
                    toolStripStatusLabel.Text = "Enter goal minutes.";
                    break;

                case "tbGoalSecs":
                    toolStripStatusLabel.Text = "Enter goal seconds.";
                    break;

                case "tbGoalDistance":
                    toolStripStatusLabel.Text = "Enter the total goal distance.";
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

        private void cbSplitUom_SelectionChangeCommitted(object sender, EventArgs e)
        {
            lblGoalDistanceUom.Text = cbSplitUom.Text;
        }

    }
}
