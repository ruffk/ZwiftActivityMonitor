using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

namespace ZwiftActivityMonitorV2
{
    partial class ConfigurationOptions
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
            this.components = new System.ComponentModel.Container();
            this.tabOptions = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            this.tpSystem = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucSystem = new ZwiftActivityMonitorV2.SystemControl();
            this.tpUserProfiles = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucUserProfiles = new ZwiftActivityMonitorV2.UserProfileControl();
            this.tpSplits = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucSplits = new ZwiftActivityMonitorV2.SplitsConfigControlV2();
            this.tpLaps = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucLaps = new ZwiftActivityMonitorV2.LapConfigControl();
            this.tpGeneral = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucGeneral = new ZwiftActivityMonitorV2.GeneralConfigControl();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.tabOptions)).BeginInit();
            this.tabOptions.SuspendLayout();
            this.tpSystem.SuspendLayout();
            this.tpUserProfiles.SuspendLayout();
            this.tpSplits.SuspendLayout();
            this.tpLaps.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabOptions
            // 
            this.tabOptions.BeforeTouchSize = new System.Drawing.Size(598, 582);
            this.tabOptions.Controls.Add(this.tpSystem);
            this.tabOptions.Controls.Add(this.tpUserProfiles);
            this.tabOptions.Controls.Add(this.tpSplits);
            this.tabOptions.Controls.Add(this.tpLaps);
            this.tabOptions.Controls.Add(this.tpGeneral);
            this.tabOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOptions.Location = new System.Drawing.Point(0, 0);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new System.Drawing.Size(598, 582);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.SelectedIndexChanged += new System.EventHandler(this.tabOptions_SelectedIndexChanged);
            this.tabOptions.SelectedIndexChanging += new Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventHandler(this.tabOptions_SelectedIndexChanging);
            // 
            // tpSystem
            // 
            this.tpSystem.BackColor = System.Drawing.SystemColors.Control;
            this.tpSystem.Controls.Add(this.ucSystem);
            this.tpSystem.Image = null;
            this.tpSystem.ImageSize = new System.Drawing.Size(16, 16);
            this.tpSystem.Location = new System.Drawing.Point(1, 27);
            this.tpSystem.Name = "tpSystem";
            this.tpSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tpSystem.ShowCloseButton = true;
            this.tpSystem.Size = new System.Drawing.Size(595, 553);
            this.tpSystem.TabIndex = 5;
            this.tpSystem.Text = "System";
            this.tpSystem.ThemesEnabled = false;
            // 
            // ucSystem
            // 
            this.ucSystem.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSystem.ForeColor = System.Drawing.Color.Black;
            this.ucSystem.Location = new System.Drawing.Point(3, 3);
            this.ucSystem.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucSystem.Name = "ucSystem";
            this.ucSystem.Size = new System.Drawing.Size(589, 547);
            this.ucSystem.TabIndex = 0;
            // 
            // tpUserProfiles
            // 
            this.tpUserProfiles.BackColor = System.Drawing.SystemColors.Control;
            this.tpUserProfiles.Controls.Add(this.ucUserProfiles);
            this.tpUserProfiles.Image = null;
            this.tpUserProfiles.ImageSize = new System.Drawing.Size(16, 16);
            this.tpUserProfiles.Location = new System.Drawing.Point(1, 27);
            this.tpUserProfiles.Name = "tpUserProfiles";
            this.tpUserProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpUserProfiles.ShowCloseButton = true;
            this.tpUserProfiles.Size = new System.Drawing.Size(595, 553);
            this.tpUserProfiles.TabIndex = 4;
            this.tpUserProfiles.Text = "User Profiles";
            this.tpUserProfiles.ThemesEnabled = false;
            // 
            // ucUserProfiles
            // 
            this.ucUserProfiles.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucUserProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucUserProfiles.ForeColor = System.Drawing.Color.Black;
            this.ucUserProfiles.Location = new System.Drawing.Point(3, 3);
            this.ucUserProfiles.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucUserProfiles.Name = "ucUserProfiles";
            this.ucUserProfiles.Size = new System.Drawing.Size(589, 547);
            this.ucUserProfiles.TabIndex = 0;
            // 
            // tpSplits
            // 
            this.tpSplits.BackColor = System.Drawing.SystemColors.Control;
            this.tpSplits.Controls.Add(this.ucSplits);
            this.tpSplits.Image = null;
            this.tpSplits.ImageSize = new System.Drawing.Size(16, 16);
            this.tpSplits.Location = new System.Drawing.Point(1, 27);
            this.tpSplits.Name = "tpSplits";
            this.tpSplits.Padding = new System.Windows.Forms.Padding(3);
            this.tpSplits.ShowCloseButton = true;
            this.tpSplits.Size = new System.Drawing.Size(595, 553);
            this.tpSplits.TabIndex = 8;
            this.tpSplits.Text = "Splits";
            this.tpSplits.ThemesEnabled = false;
            // 
            // ucSplits
            // 
            this.ucSplits.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSplits.ForeColor = System.Drawing.Color.Black;
            this.ucSplits.Location = new System.Drawing.Point(3, 3);
            this.ucSplits.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucSplits.Name = "ucSplits";
            this.ucSplits.Size = new System.Drawing.Size(589, 547);
            this.ucSplits.TabIndex = 0;
            // 
            // tpLaps
            // 
            this.tpLaps.BackColor = System.Drawing.SystemColors.Control;
            this.tpLaps.Controls.Add(this.ucLaps);
            this.tpLaps.Image = null;
            this.tpLaps.ImageSize = new System.Drawing.Size(16, 16);
            this.tpLaps.Location = new System.Drawing.Point(1, 27);
            this.tpLaps.Name = "tpLaps";
            this.tpLaps.Padding = new System.Windows.Forms.Padding(3);
            this.tpLaps.ShowCloseButton = true;
            this.tpLaps.Size = new System.Drawing.Size(595, 553);
            this.tpLaps.TabIndex = 7;
            this.tpLaps.Text = "Laps";
            this.tpLaps.ThemesEnabled = false;
            // 
            // ucLaps
            // 
            this.ucLaps.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucLaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucLaps.ForeColor = System.Drawing.Color.Black;
            this.ucLaps.Location = new System.Drawing.Point(3, 3);
            this.ucLaps.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucLaps.Name = "ucLaps";
            this.ucLaps.Size = new System.Drawing.Size(589, 547);
            this.ucLaps.TabIndex = 0;
            // 
            // tpGeneral
            // 
            this.tpGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tpGeneral.Controls.Add(this.ucGeneral);
            this.tpGeneral.Image = null;
            this.tpGeneral.ImageSize = new System.Drawing.Size(16, 16);
            this.tpGeneral.Location = new System.Drawing.Point(1, 27);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.ShowCloseButton = true;
            this.tpGeneral.Size = new System.Drawing.Size(595, 553);
            this.tpGeneral.TabIndex = 8;
            this.tpGeneral.Text = "General";
            this.tpGeneral.ThemesEnabled = false;
            // 
            // ucGeneral
            // 
            this.ucGeneral.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucGeneral.ForeColor = System.Drawing.Color.Black;
            this.ucGeneral.Location = new System.Drawing.Point(3, 3);
            this.ucGeneral.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucGeneral.Name = "ucGeneral";
            this.ucGeneral.Size = new System.Drawing.Size(589, 547);
            this.ucGeneral.TabIndex = 0;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 582);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(598, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "invisible!";
            // 
            // tsslStatus
            // 
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // ConfigurationOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(598, 604);
            this.Controls.Add(this.tabOptions);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconSize = new System.Drawing.Size(32, 32);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationOptions";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "Configuration Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationOptions_FormClosing);
            this.Load += new System.EventHandler(this.ConfigurationOptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabOptions)).EndInit();
            this.tabOptions.ResumeLayout(false);
            this.tpSystem.ResumeLayout(false);
            this.tpUserProfiles.ResumeLayout(false);
            this.tpSplits.ResumeLayout(false);
            this.tpLaps.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabControlAdv tabOptions;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private TabPageAdv tpUserProfiles;
        private UserProfileControl ucUserProfiles;
        private TabPageAdv tpSystem;
        private SystemControl ucSystem;
        private TabPageAdv tpLaps;
        private LapConfigControl ucLaps;
        private TabPageAdv tpSplits;
        private SplitsConfigControlV2 ucSplits;
        private TabPageAdv tpGeneral;
        private GeneralConfigControl ucGeneral;
    }
}