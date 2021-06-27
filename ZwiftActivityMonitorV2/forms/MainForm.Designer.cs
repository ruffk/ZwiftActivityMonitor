using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;

namespace ZwiftActivityMonitorV2
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            this.tpActivity = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucActivityView = new ZwiftActivityMonitorV2.ActivityViewerControl();
            this.tpSplit = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucSplitView = new ZwiftActivityMonitorV2.SplitViewerControl();
            this.tpLap = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucLapView = new ZwiftActivityMonitorV2.LapViewerControl();
            this.tpColor = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucColorView = new ZwiftActivityMonitorV2.ColorAndFontViewerControl();
            this.tpTimer = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.ucTimerSetupView = new ZwiftActivityMonitorV2.TimerSetupViewerControl();
            this.pnBottom = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tssbMenu = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiStart = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsslSeparator1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.formSyncTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tpActivity.SuspendLayout();
            this.tpSplit.SuspendLayout();
            this.tpLap.SuspendLayout();
            this.tpColor.SuspendLayout();
            this.tpTimer.SuspendLayout();
            this.pnBottom.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.ActiveTabColor = System.Drawing.SystemColors.ControlDark;
            this.tabControl.ActiveTabFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Right;
            this.tabControl.BackColor = System.Drawing.SystemColors.Control;
            this.tabControl.BeforeTouchSize = new System.Drawing.Size(347, 147);
            this.tabControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tabControl.CanOverrideStyle = true;
            this.tabControl.Controls.Add(this.tpActivity);
            this.tabControl.Controls.Add(this.tpSplit);
            this.tabControl.Controls.Add(this.tpLap);
            this.tabControl.Controls.Add(this.tpColor);
            this.tabControl.Controls.Add(this.tpTimer);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.FocusOnTabClick = false;
            this.tabControl.InactiveTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(230)))), ((int)(((byte)(232)))));
            this.tabControl.ItemSize = new System.Drawing.Size(0, 28);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.Office2010ColorTheme = Syncfusion.Windows.Forms.Office2010Theme.Silver;
            this.tabControl.Padding = new System.Drawing.Point(0, 0);
            this.tabControl.ShowToolTips = true;
            this.tabControl.Size = new System.Drawing.Size(347, 147);
            this.tabControl.TabGap = 10;
            this.tabControl.TabIndex = 32;
            this.tabControl.TabPanelBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(230)))), ((int)(((byte)(232)))));
            this.tabControl.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererOffice2010);
            this.tabControl.ThemeName = "TabRendererOffice2010";
            this.tabControl.ThemeStyle.PrimitiveButtonStyle.DisabledNextPageImage = null;
            this.tabControl.ThemeStyle.TabPanelBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.tabControl.SelectedIndexChanging += new Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventHandler(this.tabControl_SelectedIndexChanging);
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl_MouseClick);
            // 
            // tpActivity
            // 
            this.tpActivity.Controls.Add(this.ucActivityView);
            this.tpActivity.Image = global::ZwiftActivityMonitorV2.Properties.Resources.analytics;
            this.tpActivity.ImageSize = new System.Drawing.Size(16, 16);
            this.tpActivity.Location = new System.Drawing.Point(1, 0);
            this.tpActivity.Margin = new System.Windows.Forms.Padding(0);
            this.tpActivity.Name = "tpActivity";
            this.tpActivity.ShowCloseButton = true;
            this.tpActivity.Size = new System.Drawing.Size(320, 147);
            this.tpActivity.TabIndex = 1;
            this.tpActivity.ThemesEnabled = false;
            this.tpActivity.ToolTipText = "Activity View";
            // 
            // ucActivityView
            // 
            this.ucActivityView.AutoSize = true;
            this.ucActivityView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucActivityView.HeaderForeColor = System.Drawing.Color.Empty;
            this.ucActivityView.HeaderGradientBeginColor = System.Drawing.SystemColors.Control;
            this.ucActivityView.HeaderGradientEndColor = System.Drawing.SystemColors.ControlDark;
            this.ucActivityView.Location = new System.Drawing.Point(0, 0);
            this.ucActivityView.Margin = new System.Windows.Forms.Padding(0);
            this.ucActivityView.Name = "ucActivityView";
            this.ucActivityView.RowBackColor = System.Drawing.Color.Empty;
            this.ucActivityView.RowFont = null;
            this.ucActivityView.RowForeColor = System.Drawing.Color.Empty;
            this.ucActivityView.Size = new System.Drawing.Size(320, 147);
            this.ucActivityView.TabIndex = 0;
            // 
            // tpSplit
            // 
            this.tpSplit.Controls.Add(this.ucSplitView);
            this.tpSplit.Image = global::ZwiftActivityMonitorV2.Properties.Resources.split;
            this.tpSplit.ImageSize = new System.Drawing.Size(16, 16);
            this.tpSplit.Location = new System.Drawing.Point(1, 0);
            this.tpSplit.Margin = new System.Windows.Forms.Padding(0);
            this.tpSplit.Name = "tpSplit";
            this.tpSplit.ShowCloseButton = true;
            this.tpSplit.Size = new System.Drawing.Size(320, 147);
            this.tpSplit.TabIndex = 2;
            this.tpSplit.ThemesEnabled = false;
            this.tpSplit.ToolTipText = "Splits";
            // 
            // ucSplitView
            // 
            this.ucSplitView.AutoSize = true;
            this.ucSplitView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSplitView.HeaderForeColor = System.Drawing.Color.Empty;
            this.ucSplitView.HeaderGradientBeginColor = System.Drawing.SystemColors.Control;
            this.ucSplitView.HeaderGradientEndColor = System.Drawing.SystemColors.ControlDark;
            this.ucSplitView.Location = new System.Drawing.Point(0, 0);
            this.ucSplitView.Margin = new System.Windows.Forms.Padding(0);
            this.ucSplitView.Name = "ucSplitView";
            this.ucSplitView.RowBackColor = System.Drawing.Color.Empty;
            this.ucSplitView.RowFont = null;
            this.ucSplitView.RowForeColor = System.Drawing.Color.Empty;
            this.ucSplitView.Size = new System.Drawing.Size(320, 147);
            this.ucSplitView.TabIndex = 0;
            // 
            // tpLap
            // 
            this.tpLap.Controls.Add(this.ucLapView);
            this.tpLap.Image = global::ZwiftActivityMonitorV2.Properties.Resources.stopwatch;
            this.tpLap.ImageSize = new System.Drawing.Size(16, 16);
            this.tpLap.Location = new System.Drawing.Point(1, 0);
            this.tpLap.Margin = new System.Windows.Forms.Padding(0);
            this.tpLap.Name = "tpLap";
            this.tpLap.ShowCloseButton = true;
            this.tpLap.Size = new System.Drawing.Size(320, 147);
            this.tpLap.TabIndex = 3;
            this.tpLap.ThemesEnabled = false;
            this.tpLap.ToolTipText = "Laps";
            // 
            // ucLapView
            // 
            this.ucLapView.AutoSize = true;
            this.ucLapView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucLapView.HeaderForeColor = System.Drawing.Color.Empty;
            this.ucLapView.HeaderGradientBeginColor = System.Drawing.SystemColors.Control;
            this.ucLapView.HeaderGradientEndColor = System.Drawing.SystemColors.ControlDark;
            this.ucLapView.Location = new System.Drawing.Point(0, 0);
            this.ucLapView.Margin = new System.Windows.Forms.Padding(0);
            this.ucLapView.Name = "ucLapView";
            this.ucLapView.RowBackColor = System.Drawing.Color.Empty;
            this.ucLapView.RowFont = null;
            this.ucLapView.RowForeColor = System.Drawing.Color.Empty;
            this.ucLapView.Size = new System.Drawing.Size(320, 147);
            this.ucLapView.TabIndex = 0;
            // 
            // tpColor
            // 
            this.tpColor.Controls.Add(this.ucColorView);
            this.tpColor.Image = global::ZwiftActivityMonitorV2.Properties.Resources.palette;
            this.tpColor.ImageSize = new System.Drawing.Size(16, 16);
            this.tpColor.Location = new System.Drawing.Point(1, 0);
            this.tpColor.Margin = new System.Windows.Forms.Padding(0);
            this.tpColor.Name = "tpColor";
            this.tpColor.ShowCloseButton = false;
            this.tpColor.Size = new System.Drawing.Size(320, 147);
            this.tpColor.TabIndex = 4;
            this.tpColor.ThemesEnabled = false;
            this.tpColor.ToolTipText = "Colors and Fonts";
            // 
            // ucColorView
            // 
            this.ucColorView.AutoSize = true;
            this.ucColorView.BackColor = System.Drawing.SystemColors.Control;
            this.ucColorView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucColorView.HeaderForeColor = System.Drawing.Color.Empty;
            this.ucColorView.HeaderGradientBeginColor = System.Drawing.SystemColors.Control;
            this.ucColorView.HeaderGradientEndColor = System.Drawing.SystemColors.ControlDark;
            this.ucColorView.Location = new System.Drawing.Point(0, 0);
            this.ucColorView.Margin = new System.Windows.Forms.Padding(0);
            this.ucColorView.Name = "ucColorView";
            this.ucColorView.RowBackColor = System.Drawing.Color.Empty;
            this.ucColorView.RowFont = null;
            this.ucColorView.RowForeColor = System.Drawing.Color.Empty;
            this.ucColorView.Size = new System.Drawing.Size(320, 147);
            this.ucColorView.TabIndex = 0;
            // 
            // tpTimer
            // 
            this.tpTimer.Controls.Add(this.ucTimerSetupView);
            this.tpTimer.Image = global::ZwiftActivityMonitorV2.Properties.Resources.clock;
            this.tpTimer.ImageSize = new System.Drawing.Size(16, 16);
            this.tpTimer.Location = new System.Drawing.Point(1, 0);
            this.tpTimer.Name = "tpTimer";
            this.tpTimer.ShowCloseButton = true;
            this.tpTimer.Size = new System.Drawing.Size(320, 147);
            this.tpTimer.TabIndex = 5;
            this.tpTimer.ThemesEnabled = false;
            this.tpTimer.ToolTipText = "Timer Setup";
            // 
            // ucTimerSetupView
            // 
            this.ucTimerSetupView.AutoSize = true;
            this.ucTimerSetupView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTimerSetupView.HeaderForeColor = System.Drawing.Color.Empty;
            this.ucTimerSetupView.HeaderGradientBeginColor = System.Drawing.SystemColors.Control;
            this.ucTimerSetupView.HeaderGradientEndColor = System.Drawing.SystemColors.ControlDark;
            this.ucTimerSetupView.IsTimerRunning = false;
            this.ucTimerSetupView.Location = new System.Drawing.Point(0, 0);
            this.ucTimerSetupView.Name = "ucTimerSetupView";
            this.ucTimerSetupView.RowBackColor = System.Drawing.Color.Empty;
            this.ucTimerSetupView.RowFont = null;
            this.ucTimerSetupView.RowForeColor = System.Drawing.Color.Empty;
            this.ucTimerSetupView.Size = new System.Drawing.Size(320, 147);
            this.ucTimerSetupView.TabIndex = 0;
            // 
            // pnBottom
            // 
            this.pnBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnBottom.Controls.Add(this.statusStrip);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(0, 147);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(347, 23);
            this.pnBottom.TabIndex = 33;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssbMenu,
            this.tsslSeparator1,
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 1);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(347, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            // 
            // tssbMenu
            // 
            this.tssbMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStart,
            this.tsmiStop,
            this.toolStripSeparator1,
            this.tsmiTimer,
            this.tsmiConfiguration,
            this.toolStripSeparator2,
            this.tsmiAdvanced,
            this.toolStripSeparator3,
            this.tsmiAbout});
            this.tssbMenu.Image = ((System.Drawing.Image)(resources.GetObject("tssbMenu.Image")));
            this.tssbMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbMenu.Name = "tssbMenu";
            this.tssbMenu.Size = new System.Drawing.Size(54, 20);
            this.tssbMenu.Text = "Menu";
            // 
            // tsmiStart
            // 
            this.tsmiStart.Name = "tsmiStart";
            this.tsmiStart.Size = new System.Drawing.Size(226, 22);
            this.tsmiStart.Text = "Start";
            this.tsmiStart.Click += new System.EventHandler(this.tsmiStart_Click);
            // 
            // tsmiStop
            // 
            this.tsmiStop.Name = "tsmiStop";
            this.tsmiStop.Size = new System.Drawing.Size(226, 22);
            this.tsmiStop.Text = "Stop";
            this.tsmiStop.Click += new System.EventHandler(this.tsmiStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
            // 
            // tsmiTimer
            // 
            this.tsmiTimer.Name = "tsmiTimer";
            this.tsmiTimer.Size = new System.Drawing.Size(226, 22);
            this.tsmiTimer.Text = "Timer";
            // 
            // tsmiConfiguration
            // 
            this.tsmiConfiguration.Name = "tsmiConfiguration";
            this.tsmiConfiguration.Size = new System.Drawing.Size(226, 22);
            this.tsmiConfiguration.Text = "Configuration...";
            this.tsmiConfiguration.Click += new System.EventHandler(this.tsmiConfiguration_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(223, 6);
            // 
            // tsmiAdvanced
            // 
            this.tsmiAdvanced.Name = "tsmiAdvanced";
            this.tsmiAdvanced.Size = new System.Drawing.Size(226, 22);
            this.tsmiAdvanced.Text = "Advanced...";
            this.tsmiAdvanced.Click += new System.EventHandler(this.tsmiAdvanced_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(223, 6);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(226, 22);
            this.tsmiAbout.Text = "About Zwift Activity Monitor";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // tsslSeparator1
            // 
            this.tsslSeparator1.Name = "tsslSeparator1";
            this.tsslSeparator1.Size = new System.Drawing.Size(18, 17);
            this.tsslSeparator1.Text = " >";
            // 
            // statusLabel
            // 
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(86, 17);
            this.statusLabel.Text = "Current status";
            // 
            // formSyncTimer
            // 
            this.formSyncTimer.Interval = 1000;
            this.formSyncTimer.Tick += new System.EventHandler(this.formSyncTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CaptionAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CaptionFont = new System.Drawing.Font("Franklin Gothic Heavy", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ClientSize = new System.Drawing.Size(347, 170);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Activity Monitor";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tpActivity.ResumeLayout(false);
            this.tpActivity.PerformLayout();
            this.tpSplit.ResumeLayout(false);
            this.tpSplit.PerformLayout();
            this.tpLap.ResumeLayout(false);
            this.tpLap.PerformLayout();
            this.tpColor.ResumeLayout(false);
            this.tpColor.PerformLayout();
            this.tpTimer.ResumeLayout(false);
            this.tpTimer.PerformLayout();
            this.pnBottom.ResumeLayout(false);
            this.pnBottom.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private TabControlAdv tabControl;
        private TabPageAdv tpActivity;
        private TabPageAdv tpSplit;
        private SplitViewerControl ucSplitView;
        private TabPageAdv tpLap;
        private LapViewerControl ucLapView;
        private TabPageAdv tpColor;
        private ColorAndFontViewerControl ucColorView;
        private Panel pnBottom;
        private ToolStripMenuItem tsmiStart;
        private ToolStripMenuItem tsmiStop;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmiTimer;
        private ToolStripMenuItem tsmiConfiguration;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem tsmiAdvanced;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem tsmiAbout;
        private StatusStrip statusStrip;
        private ToolStripSplitButton tssbMenu;
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel tsslSeparator1;
        private Timer formSyncTimer;
        private TimerSetupViewerControl ucTimerSetupView;
        private TabPageAdv tpTimer;
        private ActivityViewerControl ucActivityView;
    }
}