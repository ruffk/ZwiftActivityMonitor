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
using System.Drawing.Drawing2D;
using Syncfusion.Windows.Forms;

namespace ZwiftActivityMonitorV2
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
            public Color HeaderGradientBeginColor { get; set; } = SystemColors.Control;
            public Color HeaderGradientEndColor { get; set; } = SystemColors.ControlDark;
            public bool UseGradientHeaders { get; set; } = false;
            public bool MapEnterKeyToTabWhileEditing { get; set; } = false;
            public bool MapEnterKeyToTabWhileNotEditing { get; set; } = false;
            /// <summary>
            /// DataGridView extension property
            /// Set ShowFocus=false to hide cell focus rectangle
            /// </summary>
            public bool? ShowFocus { get; set; }

            public DataGridViewExtended()
            {
                this.CellPainting += this.dataGridViewGradientHeader_CellPainting;
            }

            public Color HeaderForeColor
            {
                get
                {
                    return this.ColumnHeadersDefaultCellStyle.ForeColor;
                }
                set
                {
                    // change the text color on the data grid headers
                    if (value != Color.Empty)
                        this.ColumnHeadersDefaultCellStyle.ForeColor = value;
                }
            }

            public Font RowFont
            {
                get
                {
                    return this.RowsDefaultCellStyle.Font;
                }
                set
                {
                    // change the font on the data grid rows
                    if (value != null)
                        this.RowsDefaultCellStyle.Font = value;
                }
            }

            public Color RowBackColor
            {
                get
                {
                    return this.RowsDefaultCellStyle.BackColor;
                }
                set
                {
                    // change the back color on the data grid rows
                    if (value != Color.Empty)
                        this.RowsDefaultCellStyle.BackColor = value;
                }
            }

            public Color RowForeColor
            {
                get
                {
                    return this.RowsDefaultCellStyle.ForeColor;
                }
                set
                {
                    // change the fore color on the data grid rows
                    if (value != Color.Empty)
                        this.RowsDefaultCellStyle.ForeColor = value;
                }
            }

            protected void dataGridViewGradientHeader_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
            {
                if (e.RowIndex == -1 && this.UseGradientHeaders)
                {
                    LinearGradientBrush br = new LinearGradientBrush(e.CellBounds, this.HeaderGradientBeginColor, this.HeaderGradientEndColor, 90, true);

                    e.Graphics.FillRectangle(br, e.CellBounds);

                    // draw the 3d header
                    // 
                    Rectangle r = e.CellBounds;

                    r.Width -= 1;
                    r.Height -= 1;

                    // draw the dark border around the whole thing 
                    // 
                    e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, r);

                    r.Width -= 1;
                    r.Height -= 1;

                    // draw the light 3D border 
                    //
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.Right, r.Y);
                    e.Graphics.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.X, r.Bottom);

                    // draw the dark 3D Border 
                    //
                    e.Graphics.DrawLine(SystemPens.ControlDark, r.X + 1, r.Bottom, r.Right, r.Bottom);
                    e.Graphics.DrawLine(SystemPens.ControlDark, r.Right, r.Y + 1, r.Right, r.Bottom);

                    e.PaintContent(e.ClipBounds);
                    e.Handled = true;
                }
            }

            /// <summary>
            /// This method is called for each keystroke while editing.
            /// </summary>
            /// <param name="keyData"></param>
            /// <returns></returns>
            protected override bool ProcessDialogKey(Keys keyData)
            {
                Keys key = keyData & Keys.KeyCode;

                if (this.MapEnterKeyToTabWhileEditing && key == Keys.Enter)
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
                if (this.MapEnterKeyToTabWhileNotEditing && e.KeyCode == Keys.Enter)
                {
                    return this.ProcessTabKey(e.KeyData);
                }

                return base.ProcessDataGridViewKey(e);
            }

            /// <summary>
            /// Allows override of cell focus rectangle
            /// </summary>
            protected override bool ShowFocusCues
            {
                get
                {
                    return this.ShowFocus.HasValue ? this.ShowFocus.Value : base.ShowFocusCues;
                }
            }

        }

        private bool m_editMode;
        private bool m_editSplitMode;
        private bool m_isUserControlLoaded;
        private int m_rowIndexToDelete;

        private const int SplitDistanceCol = 0;
        private const int SplitTimeCol = 1;
        private const int SplitSpeedCol = 2;
        private const int TotalDistanceCol = 3;
        private const int TotalTimeCol = 4;
        private const int AverageSpeedCol = 5;

        public SplitsConfigControlV2()
        {
            InitializeComponent();

            this.dgvSplits.MapEnterKeyToTabWhileNotEditing = true;
            this.dgvSplits.MapEnterKeyToTabWhileEditing = true;
            this.dgvSplits.UseGradientHeaders = true;
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

            //table.Columns[SplitSpeedCol].ReadOnly = true;
            //table.Columns[TotalDistanceCol].ReadOnly = true;
            //table.Columns[TotalTimeCol].ReadOnly = true;
            //table.Columns[AverageSpeedCol].ReadOnly = true;

            this.dgvSplits.DataSource = table;

            foreach (DataGridViewColumn c in this.dgvSplits.Columns)
            {
                c.Width = 65;
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dgvSplits.Columns[SplitDistanceCol].DefaultCellStyle.Format = "0.0";
            //dgvSplits.Columns[SplitTimeCol].DefaultCellStyle.Format = "mm\\:ss";
            dgvSplits.Columns[SplitSpeedCol].DefaultCellStyle.Format = "0.0";
            dgvSplits.Columns[TotalDistanceCol].DefaultCellStyle.Format = "0.0";
            dgvSplits.Columns[AverageSpeedCol].DefaultCellStyle.Format = "0.0";

            dgvSplits.Columns[SplitSpeedCol].ReadOnly = true;
            dgvSplits.Columns[SplitDistanceCol].ReadOnly = true;
            dgvSplits.Columns[SplitTimeCol].ReadOnly = true;
            //dgvSplits.Columns[TotalDistanceCol].ReadOnly = true;
            //dgvSplits.Columns[TotalTimeCol].ReadOnly = true;
            dgvSplits.Columns[AverageSpeedCol].ReadOnly = true;
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

                lblDeleteInstructions.Enabled = value;
                ckbCustomized.Enabled = value;

                // the grid will remain responsive even when not in EditMode by leaving enabled but just setting to readonly
                dgvSplits.ReadOnly = !value;

                dgvSplits.ShowFocus = value;


                // these columns must be reset to readonly each time the grid readonly status is set
                dgvSplits.Columns[SplitSpeedCol].ReadOnly = true;
                dgvSplits.Columns[SplitDistanceCol].ReadOnly = true;
                dgvSplits.Columns[SplitTimeCol].ReadOnly = true;
                //dgvSplits.Columns[TotalDistanceCol].ReadOnly = true;
                //dgvSplits.Columns[TotalTimeCol].ReadOnly = true;
                dgvSplits.Columns[AverageSpeedCol].ReadOnly = true;

                dgvSplits.AllowUserToAddRows = value;


                // if not editing and there are calculated split goals then enable edit
                if (!value)
                {
                    btnSplitEdit.Enabled = ZAMsettings.Settings.SplitsV2.Splits.Count > 0;
                }

                //dgvSplits.Enabled = value;

                // change selected cell colors based on EditMode
                //dgvSplits.RowsDefaultCellStyle.SelectionBackColor = value ? System.Drawing.SystemColors.Highlight : System.Drawing.SystemColors.Control;
                //dgvSplits.RowsDefaultCellStyle.SelectionForeColor = value ? System.Drawing.SystemColors.HighlightText : System.Drawing.SystemColors.ControlText;

                dgvSplits.RowsDefaultCellStyle.SelectionBackColor = value ? System.Drawing.SystemColors.Highlight : dgvSplits.BackgroundColor;
                dgvSplits.RowsDefaultCellStyle.SelectionForeColor = value ? System.Drawing.SystemColors.HighlightText : dgvSplits.ForeColor;

                // change column header and cell colors based on EditMode
                //dgvSplits.ColumnHeadersDefaultCellStyle.ForeColor = value ? SystemColors.ControlText : SystemColors.ControlDark;
                //dgvSplits.DefaultCellStyle.ForeColor = value ? SystemColors.ControlText : SystemColors.ControlDark;

                if (dgvSplits.Rows.Count > 0)
                    dgvSplits.CurrentCell = dgvSplits[TotalDistanceCol, 0];

                if (value)
                {
                    dgvSplits.Focus();
                }


                m_editSplitMode = value;
            }

            get { return m_editSplitMode; }
        }

        #region Edit, Save, and Cancel Methods
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

        private void SaveSplits()
        {
            DataTable table = (DataTable)dgvSplits.DataSource;

            SplitsV2 splits = ZAMsettings.Settings.SplitsV2;

            splits.Splits.Clear();

            foreach (DataRow row in ((DataTable)dgvSplits.DataSource).Rows)
            {
                if (this.IsNullorDBNull(row.Field<object>(SplitDistanceCol)) > 0 || this.IsNullorDBNull(row.Field<object>(SplitTimeCol)) > 0)
                    continue;

                ZAMsettings.Settings.SplitsV2.Splits.Add(new SplitV2(
                    row.Field<double>(SplitDistanceCol),
                    row.Field<TimeSpan>(SplitTimeCol),
                    row.Field<double>(SplitSpeedCol),
                    row.Field<double>(TotalDistanceCol), 
                    row.Field<TimeSpan>(TotalTimeCol),
                    row.Field<double>(AverageSpeedCol),
                    splits.SplitDistanceUom.Key
                    ));
            }

            splits.Customized = true;

            // the last split row has the totals
            int lastSplitRow = splits.Splits.Count - 1;

            if (lastSplitRow >= 0)
            {
                SplitV2 lastSplit = splits.Splits[lastSplitRow];

                // Setting of these values to invalid numbers can throw an exception

                splits.GoalDistance = lastSplit.TotalDistance;
                splits.GoalTime = lastSplit.TotalTime;
                splits.GoalSpeed = lastSplit.AverageSpeed;
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
        #endregion

        #region Control Validation and ToolTips

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
                            ZAMsettings.Settings.SplitsV2.SplitDistanceUomSetting = (cbSplitUom.SelectedItem as KeyStringPair<DistanceUomType>).Key;
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
        #endregion

        #region Base class overrides for event selection
        public override void ControlLosingFocus(object sender, Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventArgs e)
        {
            base.ControlLosingFocus(sender, e);

            if (EditingSystemSettings || EditingSplitSettings)
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
            {
                return;
            }

            //Logger.LogInformation($"dgvSplits_CellValidating ({e.RowIndex}, {e.ColumnIndex}), value: {e.FormattedValue}, EditMode: {cell.IsInEditMode}");

            bool success;
            double distanceVal;
            TimeSpan timeVal;

            try
            {
                switch (e.ColumnIndex)
                {
                    //case SplitDistanceCol:
                    //    success = double.TryParse(e.FormattedValue.ToString(), out distanceVal);

                    //    if (!success || distanceVal < .1 || distanceVal > 999.9)
                    //        throw new FormatException("Split distance must be between 0.1 and 999.9 and entered without the mileage units.");
                    //    break;

                    //case SplitTimeCol:
                    //    success = TimeSpan.TryParseExact(e.FormattedValue.ToString(), "mm\\:ss", System.Globalization.CultureInfo.InvariantCulture, out timeVal);

                    //    if (!success || timeVal.TotalSeconds < 1 || timeVal.TotalSeconds > 3600)
                    //        throw new FormatException("Split time must be one hour or less and in the format of MM:SS.");

                    //    dgvSplits.EditingControl.Text = timeVal.ToString();  // put the full hh:mm:ss string back into the control so it will convert okay
                    //    break;

                    case TotalDistanceCol:
                        success = double.TryParse(e.FormattedValue.ToString(), out distanceVal);

                        if (!success || distanceVal < .1 || distanceVal > 999.9)
                            throw new FormatException("Total distance must be between 0.1 and 999.9 and entered without the mileage units.");
                        break;

                    case TotalTimeCol:
                        success = TimeSpan.TryParseExact(e.FormattedValue.ToString(), "mm\\:ss", System.Globalization.CultureInfo.InvariantCulture, out timeVal);
                        
                        if (!success)
                            success = TimeSpan.TryParse(e.FormattedValue.ToString(), System.Globalization.CultureInfo.InvariantCulture, out timeVal);
                        //success = TimeSpan.TryParseExact(e.FormattedValue.ToString(), "mm\\:ss", System.Globalization.CultureInfo.InvariantCulture, out timeVal);

                        //if (!success || timeVal.TotalSeconds < 1 || timeVal.TotalSeconds > 3600)
                        //    throw new FormatException("Total time must be one hour or less and in the format of MM:SS.");

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
            //Logger.LogInformation($"dgvSplits_CellValidated ({e.RowIndex}, {e.ColumnIndex}), value: {dgvSplits[e.ColumnIndex, e.RowIndex].Value}");

            // if split distance and time have values, recalculate
            //if (this.IsNullorDBNull(dgvSplits[SplitDistanceCol, e.RowIndex].Value) == 0 && this.IsNullorDBNull(dgvSplits[SplitTimeCol, e.RowIndex].Value) == 0)
            //{
            //    this.RecalculateTotals();
            //}
            
            // if total distance and time have values, recalculate
            if (this.IsNullorDBNull(dgvSplits[TotalDistanceCol, e.RowIndex].Value) == 0 && this.IsNullorDBNull(dgvSplits[TotalTimeCol, e.RowIndex].Value) == 0)
            {
                this.RecalculateTotals();
            }
        }


        private void dgvSplits_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
                
            if (dgvSplits.Rows[e.RowIndex].IsNewRow)
            {
                //Logger.LogInformation($"dgvSplits_RowValidating Row: {e.RowIndex}, IsNewRow: {dgvSplits.Rows[e.RowIndex].IsNewRow}");
                return;
            }

            // if both are null then don't validate
            //if (this.IsNullorDBNull(dgvSplits[SplitDistanceCol, e.RowIndex].Value) > 0 && this.IsNullorDBNull(dgvSplits[SplitTimeCol, e.RowIndex].Value) > 0)
            //{
            //    return;
            //}
            // if both are null then don't validate
            if (this.IsNullorDBNull(dgvSplits[TotalDistanceCol, e.RowIndex].Value) > 0 && this.IsNullorDBNull(dgvSplits[TotalTimeCol, e.RowIndex].Value) > 0)
            {
                return;
            }

            try
            {
                //Logger.LogInformation($"dgvSplits_RowValidating ({e.RowIndex}, {e.ColumnIndex})");

                //if (this.IsNullorDBNull(dgvSplits[SplitDistanceCol, e.RowIndex].Value) > 0)
                //{
                //    dgvSplits.CurrentCell = dgvSplits[SplitDistanceCol, e.RowIndex];
                //    throw new FormatException("Split distance is a required field.");
                //}

                //if (this.IsNullorDBNull(dgvSplits[SplitTimeCol, e.RowIndex].Value) > 0)
                //{
                //    dgvSplits.CurrentCell = dgvSplits[SplitTimeCol, e.RowIndex];
                //    throw new FormatException("Split time is a required field.");
                //}

                if (this.IsNullorDBNull(dgvSplits[TotalDistanceCol, e.RowIndex].Value) > 0)
                {
                    dgvSplits.CurrentCell = dgvSplits[TotalDistanceCol, e.RowIndex];
                    throw new FormatException("Total distance is a required field.");
                }

                if (this.IsNullorDBNull(dgvSplits[TotalTimeCol, e.RowIndex].Value) > 0)
                {
                    dgvSplits.CurrentCell = dgvSplits[TotalTimeCol, e.RowIndex];
                    throw new FormatException("Total time is a required field.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid Row", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }

        private void dgvSplits_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            
            if (dgvSplits.Rows[e.RowIndex].IsNewRow)
            {
                //Logger.LogInformation($"dgvSplits_RowValidated Row: {e.RowIndex}, IsNewRow: {dgvSplits.Rows[e.RowIndex].IsNewRow}");

                // remove the empty row from the data table
                ((DataTable)dgvSplits.DataSource).Rows[e.RowIndex].Delete();
                return;
            }

            //if (this.IsNullorDBNull(dgvSplits[SplitDistanceCol, e.RowIndex].Value) > 0 && this.IsNullorDBNull(dgvSplits[SplitTimeCol, e.RowIndex].Value) > 0)
            //{
            //    return;
            //}
            if (this.IsNullorDBNull(dgvSplits[TotalDistanceCol, e.RowIndex].Value) > 0 && this.IsNullorDBNull(dgvSplits[TotalTimeCol, e.RowIndex].Value) > 0)
            {
                return;
            }

            //Logger.LogInformation($"dgvSplits_RowValidated Row: {e.RowIndex}");


            this.RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            DataTable table = (DataTable)dgvSplits.DataSource;

            //Logger.LogInformation($"RecalculateTotals");

            TimeSpan totalTime = TimeSpan.Zero;
            double totalDistance = 0;



            //table.Columns[SplitSpeedCol].ReadOnly = false;
            //table.Columns[TotalDistanceCol].ReadOnly = false;
            //table.Columns[TotalTimeCol].ReadOnly = false;
            //table.Columns[AverageSpeedCol].ReadOnly = false;

            for (int row = 0; row < dgvSplits.Rows.Count; row++)
            {
                //Logger.LogInformation($"Distance: {this.IsNullorDBNull(dgvSplits[SplitDistanceCol, row].Value)}, time: {this.IsNullorDBNull(dgvSplits[SplitTimeCol, row].Value)}, IsNewRow: {dgvSplits.Rows[row].IsNewRow}");

                if (dgvSplits.Rows[row].IsNewRow)
                    continue;

                //if (this.IsNullorDBNull(dgvSplits[SplitDistanceCol, row].Value) > 0 || this.IsNullorDBNull(dgvSplits[SplitTimeCol, row].Value) > 0)
                //    continue;

                //dgvSplits[SplitSpeedCol, row].Value = Math.Round(((double)dgvSplits[SplitDistanceCol, row].Value / ((TimeSpan)dgvSplits[SplitTimeCol, row].Value).TotalSeconds) * 3600, 1);

                //totalDistance += (double)dgvSplits[SplitDistanceCol, row].Value;
                //totalTime += (TimeSpan)dgvSplits[SplitTimeCol, row].Value;

                //// recalculate row totals 
                //dgvSplits[TotalDistanceCol, row].Value = totalDistance;
                //dgvSplits[TotalTimeCol, row].Value = totalTime;

                //dgvSplits[AverageSpeedCol, row].Value = Math.Round((totalDistance / totalTime.TotalSeconds) * 3600, 1);

                if (this.IsNullorDBNull(dgvSplits[TotalDistanceCol, row].Value) > 0 || this.IsNullorDBNull(dgvSplits[TotalTimeCol, row].Value) > 0)
                    continue;

                if ((double)dgvSplits[TotalDistanceCol, row].Value - totalDistance <= 0)
                    dgvSplits[TotalDistanceCol, row].Value = totalDistance + 1.0;

                if ((TimeSpan)dgvSplits[TotalTimeCol, row].Value - totalTime <= TimeSpan.Zero)
                    dgvSplits[TotalTimeCol, row].Value = totalTime + TimeSpan.FromMinutes(2.0);

                dgvSplits[SplitDistanceCol, row].Value = (double)dgvSplits[TotalDistanceCol, row].Value - totalDistance;
                dgvSplits[SplitTimeCol, row].Value = (TimeSpan)dgvSplits[TotalTimeCol, row].Value - totalTime;

                totalDistance = (double)dgvSplits[TotalDistanceCol, row].Value;
                totalTime = (TimeSpan)dgvSplits[TotalTimeCol, row].Value;

                dgvSplits[SplitSpeedCol, row].Value = Math.Round(((double)dgvSplits[SplitDistanceCol, row].Value / ((TimeSpan)dgvSplits[SplitTimeCol, row].Value).TotalSeconds) * 3600, 1);

                dgvSplits[AverageSpeedCol, row].Value = Math.Round((totalDistance / totalTime.TotalSeconds) * 3600, 1);
            }

            //table.Columns[SplitSpeedCol].ReadOnly = true;
            //table.Columns[TotalDistanceCol].ReadOnly = true;
            //table.Columns[TotalTimeCol].ReadOnly = true;
            //table.Columns[AverageSpeedCol].ReadOnly = true;
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
            //Logger.LogInformation($"dgvSplits_RowEnter ({e.RowIndex}, {e.ColumnIndex})");
        }

        private void dgvSplits_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            //Logger.LogInformation($"dgvSplits_RowLeave ({e.RowIndex}, {e.ColumnIndex}, DataTable Rows: {((DataTable)dgvSplits.DataSource).Rows.Count})");
        }

        private void dgvSplits_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!this.EditingSplitSettings)
                return;

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
        protected override void Parent_BackColorChanged(object sender, EventArgs e)
        {
            base.Parent_BackColorChanged(sender, e);

            this.tbDescSystem.BackColor = this.BackColor;

            this.dgvSplits.RowBackColor = this.BackColor;
            this.dgvSplits.BackgroundColor = this.BackColor;
            this.dgvSplits.RowHeadersDefaultCellStyle.BackColor = this.BackColor;
        }

        protected override void Parent_ForeColorChanged(object sender, EventArgs e)
        {
            base.Parent_ForeColorChanged(sender, e);

            this.tbDescSystem.ForeColor = this.ForeColor;

            this.dgvSplits.RowForeColor = this.ForeColor;
            this.dgvSplits.ForeColor = this.ForeColor;
            this.dgvSplits.HeaderForeColor = this.ForeColor;

            this.gbSplitGoals.ForeColor = this.ForeColor;
            this.gbSplits.ForeColor = this.ForeColor;

            Office2010Colors t = Office2010Colors.GetColorTable(Office2010Colors.DefaultTheme);
            dgvSplits.HeaderGradientBeginColor = t.ActiveTitleGradientBegin;
            dgvSplits.HeaderGradientEndColor = t.ActiveTitleGradientEnd;
        }
    }
}
