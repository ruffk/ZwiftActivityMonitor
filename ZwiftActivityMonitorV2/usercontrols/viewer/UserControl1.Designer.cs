
namespace ZwiftActivityMonitorV2.usercontrols.viewer
{
    partial class UserControl1
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbLap = new System.Windows.Forms.ToolStripButton();
            this.tsbReset = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLap,
            this.tsbReset});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(136, 150);
            this.toolStrip.TabIndex = 21;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbLap
            // 
            this.tsbLap.AutoSize = false;
            this.tsbLap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLap.Image = global::ZwiftActivityMonitorV2.Properties.Resources.stopwatch;
            this.tsbLap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLap.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.tsbLap.Name = "tsbLap";
            this.tsbLap.Size = new System.Drawing.Size(48, 48);
            this.tsbLap.Text = "Click to start a new lap";
            // 
            // tsbReset
            // 
            this.tsbReset.AutoSize = false;
            this.tsbReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReset.Image = global::ZwiftActivityMonitorV2.Properties.Resources.split;
            this.tsbReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReset.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.tsbReset.Name = "tsbReset";
            this.tsbReset.Size = new System.Drawing.Size(48, 48);
            this.tsbReset.Text = "Click to clear laps and restart";
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Name = "UserControl1";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbLap;
        private System.Windows.Forms.ToolStripButton tsbReset;
    }
}
