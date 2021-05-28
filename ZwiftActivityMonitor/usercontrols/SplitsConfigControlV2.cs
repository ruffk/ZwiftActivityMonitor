using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public partial class SplitsConfigControlV2 : UserControlWithStatusBase
    {
        #region Internal Classes
        internal class SplitItem
        {
            public double Distance { get; set; }
            public TimeSpan Time { get; set; }
            public double TotalDistance { get; set; }
            public TimeSpan TotalTime { get; set; }

            //public string Distance { get; set; } = "";
            //public string Time { get; set; } = "";
            //public string TotalDistance { get; set; } = "";
            //public string TotalTime { get; set; } = "";
            public bool SplitsInKm { get; }


            public SplitItem()
            {

            }

            public SplitItem(SplitV2 item, bool splitsInKm)
            {
                this.Distance = item.SplitDistance;
                this.Time = item.SplitTime;
                this.TotalDistance = item.TotalDistance;
                this.TotalTime = item.TotalTime;

                this.SplitsInKm = splitsInKm;
            }



            //public void ClearDataFields()
            //{
            //    this.Distance = "";
            //    this.Time = "";
            //    this.TotalDistance = "";
            //    this.TotalTime = "";
            //}
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
                    "",
                    $"{item.Distance:#.0} {(item.SplitsInKm ? "km" : "mi")}",
                    item.Time.Hours.ToString("0#") + ":" + item.Time.Minutes.ToString("0#") + ":" + item.Time.Seconds.ToString("0#"),
                    $"{item.TotalDistance:#.0} {(item.SplitsInKm ? "km" : "mi")}",
                    item.TotalTime.Hours.ToString("0#") + ":" + item.TotalTime.Minutes.ToString("0#") + ":" + item.TotalTime.Seconds.ToString("0#")
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

        public class DataGridViewExtended : DataGridView
        {
            public ILogger Logger { get; set; }
            public DataGridViewExtended()
            {

            }

            /// <summary>
            /// This method is called for each keystroke while editing.
            /// </summary>
            /// <param name="keyData"></param>
            /// <returns></returns>
            protected override bool ProcessDialogKey(Keys keyData)
            {
                Keys key = keyData & Keys.KeyCode;
                //Logger.LogInformation($"ProcessDialogKey {key}");

                if (key == Keys.Enter)
                {
                    return this.ProcessTabKey(keyData);
                }

                return base.ProcessDialogKey(keyData);
            }

            /// <summary>
            /// This method is called for each keystroke while NOT editing.
            /// </summary>
            /// <param name="e"></param>
            /// <returns></returns>
            protected override bool ProcessDataGridViewKey(KeyEventArgs e)
            {
                //Logger.LogInformation($"ProcessDataGridViewKey {e.KeyCode}");

                if (e.KeyCode == Keys.Enter)
                {
                    return this.ProcessTabKey(e.KeyData);
                }

                return base.ProcessDataGridViewKey(e);
            }
        }

        private bool m_editMode;
        private bool m_editSplitMode;
        private bool m_isUserControlLoaded;

        private const int SplitDistanceCol = 0;
        private const int SplitTimeCol = 1;
        private const int SplitSpeedCol = 2;
        private const int TotalDistanceCol = 3;
        private const int TotalTimeCol = 4;
        private const int AverageSpeedCol = 5;

        public SplitsConfigControlV2()
        {
            InitializeComponent();
        }

        private void InitializeDataGridView()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("Split Distance", typeof(double)));
            table.Columns.Add(new DataColumn("Split Time", typeof(TimeSpan)));
            table.Columns.Add(new DataColumn("Split Speed", typeof(double)));
            table.Columns.Add(new DataColumn("Total Distance", typeof(double)));
            table.Columns.Add(new DataColumn("Total Time", typeof(TimeSpan)));
            table.Columns.Add(new DataColumn("Average Speed", typeof(double)));

            table.Columns[SplitSpeedCol].ReadOnly = true;
            table.Columns[TotalDistanceCol].ReadOnly = true;
            table.Columns[TotalTimeCol].ReadOnly = true;
            table.Columns[AverageSpeedCol].ReadOnly = true;

            this.dgvSplits.DataSource = table;

            foreach (DataGridViewColumn c in this.dgvSplits.Columns)
            {
                c.Width = 65;
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dgvSplits.Columns[SplitDistanceCol].DefaultCellStyle.Format = "0.0";
            dgvSplits.Columns[SplitTimeCol].DefaultCellStyle.Format = "mm\\:ss";
            dgvSplits.Columns[SplitSpeedCol].DefaultCellStyle.Format = "0.0";
            dgvSplits.Columns[TotalDistanceCol].DefaultCellStyle.Format = "0.0";
            dgvSplits.Columns[AverageSpeedCol].DefaultCellStyle.Format = "0.0";
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

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsConfigControlV2>();
            this.dgvSplits.Logger = this.Logger;

            cbSplitUom.BeginUpdate();
            cbSplitUom.Items.Clear();
            cbSplitUom.Items.AddRange(ZAMsettings.Settings.SplitsV2.SplitDistanceUomItems);
            cbSplitUom.EndUpdate();

            this.InitializeDataGridView();

            // initialize
            EditingSystemSettings = false;
            EditingSplitSettings = false;

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

        /// <summary>
        /// Display values stored in settings in controls.  Updates split grid.
        /// </summary>
        private void SystemSettings_LoadFields()
        {
            SplitsV2 settings = ZAMsettings.Settings.SplitsV2;

            ckbShowSplits.Checked = settings.ShowSplits;

            tbSplitDistance.Text = settings.SplitDistance.ToString();

            if (cbSplitUom.Items.Contains(settings.SplitDistanceUom))
                cbSplitUom.SelectedItem = settings.SplitDistanceUom;

            lblGoalDistanceUom.Text = settings.SplitDistanceUom.Value;

            ckbCalculateGoal.Checked = settings.CalculateGoal;

            dtpGoalTime.Value = new DateTime(2021, 1, 1, settings.GoalTime.Hours, settings.GoalTime.Minutes, settings.GoalTime.Seconds);

            tbGoalDistance.Text = settings.GoalDistance.ToString();

            lblGoalSpeed.Text = $"Goal Speed {lblGoalDistanceUom.Text}/h:";
            lblGoalSpeedValue.Text = $"{settings.GoalSpeed}";

            ckbCustomized.Checked = settings.Customized;

            this.LoadSplitChart();
        }

        /// <summary>
        /// Display splits stored in settings on grid 
        /// </summary>
        private void LoadSplitChart()
        {
            DataTable table = (DataTable)dgvSplits.DataSource;
            table.Rows.Clear();

            foreach (var split in ZAMsettings.Settings.SplitsV2.Splits)
            {
                table.Rows.Add(split.SplitDistance, split.SplitTime, split.SplitSpeed, split.TotalDistance, split.TotalTime, split.AverageSpeed);
            }
        }


        private bool EditingSystemSettings
        {
            set
            {
                btnEditSettings.Enabled = !value;
                btnSaveSettings.Enabled = value;
                btnCancelSettings.Enabled = value;
                btnSplitEdit.Enabled = !value; // default

                // if not editing and there are calculated split goals then enable edit
                if (!value)
                {
                    btnSplitEdit.Enabled = ZAMsettings.Settings.SplitsV2.Splits.Count > 0;
                }

                ckbShowSplits.Enabled = value;
                lblSplitsEvery.Enabled = value;
                tbSplitDistance.Enabled = value;
                cbSplitUom.Enabled = value;

                ckbCalculateGoal.Enabled = value;
                lblGoalTime.Enabled = value;
                dtpGoalTime.Enabled = value;
                lblGoalDistance.Enabled = value;
                tbGoalDistance.Enabled = value;
                lblGoalDistanceUom.Enabled = value;
                lblGoalSpeed.Enabled = value;
                lblGoalSpeedValue.Enabled = value;

                m_editMode = value;
            }

            get { return m_editMode; }
        }
        private bool EditingSplitSettings
        {
            set
            {
                btnEditSettings.Enabled = !value;
                btnSaveSettings.Enabled = value;
                btnCancelSettings.Enabled = value;
                btnSplitEdit.Enabled = !value;
                gbSplitGoals.Enabled = value;

                // if not editing and there are calculated split goals then enable edit
                if (!value)
                {
                    btnSplitEdit.Enabled = ZAMsettings.Settings.SplitsV2.Splits.Count > 0;
                }

                dgvSplits.Enabled = value;

                // change various colors to give illusion of grid control being enabled / disabled
                dgvSplits.RowsDefaultCellStyle.SelectionBackColor = value ? System.Drawing.SystemColors.Highlight : System.Drawing.SystemColors.Control;
                dgvSplits.RowsDefaultCellStyle.SelectionForeColor = value ? System.Drawing.SystemColors.HighlightText : System.Drawing.SystemColors.ControlDark;

                dgvSplits.ColumnHeadersDefaultCellStyle.ForeColor = value ? SystemColors.ControlText : SystemColors.ControlDark;
                dgvSplits.DefaultCellStyle.ForeColor = value ? SystemColors.ControlText : SystemColors.ControlDark;

                if (dgvSplits.Rows.Count > 0)
                    dgvSplits.CurrentCell = dgvSplits[0, 0];

                if (value)
                {
                    dgvSplits.Focus();
                }


                m_editSplitMode = value;
            }

            get { return m_editSplitMode; }
        }

        private void btnEditSettings_Click(object sender, EventArgs e)
        {
            if (ckbCustomized.Checked)
            {
                DialogResult result = MessageBox.Show(this.ParentForm, "Editing settings will overwrite all custom split goals when saved.  Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.No)
                    return;
            }

            ZAMsettings.BeginCachedConfiguration();
            EditingSystemSettings = true;
        }

        private void btnSplitEdit_Click(object sender, EventArgs e)
        {
            ZAMsettings.BeginCachedConfiguration();
            EditingSplitSettings = true;
        }


        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            bool errorOccurred = false;

            if (this.EditingSystemSettings)
            {
                errorOccurred = (errorOccurred || ValidateSystemSettings(this.ckbShowSplits));
                errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbSplitDistance));
                errorOccurred = (errorOccurred || ValidateSystemSettings(this.cbSplitUom));
                errorOccurred = (errorOccurred || ValidateSystemSettings(this.ckbCalculateGoal));
                errorOccurred = (errorOccurred || ValidateSystemSettings(this.dtpGoalTime));
                errorOccurred = (errorOccurred || ValidateSystemSettings(this.tbGoalDistance));

                if (!errorOccurred)
                {
                    // Wipe out splits and recalculate.  These will be saved in settings.
                    ZAMsettings.Settings.SplitsV2.CalculateDefaultSplits();

                    ZAMsettings.CommitCachedConfiguration();
                    EditingSystemSettings = false;

                    // show recalculated splits
                    this.SystemSettings_LoadFields();
                }
            }
            else if (this.EditingSplitSettings)
            {
                errorOccurred = (errorOccurred || ValidateSystemSettings(this.dgvSplits));

                if (!errorOccurred)
                {
                    ZAMsettings.CommitCachedConfiguration();
                    EditingSplitSettings = false;

                    this.SystemSettings_LoadFields();
                }
            }
        }

        /// <summary>
        /// Recalculate splits based upon user selections.  This wipes out any customizations.
        /// </summary>
        //private void RecalculateSplits()
        //{
        //    ZAMsettings.Settings.SplitsV2.Splits.Clear();
        //    ZAMsettings.Settings.SplitsV2.GoalSpeed = 0;

        //    SplitsManagerV2.SplitGoals splitGoals = SplitsManagerV2.GetSplitGoals();

        //    if (splitGoals == null)
        //        return;

        //    foreach (SplitsManagerV2.SplitGoal goal in splitGoals.Goals)
        //    {
        //        ZAMsettings.Settings.SplitsV2.Splits.Add(new SplitV2(goal.SplitDistance, goal.SplitTime, 0.0, goal.TotalDistance, goal.TotalTime, 0.0));
        //    }

        //    ZAMsettings.Settings.SplitsV2.GoalSpeed = splitGoals.GoalSpeed;
        //}

        private void SaveSplits()
        {
            DataTable table = (DataTable)dgvSplits.DataSource;

            ZAMsettings.Settings.SplitsV2.Splits.Clear();

            foreach (DataRow row in ((DataTable)dgvSplits.DataSource).Rows)
            {
                ZAMsettings.Settings.SplitsV2.Splits.Add(new SplitV2(
                    row.Field<double>(SplitDistanceCol),
                    row.Field<TimeSpan>(SplitTimeCol),
                    row.Field<double>(SplitSpeedCol),
                    row.Field<double>(TotalDistanceCol), 
                    row.Field<TimeSpan>(TotalTimeCol),
                    row.Field<double>(AverageSpeedCol)
                    ));
            }

            ZAMsettings.Settings.SplitsV2.Customized = true;

            // the last split row has the totals
            int lastSplitRow = ZAMsettings.Settings.SplitsV2.Splits.Count - 1;

            if (lastSplitRow >= 0)
            {
                SplitV2 lastSplit = ZAMsettings.Settings.SplitsV2.Splits[lastSplitRow];

                // Setting of these values to invalid numbers can throw an exception

                ZAMsettings.Settings.SplitsV2.GoalDistance = lastSplit.TotalDistance;

                ZAMsettings.Settings.SplitsV2.GoalTime = lastSplit.TotalTime;

                ZAMsettings.Settings.SplitsV2.GoalSpeed = lastSplit.AverageSpeed;
            }
            else
            {
                throw new FormatException("No goals entered.");
            }
        }

        private void btnCancelSettings_Click(object sender, EventArgs e)
        {
            ZAMsettings.RollbackCachedConfiguration();

            errorProvider.Clear();

            if (this.EditingSystemSettings)
            {
                // Reload values from configuration into fields since cancel was pressed
                SystemSettings_LoadFields();

                EditingSystemSettings = false;
            }
            else if (this.EditingSplitSettings)
            {
                // Reload values from configuration into fields since cancel was pressed
                SystemSettings_LoadFields();

                this.EditingSplitSettings = false;
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
                case "ckbShowSplits":
                    ZAMsettings.Settings.SplitsV2.ShowSplits = ckbShowSplits.Checked;
                    break;

                case "tbSplitDistance":
                    try
                    {
                        tbSplitDistance.Text = tbSplitDistance.Text.Trim();
                        ZAMsettings.Settings.SplitsV2.SplitDistance = int.Parse(tbSplitDistance.Text == "" ? "0" : tbSplitDistance.Text);
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
                            ZAMsettings.Settings.SplitsV2.SplitDistanceUomSetting = (cbSplitUom.SelectedItem as KeyStringPair<SplitsV2.DistanceUomType>).Key;
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
                    ZAMsettings.Settings.SplitsV2.CalculateGoal = ckbCalculateGoal.Checked;
                    break;

                case "dtpGoalTime":
                    try
                    {
                        ZAMsettings.Settings.SplitsV2.GoalTime = dtpGoalTime.Value.TimeOfDay;
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
                        ZAMsettings.Settings.SplitsV2.GoalDistance = double.Parse(tbGoalDistance.Text == "" ? "0" : tbGoalDistance.Text);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "dgvSplits":
                    try
                    {
                        this.SaveSplits();
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;


                default:
                    //Debug.Assert(1 == 0, $"Unknown control {control.Name} passed to validate method.");
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
            lblGoalSpeed.Text = $"Goal Speed {cbSplitUom.Text}/h:";

        }

        private void dgvSplits_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridViewCell cell = dgvSplits[e.ColumnIndex, e.RowIndex];

            if (!cell.IsInEditMode)
                return;

            Logger.LogInformation($"dgvSplits_CellValidating ({e.RowIndex}, {e.ColumnIndex}), value: {e.FormattedValue}, EditMode: {cell.IsInEditMode}");

            bool success;

            try
            {
                switch (e.ColumnIndex)
                {
                    case SplitDistanceCol:
                        success = double.TryParse(e.FormattedValue.ToString(), out double distanceVal);

                        if (!success || distanceVal < .1 || distanceVal > 999.9)
                            throw new FormatException("Split distance must be between 0.1 and 999.9 and entered without the mileage units.");
                        break;

                    case SplitTimeCol:
                        success = TimeSpan.TryParseExact(e.FormattedValue.ToString(), "mm\\:ss", System.Globalization.CultureInfo.InvariantCulture, out TimeSpan timeVal);

                        if (!success || timeVal.TotalSeconds < 1 || timeVal.TotalSeconds > 3600)
                            throw new FormatException("Split time must be one hour or less and in the format of MM:SS.");

                        dgvSplits.EditingControl.Text = timeVal.ToString();  // put the full hh:mm:ss string back into the control so it will convert okay
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }

        }

        private void dgvSplits_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            Logger.LogInformation($"dgvSplits_CellValidated ({e.RowIndex}, {e.ColumnIndex}), value: {dgvSplits[e.ColumnIndex, e.RowIndex].Value}");

            this.RecalculateTotals();
        }


        private void dgvSplits_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == -1 || dgvSplits.Rows[e.RowIndex].IsNewRow)
                return;

            // if both are null then don't validate
            if (this.IsNullorDBNull(dgvSplits[SplitDistanceCol, e.RowIndex].Value) > 0 && this.IsNullorDBNull(dgvSplits[SplitTimeCol, e.RowIndex].Value) > 0)
                return;

            try
            {
                Logger.LogInformation($"dgvSplits_RowValidating ({e.RowIndex}, {e.ColumnIndex})");

                if (this.IsNullorDBNull(dgvSplits[SplitDistanceCol, e.RowIndex].Value) > 0)
                {
                    dgvSplits.CurrentCell = dgvSplits[SplitDistanceCol, e.RowIndex];
                    throw new FormatException("Split distance is a required field.");
                }

                if (this.IsNullorDBNull(dgvSplits[SplitTimeCol, e.RowIndex].Value) > 0)
                {
                    dgvSplits.CurrentCell = dgvSplits[SplitTimeCol, e.RowIndex];
                    throw new FormatException("Split time is a required field.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid Row", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }

        private void btnSplitRemove_Click(object sender, EventArgs e)
        {
        }

        private void dgvSplits_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            Logger.LogInformation($"dgvSplits_RowValidated ({e.RowIndex}, {e.ColumnIndex})");

            this.RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            DataTable table = (DataTable)dgvSplits.DataSource;

            Logger.LogInformation($"RecalculateTotals");

            TimeSpan totalTime = TimeSpan.Zero;
            double totalDistance = 0;

            table.Columns[SplitSpeedCol].ReadOnly = false;
            table.Columns[TotalDistanceCol].ReadOnly = false;
            table.Columns[TotalTimeCol].ReadOnly = false;
            table.Columns[AverageSpeedCol].ReadOnly = false;

            for (int row = 0; row < dgvSplits.Rows.Count; row++)
            {
                Logger.LogInformation($"Distance: {this.IsNullorDBNull(dgvSplits[SplitDistanceCol, row].Value)}, time: {this.IsNullorDBNull(dgvSplits[SplitTimeCol, row].Value)}, IsNewRow: {dgvSplits.Rows[row].IsNewRow}");

                if (dgvSplits.Rows[row].IsNewRow)
                    continue;

                if (this.IsNullorDBNull(dgvSplits[SplitDistanceCol, row].Value) > 0 || this.IsNullorDBNull(dgvSplits[SplitTimeCol, row].Value) > 0)
                    continue;

                dgvSplits[SplitSpeedCol, row].Value = Math.Round(((double)dgvSplits[SplitDistanceCol, row].Value / ((TimeSpan)dgvSplits[SplitTimeCol, row].Value).TotalSeconds) * 3600, 1);

                totalDistance += (double)dgvSplits[SplitDistanceCol, row].Value;
                totalTime += (TimeSpan)dgvSplits[SplitTimeCol, row].Value;

                // recalculate row totals 
                dgvSplits[TotalDistanceCol, row].Value = totalDistance;
                dgvSplits[TotalTimeCol, row].Value = totalTime;

                dgvSplits[AverageSpeedCol, row].Value = Math.Round((totalDistance / totalTime.TotalSeconds) * 3600, 1);
            }

            table.Columns[SplitSpeedCol].ReadOnly = true;
            table.Columns[TotalDistanceCol].ReadOnly = true;
            table.Columns[TotalTimeCol].ReadOnly = true;
            table.Columns[AverageSpeedCol].ReadOnly = true;
        }

        private int IsNullorDBNull(object value)
        {
            if (value == null)
                return 1;

            if (value == System.DBNull.Value)
                return 2;

            return 0;
        }

        private void dgvSplits_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            Logger.LogInformation($"dgvSplits_RowEnter ({e.RowIndex}, {e.ColumnIndex})");
        }

        private void dgvSplits_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            Logger.LogInformation($"dgvSplits_RowLeave ({e.RowIndex}, {e.ColumnIndex})");
        }

        private int m_rowIndexToDelete;

        private void dgvSplits_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 || this.dgvSplits.Rows[e.RowIndex].IsNewRow)
                return;

            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex == -1)
                    this.dgvSplits.CurrentCell = this.dgvSplits[SplitDistanceCol, e.RowIndex];
                else
                    this.dgvSplits.CurrentCell = this.dgvSplits[e.ColumnIndex, e.RowIndex];

                this.m_rowIndexToDelete = e.RowIndex;
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void tsmiDeleteRow_Click(object sender, EventArgs e)
        {
            this.dgvSplits.Rows.RemoveAt(m_rowIndexToDelete);

            this.RecalculateTotals();
        }

    }
}
