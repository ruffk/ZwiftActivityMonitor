
namespace ZwiftActivityMonitorV2
{
    partial class TimerSetupViewerControl
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
            this.dtpTimeRemaining = new System.Windows.Forms.DateTimePicker();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblTimeRemaining = new System.Windows.Forms.Label();
            this.rbStartImmediately = new System.Windows.Forms.RadioButton();
            this.rbStartWithEventTimer = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSettingsDisabled = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dtpTimeRemaining
            // 
            this.dtpTimeRemaining.CustomFormat = "HH:mm:ss";
            this.dtpTimeRemaining.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.dtpTimeRemaining.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeRemaining.Location = new System.Drawing.Point(37, 52);
            this.dtpTimeRemaining.Name = "dtpTimeRemaining";
            this.dtpTimeRemaining.ShowUpDown = true;
            this.dtpTimeRemaining.Size = new System.Drawing.Size(86, 27);
            this.dtpTimeRemaining.TabIndex = 51;
            this.dtpTimeRemaining.Value = new System.DateTime(2021, 5, 10, 23, 59, 59, 0);
            this.dtpTimeRemaining.Enter += new System.EventHandler(this.dtpTimeRemaining_Enter);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(175, 51);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(77, 25);
            this.btnStart.TabIndex = 53;
            this.btnStart.Text = "Start Timer";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.AutoSize = true;
            this.lblTimeRemaining.Location = new System.Drawing.Point(3, 31);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(96, 15);
            this.lblTimeRemaining.TabIndex = 55;
            this.lblTimeRemaining.Text = "Time Remaining:";
            // 
            // rbStartImmediately
            // 
            this.rbStartImmediately.AutoSize = true;
            this.rbStartImmediately.Location = new System.Drawing.Point(37, 125);
            this.rbStartImmediately.Name = "rbStartImmediately";
            this.rbStartImmediately.Size = new System.Drawing.Size(270, 19);
            this.rbStartImmediately.TabIndex = 58;
            this.rbStartImmediately.TabStop = true;
            this.rbStartImmediately.Text = "Monitoring starts when countdown completes";
            this.rbStartImmediately.UseVisualStyleBackColor = true;
            // 
            // rbStartWithEventTimer
            // 
            this.rbStartWithEventTimer.AutoSize = true;
            this.rbStartWithEventTimer.Checked = true;
            this.rbStartWithEventTimer.Location = new System.Drawing.Point(37, 100);
            this.rbStartWithEventTimer.Name = "rbStartWithEventTimer";
            this.rbStartWithEventTimer.Size = new System.Drawing.Size(242, 19);
            this.rbStartWithEventTimer.TabIndex = 57;
            this.rbStartWithEventTimer.TabStop = true;
            this.rbStartWithEventTimer.Text = "Monitoring starts when event timer starts";
            this.rbStartWithEventTimer.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 56;
            this.label2.Text = "Behavior:";
            // 
            // lblSettingsDisabled
            // 
            this.lblSettingsDisabled.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSettingsDisabled.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSettingsDisabled.Location = new System.Drawing.Point(0, 0);
            this.lblSettingsDisabled.Name = "lblSettingsDisabled";
            this.lblSettingsDisabled.Size = new System.Drawing.Size(320, 15);
            this.lblSettingsDisabled.TabIndex = 59;
            this.lblSettingsDisabled.Text = "Timer disabled while monitoring is in progress";
            this.lblSettingsDisabled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TimerSetupViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSettingsDisabled);
            this.Controls.Add(this.rbStartImmediately);
            this.Controls.Add(this.rbStartWithEventTimer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.dtpTimeRemaining);
            this.Name = "TimerSetupViewerControl";
            this.Size = new System.Drawing.Size(320, 147);
            this.Load += new System.EventHandler(this.TimerSetupViewerControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpTimeRemaining;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblTimeRemaining;
        private System.Windows.Forms.RadioButton rbStartImmediately;
        private System.Windows.Forms.RadioButton rbStartWithEventTimer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSettingsDisabled;
    }
}
