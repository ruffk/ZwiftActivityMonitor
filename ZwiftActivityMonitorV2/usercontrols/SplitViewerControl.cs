using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ZwiftActivityMonitorV2
{

    public partial class SplitViewerControl : ViewerUserControlEx
    {
        //public Color HeaderGradientBeginColor { get; set; } = SystemColors.Control;
        //public Color HeaderGradientEndColor { get; set; } = SystemColors.ControlDark;

        private enum DetailColumn
        {
            SplitNumber = 0,
            SplitTime,
            SplitSpeed,
            SplitDistance,
            TotalTime,
            Delta,
            Blank
        }

        private enum SummaryColumn
        {
            Reserved = 0,
            GoalDistance,
            GoalSpeed,
            GoalTime,
            Blank
        }

        // A height of 19 is minimum when using Segoe UI 9pt font
        //private const int DataGridRowMinimumHeight = 19;


        //private Size BaseControlSize { get; set; }

        public SplitViewerControl()
        {
            InitializeComponent();

            InitializeDetailDataGrid();
            LoadDetailDataGrid();

            InitializeSummaryDataGrid();
            LoadSummaryDataGrid();

        }

        private void ViewControl_Load(object sender, EventArgs e)
        {
            // Trigger a resize so that dgSummary can size itself appropriately
            this.OnResize(new EventArgs());
        }


        private void InitializeDetailDataGrid()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("SplitNumber", typeof(int)));
            table.Columns.Add(new DataColumn("SplitTime", typeof(string)));
            table.Columns.Add(new DataColumn("SplitSpeed", typeof(string)));
            table.Columns.Add(new DataColumn("SplitDistance", typeof(string)));
            table.Columns.Add(new DataColumn("TotalTime", typeof(string)));
            table.Columns.Add(new DataColumn("Delta", typeof(string)));
            table.Columns.Add(new DataColumn("Blank", typeof(string)));

            // set in designer
            //dgDetail.ReadOnly = true;


            this.dgDetail.DataSource = table;

            this.dgDetail.Columns[(int)DetailColumn.SplitNumber].Width = 36;
            this.dgDetail.Columns[(int)DetailColumn.SplitNumber].HeaderText = "#";

            this.dgDetail.Columns[(int)DetailColumn.SplitTime].Width = 48;
            this.dgDetail.Columns[(int)DetailColumn.SplitTime].HeaderText = "Time";

            this.dgDetail.Columns[(int)DetailColumn.SplitSpeed].Width = 48;
            this.dgDetail.Columns[(int)DetailColumn.SplitSpeed].HeaderText = "km/h";

            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].Width = 50;
            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].HeaderText = "km";

            this.dgDetail.Columns[(int)DetailColumn.TotalTime].Width = 72;
            this.dgDetail.Columns[(int)DetailColumn.TotalTime].HeaderText = "Time";

            this.dgDetail.Columns[(int)DetailColumn.Delta].Width = 60;
            this.dgDetail.Columns[(int)DetailColumn.Delta].HeaderText = "+/-";

            this.dgDetail.Columns[(int)DetailColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero
            this.dgDetail.Columns[(int)DetailColumn.Blank].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
            foreach (DataGridViewColumn c in this.dgDetail.Columns)
            {
                c.MinimumWidth = c.Width;
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
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
        }

        private void LoadDetailDataGrid()
        {
            DataTable table = (DataTable)dgDetail.DataSource;
            table.Rows.Clear();

            table.Rows.Add(1, "88:88", "88.8", "888.8", "88:88:88", "+88:88");
            table.Rows.Add(10, "88:88", "88.8", "888.8", "88:88:88", "-88:88");
            table.Rows.Add(75, "88:88", "88.8", "888.8", "88:88:88", "+88:88");

            // A height of 19 is minimum when using Segoe UI 9pt font
            this.dgDetail.Rows[0].MinimumHeight = DataGridRowMinimumHeight;
            this.dgDetail.Rows[1].MinimumHeight = DataGridRowMinimumHeight;
            this.dgDetail.Rows[2].MinimumHeight = DataGridRowMinimumHeight;

            dgDetail.Sort(dgDetail.Columns[(int)DetailColumn.SplitNumber], System.ComponentModel.ListSortDirection.Descending);

        }

        private void InitializeSummaryDataGrid()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("Reserved", typeof(string)));
            table.Columns.Add(new DataColumn("km", typeof(string)));
            table.Columns.Add(new DataColumn("km/h", typeof(string)));
            table.Columns.Add(new DataColumn("Time", typeof(string)));
            table.Columns.Add(new DataColumn("Blank", typeof(string)));

            // set in designer
            //dgSummary.ReadOnly = true;


            this.dgSummary.DataSource = table;

            this.dgSummary.Columns[(int)SummaryColumn.Reserved].Width = 48;
            this.dgSummary.Columns[(int)SummaryColumn.Reserved].HeaderText = "";

            this.dgSummary.Columns[(int)SummaryColumn.GoalDistance].Width = 50;
            this.dgSummary.Columns[(int)SummaryColumn.GoalDistance].HeaderText = "Distance";

            this.dgSummary.Columns[(int)SummaryColumn.GoalSpeed].Width = 48;
            this.dgSummary.Columns[(int)SummaryColumn.GoalSpeed].HeaderText = "Speed";

            this.dgSummary.Columns[(int)SummaryColumn.GoalTime].Width = 48;
            this.dgSummary.Columns[(int)SummaryColumn.GoalTime].HeaderText = "Time";

            // Use the last blank column to fill the gap if user resizes
            this.dgSummary.Columns[(int)SummaryColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero
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

            table.Rows.Add("Goal", "888.8", "88.8", "88:88:88");

            // A height of 19 is minimum when using Segoe UI 9pt font
            this.dgSummary.Rows[0].MinimumHeight = DataGridRowMinimumHeight;
        }



        private void ViewControl_Resize(object sender, EventArgs e)
        {
            //Debug.WriteLine($"ViewControl_Resize - Size: {this.Size}");

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
        }

        private void ViewControl_SizeChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"ViewControl_SizeChanged - Size: {this.Size}");
        }

        private int TotalVisibleColumnWidth(DataGridView dgv)
        {
            int totalWidth = 0;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible && column.HeaderText != "")
                    totalWidth += column.Width;
            }

            Debug.WriteLine($"TotalVisibleColumnWidth - Control: {this.Name}, DataGrid: {dgv.Name}, Size: {totalWidth}");

            return totalWidth;
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
            //ContextMenuStrip menuStrip = new ContextMenuStrip();

            //if (dataGridView == this.dgDetail)
            //{
            //    if (e.ColumnIndex == (int)DetailColumn.Period)
            //    {
            //        List<string> collectors = new();
            //        collectors.Add("5 sec");
            //        collectors.Add("30 sec");
            //        collectors.Add("1 min");
            //        collectors.Add("5 min");
            //        collectors.Add("6 min");
            //        collectors.Add("10 min");
            //        collectors.Add("20 min");
            //        collectors.Add("30 min");
            //        collectors.Add("60 min");
            //        collectors.Add("90 min");

            //        for (int i = 0; i < dataGridView.Rows.Count; i++)
            //        {
            //            string period = (string)dataGridView[(int)DetailColumn.Period, i].Value;

            //            var item = (ToolStripMenuItem)menuStrip.Items.Add(period);
            //            item.Tag = i; // row index
            //            item.Checked = true;
            //            item.CheckOnClick = true;
            //            item.CheckStateChanged += this.periodContextMenu_CheckStateChanged;

            //            // Remove this period from the list
            //            collectors.Remove(period);
            //        }

            //        menuStrip.Items.Add(new ToolStripSeparator());

            //        // Add remaining items from the list to the menu
            //        foreach (string period in collectors)
            //        {
            //            var item = (ToolStripMenuItem)menuStrip.Items.Add(period);
            //            item.CheckOnClick = true;
            //            item.CheckStateChanged += this.periodContextMenu_CheckStateChanged;
            //        }

            //        menuStrip.Show(Cursor.Position);
            //        menuStrip.Items[e.RowIndex].Select(); // position the highlighted cursor on the item matching the row selected
            //    }
            //}
        }

        private void periodContextMenu_CheckStateChanged(object sender, EventArgs e)
        {
            //ToolStripMenuItem item = (ToolStripMenuItem)sender;

            //if (item.Checked)
            //{
            //    DataTable table = (DataTable)dgDetail.DataSource;
                
            //    table.Rows.Add(item.Text, "250", "247", "", "166");

            //    dgDetail.Rows[dgDetail.Rows.Count - 1].MinimumHeight = 19;
            //}
            //else
            //{
            //    // The Row Index is stored in Tag
            //    // Don't allow removal of the last row
            //    if (dgDetail.Rows.Count > 1)
            //        dgDetail.Rows.RemoveAt((int)item.Tag);
            //}
        }

        /// <summary>
        /// Handle mouse click on column headers and allow visibility change
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="e"></param>
        private void dataGridView_ColumnHeaderMouseClick(DataGridView dataGridView, DataGridViewCellMouseEventArgs e)
        {
            //ContextMenuStrip menuStrip = new ContextMenuStrip();

            ////Add the columns to context menu strip
            //foreach (DataGridViewColumn c in dataGridView.Columns)
            //{
            //    if (c.HeaderText != "") // exclude filler column
            //    {
            //        var item = (ToolStripMenuItem)menuStrip.Items.Add(c.HeaderText);
            //        item.Tag = c.Name;
            //        item.Checked = c.Visible;
            //        item.CheckOnClick = true;

            //        item.Enabled = (dataGridView != dgDetail || c.Index != (int)DetailColumn.Period);

            //        //Handle CheckStateChanged event of context menu strip items
            //        item.CheckStateChanged += (obj, args) =>
            //        {

            //            // Determine how many columns are currently visible
            //            int visibleCount = 0;
            //            for (int j = 0; j < dataGridView.Columns.Count; j++)
            //                if (dataGridView.Columns[j].Visible && dataGridView.Columns[j].HeaderText != "")
            //                    visibleCount++;

            //            ToolStripMenuItem item = (ToolStripMenuItem)obj;
            //            string columnName = (string)item.Tag;
            //            int columnIndex = dataGridView.Columns[columnName].Index;

            //            //Debug.WriteLine($"{item.GetCurrentParent().GetType()}");

            //            if (item.Checked)
            //            {
            //                dataGridView.Columns[columnName].Visible = item.Checked;
            //            }
            //            else
            //            {
            //                // Don't allow removing of every column (empty grid)
            //                if (visibleCount > 1)
            //                    dataGridView.Columns[columnName].Visible = item.Checked;
            //            }

            //            // Trigger a resize so the the last column (Blank) can be shown/hidden if necessary
            //            this.OnResize(new EventArgs());
            //        };
            //    }
            //}

            //if (menuStrip.Items.Count > 0)
            //{
            //    menuStrip.Show(Cursor.Position);
            //    menuStrip.Items[e.ColumnIndex].Select(); // position the highlighted cursor on the item matching the column selected
            //}
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
