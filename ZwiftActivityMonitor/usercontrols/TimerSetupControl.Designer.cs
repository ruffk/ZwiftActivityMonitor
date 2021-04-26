
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
            this.tbSecs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMins = new System.Windows.Forms.TextBox();
            this.lblGoalTimeMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblGoalTimeSec = new System.Windows.Forms.Label();
            this.tbDescSystem = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.gbTimer.SuspendLayout();
            this.pTimer.SuspendLayout();
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
            this.pTimer.Controls.Add(this.rbStartImmediately);
            this.pTimer.Controls.Add(this.rbStartWithEventTimer);
            this.pTimer.Controls.Add(this.tbSecs);
            this.pTimer.Controls.Add(this.label1);
            this.pTimer.Controls.Add(this.tbMins);
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
            // tbSecs
            // 
            this.tbSecs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSecs.Location = new System.Drawing.Point(165, 15);
            this.tbSecs.MaxLength = 2;
            this.tbSecs.Name = "tbSecs";
            this.tbSecs.Size = new System.Drawing.Size(23, 23);
            this.tbSecs.TabIndex = 30;
            this.tbSecs.Text = "30";
            this.tbSecs.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbSecs.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbSecs.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
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
            // tbMins
            // 
            this.tbMins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbMins.Location = new System.Drawing.Point(100, 15);
            this.tbMins.MaxLength = 2;
            this.tbMins.Name = "tbMins";
            this.tbMins.Size = new System.Drawing.Size(23, 23);
            this.tbMins.TabIndex = 20;
            this.tbMins.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbMins.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbMins.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblGoalTimeMin
            // 
            this.lblGoalTimeMin.AutoSize = true;
            this.lblGoalTimeMin.Location = new System.Drawing.Point(123, 17);
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
            this.lblGoalTimeSec.Location = new System.Drawing.Point(189, 17);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTimer;
        private System.Windows.Forms.Panel pTimer;
        private System.Windows.Forms.TextBox tbSecs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMins;
        private System.Windows.Forms.Label lblGoalTimeMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblGoalTimeSec;
        private System.Windows.Forms.TextBox tbDescSystem;
        private System.Windows.Forms.RadioButton rbStartImmediately;
        private System.Windows.Forms.RadioButton rbStartWithEventTimer;
    }
}
