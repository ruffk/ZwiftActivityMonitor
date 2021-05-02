
namespace ZwiftActivityMonitor
{
    partial class LapConfigControl
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
            this.tbDescSystem = new System.Windows.Forms.TextBox();
            this.gbLaps = new System.Windows.Forms.GroupBox();
            this.pLaps = new System.Windows.Forms.Panel();
            this.gbTriggers = new System.Windows.Forms.GroupBox();
            this.cbPosition = new System.Windows.Forms.ComboBox();
            this.rbPosition = new System.Windows.Forms.RadioButton();
            this.rbTime = new System.Windows.Forms.RadioButton();
            this.rbDistance = new System.Windows.Forms.RadioButton();
            this.tbDistance = new System.Windows.Forms.TextBox();
            this.cbDistanceUom = new System.Windows.Forms.ComboBox();
            this.tbHrs = new System.Windows.Forms.TextBox();
            this.lblGoalTimeSec = new System.Windows.Forms.Label();
            this.lblGoalTimeMin = new System.Windows.Forms.Label();
            this.tbSecs = new System.Windows.Forms.TextBox();
            this.lblGoalTimeHrs = new System.Windows.Forms.Label();
            this.tbMins = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.rbAutomatic = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnEditSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.gbLaps.SuspendLayout();
            this.pLaps.SuspendLayout();
            this.gbTriggers.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBase
            // 
            this.pBase.Controls.Add(this.gbLaps);
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
            this.tbDescSystem.Text = "Configure lap display options.  Laps can either be manual or automatic based upon" +
    " distance, time, or position.  If automatic, choose a desired trigger. ";
            this.tbDescSystem.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // gbLaps
            // 
            this.gbLaps.Controls.Add(this.pLaps);
            this.gbLaps.Location = new System.Drawing.Point(20, 73);
            this.gbLaps.Name = "gbLaps";
            this.gbLaps.Size = new System.Drawing.Size(548, 427);
            this.gbLaps.TabIndex = 5;
            this.gbLaps.TabStop = false;
            this.gbLaps.Text = "Laps";
            // 
            // pLaps
            // 
            this.pLaps.Controls.Add(this.gbTriggers);
            this.pLaps.Controls.Add(this.panel1);
            this.pLaps.Controls.Add(this.label1);
            this.pLaps.Controls.Add(this.btnCancelSettings);
            this.pLaps.Controls.Add(this.btnSaveSettings);
            this.pLaps.Controls.Add(this.btnEditSettings);
            this.pLaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pLaps.Location = new System.Drawing.Point(3, 19);
            this.pLaps.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pLaps.Name = "pLaps";
            this.pLaps.Size = new System.Drawing.Size(542, 405);
            this.pLaps.TabIndex = 0;
            // 
            // gbTriggers
            // 
            this.gbTriggers.Controls.Add(this.cbPosition);
            this.gbTriggers.Controls.Add(this.rbPosition);
            this.gbTriggers.Controls.Add(this.rbTime);
            this.gbTriggers.Controls.Add(this.rbDistance);
            this.gbTriggers.Controls.Add(this.tbDistance);
            this.gbTriggers.Controls.Add(this.cbDistanceUom);
            this.gbTriggers.Controls.Add(this.tbHrs);
            this.gbTriggers.Controls.Add(this.lblGoalTimeSec);
            this.gbTriggers.Controls.Add(this.lblGoalTimeMin);
            this.gbTriggers.Controls.Add(this.tbSecs);
            this.gbTriggers.Controls.Add(this.lblGoalTimeHrs);
            this.gbTriggers.Controls.Add(this.tbMins);
            this.gbTriggers.Location = new System.Drawing.Point(96, 106);
            this.gbTriggers.Name = "gbTriggers";
            this.gbTriggers.Size = new System.Drawing.Size(337, 131);
            this.gbTriggers.TabIndex = 30;
            this.gbTriggers.TabStop = false;
            this.gbTriggers.Text = "Lap Trigger";
            // 
            // cbPosition
            // 
            this.cbPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPosition.FormattingEnabled = true;
            this.cbPosition.Location = new System.Drawing.Point(129, 89);
            this.cbPosition.Name = "cbPosition";
            this.cbPosition.Size = new System.Drawing.Size(202, 23);
            this.cbPosition.TabIndex = 90;
            this.cbPosition.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.cbPosition.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.cbPosition.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // rbPosition
            // 
            this.rbPosition.AutoSize = true;
            this.rbPosition.Location = new System.Drawing.Point(11, 90);
            this.rbPosition.Name = "rbPosition";
            this.rbPosition.Size = new System.Drawing.Size(68, 19);
            this.rbPosition.TabIndex = 80;
            this.rbPosition.TabStop = true;
            this.rbPosition.Text = "Position";
            this.rbPosition.UseVisualStyleBackColor = true;
            // 
            // rbTime
            // 
            this.rbTime.AutoSize = true;
            this.rbTime.Location = new System.Drawing.Point(11, 58);
            this.rbTime.Name = "rbTime";
            this.rbTime.Size = new System.Drawing.Size(51, 19);
            this.rbTime.TabIndex = 50;
            this.rbTime.TabStop = true;
            this.rbTime.Text = "Time";
            this.rbTime.UseVisualStyleBackColor = true;
            // 
            // rbDistance
            // 
            this.rbDistance.AutoSize = true;
            this.rbDistance.Location = new System.Drawing.Point(11, 26);
            this.rbDistance.Name = "rbDistance";
            this.rbDistance.Size = new System.Drawing.Size(70, 19);
            this.rbDistance.TabIndex = 35;
            this.rbDistance.TabStop = true;
            this.rbDistance.Text = "Distance";
            this.rbDistance.UseVisualStyleBackColor = true;
            // 
            // tbDistance
            // 
            this.tbDistance.Location = new System.Drawing.Point(129, 25);
            this.tbDistance.MaxLength = 3;
            this.tbDistance.Name = "tbDistance";
            this.tbDistance.Size = new System.Drawing.Size(26, 23);
            this.tbDistance.TabIndex = 40;
            this.tbDistance.Text = "888";
            this.tbDistance.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbDistance.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbDistance.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // cbDistanceUom
            // 
            this.cbDistanceUom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDistanceUom.FormattingEnabled = true;
            this.cbDistanceUom.Location = new System.Drawing.Point(161, 25);
            this.cbDistanceUom.Name = "cbDistanceUom";
            this.cbDistanceUom.Size = new System.Drawing.Size(47, 23);
            this.cbDistanceUom.TabIndex = 45;
            this.cbDistanceUom.SelectionChangeCommitted += new System.EventHandler(this.cbDistanceUom_SelectionChangeCommitted);
            this.cbDistanceUom.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.cbDistanceUom.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.cbDistanceUom.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // tbHrs
            // 
            this.tbHrs.Location = new System.Drawing.Point(129, 57);
            this.tbHrs.MaxLength = 2;
            this.tbHrs.Name = "tbHrs";
            this.tbHrs.Size = new System.Drawing.Size(23, 23);
            this.tbHrs.TabIndex = 55;
            this.tbHrs.Text = "88";
            this.tbHrs.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbHrs.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbHrs.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblGoalTimeSec
            // 
            this.lblGoalTimeSec.AutoSize = true;
            this.lblGoalTimeSec.Location = new System.Drawing.Point(264, 60);
            this.lblGoalTimeSec.Name = "lblGoalTimeSec";
            this.lblGoalTimeSec.Size = new System.Drawing.Size(24, 15);
            this.lblGoalTimeSec.TabIndex = 12;
            this.lblGoalTimeSec.Text = "sec";
            // 
            // lblGoalTimeMin
            // 
            this.lblGoalTimeMin.AutoSize = true;
            this.lblGoalTimeMin.Location = new System.Drawing.Point(205, 60);
            this.lblGoalTimeMin.Name = "lblGoalTimeMin";
            this.lblGoalTimeMin.Size = new System.Drawing.Size(28, 15);
            this.lblGoalTimeMin.TabIndex = 11;
            this.lblGoalTimeMin.Text = "min";
            // 
            // tbSecs
            // 
            this.tbSecs.Location = new System.Drawing.Point(239, 57);
            this.tbSecs.MaxLength = 2;
            this.tbSecs.Name = "tbSecs";
            this.tbSecs.Size = new System.Drawing.Size(23, 23);
            this.tbSecs.TabIndex = 70;
            this.tbSecs.Text = "88";
            this.tbSecs.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbSecs.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbSecs.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblGoalTimeHrs
            // 
            this.lblGoalTimeHrs.AutoSize = true;
            this.lblGoalTimeHrs.Location = new System.Drawing.Point(153, 60);
            this.lblGoalTimeHrs.Name = "lblGoalTimeHrs";
            this.lblGoalTimeHrs.Size = new System.Drawing.Size(23, 15);
            this.lblGoalTimeHrs.TabIndex = 10;
            this.lblGoalTimeHrs.Text = "hrs";
            // 
            // tbMins
            // 
            this.tbMins.Location = new System.Drawing.Point(182, 57);
            this.tbMins.MaxLength = 2;
            this.tbMins.Name = "tbMins";
            this.tbMins.Size = new System.Drawing.Size(23, 23);
            this.tbMins.TabIndex = 60;
            this.tbMins.Text = "88";
            this.tbMins.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbMins.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbMins.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rbManual);
            this.panel1.Controls.Add(this.rbAutomatic);
            this.panel1.Location = new System.Drawing.Point(96, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(189, 69);
            this.panel1.TabIndex = 10;
            // 
            // rbManual
            // 
            this.rbManual.AutoSize = true;
            this.rbManual.Location = new System.Drawing.Point(10, 10);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(153, 19);
            this.rbManual.TabIndex = 15;
            this.rbManual.TabStop = true;
            this.rbManual.Text = "Manual - Via Lap Button";
            this.rbManual.UseVisualStyleBackColor = true;
            this.rbManual.CheckedChanged += new System.EventHandler(this.rbManual_CheckedChanged);
            // 
            // rbAutomatic
            // 
            this.rbAutomatic.AutoSize = true;
            this.rbAutomatic.Location = new System.Drawing.Point(10, 35);
            this.rbAutomatic.Name = "rbAutomatic";
            this.rbAutomatic.Size = new System.Drawing.Size(169, 19);
            this.rbAutomatic.TabIndex = 20;
            this.rbAutomatic.TabStop = true;
            this.rbAutomatic.Text = "Automatic - Via Lap Trigger";
            this.rbAutomatic.UseVisualStyleBackColor = true;
            this.rbAutomatic.CheckedChanged += new System.EventHandler(this.rbAutomatic_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 15);
            this.label1.TabIndex = 117;
            this.label1.Text = "Lap Style:";
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.Location = new System.Drawing.Point(439, 83);
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(89, 28);
            this.btnCancelSettings.TabIndex = 120;
            this.btnCancelSettings.Text = "Cancel";
            this.btnCancelSettings.UseVisualStyleBackColor = true;
            this.btnCancelSettings.Click += new System.EventHandler(this.btnCancelSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(439, 47);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(89, 28);
            this.btnSaveSettings.TabIndex = 110;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnEditSettings
            // 
            this.btnEditSettings.Location = new System.Drawing.Point(439, 11);
            this.btnEditSettings.Name = "btnEditSettings";
            this.btnEditSettings.Size = new System.Drawing.Size(89, 28);
            this.btnEditSettings.TabIndex = 100;
            this.btnEditSettings.Text = "Edit";
            this.btnEditSettings.UseVisualStyleBackColor = true;
            this.btnEditSettings.Click += new System.EventHandler(this.btnEditSettings_Click);
            // 
            // LapConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "LapConfigControl";
            this.Size = new System.Drawing.Size(587, 540);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.pBase.ResumeLayout(false);
            this.pBase.PerformLayout();
            this.gbLaps.ResumeLayout(false);
            this.pLaps.ResumeLayout(false);
            this.pLaps.PerformLayout();
            this.gbTriggers.ResumeLayout(false);
            this.gbTriggers.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescSystem;
        private System.Windows.Forms.GroupBox gbLaps;
        private System.Windows.Forms.Panel pLaps;
        private System.Windows.Forms.TextBox tbDistance;
        private System.Windows.Forms.Label lblGoalTimeSec;
        private System.Windows.Forms.Label lblGoalTimeMin;
        private System.Windows.Forms.Label lblGoalTimeHrs;
        private System.Windows.Forms.TextBox tbSecs;
        private System.Windows.Forms.TextBox tbMins;
        private System.Windows.Forms.TextBox tbHrs;
        private System.Windows.Forms.ComboBox cbDistanceUom;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnEditSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbAutomatic;
        private System.Windows.Forms.RadioButton rbManual;
        private System.Windows.Forms.GroupBox gbTriggers;
        private System.Windows.Forms.RadioButton rbPosition;
        private System.Windows.Forms.RadioButton rbTime;
        private System.Windows.Forms.RadioButton rbDistance;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbPosition;
    }
}
