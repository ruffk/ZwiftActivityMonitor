using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace ZwiftActivityMonitorV2
{

    public partial class LapViewerControl : ViewerUserControlEx
    {
        //public Color HeaderGradientBeginColor { get; set; } = SystemColors.Control;
        //public Color HeaderGradientEndColor { get; set; } = SystemColors.ControlDark;

        private enum DetailColumn
        {
            LapNumber = 0,
            LapTime,
            LapSpeed,
            LapDistance,
            LapAP,
            TotalTime,
            Blank
        }

        private enum SummaryColumn
        {
            Blank
        }

        // A height of 19 is minimum when using Segoe UI 9pt font
        //private const int DataGridRowMinimumHeight = 19;


        public LapViewerControl()
        {
            InitializeComponent();

            InitializeDetailDataGrid();
            LoadDetailDataGrid();

            //InitializeSummaryDataGrid();
            //LoadSummaryDataGrid();

        }

        private void ViewControl_Load(object sender, EventArgs e)
        {
            // Trigger a resize so the the last column (Blank) can be shown/hidden if necessary
            this.OnResize(new EventArgs());
        }

        protected override void HeaderForeColorChanged()
        {
            base.HeaderForeColorChanged();

            // change the text color on the data grid headers
            this.dgDetail.ColumnHeadersDefaultCellStyle.ForeColor = this.HeaderForeColor;
            //this.dgSummary.ColumnHeadersDefaultCellStyle.ForeColor = this.HeaderForeColor;
        }

        protected override void RowFontChanged()
        {
            base.RowFontChanged();

            // change the font on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.Font = this.RowFont;
            //this.dgSummary.RowsDefaultCellStyle.Font = this.RowFont;
        }

        protected override void RowBackColorChanged()
        {
            base.RowBackColorChanged();

            // change the back color on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.BackColor = this.RowBackColor;
            //this.dgSummary.RowsDefaultCellStyle.BackColor = this.RowBackColor;

            this.dgDetail.RowsDefaultCellStyle.SelectionBackColor = this.RowBackColor;  // this hides the Detail cell selection box by making it the same as row back color
            //this.dgSummary.RowsDefaultCellStyle.SelectionBackColor = value; // this hides the Summary cell selection box by making it the same as row back color 

            this.dgDetail.BackgroundColor = this.RowBackColor;
            //this.dgSummary.BackgroundColor = this.RowBackColor;
            this.tlPanel.BackColor = this.RowBackColor;
        }

        protected override void RowForeColorChanged()
        {
            base.RowForeColorChanged();

            // change the fore color on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.ForeColor = this.RowForeColor;
            //this.dgSummary.RowsDefaultCellStyle.ForeColor = this.RowForeColor;

            this.dgDetail.RowsDefaultCellStyle.SelectionForeColor = this.RowForeColor;  // this blends the Detail cell selection text by making it the same as row fore color 
            //this.dgSummary.RowsDefaultCellStyle.SelectionForeColor = this.RowForeColor; // this blends the Summary cell selection text by making it the same as row fore color 
        }

        private void InitializeDetailDataGrid()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("LapNumber", typeof(int)));
            table.Columns.Add(new DataColumn("LapTime", typeof(string)));
            table.Columns.Add(new DataColumn("LapSpeed", typeof(string)));
            table.Columns.Add(new DataColumn("LapDistance", typeof(string)));
            table.Columns.Add(new DataColumn("LapAP", typeof(string)));
            table.Columns.Add(new DataColumn("TotalTime", typeof(string)));
            table.Columns.Add(new DataColumn("Blank", typeof(string)));

            // set in designer
            //dgDetail.ReadOnly = true;


            this.dgDetail.DataSource = table;

            this.dgDetail.Columns[(int)DetailColumn.LapNumber].Width = 31; // minimum 28
            this.dgDetail.Columns[(int)DetailColumn.LapNumber].HeaderText = "#";

            this.dgDetail.Columns[(int)DetailColumn.LapTime].Width = 66; 
            this.dgDetail.Columns[(int)DetailColumn.LapTime].HeaderText = "Time";

            this.dgDetail.Columns[(int)DetailColumn.LapSpeed].Width = 48;
            this.dgDetail.Columns[(int)DetailColumn.LapSpeed].HeaderText = "km/h";

            this.dgDetail.Columns[(int)DetailColumn.LapDistance].Width = 50;
            this.dgDetail.Columns[(int)DetailColumn.LapDistance].HeaderText = "km";

            this.dgDetail.Columns[(int)DetailColumn.LapAP].Width = 50;
            this.dgDetail.Columns[(int)DetailColumn.LapAP].HeaderText = "AP";

            this.dgDetail.Columns[(int)DetailColumn.TotalTime].Width = 72;
            this.dgDetail.Columns[(int)DetailColumn.TotalTime].HeaderText = "Time";

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

            table.Rows.Add(1, "8:88:88", "88.8", "888.8", "888", "88:88:88");
            table.Rows.Add(2, "8:88:88", "88.8", "888.8", "888", "88:88:88");
            table.Rows.Add(88, "8:88:88", "88.8", "888.8", "888", "88:88:88");

            // A height of 19 is minimum when using Segoe UI 9pt font
            this.dgDetail.Rows[0].MinimumHeight = DataGridRowMinimumHeight;
            this.dgDetail.Rows[1].MinimumHeight = DataGridRowMinimumHeight;
            this.dgDetail.Rows[2].MinimumHeight = DataGridRowMinimumHeight;

            dgDetail.Sort(dgDetail.Columns[(int)DetailColumn.LapNumber], System.ComponentModel.ListSortDirection.Descending);
        }




        private void ViewControl_Resize(object sender, EventArgs e)
        {
            //Debug.WriteLine($"ViewControl_Resize - Size: {this.Size}, BlankWidth: {this.dgDetail.Columns[(int)DetailColumn.Blank].Width}, MinimumWidth: {this.dgDetail.Columns[(int)DetailColumn.Blank].MinimumWidth}");
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
            ToolStripMenuItem item;

            if (dataGridView == this.dgDetail)
            {
                switch (e.ColumnIndex)
                {
                    case (int)DetailColumn.LapAP:
                        item = (ToolStripMenuItem)menuStrip.Items.Add("Watts");
                        item = (ToolStripMenuItem)menuStrip.Items.Add("W/Kg");
                        item = (ToolStripMenuItem)menuStrip.Items.Add("Both Watts && W/Kg");
                        item = (ToolStripMenuItem)menuStrip.Items.Add("Hide Field");

                        foreach (ToolStripMenuItem mi in menuStrip.Items)
                        {
                            mi.CheckOnClick = true;
                            item.Tag = new KeyValuePair<string, int>("RowIndex", e.RowIndex); // not sure what to use
                            item.CheckedChanged += powerContextMenu_CheckStateChanged;

                        }
                        menuStrip.Show(Cursor.Position);
                        break;
                }
            }
        }

    }
}
