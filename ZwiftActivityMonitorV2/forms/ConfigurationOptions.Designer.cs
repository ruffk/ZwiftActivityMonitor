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
            this.tpCollectors = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucStatistics = new ZwiftActivityMonitorV2.StatisticsControl();
            this.tpSplits = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucSplits = new ZwiftActivityMonitorV2.SplitsConfigControlV2();
            this.tpLaps = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucLaps = new ZwiftActivityMonitorV2.LapConfigControl();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.tabOptions)).BeginInit();
            this.tabOptions.SuspendLayout();
            this.tpSystem.SuspendLayout();
            this.tpUserProfiles.SuspendLayout();
            this.tpCollectors.SuspendLayout();
            this.tpSplits.SuspendLayout();
            this.tpLaps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabOptions
            // 
            this.tabOptions.BeforeTouchSize = new System.Drawing.Size(598, 582);
            this.tabOptions.Controls.Add(this.tpSystem);
            this.tabOptions.Controls.Add(this.tpUserProfiles);
            this.tabOptions.Controls.Add(this.tpCollectors);
            this.tabOptions.Controls.Add(this.tpSplits);
            this.tabOptions.Controls.Add(this.tpLaps);
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
            this.ucUserProfiles.Location = new System.Drawing.Point(3, 3);
            this.ucUserProfiles.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucUserProfiles.Name = "ucUserProfiles";
            this.ucUserProfiles.Size = new System.Drawing.Size(589, 547);
            this.ucUserProfiles.TabIndex = 0;
            // 
            // tpCollectors
            // 
            this.tpCollectors.BackColor = System.Drawing.SystemColors.Control;
            this.tpCollectors.Controls.Add(this.ucStatistics);
            this.tpCollectors.Image = null;
            this.tpCollectors.ImageSize = new System.Drawing.Size(16, 16);
            this.tpCollectors.Location = new System.Drawing.Point(1, 27);
            this.tpCollectors.Name = "tpCollectors";
            this.tpCollectors.Padding = new System.Windows.Forms.Padding(3);
            this.tpCollectors.ShowCloseButton = true;
            this.tpCollectors.Size = new System.Drawing.Size(595, 553);
            this.tpCollectors.TabIndex = 3;
            this.tpCollectors.Text = "Collectors";
            this.tpCollectors.ThemesEnabled = false;
            // 
            // ucStatistics
            // 
            this.ucStatistics.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStatistics.Location = new System.Drawing.Point(3, 3);
            this.ucStatistics.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucStatistics.Name = "ucStatistics";
            this.ucStatistics.Size = new System.Drawing.Size(589, 547);
            this.ucStatistics.TabIndex = 0;
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
            this.ucLaps.Location = new System.Drawing.Point(3, 3);
            this.ucLaps.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucLaps.Name = "ucLaps";
            this.ucLaps.Size = new System.Drawing.Size(589, 547);
            this.ucLaps.TabIndex = 0;
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
            this.statusStrip.Text = "This doesn\'t get seen";
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationOptions_FormClosing);
            this.Load += new System.EventHandler(this.ConfigurationOptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabOptions)).EndInit();
            this.tabOptions.ResumeLayout(false);
            this.tpSystem.ResumeLayout(false);
            this.tpUserProfiles.ResumeLayout(false);
            this.tpCollectors.ResumeLayout(false);
            this.tpSplits.ResumeLayout(false);
            this.tpLaps.ResumeLayout(false);
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
        private TabPageAdv tpCollectors;
        private StatisticsControl ucStatistics;
        private TabPageAdv tpUserProfiles;
        private UserProfileControl ucUserProfiles;
        private TabPageAdv tpSystem;
        private SystemControl ucSystem;
        private TabPageAdv tpLaps;
        private LapConfigControl ucLaps;
        private TabPageAdv tpSplits;
        private SplitsConfigControlV2 ucSplits;
    }
}