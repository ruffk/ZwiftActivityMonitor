
namespace ZwiftActivityMonitor
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
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tpSystem = new System.Windows.Forms.TabPage();
            this.ucSystem = new ZwiftActivityMonitor.SystemControl();
            this.tpUserProfiles = new System.Windows.Forms.TabPage();
            this.ucUserProfiles = new ZwiftActivityMonitor.UserProfileControl();
            this.tpCollectors = new System.Windows.Forms.TabPage();
            this.ucStatistics = new ZwiftActivityMonitor.StatisticsControl();
            this.tpSplits = new System.Windows.Forms.TabPage();
            this.ucSplits = new ZwiftActivityMonitor.SplitsConfigControl();
            this.tpLaps = new System.Windows.Forms.TabPage();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ucLaps = new ZwiftActivityMonitor.LapConfigControl();
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
            this.tabOptions.Controls.Add(this.tpSystem);
            this.tabOptions.Controls.Add(this.tpUserProfiles);
            this.tabOptions.Controls.Add(this.tpCollectors);
            this.tabOptions.Controls.Add(this.tpSplits);
            this.tabOptions.Controls.Add(this.tpLaps);
            this.tabOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOptions.Location = new System.Drawing.Point(0, 0);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(598, 582);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabOptions_Selecting);
            this.tabOptions.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabOptions_Selected);
            this.tabOptions.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabOptions_Selecting);
            this.tabOptions.Deselected += new System.Windows.Forms.TabControlEventHandler(this.tabOptions_Selected);
            // 
            // tpSystem
            // 
            this.tpSystem.BackColor = System.Drawing.SystemColors.Control;
            this.tpSystem.Controls.Add(this.ucSystem);
            this.tpSystem.Location = new System.Drawing.Point(4, 24);
            this.tpSystem.Name = "tpSystem";
            this.tpSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tpSystem.Size = new System.Drawing.Size(590, 554);
            this.tpSystem.TabIndex = 5;
            this.tpSystem.Text = "System";
            // 
            // ucSystem
            // 
            this.ucSystem.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSystem.Location = new System.Drawing.Point(3, 3);
            this.ucSystem.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucSystem.Name = "ucSystem";
            this.ucSystem.Size = new System.Drawing.Size(584, 548);
            this.ucSystem.TabIndex = 0;
            // 
            // tpUserProfiles
            // 
            this.tpUserProfiles.BackColor = System.Drawing.SystemColors.Control;
            this.tpUserProfiles.Controls.Add(this.ucUserProfiles);
            this.tpUserProfiles.Location = new System.Drawing.Point(4, 24);
            this.tpUserProfiles.Name = "tpUserProfiles";
            this.tpUserProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpUserProfiles.Size = new System.Drawing.Size(590, 554);
            this.tpUserProfiles.TabIndex = 4;
            this.tpUserProfiles.Text = "User Profiles";
            // 
            // ucUserProfiles
            // 
            this.ucUserProfiles.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucUserProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucUserProfiles.Location = new System.Drawing.Point(3, 3);
            this.ucUserProfiles.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucUserProfiles.Name = "ucUserProfiles";
            this.ucUserProfiles.Size = new System.Drawing.Size(584, 548);
            this.ucUserProfiles.TabIndex = 0;
            // 
            // tpCollectors
            // 
            this.tpCollectors.BackColor = System.Drawing.SystemColors.Control;
            this.tpCollectors.Controls.Add(this.ucStatistics);
            this.tpCollectors.Location = new System.Drawing.Point(4, 24);
            this.tpCollectors.Name = "tpCollectors";
            this.tpCollectors.Padding = new System.Windows.Forms.Padding(3);
            this.tpCollectors.Size = new System.Drawing.Size(590, 554);
            this.tpCollectors.TabIndex = 3;
            this.tpCollectors.Text = "Collectors";
            // 
            // ucStatistics
            // 
            this.ucStatistics.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStatistics.Location = new System.Drawing.Point(3, 3);
            this.ucStatistics.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucStatistics.Name = "ucStatistics";
            this.ucStatistics.Size = new System.Drawing.Size(584, 548);
            this.ucStatistics.TabIndex = 0;
            // 
            // tpSplits
            // 
            this.tpSplits.BackColor = System.Drawing.SystemColors.Control;
            this.tpSplits.Controls.Add(this.ucSplits);
            this.tpSplits.Location = new System.Drawing.Point(4, 24);
            this.tpSplits.Name = "tpSplits";
            this.tpSplits.Padding = new System.Windows.Forms.Padding(3);
            this.tpSplits.Size = new System.Drawing.Size(590, 554);
            this.tpSplits.TabIndex = 6;
            this.tpSplits.Text = "Splits";
            // 
            // ucSplits
            // 
            this.ucSplits.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSplits.Location = new System.Drawing.Point(3, 3);
            this.ucSplits.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucSplits.Name = "ucSplits";
            this.ucSplits.Size = new System.Drawing.Size(584, 548);
            this.ucSplits.TabIndex = 0;
            // 
            // tpLaps
            // 
            this.tpLaps.BackColor = System.Drawing.SystemColors.Control;
            this.tpLaps.Controls.Add(this.ucLaps);
            this.tpLaps.Location = new System.Drawing.Point(4, 24);
            this.tpLaps.Name = "tpLaps";
            this.tpLaps.Padding = new System.Windows.Forms.Padding(3);
            this.tpLaps.Size = new System.Drawing.Size(590, 554);
            this.tpLaps.TabIndex = 7;
            this.tpLaps.Text = "Laps";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // statusStrip
            // 
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
            // ucLaps
            // 
            this.ucLaps.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucLaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucLaps.Location = new System.Drawing.Point(3, 3);
            this.ucLaps.Margin = new System.Windows.Forms.Padding(23, 20, 20, 3);
            this.ucLaps.Name = "ucLaps";
            this.ucLaps.Size = new System.Drawing.Size(584, 548);
            this.ucLaps.TabIndex = 0;
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

        private System.Windows.Forms.TabControl tabOptions;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private System.Windows.Forms.TabPage tpCollectors;
        private StatisticsControl ucStatistics;
        private System.Windows.Forms.TabPage tpUserProfiles;
        private UserProfileControl ucUserProfiles;
        private System.Windows.Forms.TabPage tpSystem;
        private SystemControl ucSystem;
        private System.Windows.Forms.TabPage tpSplits;
        private SplitsConfigControl ucSplits;
        private System.Windows.Forms.TabPage tpLaps;
        private LapConfigControl ucLaps;
    }
}