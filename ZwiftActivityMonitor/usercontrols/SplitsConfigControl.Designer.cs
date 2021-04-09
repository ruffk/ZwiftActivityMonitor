
namespace ZwiftActivityMonitor
{
    partial class SplitsConfigControl
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
            "5 km",
            "00:08:43",
            "5 km",
            "00:08:43"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "5 km",
            "00:08:43",
            "10 km",
            "00:17:25"}, -1);
            this.tbDescSystem = new System.Windows.Forms.TextBox();
            this.gbSplits = new System.Windows.Forms.GroupBox();
            this.pSplits = new System.Windows.Forms.Panel();
            this.tbGoalDistance = new System.Windows.Forms.TextBox();
            this.lblGoalDistance = new System.Windows.Forms.Label();
            this.pSplitChart = new System.Windows.Forms.Panel();
            this.lvSplits = new System.Windows.Forms.ListView();
            this.chSplitDistance = new System.Windows.Forms.ColumnHeader();
            this.chSplitTime = new System.Windows.Forms.ColumnHeader();
            this.chTotalDistance = new System.Windows.Forms.ColumnHeader();
            this.chTotalTime = new System.Windows.Forms.ColumnHeader();
            this.chBlank = new System.Windows.Forms.ColumnHeader();
            this.pSplitChartFooter = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.lblGoalSpeed = new System.Windows.Forms.Label();
            this.pSplitChartTitle = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnEditSettings = new System.Windows.Forms.Button();
            this.tbGoalSecs = new System.Windows.Forms.TextBox();
            this.tbGoalMins = new System.Windows.Forms.TextBox();
            this.tbGoalHrs = new System.Windows.Forms.TextBox();
            this.lblGoalTime = new System.Windows.Forms.Label();
            this.ckbCalculateGoal = new System.Windows.Forms.CheckBox();
            this.cbSplitUom = new System.Windows.Forms.ComboBox();
            this.tbSplitDistance = new System.Windows.Forms.TextBox();
            this.lblSplitsEvery = new System.Windows.Forms.Label();
            this.ckbShowSplits = new System.Windows.Forms.CheckBox();
            this.lblGoalDistanceUom = new System.Windows.Forms.Label();
            this.lblGoalTimeHrs = new System.Windows.Forms.Label();
            this.lblGoalTimeMin = new System.Windows.Forms.Label();
            this.lblGoalTimeSec = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.gbSplits.SuspendLayout();
            this.pSplits.SuspendLayout();
            this.pSplitChart.SuspendLayout();
            this.pSplitChartFooter.SuspendLayout();
            this.pSplitChartTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBase
            // 
            this.pBase.Controls.Add(this.gbSplits);
            this.pBase.Controls.Add(this.tbDescSystem);
            this.pBase.Size = new System.Drawing.Size(587, 518);
            // 
            // tbDescSystem
            // 
            this.tbDescSystem.BackColor = System.Drawing.SystemColors.Control;
            this.tbDescSystem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDescSystem.Location = new System.Drawing.Point(20, 20);
            this.tbDescSystem.Multiline = true;
            this.tbDescSystem.Name = "tbDescSystem";
            this.tbDescSystem.ReadOnly = true;
            this.tbDescSystem.Size = new System.Drawing.Size(545, 47);
            this.tbDescSystem.TabIndex = 4;
            this.tbDescSystem.TabStop = false;
            this.tbDescSystem.Text = "Configure splits display options.  Splits will appear whenever the split distance" +
    " is traveled.  Set an optional goal to see if your pace is on-track.";
            this.tbDescSystem.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // gbSplits
            // 
            this.gbSplits.Controls.Add(this.pSplits);
            this.gbSplits.Location = new System.Drawing.Point(20, 73);
            this.gbSplits.Name = "gbSplits";
            this.gbSplits.Size = new System.Drawing.Size(548, 427);
            this.gbSplits.TabIndex = 5;
            this.gbSplits.TabStop = false;
            this.gbSplits.Text = "Splits";
            // 
            // pSplits
            // 
            this.pSplits.Controls.Add(this.tbGoalDistance);
            this.pSplits.Controls.Add(this.lblGoalDistance);
            this.pSplits.Controls.Add(this.pSplitChart);
            this.pSplits.Controls.Add(this.btnCancelSettings);
            this.pSplits.Controls.Add(this.btnSaveSettings);
            this.pSplits.Controls.Add(this.btnEditSettings);
            this.pSplits.Controls.Add(this.tbGoalSecs);
            this.pSplits.Controls.Add(this.tbGoalMins);
            this.pSplits.Controls.Add(this.tbGoalHrs);
            this.pSplits.Controls.Add(this.lblGoalTime);
            this.pSplits.Controls.Add(this.ckbCalculateGoal);
            this.pSplits.Controls.Add(this.cbSplitUom);
            this.pSplits.Controls.Add(this.tbSplitDistance);
            this.pSplits.Controls.Add(this.lblSplitsEvery);
            this.pSplits.Controls.Add(this.ckbShowSplits);
            this.pSplits.Controls.Add(this.lblGoalDistanceUom);
            this.pSplits.Controls.Add(this.lblGoalTimeHrs);
            this.pSplits.Controls.Add(this.lblGoalTimeMin);
            this.pSplits.Controls.Add(this.lblGoalTimeSec);
            this.pSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplits.Location = new System.Drawing.Point(3, 19);
            this.pSplits.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pSplits.Name = "pSplits";
            this.pSplits.Size = new System.Drawing.Size(542, 405);
            this.pSplits.TabIndex = 0;
            // 
            // tbGoalDistance
            // 
            this.tbGoalDistance.Location = new System.Drawing.Point(126, 142);
            this.tbGoalDistance.MaxLength = 5;
            this.tbGoalDistance.Name = "tbGoalDistance";
            this.tbGoalDistance.Size = new System.Drawing.Size(39, 23);
            this.tbGoalDistance.TabIndex = 75;
            this.tbGoalDistance.Text = "88888";
            this.tbGoalDistance.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbGoalDistance.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbGoalDistance.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblGoalDistance
            // 
            this.lblGoalDistance.AutoSize = true;
            this.lblGoalDistance.Location = new System.Drawing.Point(38, 145);
            this.lblGoalDistance.Name = "lblGoalDistance";
            this.lblGoalDistance.Size = new System.Drawing.Size(82, 15);
            this.lblGoalDistance.TabIndex = 112;
            this.lblGoalDistance.Text = "Goal Distance:";
            // 
            // pSplitChart
            // 
            this.pSplitChart.AutoScroll = true;
            this.pSplitChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pSplitChart.Controls.Add(this.lvSplits);
            this.pSplitChart.Controls.Add(this.pSplitChartFooter);
            this.pSplitChart.Controls.Add(this.pSplitChartTitle);
            this.pSplitChart.Location = new System.Drawing.Point(126, 177);
            this.pSplitChart.Name = "pSplitChart";
            this.pSplitChart.Size = new System.Drawing.Size(282, 211);
            this.pSplitChart.TabIndex = 111;
            // 
            // lvSplits
            // 
            this.lvSplits.BackColor = System.Drawing.SystemColors.Control;
            this.lvSplits.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvSplits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSplitDistance,
            this.chSplitTime,
            this.chTotalDistance,
            this.chTotalTime,
            this.chBlank});
            this.lvSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSplits.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvSplits.HideSelection = false;
            this.lvSplits.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lvSplits.Location = new System.Drawing.Point(0, 18);
            this.lvSplits.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lvSplits.MultiSelect = false;
            this.lvSplits.Name = "lvSplits";
            this.lvSplits.OwnerDraw = true;
            this.lvSplits.Size = new System.Drawing.Size(280, 173);
            this.lvSplits.TabIndex = 80;
            this.lvSplits.TabStop = false;
            this.lvSplits.UseCompatibleStateImageBehavior = false;
            this.lvSplits.View = System.Windows.Forms.View.Details;
            this.lvSplits.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvSplits.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvSplits.Resize += new System.EventHandler(this.ListView_Resize);
            // 
            // chSplitDistance
            // 
            this.chSplitDistance.Name = "chSplitDistance";
            this.chSplitDistance.Text = "Distance";
            // 
            // chSplitTime
            // 
            this.chSplitTime.Name = "chSplitTime";
            this.chSplitTime.Text = "Time";
            this.chSplitTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chSplitTime.Width = 70;
            // 
            // chTotalDistance
            // 
            this.chTotalDistance.Name = "chTotalDistance";
            this.chTotalDistance.Text = "Distance";
            // 
            // chTotalTime
            // 
            this.chTotalTime.Name = "chTotalTime";
            this.chTotalTime.Text = "Time";
            this.chTotalTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chTotalTime.Width = 70;
            // 
            // chBlank
            // 
            this.chBlank.Text = "";
            this.chBlank.Width = 20;
            // 
            // pSplitChartFooter
            // 
            this.pSplitChartFooter.Controls.Add(this.label8);
            this.pSplitChartFooter.Controls.Add(this.lblGoalSpeed);
            this.pSplitChartFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pSplitChartFooter.Location = new System.Drawing.Point(0, 191);
            this.pSplitChartFooter.Name = "pSplitChartFooter";
            this.pSplitChartFooter.Size = new System.Drawing.Size(280, 18);
            this.pSplitChartFooter.TabIndex = 81;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(0, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 15);
            this.label8.TabIndex = 95;
            this.label8.Text = "Goal Speed:";
            // 
            // lblGoalSpeed
            // 
            this.lblGoalSpeed.Location = new System.Drawing.Point(67, 1);
            this.lblGoalSpeed.Name = "lblGoalSpeed";
            this.lblGoalSpeed.Size = new System.Drawing.Size(80, 15);
            this.lblGoalSpeed.TabIndex = 96;
            this.lblGoalSpeed.Text = "43.6 kph";
            // 
            // pSplitChartTitle
            // 
            this.pSplitChartTitle.BackColor = System.Drawing.SystemColors.Control;
            this.pSplitChartTitle.Controls.Add(this.label6);
            this.pSplitChartTitle.Controls.Add(this.label7);
            this.pSplitChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pSplitChartTitle.Location = new System.Drawing.Point(0, 0);
            this.pSplitChartTitle.Name = "pSplitChartTitle";
            this.pSplitChartTitle.Size = new System.Drawing.Size(280, 18);
            this.pSplitChartTitle.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(16, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 15);
            this.label6.TabIndex = 90;
            this.label6.Text = "Split";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(146, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 15);
            this.label7.TabIndex = 91;
            this.label7.Text = "Total";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.Location = new System.Drawing.Point(439, 83);
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(89, 28);
            this.btnCancelSettings.TabIndex = 110;
            this.btnCancelSettings.Text = "Cancel";
            this.btnCancelSettings.UseVisualStyleBackColor = true;
            this.btnCancelSettings.Click += new System.EventHandler(this.btnCancelSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(439, 47);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(89, 28);
            this.btnSaveSettings.TabIndex = 100;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnEditSettings
            // 
            this.btnEditSettings.Location = new System.Drawing.Point(439, 11);
            this.btnEditSettings.Name = "btnEditSettings";
            this.btnEditSettings.Size = new System.Drawing.Size(89, 28);
            this.btnEditSettings.TabIndex = 90;
            this.btnEditSettings.Text = "Edit";
            this.btnEditSettings.UseVisualStyleBackColor = true;
            this.btnEditSettings.Click += new System.EventHandler(this.btnEditSettings_Click);
            // 
            // tbGoalSecs
            // 
            this.tbGoalSecs.Location = new System.Drawing.Point(236, 110);
            this.tbGoalSecs.MaxLength = 2;
            this.tbGoalSecs.Name = "tbGoalSecs";
            this.tbGoalSecs.Size = new System.Drawing.Size(23, 23);
            this.tbGoalSecs.TabIndex = 70;
            this.tbGoalSecs.Text = "88";
            this.tbGoalSecs.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbGoalSecs.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbGoalSecs.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // tbGoalMins
            // 
            this.tbGoalMins.Location = new System.Drawing.Point(179, 110);
            this.tbGoalMins.MaxLength = 2;
            this.tbGoalMins.Name = "tbGoalMins";
            this.tbGoalMins.Size = new System.Drawing.Size(23, 23);
            this.tbGoalMins.TabIndex = 60;
            this.tbGoalMins.Text = "88";
            this.tbGoalMins.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbGoalMins.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbGoalMins.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // tbGoalHrs
            // 
            this.tbGoalHrs.Location = new System.Drawing.Point(126, 110);
            this.tbGoalHrs.MaxLength = 2;
            this.tbGoalHrs.Name = "tbGoalHrs";
            this.tbGoalHrs.Size = new System.Drawing.Size(23, 23);
            this.tbGoalHrs.TabIndex = 50;
            this.tbGoalHrs.Text = "88";
            this.tbGoalHrs.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbGoalHrs.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbGoalHrs.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblGoalTime
            // 
            this.lblGoalTime.AutoSize = true;
            this.lblGoalTime.Location = new System.Drawing.Point(57, 113);
            this.lblGoalTime.Name = "lblGoalTime";
            this.lblGoalTime.Size = new System.Drawing.Size(63, 15);
            this.lblGoalTime.TabIndex = 6;
            this.lblGoalTime.Text = "Goal Time:";
            // 
            // ckbCalculateGoal
            // 
            this.ckbCalculateGoal.AutoSize = true;
            this.ckbCalculateGoal.Location = new System.Drawing.Point(34, 81);
            this.ckbCalculateGoal.Name = "ckbCalculateGoal";
            this.ckbCalculateGoal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ckbCalculateGoal.Size = new System.Drawing.Size(102, 19);
            this.ckbCalculateGoal.TabIndex = 40;
            this.ckbCalculateGoal.Text = "Calculate Goal";
            this.ckbCalculateGoal.UseVisualStyleBackColor = true;
            this.ckbCalculateGoal.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.ckbCalculateGoal.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.ckbCalculateGoal.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // cbSplitUom
            // 
            this.cbSplitUom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSplitUom.FormattingEnabled = true;
            this.cbSplitUom.Location = new System.Drawing.Point(161, 46);
            this.cbSplitUom.Name = "cbSplitUom";
            this.cbSplitUom.Size = new System.Drawing.Size(47, 23);
            this.cbSplitUom.TabIndex = 30;
            this.cbSplitUom.SelectionChangeCommitted += new System.EventHandler(this.cbSplitUom_SelectionChangeCommitted);
            this.cbSplitUom.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.cbSplitUom.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.cbSplitUom.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // tbSplitDistance
            // 
            this.tbSplitDistance.Location = new System.Drawing.Point(126, 46);
            this.tbSplitDistance.MaxLength = 3;
            this.tbSplitDistance.Name = "tbSplitDistance";
            this.tbSplitDistance.Size = new System.Drawing.Size(26, 23);
            this.tbSplitDistance.TabIndex = 20;
            this.tbSplitDistance.Text = "888";
            this.tbSplitDistance.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbSplitDistance.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbSplitDistance.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblSplitsEvery
            // 
            this.lblSplitsEvery.AutoSize = true;
            this.lblSplitsEvery.Location = new System.Drawing.Point(51, 49);
            this.lblSplitsEvery.Name = "lblSplitsEvery";
            this.lblSplitsEvery.Size = new System.Drawing.Size(69, 15);
            this.lblSplitsEvery.TabIndex = 2;
            this.lblSplitsEvery.Text = "Splits Every:";
            // 
            // ckbShowSplits
            // 
            this.ckbShowSplits.AutoSize = true;
            this.ckbShowSplits.Location = new System.Drawing.Point(34, 17);
            this.ckbShowSplits.Name = "ckbShowSplits";
            this.ckbShowSplits.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ckbShowSplits.Size = new System.Drawing.Size(86, 19);
            this.ckbShowSplits.TabIndex = 10;
            this.ckbShowSplits.Text = "Show Splits";
            this.ckbShowSplits.UseVisualStyleBackColor = true;
            this.ckbShowSplits.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.ckbShowSplits.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.ckbShowSplits.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblGoalDistanceUom
            // 
            this.lblGoalDistanceUom.AutoSize = true;
            this.lblGoalDistanceUom.Location = new System.Drawing.Point(165, 145);
            this.lblGoalDistanceUom.Name = "lblGoalDistanceUom";
            this.lblGoalDistanceUom.Size = new System.Drawing.Size(24, 15);
            this.lblGoalDistanceUom.TabIndex = 114;
            this.lblGoalDistanceUom.Text = "km";
            // 
            // lblGoalTimeHrs
            // 
            this.lblGoalTimeHrs.AutoSize = true;
            this.lblGoalTimeHrs.Location = new System.Drawing.Point(150, 113);
            this.lblGoalTimeHrs.Name = "lblGoalTimeHrs";
            this.lblGoalTimeHrs.Size = new System.Drawing.Size(23, 15);
            this.lblGoalTimeHrs.TabIndex = 10;
            this.lblGoalTimeHrs.Text = "hrs";
            // 
            // lblGoalTimeMin
            // 
            this.lblGoalTimeMin.AutoSize = true;
            this.lblGoalTimeMin.Location = new System.Drawing.Point(202, 113);
            this.lblGoalTimeMin.Name = "lblGoalTimeMin";
            this.lblGoalTimeMin.Size = new System.Drawing.Size(28, 15);
            this.lblGoalTimeMin.TabIndex = 11;
            this.lblGoalTimeMin.Text = "min";
            // 
            // lblGoalTimeSec
            // 
            this.lblGoalTimeSec.AutoSize = true;
            this.lblGoalTimeSec.Location = new System.Drawing.Point(259, 113);
            this.lblGoalTimeSec.Name = "lblGoalTimeSec";
            this.lblGoalTimeSec.Size = new System.Drawing.Size(24, 15);
            this.lblGoalTimeSec.TabIndex = 12;
            this.lblGoalTimeSec.Text = "sec";
            // 
            // SplitsConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SplitsConfigControl";
            this.Size = new System.Drawing.Size(587, 540);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.pBase.ResumeLayout(false);
            this.pBase.PerformLayout();
            this.gbSplits.ResumeLayout(false);
            this.pSplits.ResumeLayout(false);
            this.pSplits.PerformLayout();
            this.pSplitChart.ResumeLayout(false);
            this.pSplitChartFooter.ResumeLayout(false);
            this.pSplitChartTitle.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescSystem;
        private System.Windows.Forms.GroupBox gbSplits;
        private System.Windows.Forms.Panel pSplits;
        private System.Windows.Forms.Label lblSplitsEvery;
        private System.Windows.Forms.CheckBox ckbShowSplits;
        private System.Windows.Forms.TextBox tbSplitDistance;
        private System.Windows.Forms.Label lblGoalTimeSec;
        private System.Windows.Forms.Label lblGoalTimeMin;
        private System.Windows.Forms.Label lblGoalTimeHrs;
        private System.Windows.Forms.TextBox tbGoalSecs;
        private System.Windows.Forms.TextBox tbGoalMins;
        private System.Windows.Forms.TextBox tbGoalHrs;
        private System.Windows.Forms.Label lblGoalTime;
        private System.Windows.Forms.CheckBox ckbCalculateGoal;
        private System.Windows.Forms.ComboBox cbSplitUom;
        private System.Windows.Forms.ListView lvSplits;
        private System.Windows.Forms.ColumnHeader chSplitDistance;
        private System.Windows.Forms.ColumnHeader chSplitTime;
        private System.Windows.Forms.ColumnHeader chTotalDistance;
        private System.Windows.Forms.ColumnHeader chTotalTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnEditSettings;
        private System.Windows.Forms.Label lblGoalSpeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblGoalDistanceUom;
        private System.Windows.Forms.TextBox tbGoalDistance;
        private System.Windows.Forms.Label lblGoalDistance;
        private System.Windows.Forms.Panel pSplitChart;
        private System.Windows.Forms.Panel pSplitChartTitle;
        private System.Windows.Forms.Panel pSplitChartFooter;
        private System.Windows.Forms.ColumnHeader chBlank;
    }
}
