
namespace ZwiftActivityMonitor
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "20 Min",
            "8.88",
            "8.88",
            "8.88",
            "888"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "20 Min",
            "8.88",
            "8.88",
            "8.88",
            "888"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "20 Min",
            "8.88",
            "8.88",
            "8.88",
            "888"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Overall",
            "888",
            "888",
            "8.88",
            "88.8"}, -1);
            this.pnTitle = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnMenu = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslSeparator1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiAnalyze = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStart = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStop = new System.Windows.Forms.ToolStripMenuItem();
            this.tssCollect1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSetupTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStopTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.tssCollect2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi5sec = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi30sec = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi1min = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi5min = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi10min = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi20min = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi30min = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi60min = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi90min = new System.Windows.Forms.ToolStripMenuItem();
            this.tssCollect3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.tssHelp1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.pnViewer = new System.Windows.Forms.Panel();
            this.lvViewer = new System.Windows.Forms.ListView();
            this.chDesc = new System.Windows.Forms.ColumnHeader();
            this.chAvg = new System.Windows.Forms.ColumnHeader();
            this.chAvgMax = new System.Windows.Forms.ColumnHeader();
            this.chFTP = new System.Windows.Forms.ColumnHeader();
            this.chHR = new System.Windows.Forms.ColumnHeader();
            this.pnOverall = new System.Windows.Forms.Panel();
            this.lvOverall = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.countdownTimer = new System.Windows.Forms.Timer(this.components);
            this.runTimer = new System.Windows.Forms.Timer(this.components);
            this.pnTitle.SuspendLayout();
            this.pnMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.pnViewer.SuspendLayout();
            this.pnOverall.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnTitle
            // 
            this.pnTitle.BackColor = System.Drawing.Color.White;
            this.pnTitle.Controls.Add(this.btnClose);
            this.pnTitle.Controls.Add(this.lblTitle);
            this.pnTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTitle.Location = new System.Drawing.Point(0, 0);
            this.pnTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pnTitle.Name = "pnTitle";
            this.pnTitle.Size = new System.Drawing.Size(334, 26);
            this.pnTitle.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnClose.Location = new System.Drawing.Point(305, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(29, 26);
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(334, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Activity Monitor";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseMove);
            this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseUp);
            // 
            // pnMenu
            // 
            this.pnMenu.BackColor = System.Drawing.SystemColors.Control;
            this.pnMenu.Controls.Add(this.statusStrip1);
            this.pnMenu.Controls.Add(this.menuStrip1);
            this.pnMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnMenu.Location = new System.Drawing.Point(0, 26);
            this.pnMenu.Name = "pnMenu";
            this.pnMenu.Size = new System.Drawing.Size(334, 24);
            this.pnMenu.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslSeparator1,
            this.tsslStatus});
            this.statusStrip1.Location = new System.Drawing.Point(116, -2);
            this.statusStrip1.MinimumSize = new System.Drawing.Size(0, 26);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(202, 26);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 23;
            // 
            // tsslSeparator1
            // 
            this.tsslSeparator1.Name = "tsslSeparator1";
            this.tsslSeparator1.Size = new System.Drawing.Size(10, 21);
            this.tsslSeparator1.Text = "|";
            // 
            // tsslStatus
            // 
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Size = new System.Drawing.Size(39, 21);
            this.tsslStatus.Text = "Ready";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAnalyze,
            this.tsmiHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(334, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "File";
            // 
            // tsmiAnalyze
            // 
            this.tsmiAnalyze.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiStart,
            this.tsmiStop,
            this.tssCollect1,
            this.tsmiTimer,
            this.tssCollect2,
            this.tsmi5sec,
            this.tsmi30sec,
            this.tsmi1min,
            this.tsmi5min,
            this.tsmi10min,
            this.tsmi20min,
            this.tsmi30min,
            this.tsmi60min,
            this.tsmi90min,
            this.tssCollect3,
            this.tsmiOptions,
            this.tsmiAdvanced});
            this.tsmiAnalyze.Name = "tsmiAnalyze";
            this.tsmiAnalyze.Size = new System.Drawing.Size(60, 20);
            this.tsmiAnalyze.Text = "Analyze";
            // 
            // tsmiStart
            // 
            this.tsmiStart.Name = "tsmiStart";
            this.tsmiStart.Size = new System.Drawing.Size(136, 22);
            this.tsmiStart.Text = "Start";
            this.tsmiStart.Click += new System.EventHandler(this.tsmiStart_Click);
            // 
            // tsmiStop
            // 
            this.tsmiStop.Name = "tsmiStop";
            this.tsmiStop.Size = new System.Drawing.Size(136, 22);
            this.tsmiStop.Text = "Stop";
            this.tsmiStop.Click += new System.EventHandler(this.tsmiStop_Click);
            // 
            // tssCollect1
            // 
            this.tssCollect1.Name = "tssCollect1";
            this.tssCollect1.Size = new System.Drawing.Size(133, 6);
            // 
            // tsmiTimer
            // 
            this.tsmiTimer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSetupTimer,
            this.tsmiStopTimer});
            this.tsmiTimer.Name = "tsmiTimer";
            this.tsmiTimer.Size = new System.Drawing.Size(136, 22);
            this.tsmiTimer.Text = "Timer";
            // 
            // tsmiSetupTimer
            // 
            this.tsmiSetupTimer.Name = "tsmiSetupTimer";
            this.tsmiSetupTimer.Size = new System.Drawing.Size(146, 22);
            this.tsmiSetupTimer.Text = "Setup Timer...";
            this.tsmiSetupTimer.Click += new System.EventHandler(this.tsmiSetupTimer_Click);
            // 
            // tsmiStopTimer
            // 
            this.tsmiStopTimer.Name = "tsmiStopTimer";
            this.tsmiStopTimer.Size = new System.Drawing.Size(146, 22);
            this.tsmiStopTimer.Text = "Stop Timer";
            this.tsmiStopTimer.Click += new System.EventHandler(this.tsmiStopTimer_Click);
            // 
            // tssCollect2
            // 
            this.tssCollect2.Name = "tssCollect2";
            this.tssCollect2.Size = new System.Drawing.Size(133, 6);
            // 
            // tsmi5sec
            // 
            this.tsmi5sec.CheckOnClick = true;
            this.tsmi5sec.Name = "tsmi5sec";
            this.tsmi5sec.Size = new System.Drawing.Size(136, 22);
            this.tsmi5sec.Tag = "FiveSeconds";
            this.tsmi5sec.Text = "5 sec";
            this.tsmi5sec.Click += new System.EventHandler(this.anyDuration_Click);
            // 
            // tsmi30sec
            // 
            this.tsmi30sec.CheckOnClick = true;
            this.tsmi30sec.Name = "tsmi30sec";
            this.tsmi30sec.Size = new System.Drawing.Size(136, 22);
            this.tsmi30sec.Tag = "ThirtySeconds";
            this.tsmi30sec.Text = "30 sec";
            this.tsmi30sec.Click += new System.EventHandler(this.anyDuration_Click);
            // 
            // tsmi1min
            // 
            this.tsmi1min.CheckOnClick = true;
            this.tsmi1min.Name = "tsmi1min";
            this.tsmi1min.Size = new System.Drawing.Size(136, 22);
            this.tsmi1min.Tag = "OneMinute";
            this.tsmi1min.Text = "1 min";
            this.tsmi1min.Click += new System.EventHandler(this.anyDuration_Click);
            // 
            // tsmi5min
            // 
            this.tsmi5min.CheckOnClick = true;
            this.tsmi5min.Name = "tsmi5min";
            this.tsmi5min.Size = new System.Drawing.Size(136, 22);
            this.tsmi5min.Tag = "FiveMinutes";
            this.tsmi5min.Text = "5 min";
            this.tsmi5min.Click += new System.EventHandler(this.anyDuration_Click);
            // 
            // tsmi10min
            // 
            this.tsmi10min.CheckOnClick = true;
            this.tsmi10min.Name = "tsmi10min";
            this.tsmi10min.Size = new System.Drawing.Size(136, 22);
            this.tsmi10min.Tag = "TenMinutes";
            this.tsmi10min.Text = "10 min";
            this.tsmi10min.Click += new System.EventHandler(this.anyDuration_Click);
            // 
            // tsmi20min
            // 
            this.tsmi20min.CheckOnClick = true;
            this.tsmi20min.Name = "tsmi20min";
            this.tsmi20min.Size = new System.Drawing.Size(136, 22);
            this.tsmi20min.Tag = "TwentyMinutes";
            this.tsmi20min.Text = "20 min";
            this.tsmi20min.Click += new System.EventHandler(this.anyDuration_Click);
            // 
            // tsmi30min
            // 
            this.tsmi30min.CheckOnClick = true;
            this.tsmi30min.Name = "tsmi30min";
            this.tsmi30min.Size = new System.Drawing.Size(136, 22);
            this.tsmi30min.Tag = "ThirtyMinutes";
            this.tsmi30min.Text = "30 min";
            this.tsmi30min.Click += new System.EventHandler(this.anyDuration_Click);
            // 
            // tsmi60min
            // 
            this.tsmi60min.CheckOnClick = true;
            this.tsmi60min.Name = "tsmi60min";
            this.tsmi60min.Size = new System.Drawing.Size(136, 22);
            this.tsmi60min.Tag = "SixtyMinutes";
            this.tsmi60min.Text = "60 min";
            this.tsmi60min.Click += new System.EventHandler(this.anyDuration_Click);
            // 
            // tsmi90min
            // 
            this.tsmi90min.CheckOnClick = true;
            this.tsmi90min.Name = "tsmi90min";
            this.tsmi90min.Size = new System.Drawing.Size(136, 22);
            this.tsmi90min.Tag = "NinetyMinutes";
            this.tsmi90min.Text = "90 min";
            this.tsmi90min.Click += new System.EventHandler(this.anyDuration_Click);
            // 
            // tssCollect3
            // 
            this.tssCollect3.Name = "tssCollect3";
            this.tssCollect3.Size = new System.Drawing.Size(133, 6);
            // 
            // tsmiOptions
            // 
            this.tsmiOptions.Name = "tsmiOptions";
            this.tsmiOptions.Size = new System.Drawing.Size(136, 22);
            this.tsmiOptions.Text = "Options...";
            this.tsmiOptions.Click += new System.EventHandler(this.tsmiOptions_Click);
            // 
            // tsmiAdvanced
            // 
            this.tsmiAdvanced.Name = "tsmiAdvanced";
            this.tsmiAdvanced.Size = new System.Drawing.Size(136, 22);
            this.tsmiAdvanced.Text = "Advanced...";
            this.tsmiAdvanced.Click += new System.EventHandler(this.tsmiAdvanced_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCheckForUpdates,
            this.tssHelp1,
            this.tsmiAbout});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(44, 20);
            this.tsmiHelp.Text = "Help";
            // 
            // tsmiCheckForUpdates
            // 
            this.tsmiCheckForUpdates.Name = "tsmiCheckForUpdates";
            this.tsmiCheckForUpdates.Size = new System.Drawing.Size(226, 22);
            this.tsmiCheckForUpdates.Text = "Check For Updates";
            this.tsmiCheckForUpdates.Click += new System.EventHandler(this.tsmiCheckForUpdates_Click);
            // 
            // tssHelp1
            // 
            this.tssHelp1.Name = "tssHelp1";
            this.tssHelp1.Size = new System.Drawing.Size(223, 6);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(226, 22);
            this.tsmiAbout.Text = "About Zwift Activity Monitor";
            // 
            // pnViewer
            // 
            this.pnViewer.Controls.Add(this.lvViewer);
            this.pnViewer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnViewer.Location = new System.Drawing.Point(0, 50);
            this.pnViewer.Name = "pnViewer";
            this.pnViewer.Size = new System.Drawing.Size(334, 98);
            this.pnViewer.TabIndex = 2;
            // 
            // lvViewer
            // 
            this.lvViewer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
            this.lvViewer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvViewer.CausesValidation = false;
            this.lvViewer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDesc,
            this.chAvg,
            this.chAvgMax,
            this.chFTP,
            this.chHR});
            this.lvViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvViewer.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvViewer.ForeColor = System.Drawing.Color.White;
            this.lvViewer.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvViewer.HideSelection = false;
            this.lvViewer.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lvViewer.Location = new System.Drawing.Point(0, 0);
            this.lvViewer.MultiSelect = false;
            this.lvViewer.Name = "lvViewer";
            this.lvViewer.OwnerDraw = true;
            this.lvViewer.Scrollable = false;
            this.lvViewer.Size = new System.Drawing.Size(334, 98);
            this.lvViewer.TabIndex = 0;
            this.lvViewer.TabStop = false;
            this.lvViewer.UseCompatibleStateImageBehavior = false;
            this.lvViewer.View = System.Windows.Forms.View.Details;
            this.lvViewer.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvViewer.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListView_ItemSelectionChanged_Disable);
            this.lvViewer.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // chDesc
            // 
            this.chDesc.Text = "";
            this.chDesc.Width = 75;
            // 
            // chAvg
            // 
            this.chAvg.Text = "Avg";
            this.chAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chAvg.Width = 50;
            // 
            // chAvgMax
            // 
            this.chAvgMax.Text = "Avg (Max)";
            this.chAvgMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chAvgMax.Width = 105;
            // 
            // chFTP
            // 
            this.chFTP.Text = "FTP";
            this.chFTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chFTP.Width = 50;
            // 
            // chHR
            // 
            this.chHR.Text = "HR";
            this.chHR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chHR.Width = 54;
            // 
            // pnOverall
            // 
            this.pnOverall.Controls.Add(this.lvOverall);
            this.pnOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnOverall.Location = new System.Drawing.Point(0, 148);
            this.pnOverall.Margin = new System.Windows.Forms.Padding(0);
            this.pnOverall.Name = "pnOverall";
            this.pnOverall.Size = new System.Drawing.Size(334, 55);
            this.pnOverall.TabIndex = 3;
            // 
            // lvOverall
            // 
            this.lvOverall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
            this.lvOverall.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvOverall.CausesValidation = false;
            this.lvOverall.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvOverall.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvOverall.ForeColor = System.Drawing.Color.White;
            this.lvOverall.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvOverall.HideSelection = false;
            this.lvOverall.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4});
            this.lvOverall.Location = new System.Drawing.Point(0, 0);
            this.lvOverall.MultiSelect = false;
            this.lvOverall.Name = "lvOverall";
            this.lvOverall.OwnerDraw = true;
            this.lvOverall.Scrollable = false;
            this.lvOverall.Size = new System.Drawing.Size(334, 55);
            this.lvOverall.TabIndex = 0;
            this.lvOverall.TabStop = false;
            this.lvOverall.UseCompatibleStateImageBehavior = false;
            this.lvOverall.View = System.Windows.Forms.View.Details;
            this.lvOverall.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvOverall.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListView_ItemSelectionChanged_Disable);
            this.lvOverall.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 75;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Avg";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 50;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Normalized";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 105;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "IF";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 50;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Kph";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 54;
            // 
            // countdownTimer
            // 
            this.countdownTimer.Interval = 1000;
            this.countdownTimer.Tick += new System.EventHandler(this.countdownTimer_Tick);
            // 
            // runTimer
            // 
            this.runTimer.Interval = 1000;
            this.runTimer.Tick += new System.EventHandler(this.runTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
            this.ClientSize = new System.Drawing.Size(334, 203);
            this.Controls.Add(this.pnOverall);
            this.Controls.Add(this.pnViewer);
            this.Controls.Add(this.pnMenu);
            this.Controls.Add(this.pnTitle);
            this.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.pnTitle.ResumeLayout(false);
            this.pnMenu.ResumeLayout(false);
            this.pnMenu.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnViewer.ResumeLayout(false);
            this.pnOverall.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnMenu;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiAnalyze;
        private System.Windows.Forms.ToolStripMenuItem tsmiStart;
        private System.Windows.Forms.ToolStripMenuItem tsmiStop;
        private System.Windows.Forms.ToolStripSeparator tssCollect1;
        private System.Windows.Forms.ToolStripMenuItem tsmi5sec;
        private System.Windows.Forms.ToolStripMenuItem tsmi1min;
        private System.Windows.Forms.ToolStripMenuItem tsmi5min;
        private System.Windows.Forms.ToolStripMenuItem tsmi10min;
        private System.Windows.Forms.ToolStripMenuItem tsmi20min;
        private System.Windows.Forms.ToolStripMenuItem tsmi60min;
        private System.Windows.Forms.ToolStripMenuItem tsmi90min;
        private System.Windows.Forms.ToolStripMenuItem tsmiTimer;
        private System.Windows.Forms.ToolStripMenuItem tsmiSetupTimer;
        private System.Windows.Forms.ToolStripMenuItem tsmiStopTimer;
        private System.Windows.Forms.ToolStripSeparator tssCollect2;
        private System.Windows.Forms.ToolStripSeparator tssCollect3;
        private System.Windows.Forms.ToolStripMenuItem tsmiAdvanced;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckForUpdates;
        private System.Windows.Forms.ToolStripSeparator tssHelp1;
        private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
        private System.Windows.Forms.ToolStripMenuItem tsmi30sec;
        private System.Windows.Forms.ToolStripMenuItem tsmi30min;
        private System.Windows.Forms.Panel pnViewer;
        private System.Windows.Forms.ListView lvViewer;
        private System.Windows.Forms.ColumnHeader chAvg;
        private System.Windows.Forms.ColumnHeader chAvgMax;
        private System.Windows.Forms.ColumnHeader chDesc;
        private System.Windows.Forms.ColumnHeader chFTP;
        private System.Windows.Forms.ColumnHeader chHR;
        private System.Windows.Forms.Panel pnOverall;
        private System.Windows.Forms.ListView lvOverall;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private System.Windows.Forms.Timer countdownTimer;
        private System.Windows.Forms.Timer runTimer;
    }
}