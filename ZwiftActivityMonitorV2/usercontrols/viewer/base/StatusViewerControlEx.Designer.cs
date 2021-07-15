
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
            this.components = new System.ComponentModel.Container();
            this.pStatus = new System.Windows.Forms.Panel();
            this.btnAutoDismiss = new System.Windows.Forms.Button();
            this.tlPanel = new System.Windows.Forms.TableLayoutPanel();
            this.webBrowserControl = new ZwiftActivityMonitorV2.WebBrowserControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pStatus.SuspendLayout();
            this.tlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pStatus
            // 
            this.pStatus.BackColor = System.Drawing.Color.Transparent;
            this.pStatus.Controls.Add(this.btnAutoDismiss);
            this.pStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pStatus.Location = new System.Drawing.Point(0, 122);
            this.pStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pStatus.Name = "pStatus";
            this.pStatus.Size = new System.Drawing.Size(320, 25);
            this.pStatus.TabIndex = 0;
            // 
            // btnAutoDismiss
            // 
            this.btnAutoDismiss.AutoSize = true;
            this.btnAutoDismiss.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAutoDismiss.Location = new System.Drawing.Point(221, 0);
            this.btnAutoDismiss.Name = "btnAutoDismiss";
            this.btnAutoDismiss.Size = new System.Drawing.Size(88, 25);
            this.btnAutoDismiss.TabIndex = 0;
            this.btnAutoDismiss.Text = "Auto-Dismiss";
            this.btnAutoDismiss.Click += new System.EventHandler(this.btnAutoDismiss_Click);
            // 
            // tlPanel
            // 
            this.tlPanel.ColumnCount = 1;
            this.tlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPanel.Controls.Add(this.webBrowserControl, 0, 0);
            this.tlPanel.Controls.Add(this.pStatus, 0, 1);
            this.tlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlPanel.Location = new System.Drawing.Point(0, 0);
            this.tlPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tlPanel.Name = "tlPanel";
            this.tlPanel.RowCount = 2;
            this.tlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlPanel.Size = new System.Drawing.Size(320, 147);
            this.tlPanel.TabIndex = 21;
            // 
            // webBrowserControl
            // 
            this.webBrowserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserControl.Location = new System.Drawing.Point(3, 3);
            this.webBrowserControl.Name = "webBrowserControl";
            this.webBrowserControl.Size = new System.Drawing.Size(314, 116);
            this.webBrowserControl.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // StatusViewerControlEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tlPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StatusViewerControlEx";
            this.Size = new System.Drawing.Size(320, 147);
            this.pStatus.ResumeLayout(false);
            this.pStatus.PerformLayout();
            this.tlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pStatus;
        private System.Windows.Forms.TableLayoutPanel tlPanel;
        protected WebBrowserControl webBrowserControl;
        private System.Windows.Forms.Button btnAutoDismiss;
        private System.Windows.Forms.Timer timer1;
    }
}
