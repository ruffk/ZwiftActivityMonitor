
namespace ZwiftActivityMonitor
{
    partial class MonitorTimer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ucTimerSetup = new ZwiftActivityMonitor.TimerSetupControl();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ucTimerSetup
            // 
            this.ucTimerSetup.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucTimerSetup.Location = new System.Drawing.Point(0, 0);
            this.ucTimerSetup.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucTimerSetup.Minutes = 0;
            this.ucTimerSetup.Name = "ucTimerSetup";
            this.ucTimerSetup.Seconds = 0;
            this.ucTimerSetup.Size = new System.Drawing.Size(456, 257);
            this.ucTimerSetup.StartImmediately = false;
            this.ucTimerSetup.StartWithEventTimer = false;
            this.ucTimerSetup.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(479, 79);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(87, 29);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "Start Timer";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(479, 114);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 29);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // MonitorTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 269);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.ucTimerSetup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MonitorTimer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Timer Setup";
            this.ResumeLayout(false);

        }

        #endregion

        private TimerSetupControl ucTimerSetup;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
    }
}