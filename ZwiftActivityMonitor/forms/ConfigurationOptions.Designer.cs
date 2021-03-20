
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationOptions));
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tpSystem = new System.Windows.Forms.TabPage();
            this.gbSystemZpm = new System.Windows.Forms.GroupBox();
            this.pZpm = new System.Windows.Forms.Panel();
            this.lblEventCount = new System.Windows.Forms.Label();
            this.lblEvents = new System.Windows.Forms.Label();
            this.lvTrace = new System.Windows.Forms.ListView();
            this.chPlayerId = new System.Windows.Forms.ColumnHeader();
            this.chPower = new System.Windows.Forms.ColumnHeader();
            this.chHeartrate = new System.Windows.Forms.ColumnHeader();
            this.chEventTime = new System.Windows.Forms.ColumnHeader();
            this.lblEventsProcessed = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.ckbRunning = new System.Windows.Forms.CheckBox();
            this.tbDescSystem = new System.Windows.Forms.TextBox();
            this.gbSystemSettings = new System.Windows.Forms.GroupBox();
            this.pSystemSettings = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbCurWindowPosY = new System.Windows.Forms.TextBox();
            this.tbCurWindowPosX = new System.Windows.Forms.TextBox();
            this.lblCurWindow = new System.Windows.Forms.Label();
            this.tbWindowPosY = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbWindowPosX = new System.Windows.Forms.TextBox();
            this.lblDefWindow = new System.Windows.Forms.Label();
            this.lblZpm = new System.Windows.Forms.Label();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnEditSettings = new System.Windows.Forms.Button();
            this.cbNetwork = new System.Windows.Forms.ComboBox();
            this.cbCurrentUser = new System.Windows.Forms.ComboBox();
            this.ckbAutoStart = new System.Windows.Forms.CheckBox();
            this.lblNetwork = new System.Windows.Forms.Label();
            this.lblCurrentUser = new System.Windows.Forms.Label();
            this.tpUsers = new System.Windows.Forms.TabPage();
            this.tbDescUsers = new System.Windows.Forms.TextBox();
            this.gbUserProfiles = new System.Windows.Forms.GroupBox();
            this.pUserProfiles = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.clbCollectors = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pWeightUomGroup = new System.Windows.Forms.Panel();
            this.rbKgs = new System.Windows.Forms.RadioButton();
            this.rbLbs = new System.Windows.Forms.RadioButton();
            this.tbWeight = new System.Windows.Forms.TextBox();
            this.btnCancelProfile = new System.Windows.Forms.Button();
            this.btnSaveProfile = new System.Windows.Forms.Button();
            this.btnRemoveProfile = new System.Windows.Forms.Button();
            this.btnAddProfile = new System.Windows.Forms.Button();
            this.btnEditProfile = new System.Windows.Forms.Button();
            this.ckbDefault = new System.Windows.Forms.CheckBox();
            this.nPowerThreshold = new System.Windows.Forms.NumericUpDown();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lvUserProfiles = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chDefault = new System.Windows.Forms.ColumnHeader();
            this.chWeight = new System.Windows.Forms.ColumnHeader();
            this.chThreshold = new System.Windows.Forms.ColumnHeader();
            this.chBlank = new System.Windows.Forms.ColumnHeader();
            this.tpStatisticsControl = new System.Windows.Forms.TabPage();
            this.ucStatistics = new ZwiftActivityMonitor.StatisticsControl();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabOptions.SuspendLayout();
            this.tpSystem.SuspendLayout();
            this.gbSystemZpm.SuspendLayout();
            this.pZpm.SuspendLayout();
            this.gbSystemSettings.SuspendLayout();
            this.pSystemSettings.SuspendLayout();
            this.tpUsers.SuspendLayout();
            this.gbUserProfiles.SuspendLayout();
            this.pUserProfiles.SuspendLayout();
            this.pWeightUomGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPowerThreshold)).BeginInit();
            this.tpStatisticsControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.tpSystem);
            this.tabOptions.Controls.Add(this.tpUsers);
            this.tabOptions.Controls.Add(this.tpStatisticsControl);
            this.tabOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOptions.Location = new System.Drawing.Point(0, 0);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(598, 587);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabOptions_Selecting);
            this.tabOptions.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabOptions_Selected);
            this.tabOptions.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabOptions_Selecting);
            this.tabOptions.Deselected += new System.Windows.Forms.TabControlEventHandler(this.tabOptions_Selected);
            // 
            // tpSystem
            // 
            this.tpSystem.BackColor = System.Drawing.SystemColors.Control;
            this.tpSystem.Controls.Add(this.gbSystemZpm);
            this.tpSystem.Controls.Add(this.tbDescSystem);
            this.tpSystem.Controls.Add(this.gbSystemSettings);
            this.tpSystem.Location = new System.Drawing.Point(4, 24);
            this.tpSystem.Name = "tpSystem";
            this.tpSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tpSystem.Size = new System.Drawing.Size(590, 559);
            this.tpSystem.TabIndex = 0;
            this.tpSystem.Text = "System";
            // 
            // gbSystemZpm
            // 
            this.gbSystemZpm.Controls.Add(this.pZpm);
            this.gbSystemZpm.Location = new System.Drawing.Point(23, 287);
            this.gbSystemZpm.Name = "gbSystemZpm";
            this.gbSystemZpm.Size = new System.Drawing.Size(542, 221);
            this.gbSystemZpm.TabIndex = 2;
            this.gbSystemZpm.TabStop = false;
            this.gbSystemZpm.Text = "Zwift Packet Monitoring";
            // 
            // pZpm
            // 
            this.pZpm.Controls.Add(this.lblEventCount);
            this.pZpm.Controls.Add(this.lblEvents);
            this.pZpm.Controls.Add(this.lvTrace);
            this.pZpm.Controls.Add(this.lblEventsProcessed);
            this.pZpm.Controls.Add(this.btnStart);
            this.pZpm.Controls.Add(this.btnStop);
            this.pZpm.Controls.Add(this.lblStatus);
            this.pZpm.Controls.Add(this.ckbRunning);
            this.pZpm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pZpm.Location = new System.Drawing.Point(3, 19);
            this.pZpm.Name = "pZpm";
            this.pZpm.Size = new System.Drawing.Size(536, 199);
            this.pZpm.TabIndex = 0;
            // 
            // lblEventCount
            // 
            this.lblEventCount.Location = new System.Drawing.Point(149, 48);
            this.lblEventCount.Name = "lblEventCount";
            this.lblEventCount.Size = new System.Drawing.Size(63, 15);
            this.lblEventCount.TabIndex = 101;
            // 
            // lblEvents
            // 
            this.lblEvents.AutoSize = true;
            this.lblEvents.Location = new System.Drawing.Point(99, 121);
            this.lblEvents.Name = "lblEvents";
            this.lblEvents.Size = new System.Drawing.Size(44, 15);
            this.lblEvents.TabIndex = 89;
            this.lblEvents.Text = "Events:";
            // 
            // lvTrace
            // 
            this.lvTrace.BackColor = System.Drawing.SystemColors.Control;
            this.lvTrace.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPlayerId,
            this.chPower,
            this.chHeartrate,
            this.chEventTime});
            this.lvTrace.HideSelection = false;
            this.lvTrace.Location = new System.Drawing.Point(149, 79);
            this.lvTrace.MultiSelect = false;
            this.lvTrace.Name = "lvTrace";
            this.lvTrace.Scrollable = false;
            this.lvTrace.Size = new System.Drawing.Size(289, 106);
            this.lvTrace.TabIndex = 88;
            this.lvTrace.TabStop = false;
            this.lvTrace.UseCompatibleStateImageBehavior = false;
            this.lvTrace.View = System.Windows.Forms.View.Details;
            this.lvTrace.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // chPlayerId
            // 
            this.chPlayerId.Name = "chPlayerId";
            this.chPlayerId.Text = "PlayerId";
            // 
            // chPower
            // 
            this.chPower.Name = "chPower";
            this.chPower.Text = "Power";
            this.chPower.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chPower.Width = 45;
            // 
            // chHeartrate
            // 
            this.chHeartrate.Name = "chHeartrate";
            this.chHeartrate.Text = "Heart Rate";
            this.chHeartrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chHeartrate.Width = 70;
            // 
            // chEventTime
            // 
            this.chEventTime.Name = "chEventTime";
            this.chEventTime.Text = "Event Time";
            this.chEventTime.Width = 300;
            // 
            // lblEventsProcessed
            // 
            this.lblEventsProcessed.AutoSize = true;
            this.lblEventsProcessed.Location = new System.Drawing.Point(43, 48);
            this.lblEventsProcessed.Name = "lblEventsProcessed";
            this.lblEventsProcessed.Size = new System.Drawing.Size(100, 15);
            this.lblEventsProcessed.TabIndex = 51;
            this.lblEventsProcessed.Text = "Events Processed:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(435, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(89, 28);
            this.btnStart.TabIndex = 90;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(435, 44);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(89, 28);
            this.btnStop.TabIndex = 100;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(101, 17);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 15);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status:";
            // 
            // ckbRunning
            // 
            this.ckbRunning.AutoCheck = false;
            this.ckbRunning.AutoSize = true;
            this.ckbRunning.Location = new System.Drawing.Point(149, 16);
            this.ckbRunning.Name = "ckbRunning";
            this.ckbRunning.Size = new System.Drawing.Size(71, 19);
            this.ckbRunning.TabIndex = 31;
            this.ckbRunning.TabStop = false;
            this.ckbRunning.Text = "Running";
            this.ckbRunning.UseVisualStyleBackColor = true;
            // 
            // tbDescSystem
            // 
            this.tbDescSystem.BackColor = System.Drawing.SystemColors.Control;
            this.tbDescSystem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDescSystem.Location = new System.Drawing.Point(20, 20);
            this.tbDescSystem.Multiline = true;
            this.tbDescSystem.Name = "tbDescSystem";
            this.tbDescSystem.ReadOnly = true;
            this.tbDescSystem.Size = new System.Drawing.Size(545, 47);
            this.tbDescSystem.TabIndex = 0;
            this.tbDescSystem.TabStop = false;
            this.tbDescSystem.Text = "Configure system options to use while running the Zwift Activity Monitor.";
            this.tbDescSystem.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // gbSystemSettings
            // 
            this.gbSystemSettings.Controls.Add(this.pSystemSettings);
            this.gbSystemSettings.Location = new System.Drawing.Point(20, 73);
            this.gbSystemSettings.Name = "gbSystemSettings";
            this.gbSystemSettings.Size = new System.Drawing.Size(548, 196);
            this.gbSystemSettings.TabIndex = 1;
            this.gbSystemSettings.TabStop = false;
            this.gbSystemSettings.Text = "System Settings";
            // 
            // pSystemSettings
            // 
            this.pSystemSettings.Controls.Add(this.label7);
            this.pSystemSettings.Controls.Add(this.label8);
            this.pSystemSettings.Controls.Add(this.tbCurWindowPosY);
            this.pSystemSettings.Controls.Add(this.tbCurWindowPosX);
            this.pSystemSettings.Controls.Add(this.lblCurWindow);
            this.pSystemSettings.Controls.Add(this.tbWindowPosY);
            this.pSystemSettings.Controls.Add(this.label2);
            this.pSystemSettings.Controls.Add(this.tbWindowPosX);
            this.pSystemSettings.Controls.Add(this.lblDefWindow);
            this.pSystemSettings.Controls.Add(this.lblZpm);
            this.pSystemSettings.Controls.Add(this.btnCancelSettings);
            this.pSystemSettings.Controls.Add(this.btnSaveSettings);
            this.pSystemSettings.Controls.Add(this.btnEditSettings);
            this.pSystemSettings.Controls.Add(this.cbNetwork);
            this.pSystemSettings.Controls.Add(this.cbCurrentUser);
            this.pSystemSettings.Controls.Add(this.ckbAutoStart);
            this.pSystemSettings.Controls.Add(this.lblNetwork);
            this.pSystemSettings.Controls.Add(this.lblCurrentUser);
            this.pSystemSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSystemSettings.Location = new System.Drawing.Point(3, 19);
            this.pSystemSettings.Name = "pSystemSettings";
            this.pSystemSettings.Size = new System.Drawing.Size(542, 174);
            this.pSystemSettings.TabIndex = 3;
            this.pSystemSettings.TabStop = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(259, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(148, 15);
            this.label7.TabIndex = 89;
            this.label7.Text = "(Activity Monitor Window)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(152, 141);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 15);
            this.label8.TabIndex = 88;
            this.label8.Text = "x, y";
            // 
            // tbCurWindowPosY
            // 
            this.tbCurWindowPosY.BackColor = System.Drawing.SystemColors.Control;
            this.tbCurWindowPosY.Location = new System.Drawing.Point(221, 138);
            this.tbCurWindowPosY.Name = "tbCurWindowPosY";
            this.tbCurWindowPosY.Size = new System.Drawing.Size(32, 23);
            this.tbCurWindowPosY.TabIndex = 87;
            this.tbCurWindowPosY.TabStop = false;
            this.tbCurWindowPosY.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // tbCurWindowPosX
            // 
            this.tbCurWindowPosX.BackColor = System.Drawing.SystemColors.Control;
            this.tbCurWindowPosX.Location = new System.Drawing.Point(183, 138);
            this.tbCurWindowPosX.Name = "tbCurWindowPosX";
            this.tbCurWindowPosX.Size = new System.Drawing.Size(32, 23);
            this.tbCurWindowPosX.TabIndex = 86;
            this.tbCurWindowPosX.TabStop = false;
            this.tbCurWindowPosX.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // lblCurWindow
            // 
            this.lblCurWindow.AutoSize = true;
            this.lblCurWindow.Location = new System.Drawing.Point(3, 141);
            this.lblCurWindow.Name = "lblCurWindow";
            this.lblCurWindow.Size = new System.Drawing.Size(143, 15);
            this.lblCurWindow.TabIndex = 85;
            this.lblCurWindow.Text = "Current Window Position:";
            // 
            // tbWindowPosY
            // 
            this.tbWindowPosY.Location = new System.Drawing.Point(221, 107);
            this.tbWindowPosY.MaxLength = 3;
            this.tbWindowPosY.Name = "tbWindowPosY";
            this.tbWindowPosY.Size = new System.Drawing.Size(32, 23);
            this.tbWindowPosY.TabIndex = 35;
            this.tbWindowPosY.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbWindowPosY.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbWindowPosY.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 15);
            this.label2.TabIndex = 83;
            this.label2.Text = "x, y";
            // 
            // tbWindowPosX
            // 
            this.tbWindowPosX.Location = new System.Drawing.Point(183, 107);
            this.tbWindowPosX.MaxLength = 3;
            this.tbWindowPosX.Name = "tbWindowPosX";
            this.tbWindowPosX.Size = new System.Drawing.Size(32, 23);
            this.tbWindowPosX.TabIndex = 30;
            this.tbWindowPosX.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbWindowPosX.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbWindowPosX.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblDefWindow
            // 
            this.lblDefWindow.AutoSize = true;
            this.lblDefWindow.Location = new System.Drawing.Point(5, 110);
            this.lblDefWindow.Name = "lblDefWindow";
            this.lblDefWindow.Size = new System.Drawing.Size(141, 15);
            this.lblDefWindow.TabIndex = 81;
            this.lblDefWindow.Text = "Default Window Position:";
            // 
            // lblZpm
            // 
            this.lblZpm.AutoSize = true;
            this.lblZpm.Location = new System.Drawing.Point(25, 81);
            this.lblZpm.Name = "lblZpm";
            this.lblZpm.Size = new System.Drawing.Size(121, 15);
            this.lblZpm.TabIndex = 11;
            this.lblZpm.Text = "Zwift Packet Monitor:";
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.Location = new System.Drawing.Point(438, 82);
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(89, 28);
            this.btnCancelSettings.TabIndex = 80;
            this.btnCancelSettings.Text = "Cancel";
            this.btnCancelSettings.UseVisualStyleBackColor = true;
            this.btnCancelSettings.Click += new System.EventHandler(this.btnCancelSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(438, 46);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(89, 28);
            this.btnSaveSettings.TabIndex = 70;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnEditSettings
            // 
            this.btnEditSettings.Location = new System.Drawing.Point(438, 10);
            this.btnEditSettings.Name = "btnEditSettings";
            this.btnEditSettings.Size = new System.Drawing.Size(89, 28);
            this.btnEditSettings.TabIndex = 60;
            this.btnEditSettings.Text = "Edit";
            this.btnEditSettings.UseVisualStyleBackColor = true;
            this.btnEditSettings.Click += new System.EventHandler(this.btnEditSettings_Click);
            // 
            // cbNetwork
            // 
            this.cbNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNetwork.FormattingEnabled = true;
            this.cbNetwork.Location = new System.Drawing.Point(152, 46);
            this.cbNetwork.Name = "cbNetwork";
            this.cbNetwork.Size = new System.Drawing.Size(203, 23);
            this.cbNetwork.TabIndex = 20;
            this.cbNetwork.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.cbNetwork.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            // 
            // cbCurrentUser
            // 
            this.cbCurrentUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrentUser.FormattingEnabled = true;
            this.cbCurrentUser.Location = new System.Drawing.Point(152, 14);
            this.cbCurrentUser.Name = "cbCurrentUser";
            this.cbCurrentUser.Size = new System.Drawing.Size(203, 23);
            this.cbCurrentUser.TabIndex = 10;
            this.cbCurrentUser.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.cbCurrentUser.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            // 
            // ckbAutoStart
            // 
            this.ckbAutoStart.AutoSize = true;
            this.ckbAutoStart.Location = new System.Drawing.Point(152, 80);
            this.ckbAutoStart.Name = "ckbAutoStart";
            this.ckbAutoStart.Size = new System.Drawing.Size(81, 19);
            this.ckbAutoStart.TabIndex = 25;
            this.ckbAutoStart.Text = "Auto-Start";
            this.ckbAutoStart.UseVisualStyleBackColor = true;
            this.ckbAutoStart.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.ckbAutoStart.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            // 
            // lblNetwork
            // 
            this.lblNetwork.AutoSize = true;
            this.lblNetwork.Location = new System.Drawing.Point(91, 49);
            this.lblNetwork.Name = "lblNetwork";
            this.lblNetwork.Size = new System.Drawing.Size(55, 15);
            this.lblNetwork.TabIndex = 1;
            this.lblNetwork.Text = "Network:";
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.AutoSize = true;
            this.lblCurrentUser.Location = new System.Drawing.Point(70, 17);
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(76, 15);
            this.lblCurrentUser.TabIndex = 0;
            this.lblCurrentUser.Text = "Current User:";
            // 
            // tpUsers
            // 
            this.tpUsers.BackColor = System.Drawing.SystemColors.Control;
            this.tpUsers.Controls.Add(this.tbDescUsers);
            this.tpUsers.Controls.Add(this.gbUserProfiles);
            this.tpUsers.Location = new System.Drawing.Point(4, 24);
            this.tpUsers.Name = "tpUsers";
            this.tpUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tpUsers.Size = new System.Drawing.Size(590, 559);
            this.tpUsers.TabIndex = 1;
            this.tpUsers.Text = "Users";
            // 
            // tbDescUsers
            // 
            this.tbDescUsers.BackColor = System.Drawing.SystemColors.Control;
            this.tbDescUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDescUsers.Location = new System.Drawing.Point(20, 20);
            this.tbDescUsers.Multiline = true;
            this.tbDescUsers.Name = "tbDescUsers";
            this.tbDescUsers.ReadOnly = true;
            this.tbDescUsers.Size = new System.Drawing.Size(545, 40);
            this.tbDescUsers.TabIndex = 0;
            this.tbDescUsers.TabStop = false;
            this.tbDescUsers.Text = "Configure user details for the default profile.  Values added here determine how " +
    "w/kg and intensity factor (IF) are calculated.  Additional profiles for other us" +
    "ers can also be created.";
            this.tbDescUsers.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // gbUserProfiles
            // 
            this.gbUserProfiles.Controls.Add(this.pUserProfiles);
            this.gbUserProfiles.Location = new System.Drawing.Point(20, 73);
            this.gbUserProfiles.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.gbUserProfiles.Name = "gbUserProfiles";
            this.gbUserProfiles.Size = new System.Drawing.Size(545, 449);
            this.gbUserProfiles.TabIndex = 2;
            this.gbUserProfiles.TabStop = false;
            this.gbUserProfiles.Text = "User Profiles";
            // 
            // pUserProfiles
            // 
            this.pUserProfiles.Controls.Add(this.label4);
            this.pUserProfiles.Controls.Add(this.clbCollectors);
            this.pUserProfiles.Controls.Add(this.label3);
            this.pUserProfiles.Controls.Add(this.pWeightUomGroup);
            this.pUserProfiles.Controls.Add(this.tbWeight);
            this.pUserProfiles.Controls.Add(this.btnCancelProfile);
            this.pUserProfiles.Controls.Add(this.btnSaveProfile);
            this.pUserProfiles.Controls.Add(this.btnRemoveProfile);
            this.pUserProfiles.Controls.Add(this.btnAddProfile);
            this.pUserProfiles.Controls.Add(this.btnEditProfile);
            this.pUserProfiles.Controls.Add(this.ckbDefault);
            this.pUserProfiles.Controls.Add(this.nPowerThreshold);
            this.pUserProfiles.Controls.Add(this.tbName);
            this.pUserProfiles.Controls.Add(this.label1);
            this.pUserProfiles.Controls.Add(this.lblWeight);
            this.pUserProfiles.Controls.Add(this.lblName);
            this.pUserProfiles.Controls.Add(this.lvUserProfiles);
            this.pUserProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pUserProfiles.Location = new System.Drawing.Point(3, 19);
            this.pUserProfiles.Name = "pUserProfiles";
            this.pUserProfiles.Size = new System.Drawing.Size(539, 427);
            this.pUserProfiles.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 122;
            this.label4.Text = "Collectors:";
            // 
            // clbCollectors
            // 
            this.clbCollectors.BackColor = System.Drawing.SystemColors.Control;
            this.clbCollectors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbCollectors.CheckOnClick = true;
            this.clbCollectors.FormattingEnabled = true;
            this.clbCollectors.Location = new System.Drawing.Point(109, 231);
            this.clbCollectors.Name = "clbCollectors";
            this.clbCollectors.Size = new System.Drawing.Size(114, 162);
            this.clbCollectors.TabIndex = 75;
            this.clbCollectors.Enter += new System.EventHandler(this.UserProfiles_TooltipOnEnter);
            this.clbCollectors.Leave += new System.EventHandler(this.UserProfiles_TooltipOnLeave);
            this.clbCollectors.Validating += new System.ComponentModel.CancelEventHandler(this.UserProfiles_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 15);
            this.label3.TabIndex = 18;
            this.label3.Text = "(Enter the same values used in Zwift)";
            // 
            // pWeightUomGroup
            // 
            this.pWeightUomGroup.Controls.Add(this.rbKgs);
            this.pWeightUomGroup.Controls.Add(this.rbLbs);
            this.pWeightUomGroup.Location = new System.Drawing.Point(174, 163);
            this.pWeightUomGroup.Name = "pWeightUomGroup";
            this.pWeightUomGroup.Size = new System.Drawing.Size(111, 28);
            this.pWeightUomGroup.TabIndex = 17;
            // 
            // rbKgs
            // 
            this.rbKgs.AutoSize = true;
            this.rbKgs.Location = new System.Drawing.Point(52, 3);
            this.rbKgs.Name = "rbKgs";
            this.rbKgs.Size = new System.Drawing.Size(44, 19);
            this.rbKgs.TabIndex = 60;
            this.rbKgs.TabStop = true;
            this.rbKgs.Text = "Kgs";
            this.rbKgs.UseVisualStyleBackColor = true;
            this.rbKgs.Enter += new System.EventHandler(this.UserProfiles_TooltipOnEnter);
            this.rbKgs.Leave += new System.EventHandler(this.UserProfiles_TooltipOnLeave);
            this.rbKgs.Validating += new System.ComponentModel.CancelEventHandler(this.UserProfiles_Validating);
            // 
            // rbLbs
            // 
            this.rbLbs.AutoSize = true;
            this.rbLbs.Location = new System.Drawing.Point(3, 3);
            this.rbLbs.Name = "rbLbs";
            this.rbLbs.Size = new System.Drawing.Size(43, 19);
            this.rbLbs.TabIndex = 50;
            this.rbLbs.TabStop = true;
            this.rbLbs.Text = "Lbs";
            this.rbLbs.UseVisualStyleBackColor = true;
            this.rbLbs.Enter += new System.EventHandler(this.UserProfiles_TooltipOnEnter);
            this.rbLbs.Leave += new System.EventHandler(this.UserProfiles_TooltipOnLeave);
            this.rbLbs.Validating += new System.ComponentModel.CancelEventHandler(this.UserProfiles_Validating);
            // 
            // tbWeight
            // 
            this.tbWeight.Location = new System.Drawing.Point(109, 165);
            this.tbWeight.MaxLength = 5;
            this.tbWeight.Name = "tbWeight";
            this.tbWeight.Size = new System.Drawing.Size(43, 23);
            this.tbWeight.TabIndex = 40;
            this.tbWeight.Text = "888";
            this.tbWeight.Enter += new System.EventHandler(this.UserProfiles_TooltipOnEnter);
            this.tbWeight.Leave += new System.EventHandler(this.UserProfiles_TooltipOnLeave);
            this.tbWeight.Validating += new System.ComponentModel.CancelEventHandler(this.UserProfiles_Validating);
            // 
            // btnCancelProfile
            // 
            this.btnCancelProfile.Location = new System.Drawing.Point(435, 148);
            this.btnCancelProfile.Name = "btnCancelProfile";
            this.btnCancelProfile.Size = new System.Drawing.Size(89, 28);
            this.btnCancelProfile.TabIndex = 120;
            this.btnCancelProfile.Text = "Cancel";
            this.btnCancelProfile.UseVisualStyleBackColor = true;
            this.btnCancelProfile.Click += new System.EventHandler(this.btnCancelProfile_Click);
            // 
            // btnSaveProfile
            // 
            this.btnSaveProfile.Location = new System.Drawing.Point(435, 114);
            this.btnSaveProfile.Name = "btnSaveProfile";
            this.btnSaveProfile.Size = new System.Drawing.Size(89, 28);
            this.btnSaveProfile.TabIndex = 110;
            this.btnSaveProfile.Text = "Save";
            this.btnSaveProfile.UseVisualStyleBackColor = true;
            this.btnSaveProfile.Click += new System.EventHandler(this.btnSaveProfile_Click);
            // 
            // btnRemoveProfile
            // 
            this.btnRemoveProfile.Location = new System.Drawing.Point(435, 80);
            this.btnRemoveProfile.Name = "btnRemoveProfile";
            this.btnRemoveProfile.Size = new System.Drawing.Size(89, 28);
            this.btnRemoveProfile.TabIndex = 100;
            this.btnRemoveProfile.Text = "Remove";
            this.btnRemoveProfile.UseVisualStyleBackColor = true;
            this.btnRemoveProfile.Click += new System.EventHandler(this.btnRemoveProfile_Click);
            // 
            // btnAddProfile
            // 
            this.btnAddProfile.Location = new System.Drawing.Point(435, 46);
            this.btnAddProfile.Name = "btnAddProfile";
            this.btnAddProfile.Size = new System.Drawing.Size(89, 28);
            this.btnAddProfile.TabIndex = 90;
            this.btnAddProfile.Text = "Add";
            this.btnAddProfile.UseVisualStyleBackColor = true;
            this.btnAddProfile.Click += new System.EventHandler(this.btnAddProfile_Click);
            // 
            // btnEditProfile
            // 
            this.btnEditProfile.Location = new System.Drawing.Point(435, 12);
            this.btnEditProfile.Name = "btnEditProfile";
            this.btnEditProfile.Size = new System.Drawing.Size(89, 28);
            this.btnEditProfile.TabIndex = 80;
            this.btnEditProfile.Text = "Edit";
            this.btnEditProfile.UseVisualStyleBackColor = true;
            this.btnEditProfile.Click += new System.EventHandler(this.btnEditProfile_Click);
            // 
            // ckbDefault
            // 
            this.ckbDefault.AutoSize = true;
            this.ckbDefault.Location = new System.Drawing.Point(291, 136);
            this.ckbDefault.Name = "ckbDefault";
            this.ckbDefault.Size = new System.Drawing.Size(122, 19);
            this.ckbDefault.TabIndex = 30;
            this.ckbDefault.Text = "Default on Startup";
            this.ckbDefault.UseVisualStyleBackColor = true;
            this.ckbDefault.Enter += new System.EventHandler(this.UserProfiles_TooltipOnEnter);
            this.ckbDefault.Leave += new System.EventHandler(this.UserProfiles_TooltipOnLeave);
            this.ckbDefault.Validating += new System.ComponentModel.CancelEventHandler(this.UserProfiles_Validating);
            // 
            // nPowerThreshold
            // 
            this.nPowerThreshold.Location = new System.Drawing.Point(109, 198);
            this.nPowerThreshold.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nPowerThreshold.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nPowerThreshold.Name = "nPowerThreshold";
            this.nPowerThreshold.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nPowerThreshold.Size = new System.Drawing.Size(43, 23);
            this.nPowerThreshold.TabIndex = 70;
            this.nPowerThreshold.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nPowerThreshold.Enter += new System.EventHandler(this.UserProfiles_TooltipOnEnter);
            this.nPowerThreshold.Leave += new System.EventHandler(this.UserProfiles_TooltipOnLeave);
            this.nPowerThreshold.Validating += new System.ComponentModel.CancelEventHandler(this.UserProfiles_Validating);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(109, 134);
            this.tbName.MaxLength = 30;
            this.tbName.Name = "tbName";
            this.tbName.PlaceholderText = "Enter user name";
            this.tbName.Size = new System.Drawing.Size(176, 23);
            this.tbName.TabIndex = 20;
            this.tbName.Enter += new System.EventHandler(this.UserProfiles_TooltipOnEnter);
            this.tbName.Leave += new System.EventHandler(this.UserProfiles_TooltipOnLeave);
            this.tbName.Validating += new System.ComponentModel.CancelEventHandler(this.UserProfiles_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "FTP:";
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(53, 168);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(48, 15);
            this.lblWeight.TabIndex = 3;
            this.lblWeight.Text = "Weight:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(59, 137);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(42, 15);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name:";
            // 
            // lvUserProfiles
            // 
            this.lvUserProfiles.BackColor = System.Drawing.SystemColors.Control;
            this.lvUserProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chDefault,
            this.chWeight,
            this.chThreshold,
            this.chBlank});
            this.lvUserProfiles.FullRowSelect = true;
            this.lvUserProfiles.HideSelection = false;
            this.lvUserProfiles.Location = new System.Drawing.Point(3, 3);
            this.lvUserProfiles.MultiSelect = false;
            this.lvUserProfiles.Name = "lvUserProfiles";
            this.lvUserProfiles.Size = new System.Drawing.Size(375, 116);
            this.lvUserProfiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvUserProfiles.TabIndex = 10;
            this.lvUserProfiles.UseCompatibleStateImageBehavior = false;
            this.lvUserProfiles.View = System.Windows.Forms.View.Details;
            this.lvUserProfiles.SelectedIndexChanged += new System.EventHandler(this.lvUserProfiles_SelectedIndexChanged);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 180;
            // 
            // chDefault
            // 
            this.chDefault.Text = "Default";
            this.chDefault.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chDefault.Width = 53;
            // 
            // chWeight
            // 
            this.chWeight.Text = "Weight";
            this.chWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chWeight.Width = 58;
            // 
            // chThreshold
            // 
            this.chThreshold.Text = "Threshold";
            this.chThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chThreshold.Width = 65;
            // 
            // chBlank
            // 
            this.chBlank.Text = "";
            // 
            // tpStatisticsControl
            // 
            this.tpStatisticsControl.BackColor = System.Drawing.SystemColors.Control;
            this.tpStatisticsControl.Controls.Add(this.ucStatistics);
            this.tpStatisticsControl.Location = new System.Drawing.Point(4, 24);
            this.tpStatisticsControl.Name = "tpStatisticsControl";
            this.tpStatisticsControl.Padding = new System.Windows.Forms.Padding(3);
            this.tpStatisticsControl.Size = new System.Drawing.Size(590, 559);
            this.tpStatisticsControl.TabIndex = 3;
            this.tpStatisticsControl.Text = "Collectors";
            // 
            // ucStatistics
            // 
            this.ucStatistics.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ucStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStatistics.Location = new System.Drawing.Point(3, 3);
            this.ucStatistics.Name = "ucStatistics";
            this.ucStatistics.Size = new System.Drawing.Size(584, 553);
            this.ucStatistics.TabIndex = 0;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 565);
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
            this.ClientSize = new System.Drawing.Size(598, 587);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationOptions_FormClosing);
            this.Load += new System.EventHandler(this.ConfigurationOptions_Load);
            this.tabOptions.ResumeLayout(false);
            this.tpSystem.ResumeLayout(false);
            this.tpSystem.PerformLayout();
            this.gbSystemZpm.ResumeLayout(false);
            this.pZpm.ResumeLayout(false);
            this.pZpm.PerformLayout();
            this.gbSystemSettings.ResumeLayout(false);
            this.pSystemSettings.ResumeLayout(false);
            this.pSystemSettings.PerformLayout();
            this.tpUsers.ResumeLayout(false);
            this.tpUsers.PerformLayout();
            this.gbUserProfiles.ResumeLayout(false);
            this.pUserProfiles.ResumeLayout(false);
            this.pUserProfiles.PerformLayout();
            this.pWeightUomGroup.ResumeLayout(false);
            this.pWeightUomGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPowerThreshold)).EndInit();
            this.tpStatisticsControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabOptions;
        private System.Windows.Forms.TabPage tpSystem;
        private System.Windows.Forms.TabPage tpUsers;
        private System.Windows.Forms.GroupBox gbSystemSettings;
        private System.Windows.Forms.Panel pSystemSettings;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblZpm;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnEditSettings;
        private System.Windows.Forms.ComboBox cbNetwork;
        private System.Windows.Forms.ComboBox cbCurrentUser;
        private System.Windows.Forms.CheckBox ckbAutoStart;
        private System.Windows.Forms.Label lblNetwork;
        private System.Windows.Forms.Label lblCurrentUser;
        private System.Windows.Forms.TextBox tbDescUsers;
        private System.Windows.Forms.GroupBox gbUserProfiles;
        private System.Windows.Forms.Panel pUserProfiles;
        private System.Windows.Forms.Button btnCancelProfile;
        private System.Windows.Forms.Button btnSaveProfile;
        private System.Windows.Forms.Button btnRemoveProfile;
        private System.Windows.Forms.Button btnAddProfile;
        private System.Windows.Forms.Button btnEditProfile;
        private System.Windows.Forms.CheckBox ckbDefault;
        private System.Windows.Forms.NumericUpDown nPowerThreshold;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ListView lvUserProfiles;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.Panel pWeightUomGroup;
        private System.Windows.Forms.RadioButton rbKgs;
        private System.Windows.Forms.RadioButton rbLbs;
        private System.Windows.Forms.TextBox tbWeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDescSystem;
        private System.Windows.Forms.GroupBox gbSystemZpm;
        private System.Windows.Forms.CheckBox ckbRunning;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbCurWindowPosY;
        private System.Windows.Forms.TextBox tbCurWindowPosX;
        private System.Windows.Forms.Label lblCurWindow;
        private System.Windows.Forms.TextBox tbWindowPosY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbWindowPosX;
        private System.Windows.Forms.Label lblDefWindow;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pZpm;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblEventsProcessed;
        private System.Windows.Forms.Label lblEvents;
        private System.Windows.Forms.ListView lvTrace;
        private System.Windows.Forms.ColumnHeader chPlayerId;
        private System.Windows.Forms.ColumnHeader chPower;
        private System.Windows.Forms.ColumnHeader chHeartrate;
        private System.Windows.Forms.ColumnHeader chEventTime;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lblEventCount;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private System.Windows.Forms.ColumnHeader chDefault;
        private System.Windows.Forms.ColumnHeader chWeight;
        private System.Windows.Forms.ColumnHeader chThreshold;
        private System.Windows.Forms.ColumnHeader chBlank;
        private System.Windows.Forms.TabPage tpStatisticsControl;
        private StatisticsControl ucStatistics;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox clbCollectors;
    }
}