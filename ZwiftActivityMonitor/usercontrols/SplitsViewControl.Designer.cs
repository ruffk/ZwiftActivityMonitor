
namespace ZwiftActivityMonitor
{
    partial class SplitsViewControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "03",
            "88:88",
            "88.8",
            "888.8",
            "88:88:88",
            "+88:88"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "02",
            "88:88",
            "88.8",
            "888.8",
            "88:88:88",
            "+88:88"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "01",
            "88:88",
            "88.8",
            "888.8",
            "88:88:88",
            "+88:88"}, -1);
            this.pSplits = new System.Windows.Forms.Panel();
            this.lvSplits = new System.Windows.Forms.ListView();
            this.chSplitNum = new System.Windows.Forms.ColumnHeader();
            this.chTime = new System.Windows.Forms.ColumnHeader();
            this.chSpeed = new System.Windows.Forms.ColumnHeader();
            this.chDistance = new System.Windows.Forms.ColumnHeader();
            this.chTime2 = new System.Windows.Forms.ColumnHeader();
            this.chDelta = new System.Windows.Forms.ColumnHeader();
            this.chBlank = new System.Windows.Forms.ColumnHeader();
            this.pSplitChartTitle = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.lblSplit = new System.Windows.Forms.Label();
            this.pSplitChartFooter = new System.Windows.Forms.Panel();
            this.lblGoalSpeed = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pSplits.SuspendLayout();
            this.pSplitChartTitle.SuspendLayout();
            this.pSplitChartFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pSplits
            // 
            this.pSplits.Controls.Add(this.lvSplits);
            this.pSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplits.Location = new System.Drawing.Point(0, 22);
            this.pSplits.Name = "pSplits";
            this.pSplits.Size = new System.Drawing.Size(334, 109);
            this.pSplits.TabIndex = 0;
            // 
            // lvSplits
            // 
            this.lvSplits.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
            this.lvSplits.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvSplits.CausesValidation = false;
            this.lvSplits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSplitNum,
            this.chTime,
            this.chSpeed,
            this.chDistance,
            this.chTime2,
            this.chDelta,
            this.chBlank});
            this.lvSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSplits.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvSplits.ForeColor = System.Drawing.Color.White;
            this.lvSplits.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvSplits.HideSelection = false;
            this.lvSplits.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lvSplits.Location = new System.Drawing.Point(0, 0);
            this.lvSplits.MultiSelect = false;
            this.lvSplits.Name = "lvSplits";
            this.lvSplits.OwnerDraw = true;
            this.lvSplits.Size = new System.Drawing.Size(334, 109);
            this.lvSplits.TabIndex = 1;
            this.lvSplits.TabStop = false;
            this.lvSplits.UseCompatibleStateImageBehavior = false;
            this.lvSplits.View = System.Windows.Forms.View.Details;
            this.lvSplits.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvSplits.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvSplits.Resize += new System.EventHandler(this.ListView_Resize_HideHorizontalScrollBar);
            // 
            // chSplitNum
            // 
            this.chSplitNum.Text = " #";
            this.chSplitNum.Width = 27;
            // 
            // chTime
            // 
            this.chTime.Text = "Time";
            this.chTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chTime.Width = 55;
            // 
            // chSpeed
            // 
            this.chSpeed.Text = "km/h";
            this.chSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chSpeed.Width = 52;
            // 
            // chDistance
            // 
            this.chDistance.Text = "km";
            this.chDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chDistance.Width = 53;
            // 
            // chTime2
            // 
            this.chTime2.Text = "Time";
            this.chTime2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chTime2.Width = 75;
            // 
            // chDelta
            // 
            this.chDelta.Text = "Delta";
            this.chDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chDelta.Width = 62;
            // 
            // chBlank
            // 
            this.chBlank.Text = "";
            this.chBlank.Width = 10;
            // 
            // pSplitChartTitle
            // 
            this.pSplitChartTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61)))));
            this.pSplitChartTitle.Controls.Add(this.label7);
            this.pSplitChartTitle.Controls.Add(this.lblSplit);
            this.pSplitChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pSplitChartTitle.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pSplitChartTitle.ForeColor = System.Drawing.Color.White;
            this.pSplitChartTitle.Location = new System.Drawing.Point(0, 0);
            this.pSplitChartTitle.Name = "pSplitChartTitle";
            this.pSplitChartTitle.Size = new System.Drawing.Size(334, 22);
            this.pSplitChartTitle.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(194, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 19);
            this.label7.TabIndex = 91;
            this.label7.Text = "Total";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblSplit
            // 
            this.lblSplit.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSplit.Location = new System.Drawing.Point(41, 2);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(140, 19);
            this.lblSplit.TabIndex = 90;
            this.lblSplit.Text = "Split";
            this.lblSplit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pSplitChartFooter
            // 
            this.pSplitChartFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61)))));
            this.pSplitChartFooter.Controls.Add(this.lblGoalSpeed);
            this.pSplitChartFooter.Controls.Add(this.label8);
            this.pSplitChartFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pSplitChartFooter.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pSplitChartFooter.ForeColor = System.Drawing.Color.White;
            this.pSplitChartFooter.Location = new System.Drawing.Point(0, 131);
            this.pSplitChartFooter.Name = "pSplitChartFooter";
            this.pSplitChartFooter.Size = new System.Drawing.Size(334, 22);
            this.pSplitChartFooter.TabIndex = 82;
            // 
            // lblGoalSpeed
            // 
            this.lblGoalSpeed.Location = new System.Drawing.Point(43, 1);
            this.lblGoalSpeed.Name = "lblGoalSpeed";
            this.lblGoalSpeed.Size = new System.Drawing.Size(291, 19);
            this.lblGoalSpeed.TabIndex = 96;
            this.lblGoalSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 22);
            this.label8.TabIndex = 95;
            this.label8.Text = "Goal:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SplitsViewControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pSplits);
            this.Controls.Add(this.pSplitChartTitle);
            this.Controls.Add(this.pSplitChartFooter);
            this.Name = "SplitsViewControl";
            this.Size = new System.Drawing.Size(334, 153);
            this.Load += new System.EventHandler(this.UserControlBase_Load);
            this.pSplits.ResumeLayout(false);
            this.pSplitChartTitle.ResumeLayout(false);
            this.pSplitChartFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pSplits;
        private System.Windows.Forms.Panel pSplitChartTitle;
        private System.Windows.Forms.Label lblSplit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pSplitChartFooter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblGoalSpeed;
        private System.Windows.Forms.ListView lvSplits;
        private System.Windows.Forms.ColumnHeader chSplitNum;
        private System.Windows.Forms.ColumnHeader chTime;
        private System.Windows.Forms.ColumnHeader chSpeed;
        private System.Windows.Forms.ColumnHeader chDistance;
        private System.Windows.Forms.ColumnHeader chTime2;
        private System.Windows.Forms.ColumnHeader chDelta;
        private System.Windows.Forms.ColumnHeader chBlank;
    }
}
