
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
            this.lblGoalSpeed = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnEditSettings = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lvSplits = new System.Windows.Forms.ListView();
            this.chSplitDistance = new System.Windows.Forms.ColumnHeader();
            this.chSplitTime = new System.Windows.Forms.ColumnHeader();
            this.chTotalDistance = new System.Windows.Forms.ColumnHeader();
            this.chTotalTime = new System.Windows.Forms.ColumnHeader();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbGoalSecs = new System.Windows.Forms.TextBox();
            this.tbGoalMins = new System.Windows.Forms.TextBox();
            this.tbGoalHrs = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ckbCalculateGoal = new System.Windows.Forms.CheckBox();
            this.cbSplitUom = new System.Windows.Forms.ComboBox();
            this.tbSplitDistance = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ckbShowSplits = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.gbSplits.SuspendLayout();
            this.pSplits.SuspendLayout();
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
            this.tbDescSystem.Text = "Configure splits display options.";
            this.tbDescSystem.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // gbSplits
            // 
            this.gbSplits.Controls.Add(this.pSplits);
            this.gbSplits.Location = new System.Drawing.Point(20, 73);
            this.gbSplits.Name = "gbSplits";
            this.gbSplits.Size = new System.Drawing.Size(548, 408);
            this.gbSplits.TabIndex = 5;
            this.gbSplits.TabStop = false;
            this.gbSplits.Text = "Splits";
            // 
            // pSplits
            // 
            this.pSplits.Controls.Add(this.lblGoalSpeed);
            this.pSplits.Controls.Add(this.label8);
            this.pSplits.Controls.Add(this.btnCancelSettings);
            this.pSplits.Controls.Add(this.btnSaveSettings);
            this.pSplits.Controls.Add(this.btnEditSettings);
            this.pSplits.Controls.Add(this.label7);
            this.pSplits.Controls.Add(this.label6);
            this.pSplits.Controls.Add(this.lvSplits);
            this.pSplits.Controls.Add(this.label5);
            this.pSplits.Controls.Add(this.label4);
            this.pSplits.Controls.Add(this.label3);
            this.pSplits.Controls.Add(this.tbGoalSecs);
            this.pSplits.Controls.Add(this.tbGoalMins);
            this.pSplits.Controls.Add(this.tbGoalHrs);
            this.pSplits.Controls.Add(this.label2);
            this.pSplits.Controls.Add(this.ckbCalculateGoal);
            this.pSplits.Controls.Add(this.cbSplitUom);
            this.pSplits.Controls.Add(this.tbSplitDistance);
            this.pSplits.Controls.Add(this.label1);
            this.pSplits.Controls.Add(this.ckbShowSplits);
            this.pSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplits.Location = new System.Drawing.Point(3, 19);
            this.pSplits.Name = "pSplits";
            this.pSplits.Size = new System.Drawing.Size(542, 386);
            this.pSplits.TabIndex = 0;
            // 
            // lblGoalSpeed
            // 
            this.lblGoalSpeed.AutoSize = true;
            this.lblGoalSpeed.Location = new System.Drawing.Point(123, 327);
            this.lblGoalSpeed.Name = "lblGoalSpeed";
            this.lblGoalSpeed.Size = new System.Drawing.Size(51, 15);
            this.lblGoalSpeed.TabIndex = 96;
            this.lblGoalSpeed.Text = "43.6 kph";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(51, 327);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 15);
            this.label8.TabIndex = 95;
            this.label8.Text = "Goal Speed:";
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
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(179, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 15);
            this.label7.TabIndex = 91;
            this.label7.Text = "Total";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(50, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 15);
            this.label6.TabIndex = 90;
            this.label6.Text = "Split";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lvSplits
            // 
            this.lvSplits.BackColor = System.Drawing.SystemColors.Control;
            this.lvSplits.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvSplits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSplitDistance,
            this.chSplitTime,
            this.chTotalDistance,
            this.chTotalTime});
            this.lvSplits.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvSplits.HideSelection = false;
            this.lvSplits.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lvSplits.Location = new System.Drawing.Point(34, 171);
            this.lvSplits.MultiSelect = false;
            this.lvSplits.Name = "lvSplits";
            this.lvSplits.Scrollable = false;
            this.lvSplits.Size = new System.Drawing.Size(289, 132);
            this.lvSplits.TabIndex = 80;
            this.lvSplits.TabStop = false;
            this.lvSplits.UseCompatibleStateImageBehavior = false;
            this.lvSplits.View = System.Windows.Forms.View.Details;
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(303, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "sec";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(231, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "min";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "hrs";
            // 
            // tbGoalSecs
            // 
            this.tbGoalSecs.Location = new System.Drawing.Point(265, 110);
            this.tbGoalSecs.MaxLength = 2;
            this.tbGoalSecs.Name = "tbGoalSecs";
            this.tbGoalSecs.Size = new System.Drawing.Size(32, 23);
            this.tbGoalSecs.TabIndex = 70;
            this.tbGoalSecs.Text = "0";
            // 
            // tbGoalMins
            // 
            this.tbGoalMins.Location = new System.Drawing.Point(193, 110);
            this.tbGoalMins.MaxLength = 2;
            this.tbGoalMins.Name = "tbGoalMins";
            this.tbGoalMins.Size = new System.Drawing.Size(32, 23);
            this.tbGoalMins.TabIndex = 60;
            this.tbGoalMins.Text = "30";
            // 
            // tbGoalHrs
            // 
            this.tbGoalHrs.Location = new System.Drawing.Point(126, 110);
            this.tbGoalHrs.MaxLength = 2;
            this.tbGoalHrs.Name = "tbGoalHrs";
            this.tbGoalHrs.Size = new System.Drawing.Size(32, 23);
            this.tbGoalHrs.TabIndex = 50;
            this.tbGoalHrs.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Goal Time:";
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
            // 
            // cbSplitUom
            // 
            this.cbSplitUom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSplitUom.FormattingEnabled = true;
            this.cbSplitUom.Items.AddRange(new object[] {
            "km",
            "mi"});
            this.cbSplitUom.Location = new System.Drawing.Point(164, 46);
            this.cbSplitUom.Name = "cbSplitUom";
            this.cbSplitUom.Size = new System.Drawing.Size(47, 23);
            this.cbSplitUom.TabIndex = 30;
            // 
            // tbSplitDistance
            // 
            this.tbSplitDistance.Location = new System.Drawing.Point(126, 46);
            this.tbSplitDistance.MaxLength = 2;
            this.tbSplitDistance.Name = "tbSplitDistance";
            this.tbSplitDistance.Size = new System.Drawing.Size(32, 23);
            this.tbSplitDistance.TabIndex = 20;
            this.tbSplitDistance.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Splits Every:";
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescSystem;
        private System.Windows.Forms.GroupBox gbSplits;
        private System.Windows.Forms.Panel pSplits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckbShowSplits;
        private System.Windows.Forms.TextBox tbSplitDistance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbGoalSecs;
        private System.Windows.Forms.TextBox tbGoalMins;
        private System.Windows.Forms.TextBox tbGoalHrs;
        private System.Windows.Forms.Label label2;
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
    }
}
