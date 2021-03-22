
namespace ZwiftActivityMonitor
{
    partial class UserControlBase
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
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pBase = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 162);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(221, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "Not seen";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // pBase
            // 
            this.pBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBase.Location = new System.Drawing.Point(0, 0);
            this.pBase.Name = "pBase";
            this.pBase.Size = new System.Drawing.Size(221, 162);
            this.pBase.TabIndex = 1;
            // 
            // UserControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.pBase);
            this.Controls.Add(this.statusStrip);
            this.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.Name = "UserControlBase";
            this.Size = new System.Drawing.Size(221, 184);
            this.Load += new System.EventHandler(this.UserControlBase_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        protected System.Windows.Forms.ErrorProvider errorProvider;
        protected System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        protected System.Windows.Forms.Panel pBase;
    }
}
