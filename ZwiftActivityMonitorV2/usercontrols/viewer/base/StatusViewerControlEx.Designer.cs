
namespace ZwiftActivityMonitorV2
{
    partial class StatusViewerControlEx
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
            this.pStatus = new System.Windows.Forms.Panel();
            this.webBrowserControl = new ZwiftActivityMonitorV2.WebBrowserControl();
            this.pStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // pStatus
            // 
            this.pStatus.Controls.Add(this.webBrowserControl);
            this.pStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pStatus.Location = new System.Drawing.Point(0, 0);
            this.pStatus.Name = "pStatus";
            this.pStatus.Size = new System.Drawing.Size(320, 147);
            this.pStatus.TabIndex = 0;
            // 
            // webBrowser
            // 
            this.webBrowserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserControl.Location = new System.Drawing.Point(0, 0);
            this.webBrowserControl.Name = "webBrowser";
            this.webBrowserControl.Size = new System.Drawing.Size(320, 147);
            this.webBrowserControl.TabIndex = 0;
            // 
            // StatusViewerControlEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pStatus);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StatusViewerControlEx";
            this.Size = new System.Drawing.Size(320, 147);
            this.pStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pStatus;
        protected WebBrowserControl webBrowserControl;
    }
}
