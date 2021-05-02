
namespace ZwiftActivityMonitor
{
    partial class LapViewControl
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
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "88",
            "8:88:88",
            "88.8",
            "888.8",
            "888",
            "88:88:88"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Franklin Gothic Demi Cond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "2",
            "8:88:88",
            "88.8",
            "888.8",
            "888",
            "88:88:88"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "1",
            "8:88:88",
            "88.8",
            "888.8",
            "888",
            "88:88:88"}, -1);
            this.pSplits = new System.Windows.Forms.Panel();
            this.lvLaps = new System.Windows.Forms.ListView();
            this.chFirst = new System.Windows.Forms.ColumnHeader();
            this.chLapNum = new System.Windows.Forms.ColumnHeader();
            this.chTime = new System.Windows.Forms.ColumnHeader();
            this.chSpeed = new System.Windows.Forms.ColumnHeader();
            this.chDistance = new System.Windows.Forms.ColumnHeader();
            this.chAvg = new System.Windows.Forms.ColumnHeader();
            this.chTime2 = new System.Windows.Forms.ColumnHeader();
            this.chBlank = new System.Windows.Forms.ColumnHeader();
            this.pChartTitle = new System.Windows.Forms.Panel();
            this.lblSplit = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbStart = new System.Windows.Forms.ToolStripButton();
            this.tsbStop = new System.Windows.Forms.ToolStripButton();
            this.tsbLap = new System.Windows.Forms.ToolStripButton();
            this.tsbReset = new System.Windows.Forms.ToolStripButton();
            this.label7 = new System.Windows.Forms.Label();
            this.pSplitChartFooter = new System.Windows.Forms.Panel();
            this.lblGoalSpeed = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pSplits.SuspendLayout();
            this.pChartTitle.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pSplitChartFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pSplits
            // 
            this.pSplits.Controls.Add(this.lvLaps);
            this.pSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplits.Location = new System.Drawing.Point(0, 22);
            this.pSplits.Name = "pSplits";
            this.pSplits.Size = new System.Drawing.Size(334, 109);
            this.pSplits.TabIndex = 0;
            // 
            // lvLaps
            // 
            this.lvLaps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
            this.lvLaps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvLaps.CausesValidation = false;
            this.lvLaps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFirst,
            this.chLapNum,
            this.chTime,
            this.chSpeed,
            this.chDistance,
            this.chAvg,
            this.chTime2,
            this.chBlank});
            this.lvLaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLaps.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvLaps.ForeColor = System.Drawing.Color.White;
            this.lvLaps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvLaps.HideSelection = false;
            this.lvLaps.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.lvLaps.Location = new System.Drawing.Point(0, 0);
            this.lvLaps.MultiSelect = false;
            this.lvLaps.Name = "lvLaps";
            this.lvLaps.OwnerDraw = true;
            this.lvLaps.Size = new System.Drawing.Size(334, 109);
            this.lvLaps.TabIndex = 1;
            this.lvLaps.TabStop = false;
            this.lvLaps.UseCompatibleStateImageBehavior = false;
            this.lvLaps.View = System.Windows.Forms.View.Details;
            this.lvLaps.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvLaps.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvLaps.Resize += new System.EventHandler(this.ListView_Resize_HideHorizontalScrollBar);
            // 
            // chFirst
            // 
            this.chFirst.Text = "";
            this.chFirst.Width = 0;
            // 
            // chLapNum
            // 
            this.chLapNum.Tag = "Center";
            this.chLapNum.Text = "#";
            this.chLapNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chLapNum.Width = 28;
            // 
            // chTime
            // 
            this.chTime.Text = "Time";
            this.chTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chTime.Width = 66;
            // 
            // chSpeed
            // 
            this.chSpeed.Text = "km/h";
            this.chSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chSpeed.Width = 48;
            // 
            // chDistance
            // 
            this.chDistance.Text = "km";
            this.chDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chDistance.Width = 50;
            // 
            // chAvg
            // 
            this.chAvg.Text = "Avg";
            this.chAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chAvg.Width = 50;
            // 
            // chTime2
            // 
            this.chTime2.Text = "Time";
            this.chTime2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chTime2.Width = 72;
            // 
            // chBlank
            // 
            this.chBlank.Text = "";
            this.chBlank.Width = 20;
            // 
            // pChartTitle
            // 
            this.pChartTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61)))));
            this.pChartTitle.Controls.Add(this.lblSplit);
            this.pChartTitle.Controls.Add(this.toolStrip1);
            this.pChartTitle.Controls.Add(this.label7);
            this.pChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pChartTitle.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pChartTitle.ForeColor = System.Drawing.Color.White;
            this.pChartTitle.Location = new System.Drawing.Point(0, 0);
            this.pChartTitle.Name = "pChartTitle";
            this.pChartTitle.Size = new System.Drawing.Size(334, 22);
            this.pChartTitle.TabIndex = 1;
            // 
            // lblSplit
            // 
            this.lblSplit.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSplit.Location = new System.Drawing.Point(89, 2);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(96, 19);
            this.lblSplit.TabIndex = 90;
            this.lblSplit.Text = "Lap";
            this.lblSplit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbStart,
            this.tsbStop,
            this.tsbLap,
            this.tsbReset});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(94, 24);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbStart
            // 
            this.tsbStart.AutoSize = false;
            this.tsbStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStart.ForeColor = System.Drawing.Color.White;
            this.tsbStart.Image = global::ZwiftActivityMonitor.Properties.Resources.start;
            this.tsbStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStart.Name = "tsbStart";
            this.tsbStart.Size = new System.Drawing.Size(22, 22);
            this.tsbStart.Text = "Start lap counter";
            // 
            // tsbStop
            // 
            this.tsbStop.AutoSize = false;
            this.tsbStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStop.Image = global::ZwiftActivityMonitor.Properties.Resources.stop;
            this.tsbStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStop.Name = "tsbStop";
            this.tsbStop.Size = new System.Drawing.Size(22, 22);
            this.tsbStop.Text = "Stop lap counter";
            // 
            // tsbLap
            // 
            this.tsbLap.AutoSize = false;
            this.tsbLap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLap.Image = global::ZwiftActivityMonitor.Properties.Resources.stopwatch;
            this.tsbLap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLap.Name = "tsbLap";
            this.tsbLap.Size = new System.Drawing.Size(22, 22);
            this.tsbLap.Text = "Begin new lap";
            this.tsbLap.Click += new System.EventHandler(this.tsbLap_Click);
            // 
            // tsbReset
            // 
            this.tsbReset.AutoSize = false;
            this.tsbReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReset.Image = global::ZwiftActivityMonitor.Properties.Resources.reset;
            this.tsbReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReset.Name = "tsbReset";
            this.tsbReset.Size = new System.Drawing.Size(22, 22);
            this.tsbReset.Text = "Reset lap counter";
            this.tsbReset.Click += new System.EventHandler(this.tsbReset_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(242, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 19);
            this.label7.TabIndex = 91;
            this.label7.Text = "Total";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
            this.lblGoalSpeed.Location = new System.Drawing.Point(43, 0);
            this.lblGoalSpeed.Name = "lblGoalSpeed";
            this.lblGoalSpeed.Size = new System.Drawing.Size(291, 22);
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
            // LapViewControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pSplits);
            this.Controls.Add(this.pChartTitle);
            this.Controls.Add(this.pSplitChartFooter);
            this.Name = "LapViewControl";
            this.Size = new System.Drawing.Size(334, 153);
            this.Load += new System.EventHandler(this.UserControlBase_Load);
            this.pSplits.ResumeLayout(false);
            this.pChartTitle.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pSplitChartFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pSplits;
        private System.Windows.Forms.Panel pChartTitle;
        private System.Windows.Forms.Label lblSplit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pSplitChartFooter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblGoalSpeed;
        private System.Windows.Forms.ListView lvLaps;
        private System.Windows.Forms.ColumnHeader chLapNum;
        private System.Windows.Forms.ColumnHeader chTime;
        private System.Windows.Forms.ColumnHeader chSpeed;
        private System.Windows.Forms.ColumnHeader chDistance;
        private System.Windows.Forms.ColumnHeader chTime2;
        private System.Windows.Forms.ColumnHeader chAvg;
        private System.Windows.Forms.ColumnHeader chBlank;
        private System.Windows.Forms.ColumnHeader chFirst;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbStart;
        private System.Windows.Forms.ToolStripButton tsbStop;
        private System.Windows.Forms.ToolStripButton tsbLap;
        private System.Windows.Forms.ToolStripButton tsbReset;
    }
}
