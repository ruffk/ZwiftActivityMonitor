
namespace ZwiftActivityMonitor
{
    partial class TimerSetupControl
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
            this.gbTimer = new System.Windows.Forms.GroupBox();
            this.pTimer = new System.Windows.Forms.Panel();
            this.rbStartImmediately = new System.Windows.Forms.RadioButton();
            this.rbStartWithEventTimer = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lblGoalTimeMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblGoalTimeSec = new System.Windows.Forms.Label();
            this.tbDescSystem = new System.Windows.Forms.TextBox();
            this.nMins = new System.Windows.Forms.NumericUpDown();
            this.nSecs = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.gbTimer.SuspendLayout();
            this.pTimer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nMins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSecs)).BeginInit();
            this.SuspendLayout();
            // 
            // pBase
            // 
            this.pBase.Controls.Add(this.gbTimer);
            this.pBase.Controls.Add(this.tbDescSystem);
            this.pBase.Size = new System.Drawing.Size(457, 253);
            // 
            // gbTimer
            // 
            this.gbTimer.Controls.Add(this.pTimer);
            this.gbTimer.Location = new System.Drawing.Point(20, 73);
            this.gbTimer.Name = "gbTimer";
            this.gbTimer.Size = new System.Drawing.Size(415, 146);
            this.gbTimer.TabIndex = 10;
            this.gbTimer.TabStop = false;
            this.gbTimer.Text = "Set Timer";
            // 
            // pTimer
            // 
            this.pTimer.Controls.Add(this.nSecs);
            this.pTimer.Controls.Add(this.nMins);
            this.pTimer.Controls.Add(this.rbStartImmediately);
            this.pTimer.Controls.Add(this.rbStartWithEventTimer);
            this.pTimer.Controls.Add(this.label1);
            this.pTimer.Controls.Add(this.lblGoalTimeMin);
            this.pTimer.Controls.Add(this.label2);
            this.pTimer.Controls.Add(this.lblGoalTimeSec);
            this.pTimer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pTimer.Location = new System.Drawing.Point(3, 19);
            this.pTimer.Name = "pTimer";
            this.pTimer.Size = new System.Drawing.Size(409, 124);
            this.pTimer.TabIndex = 0;
            // 
            // rbStartImmediately
            // 
            this.rbStartImmediately.AutoSize = true;
            this.rbStartImmediately.Location = new System.Drawing.Point(105, 86);
            this.rbStartImmediately.Name = "rbStartImmediately";
            this.rbStartImmediately.Size = new System.Drawing.Size(270, 19);
            this.rbStartImmediately.TabIndex = 50;
            this.rbStartImmediately.TabStop = true;
            this.rbStartImmediately.Text = "Monitoring starts when countdown completes";
            this.rbStartImmediately.UseVisualStyleBackColor = true;
            this.rbStartImmediately.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.rbStartImmediately.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.rbStartImmediately.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // rbStartWithEventTimer
            // 
            this.rbStartWithEventTimer.AutoSize = true;
            this.rbStartWithEventTimer.Checked = true;
            this.rbStartWithEventTimer.Location = new System.Drawing.Point(105, 61);
            this.rbStartWithEventTimer.Name = "rbStartWithEventTimer";
            this.rbStartWithEventTimer.Size = new System.Drawing.Size(242, 19);
            this.rbStartWithEventTimer.TabIndex = 40;
            this.rbStartWithEventTimer.TabStop = true;
            this.rbStartWithEventTimer.Text = "Monitoring starts when event timer starts";
            this.rbStartWithEventTimer.UseVisualStyleBackColor = true;
            this.rbStartWithEventTimer.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.rbStartWithEventTimer.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.rbStartWithEventTimer.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Time Remaining:";
            // 
            // lblGoalTimeMin
            // 
            this.lblGoalTimeMin.AutoSize = true;
            this.lblGoalTimeMin.Location = new System.Drawing.Point(146, 17);
            this.lblGoalTimeMin.Name = "lblGoalTimeMin";
            this.lblGoalTimeMin.Size = new System.Drawing.Size(28, 15);
            this.lblGoalTimeMin.TabIndex = 71;
            this.lblGoalTimeMin.Text = "min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Behavior:";
            // 
            // lblGoalTimeSec
            // 
            this.lblGoalTimeSec.AutoSize = true;
            this.lblGoalTimeSec.Location = new System.Drawing.Point(221, 17);
            this.lblGoalTimeSec.Name = "lblGoalTimeSec";
            this.lblGoalTimeSec.Size = new System.Drawing.Size(24, 15);
            this.lblGoalTimeSec.TabIndex = 72;
            this.lblGoalTimeSec.Text = "sec";
            // 
            // tbDescSystem
            // 
            this.tbDescSystem.BackColor = System.Drawing.SystemColors.Control;
            this.tbDescSystem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDescSystem.Location = new System.Drawing.Point(20, 20);
            this.tbDescSystem.Multiline = true;
            this.tbDescSystem.Name = "tbDescSystem";
            this.tbDescSystem.ReadOnly = true;
            this.tbDescSystem.Size = new System.Drawing.Size(424, 47);
            this.tbDescSystem.TabIndex = 10;
            this.tbDescSystem.TabStop = false;
            this.tbDescSystem.Text = "Set a timer to automatically \'Start\' monitoring when countdown expires.  When mon" +
    "itoring actually begins can also be configured.  ";
            // 
            // nMins
            // 
            this.nMins.Location = new System.Drawing.Point(105, 15);
            this.nMins.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nMins.Name = "nMins";
            this.nMins.Size = new System.Drawing.Size(35, 23);
            this.nMins.TabIndex = 20;
            this.nMins.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nMins.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.nMins.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.nMins.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // nSecs
            // 
            this.nSecs.Location = new System.Drawing.Point(180, 15);
            this.nSecs.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nSecs.Name = "nSecs";
            this.nSecs.Size = new System.Drawing.Size(35, 23);
            this.nSecs.TabIndex = 30;
            this.nSecs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nSecs.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nSecs.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.nSecs.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.nSecs.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // TimerSetupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TimerSetupControl";
            this.Size = new System.Drawing.Size(457, 275);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.pBase.ResumeLayout(false);
            this.pBase.PerformLayout();
            this.gbTimer.ResumeLayout(false);
            this.pTimer.ResumeLayout(false);
            this.pTimer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nMins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSecs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTimer;
        private System.Windows.Forms.Panel pTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblGoalTimeMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblGoalTimeSec;
        private System.Windows.Forms.TextBox tbDescSystem;
        private System.Windows.Forms.RadioButton rbStartImmediately;
        private System.Windows.Forms.RadioButton rbStartWithEventTimer;
        private System.Windows.Forms.NumericUpDown nSecs;
        private System.Windows.Forms.NumericUpDown nMins;
    }
}
