
namespace ZwiftActivityMonitor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonLog = new System.Windows.Forms.Button();
            this.cbxDebugMode = new System.Windows.Forms.CheckBox();
            this.btnRunMonitor = new System.Windows.Forms.Button();
            this.btnStopMonitor = new System.Windows.Forms.Button();
            this.btnMonitorStatistics = new System.Windows.Forms.Button();
            this.tbTargetHR = new System.Windows.Forms.TextBox();
            this.tbRiderTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(164, 259);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonLog
            // 
            this.buttonLog.Location = new System.Drawing.Point(164, 72);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(75, 44);
            this.buttonLog.TabIndex = 1;
            this.buttonLog.Text = "Log Something";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
            // 
            // cbxDebugMode
            // 
            this.cbxDebugMode.AutoSize = true;
            this.cbxDebugMode.Checked = true;
            this.cbxDebugMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxDebugMode.Location = new System.Drawing.Point(12, 34);
            this.cbxDebugMode.Name = "cbxDebugMode";
            this.cbxDebugMode.Size = new System.Drawing.Size(95, 19);
            this.cbxDebugMode.TabIndex = 2;
            this.cbxDebugMode.Text = "Debug Mode";
            this.cbxDebugMode.UseVisualStyleBackColor = true;
            // 
            // btnRunMonitor
            // 
            this.btnRunMonitor.Location = new System.Drawing.Point(12, 72);
            this.btnRunMonitor.Name = "btnRunMonitor";
            this.btnRunMonitor.Size = new System.Drawing.Size(95, 48);
            this.btnRunMonitor.TabIndex = 3;
            this.btnRunMonitor.Text = "Run Monitor";
            this.btnRunMonitor.UseVisualStyleBackColor = true;
            this.btnRunMonitor.Click += new System.EventHandler(this.btnRunMonitor_Click);
            // 
            // btnStopMonitor
            // 
            this.btnStopMonitor.Enabled = false;
            this.btnStopMonitor.Location = new System.Drawing.Point(12, 126);
            this.btnStopMonitor.Name = "btnStopMonitor";
            this.btnStopMonitor.Size = new System.Drawing.Size(95, 48);
            this.btnStopMonitor.TabIndex = 4;
            this.btnStopMonitor.Text = "Stop Monitor";
            this.btnStopMonitor.UseVisualStyleBackColor = true;
            this.btnStopMonitor.Click += new System.EventHandler(this.btnStopMonitor_Click);
            // 
            // btnMonitorStatistics
            // 
            this.btnMonitorStatistics.Location = new System.Drawing.Point(165, 125);
            this.btnMonitorStatistics.Name = "btnMonitorStatistics";
            this.btnMonitorStatistics.Size = new System.Drawing.Size(75, 44);
            this.btnMonitorStatistics.TabIndex = 5;
            this.btnMonitorStatistics.Text = "Monitor Statistics";
            this.btnMonitorStatistics.UseVisualStyleBackColor = true;
            this.btnMonitorStatistics.Click += new System.EventHandler(this.btnMonitorStatistics_Click);
            // 
            // tbTargetHR
            // 
            this.tbTargetHR.Location = new System.Drawing.Point(324, 30);
            this.tbTargetHR.Name = "tbTargetHR";
            this.tbTargetHR.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbTargetHR.Size = new System.Drawing.Size(68, 23);
            this.tbTargetHR.TabIndex = 6;
            this.tbTargetHR.Text = "0";
            // 
            // tbRiderTime
            // 
            this.tbRiderTime.Location = new System.Drawing.Point(164, 30);
            this.tbRiderTime.Name = "tbRiderTime";
            this.tbRiderTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbRiderTime.Size = new System.Drawing.Size(68, 23);
            this.tbRiderTime.TabIndex = 7;
            this.tbRiderTime.Text = "00:00";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 294);
            this.Controls.Add(this.tbRiderTime);
            this.Controls.Add(this.tbTargetHR);
            this.Controls.Add(this.btnMonitorStatistics);
            this.Controls.Add(this.btnStopMonitor);
            this.Controls.Add(this.btnRunMonitor);
            this.Controls.Add(this.cbxDebugMode);
            this.Controls.Add(this.buttonLog);
            this.Controls.Add(this.buttonExit);
            this.Name = "MainForm";
            this.Text = "Zwift Activity Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.CheckBox cbxDebugMode;
        private System.Windows.Forms.Button btnRunMonitor;
        private System.Windows.Forms.Button btnStopMonitor;
        private System.Windows.Forms.Button btnMonitorStatistics;
        private System.Windows.Forms.TextBox tbTargetHR;
        private System.Windows.Forms.TextBox tbRiderTime;
    }
}

