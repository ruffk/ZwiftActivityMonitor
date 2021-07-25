
namespace ZwiftActivityMonitorV2
{
    partial class RideRecap
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
            this.pControls = new System.Windows.Forms.Panel();
            this.webBrowser = new ZwiftActivityMonitorV2.WebBrowserControl();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbEmail = new System.Windows.Forms.ToolStripButton();
            this.tsbPrint = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.pControls.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pControls
            // 
            this.pControls.BackColor = System.Drawing.Color.Transparent;
            this.pControls.Controls.Add(this.btnClose);
            this.pControls.Controls.Add(this.toolStrip);
            this.pControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pControls.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pControls.Location = new System.Drawing.Point(0, 597);
            this.pControls.Margin = new System.Windows.Forms.Padding(0);
            this.pControls.Name = "pControls";
            this.pControls.Size = new System.Drawing.Size(745, 48);
            this.pControls.TabIndex = 0;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(745, 597);
            this.webBrowser.TabIndex = 1;
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbEmail,
            this.tsbPrint});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(99, 48);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // tsbEmail
            // 
            this.tsbEmail.AutoSize = false;
            this.tsbEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEmail.Image = global::ZwiftActivityMonitorV2.Properties.Resources.email_color;
            this.tsbEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEmail.Name = "tsbEmail";
            this.tsbEmail.Size = new System.Drawing.Size(48, 48);
            this.tsbEmail.Text = "Email Ride Recap";
            this.tsbEmail.Click += new System.EventHandler(this.tsbEmail_Click);
            // 
            // tsbPrint
            // 
            this.tsbPrint.AutoSize = false;
            this.tsbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrint.Image = global::ZwiftActivityMonitorV2.Properties.Resources.print_color;
            this.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Size = new System.Drawing.Size(48, 48);
            this.tsbPrint.Text = "Print Ride Recap";
            this.tsbPrint.Click += new System.EventHandler(this.tsbPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(662, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            // 
            // RideRecap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 645);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.pControls);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconSize = new System.Drawing.Size(32, 32);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RideRecap";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "Ride Recap";
            this.Load += new System.EventHandler(this.RideRecap_Load);
            this.pControls.ResumeLayout(false);
            this.pControls.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pControls;
        private WebBrowserControl webBrowser;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbEmail;
        private System.Windows.Forms.ToolStripButton tsbPrint;
        private System.Windows.Forms.Button btnClose;
    }
}