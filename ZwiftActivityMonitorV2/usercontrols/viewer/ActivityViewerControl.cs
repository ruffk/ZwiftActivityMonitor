using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.ComponentModel;

namespace ZwiftActivityMonitorV2
{

    public partial class ActivityViewerControl : ViewerUserControlEx
    {
        private UserProfile CurrentUserProfile { get; set; }

        private enum DetailColumn
        {
            Period = 0,
            PeriodSecs,
            AP,
            APmax,
            FTP,
            HR,
            Blank
        }

        private enum SummaryColumn
        {
            Speed = 0,
            AP,
            NP,
            IF,
            TSS,
            Blank
        }

        // A height of 19 is minimum when using Segoe UI 9pt font
        //private const int DataGridRowMinimumHeight = 19;

        private BindingSource DetailBindingSource { get; set; }

        public ActivityViewerControl()
        {
            Debug.WriteLine($"ActivityViewerControl_ctor");
            InitializeComponent();

            InitializeDetailDataGrid();
            //LoadDetailDataGrid();

            InitializeSummaryDataGrid();
            //LoadSummaryDataGrid();


            // Subscribe to any SystemConfig changes
            ZAMsettings.SystemConfigChanged += ZAMsettings_SystemConfigChanged;
        }

        private void ZAMsettings_SystemConfigChanged(object sender, EventArgs e)
        {
            this.CurrentUserProfile = ZAMsettings.Settings.CurrentUser;
            
            Debug.WriteLine($"ZAMsettings_SystemConfigChanged - {this.GetType()}");
        }

        private void ViewControl_Load(object sender, EventArgs e)
        {
            Debug.WriteLine($"ActivityViewerControl_Load");

            // Get the currently selected user
            this.CurrentUserProfile = ZAMsettings.Settings.CurrentUser;

            this.LoadDetailDataGrid();
            this.LoadSummaryDataGrid();

            this.SetRowVisibilityStatus();

            // Trigger a resize so that dgSummary can size itself appropriately
            this.OnResize(new EventArgs());

            //Debug.WriteLine($"ViewControl_Load2 - Row[0].Visible: {dgDetail.Rows[0].Visible}");
        }


        private void InitializeDetailDataGrid()
        {
            //Debug.WriteLine($"InitializeDetailDataGrid1");

            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("Period", typeof(string)));
            table.Columns.Add(new DataColumn("PeriodSecs", typeof(int)));
            table.Columns.Add(new DataColumn("AP", typeof(string)));
            table.Columns.Add(new DataColumn("AP (Max)", typeof(string)));
            table.Columns.Add(new DataColumn("FTP", typeof(string)));
            table.Columns.Add(new DataColumn("HR", typeof(string)));
            table.Columns.Add(new DataColumn("Blank", typeof(string)));

            // set in designer
            //dgDetail.ReadOnly = true;

            this.DetailBindingSource = new BindingSource(table, null);
            this.dgDetail.DataSource = this.DetailBindingSource;

            this.dgDetail.Columns[(int)DetailColumn.Period].Width = 76; // minimum 75

            this.dgDetail.Columns[(int)DetailColumn.PeriodSecs].Width = 5;
            this.dgDetail.Columns[(int)DetailColumn.PeriodSecs].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.PeriodSecs].Visible = false;

            this.dgDetail.Columns[(int)DetailColumn.AP].Width = 51; // minimum 50

            this.dgDetail.Columns[(int)DetailColumn.APmax].Width = 86; // minimum 85

            this.dgDetail.Columns[(int)DetailColumn.FTP].Width = 52; // minimum 50

            this.dgDetail.Columns[(int)DetailColumn.HR].Width = 55; // minimum 54

            this.dgDetail.Columns[(int)DetailColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero
            this.dgDetail.Columns[(int)DetailColumn.Blank].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (DataGridViewColumn c in this.dgDetail.Columns)
            {
                c.MinimumWidth = c.Width;
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            for (int i = 0; i < dgDetail.Rows.Count; i++)
            {
                DataGridViewRow r = dgDetail.Rows[i];

                // A height of 19 is minimum when using Segoe UI 9pt font
                r.MinimumHeight = DataGridRowMinimumHeight;
            }


            // set in designer
            //this.dgDetail.RowHeadersVisible = false;

            // Set the row default cell font to the grid's default cell font.  Then clear the grid default so the row default is all that has to change. 
            this.dgDetail.RowsDefaultCellStyle.Font = this.dgDetail.DefaultCellStyle.Font;
            this.dgDetail.DefaultCellStyle.Font = null;

            // These must be set here not in designer otherwise column widths change. not sure why
            dgDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // set in designer
            //this.dgDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            //Debug.WriteLine($"ColumnHeadersHeight: {this.dgDetail.ColumnHeadersHeight}");

            this.dgDetail.ShowFocus = false;
            //Debug.WriteLine($"InitializeDetailDataGrid2");
        }

        private void LoadDetailDataGrid()
        {
            //Debug.WriteLine($"LoadDetailDataGrid1");

            DataTable table = (DataTable)((BindingSource)dgDetail.DataSource).DataSource;
            table.Rows.Clear(); // not really necessary

            // Add all known Collectors to the view.  Later, row visibility will be set.
            foreach(var collector in ZAMsettings.Settings.GetCollectors)
            {
                table.Rows.Add(collector.Name, collector.DurationSecs, "", "", "", "");
            }

            //Debug.WriteLine($"LoadDetailDataGrid2");
        }

        private void InitializeSummaryDataGrid()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("mi/h", typeof(string)));
            table.Columns.Add(new DataColumn("AP", typeof(string)));
            table.Columns.Add(new DataColumn("NP", typeof(string)));
            table.Columns.Add(new DataColumn("IF", typeof(string)));
            table.Columns.Add(new DataColumn("TSS", typeof(string)));
            table.Columns.Add(new DataColumn("Blank", typeof(string)));

            // set in designer
            //dgSummary.ReadOnly = true;


            this.dgSummary.DataSource = table;

            this.dgSummary.Columns[(int)SummaryColumn.Speed].Width = 76;  // minimum 75
            this.dgSummary.Columns[(int)SummaryColumn.AP].Width = 51; // minimum 50
            this.dgSummary.Columns[(int)SummaryColumn.NP].Width = 86; // minimum 85
            this.dgSummary.Columns[(int)SummaryColumn.IF].Width = 52; // minimum 50
            this.dgSummary.Columns[(int)SummaryColumn.TSS].Width = 55; // minimum 54
            this.dgSummary.Columns[(int)SummaryColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero

            // Use the last blank column to fill the gap if user resizes
            this.dgSummary.Columns[(int)SummaryColumn.Blank].HeaderText = "";
            this.dgSummary.Columns[(int)SummaryColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (DataGridViewColumn c in this.dgSummary.Columns)
            {
                c.MinimumWidth = c.Width;
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // set in designer
            //this.dgSummary.RowHeadersVisible = false;

            // Set the row default cell font to the grid's default cell font.  Then clear the grid default so the row default is all that has to change. 
            this.dgSummary.RowsDefaultCellStyle.Font = this.dgSummary.DefaultCellStyle.Font;
            this.dgSummary.DefaultCellStyle.Font = null;

            // These must be set here not in designer otherwise column widths change. not sure why
            dgSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // set in designer
            //this.dgSummary.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            //Debug.WriteLine($"ColumnHeadersHeight: {this.dgSummary.ColumnHeadersHeight}");

            this.dgSummary.ShowFocus = false;
        }
        private void LoadSummaryDataGrid()
        {
            DataTable table = (DataTable)dgSummary.DataSource;
            table.Rows.Clear();

            table.Rows.Add("28.3", "250", "267", "0.87", "888");

            // A height of 19 is minimum when using Segoe UI 9pt font
            this.dgSummary.Rows[0].MinimumHeight = DataGridRowMinimumHeight;
        }

        private void dgDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Debug.WriteLine($"dgDetail_DataBindingComplete - ListChangedType: {e.ListChangedType}");
        }

        private void SetRowVisibilityStatus()
        {
            //Debug.WriteLine($"SetRowVisibilityStatus1 - Row[0].Visible: {dgDetail.Rows[0].Visible}, CurrentCell: {dgDetail.CurrentCell} BindingSource.Position: {this.DetailBindingSource.Position}");

            //int[] list = { 60, 300, 1200 };


            DetailBindingSource.SuspendBinding();
            dgDetail.CurrentCell = null;
            for (int i = 0; i < dgDetail.Rows.Count; i++)
            {
                DataGridViewRow r = dgDetail.Rows[i];

                // determine whether to hide or show row
                //int? value = list.Cast<int?>().FirstOrDefault(n => n == (int)r.Cells[(int)DetailColumn.PeriodSecs].Value);
                //if (!value.HasValue)
                //    r.Visible = false;

                Collector c = CurrentUserProfile.GetCollectors.FirstOrDefault(c => c.DurationSecs == (int)r.Cells[(int)DetailColumn.PeriodSecs].Value);
                if (c == null)
                    r.Visible = false;

                //Debug.WriteLine($"value: {value}, rowval: {(int)r.Cells[(int)DetailColumn.PeriodSecs].Value}");

            }
            DetailBindingSource.ResumeBinding();
            dgDetail.CurrentCell = dgDetail.FirstDisplayedCell; // Needs to be set after ResumeBinding

            //Debug.WriteLine($"SetRowVisibilityStatus2 - Row[0].Visible: {dgDetail.Rows[0].Visible}, CurrentCell: {dgDetail.CurrentCell} BindingSource.Position: {this.DetailBindingSource.Position}");
        }




        private void ViewControl_Resize(object sender, EventArgs e)
        {
            //Debug.WriteLine($"DetailSummaryViewControl_Resize1 - Size: {this.Size}");

            // TableLayoutPanel tlPanel helps keep things organized when resizing.
            //
            // tlPanel has been set up in designer with the following:
            // tlPanel.RowStyles[0].SizeType = SizeType.Percent;
            // tlPanel.RowStyles[0].Height = 100; // 100%
            // tlPanel.RowStyles[1].SizeType = SizeType.Absolute;
            // tlPanel.RowStyles[1].Height = 50; // default size


            // Calculate the height required to show all of dgSummary
            int dgSummaryHeight = dgSummary.Rows.GetRowsHeight(DataGridViewElementStates.None) + dgSummary.ColumnHeadersHeight;
            dgSummaryHeight += (dgSummary.Controls.OfType<HScrollBar>().FirstOrDefault(c => c.Visible) != null ? SystemInformation.HorizontalScrollBarHeight : 0);

            // Set the height of the lower row for dgSummary, the upper row will automatically adjust
            tlPanel.RowStyles[1].Height = dgSummaryHeight;

            // The following is not needed but just shown for completeness
            //int dgDetailHeight = dgDetail.Rows.GetRowsHeight(states) + dgDetail.ColumnHeadersHeight;
            //dgDetailHeight += (dgDetail.Controls.OfType<HScrollBar>().FirstOrDefault(c => c.Visible) != null ? SystemInformation.HorizontalScrollBarHeight : 0);
           
            

            //Debug.WriteLine($"Row[0].Visible1: {dgDetail.Rows[0].Visible}, CurrentCell: {dgDetail.CurrentCell} BindingSource.Position: {this.DetailBindingSource.Position}");

            //DataTable table = (DataTable)((BindingSource)this.dgDetail.DataSource).DataSource;

            //dgDetail.CurrentCell = null;
            //for (int i = 0; i < table.Rows.Count; i++)
            //{
            //    DataRow r = table.Rows[i];
            //    r.SetField<string>((int)DetailColumn.AP, (Convert.ToInt32(r.Field<string>((int)DetailColumn.AP)) + 1).ToString());
            //}

            //Debug.WriteLine($"Row[0].Visible2: {dgDetail.Rows[0].Visible}");
        }


        /// <summary>
        /// Determine action on cell mouse click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            if (dataGridView == null)
                return;

            if (e.Button != MouseButtons.Right)
                return;

            if (e.RowIndex == -1) // Right click on column headers?
            {
                this.dataGridView_ColumnHeaderMouseClick(dataGridView, e);
            }
            else
            {
                this.dataGridView_RowMouseClick(dataGridView, e);
            }
        }

        private void dataGridView_RowMouseClick(DataGridView dataGridView, DataGridViewCellMouseEventArgs e)
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();
            ToolStripMenuItem item;

            if (dataGridView == this.dgDetail)
            {
                switch(e.ColumnIndex)
                {
                    case (int)DetailColumn.Period:
                        for (int i = 0; i < dataGridView.Rows.Count; i++)
                        {
                            DataGridViewRow r = dataGridView.Rows[i];

                            string period = (string)dataGridView[(int)DetailColumn.Period, i].Value;

                            item = (ToolStripMenuItem)menuStrip.Items.Add(period);
                            item.Tag = new KeyValuePair<string, int>("RowIndex", i);
                            item.Checked = r.Visible;
                            item.CheckOnClick = true;
                            item.CheckStateChanged += this.periodContextMenu_CheckStateChanged;
                        }

                        menuStrip.Show(Cursor.Position);
                        //menuStrip.Items[e.RowIndex].Select(); // position the highlighted cursor on the item matching the row selected
                        break;

                    case (int)DetailColumn.AP:
                    case (int)DetailColumn.APmax:
                    case (int)DetailColumn.FTP:
                        item = (ToolStripMenuItem)menuStrip.Items.Add("Watts");
                        item = (ToolStripMenuItem)menuStrip.Items.Add("W/Kg");
                        item = (ToolStripMenuItem)menuStrip.Items.Add("Both Watts && W/Kg");
                        item = (ToolStripMenuItem)menuStrip.Items.Add("Hide Field");

                        foreach(ToolStripMenuItem mi in menuStrip.Items)
                        {
                            mi.CheckOnClick = true;
                            mi.Tag = new KeyValuePair<string, int>("RowIndex", e.RowIndex); // not sure what to use
                            mi.CheckedChanged += powerContextMenu_CheckStateChanged;
                        }
                        menuStrip.Show(Cursor.Position);
                        break;
                }
            }
        }

        private void periodContextMenu_CheckStateChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            KeyValuePair<string, int> itemTag = (KeyValuePair<string, int>)item.Tag;

            if (item.Checked || dgDetail.Rows.GetRowCount(DataGridViewElementStates.Visible) > 1)
            {
                dgDetail.CurrentCell = null;
                dgDetail.Rows[itemTag.Value].Visible = item.Checked;

                // Place the CurrentCell on a valid (visible) cell
                // Failing to do this will make the row reappear when it is next updated.  (BindingSource.Position being on a hidden row is a bad thing).
                dgDetail.CurrentCell = dgDetail.FirstDisplayedCell;
            }
        }
        private void powerContextMenu_CheckStateChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            KeyValuePair<string, int> itemTag = (KeyValuePair<string, int>)item.Tag;

        }

        /// <summary>
        /// Handle mouse click on column headers and allow visibility change
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="e"></param>
        private void dataGridView_ColumnHeaderMouseClick(DataGridView dataGridView, DataGridViewCellMouseEventArgs e)
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            //Add the columns to context menu strip
            foreach (DataGridViewColumn c in dataGridView.Columns)
            {
                if (c.HeaderText != "") // exclude filler column
                {
                    var item = (ToolStripMenuItem)menuStrip.Items.Add(c.HeaderText);
                    item.Tag = c.Name;
                    item.Checked = c.Visible;
                    item.CheckOnClick = true;

                    item.Enabled = (dataGridView != dgDetail || c.Index != (int)DetailColumn.Period);

                    //Handle CheckStateChanged event of context menu strip items
                    item.CheckStateChanged += (obj, args) =>
                    {

                        // Determine how many columns are currently visible
                        int visibleCount = 0;
                        for (int j = 0; j < dataGridView.Columns.Count; j++)
                            if (dataGridView.Columns[j].Visible && dataGridView.Columns[j].HeaderText != "")
                                visibleCount++;

                        ToolStripMenuItem item = (ToolStripMenuItem)obj;
                        string columnName = (string)item.Tag;
                        int columnIndex = dataGridView.Columns[columnName].Index;

                        //Debug.WriteLine($"{item.GetCurrentParent().GetType()}");

                        if (item.Checked)
                        {
                            dataGridView.Columns[columnName].Visible = item.Checked;
                        }
                        else
                        {
                            // Don't allow removing of every column (empty grid)
                            if (visibleCount > 1)
                                dataGridView.Columns[columnName].Visible = item.Checked;
                        }

                        // Trigger a resize so that dgSummary can size itself appropriately
                        this.OnResize(new EventArgs());
                    };
                }
            }

            if (menuStrip.Items.Count > 0)
            {
                menuStrip.Show(Cursor.Position);
            }
        }

        #region Base Class Overrides
        protected override void HeaderForeColorChanged()
        {
            base.HeaderForeColorChanged();

            // change the text color on the data grid headers
            this.dgDetail.ColumnHeadersDefaultCellStyle.ForeColor = this.HeaderForeColor;
            this.dgSummary.ColumnHeadersDefaultCellStyle.ForeColor = this.HeaderForeColor;
        }

        protected override void RowFontChanged()
        {
            base.RowFontChanged();

            // change the font on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.Font = this.RowFont;
            this.dgSummary.RowsDefaultCellStyle.Font = this.RowFont;

            // Trigger a resize so that dgSummary can size itself appropriately
            this.OnResize(new EventArgs());
        }

        protected override void RowBackColorChanged()
        {
            base.RowBackColorChanged();

            // change the back color on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.BackColor = this.RowBackColor;
            this.dgSummary.RowsDefaultCellStyle.BackColor = this.RowBackColor;

            this.dgDetail.RowsDefaultCellStyle.SelectionBackColor = this.RowBackColor;  // this hides the Detail cell selection box by making it the same as row back color
            this.dgSummary.RowsDefaultCellStyle.SelectionBackColor = this.RowBackColor; // this hides the Summary cell selection box by making it the same as row back color 

            this.dgDetail.BackgroundColor = this.RowBackColor;
            this.dgSummary.BackgroundColor = this.RowBackColor;
            this.tlPanel.BackColor = this.RowBackColor;
        }

        protected override void RowForeColorChanged()
        {
            base.RowForeColorChanged();

            // change the fore color on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.ForeColor = this.RowForeColor;
            this.dgSummary.RowsDefaultCellStyle.ForeColor = this.RowForeColor;

            this.dgDetail.RowsDefaultCellStyle.SelectionForeColor = this.RowForeColor;  // this blends the Detail cell selection text by making it the same as row fore color 
            this.dgSummary.RowsDefaultCellStyle.SelectionForeColor = this.RowForeColor; // this blends the Summary cell selection text by making it the same as row fore color 
        }
        #endregion


    }
}
