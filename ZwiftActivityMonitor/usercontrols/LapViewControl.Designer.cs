
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "88",
            "8:88:88",
            "88.8",
            "888.8",
            "888",
            "88:88:88"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Franklin Gothic Demi Cond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point));
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "2",
            "8:88:88",
            "88.8",
            "888.8",
            "888",
            "88:88:88"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
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
            this.tsbLap = new System.Windows.Forms.ToolStripButton();
            this.tsbReset = new System.Windows.Forms.ToolStripButton();
            this.label7 = new System.Windows.Forms.Label();
            this.pChartFooter = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pSplits.SuspendLayout();
            this.pChartTitle.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pChartFooter.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pSplits
            // 
            this.pSplits.Controls.Add(this.lvLaps);
            this.pSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplits.Location = new System.Drawing.Point(0, 22);
            this.pSplits.Name = "pSplits";
            this.pSplits.Size = new System.Drawing.Size(334, 107);
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
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lvLaps.Location = new System.Drawing.Point(0, 0);
            this.lvLaps.MultiSelect = false;
            this.lvLaps.Name = "lvLaps";
            this.lvLaps.OwnerDraw = true;
            this.lvLaps.Size = new System.Drawing.Size(334, 107);
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
            this.tsbLap,
            this.tsbReset});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(94, 24);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbLap
            // 
            this.tsbLap.AutoSize = false;
            this.tsbLap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLap.Image = global::ZwiftActivityMonitor.Properties.Resources.stopwatch;
            this.tsbLap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLap.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.tsbLap.Name = "tsbLap";
            this.tsbLap.Size = new System.Drawing.Size(22, 22);
            this.tsbLap.Text = "Click to start a new lap";
            this.tsbLap.Click += new System.EventHandler(this.tsbLap_Click);
            // 
            // tsbReset
            // 
            this.tsbReset.AutoSize = false;
            this.tsbReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReset.Image = global::ZwiftActivityMonitor.Properties.Resources.reset;
            this.tsbReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReset.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.tsbReset.Name = "tsbReset";
            this.tsbReset.Size = new System.Drawing.Size(22, 22);
            this.tsbReset.Text = "Click to clear laps and restart";
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
            // pChartFooter
            // 
            this.pChartFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61)))));
            this.pChartFooter.Controls.Add(this.statusStrip1);
            this.pChartFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pChartFooter.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pChartFooter.ForeColor = System.Drawing.Color.White;
            this.pChartFooter.Location = new System.Drawing.Point(0, 129);
            this.pChartFooter.Name = "pChartFooter";
            this.pChartFooter.Size = new System.Drawing.Size(334, 24);
            this.pChartFooter.TabIndex = 82;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61)))));
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(334, 24);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61)))));
            this.toolStripStatusLabel.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.toolStripStatusLabel.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 19);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // LapViewControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pSplits);
            this.Controls.Add(this.pChartTitle);
            this.Controls.Add(this.pChartFooter);
            this.Name = "LapViewControl";
            this.Size = new System.Drawing.Size(334, 153);
            this.Load += new System.EventHandler(this.UserControlBase_Load);
            this.pSplits.ResumeLayout(false);
            this.pChartTitle.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pChartFooter.ResumeLayout(false);
            this.pChartFooter.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pSplits;
        private System.Windows.Forms.Panel pChartTitle;
        private System.Windows.Forms.Label lblSplit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pChartFooter;
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
        private System.Windows.Forms.ToolStripButton tsbLap;
        private System.Windows.Forms.ToolStripButton tsbReset;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}
