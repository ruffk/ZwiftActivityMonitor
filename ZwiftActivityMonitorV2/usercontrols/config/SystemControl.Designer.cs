
namespace ZwiftActivityMonitorV2
{
    partial class SystemControl
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
            this.pControl = new System.Windows.Forms.Panel();
            this.gbSystemZpm = new GroupBoxEx();
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
            this.gbSystemSettings = new GroupBoxEx();
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
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.pControl.SuspendLayout();
            this.gbSystemZpm.SuspendLayout();
            this.pZpm.SuspendLayout();
            this.gbSystemSettings.SuspendLayout();
            this.pSystemSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBase
            // 
            this.pBase.Controls.Add(this.pControl);
            this.pBase.Size = new System.Drawing.Size(587, 518);
            // 
            // pControl
            // 
            this.pControl.Controls.Add(this.gbSystemZpm);
            this.pControl.Controls.Add(this.tbDescSystem);
            this.pControl.Controls.Add(this.gbSystemSettings);
            this.pControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pControl.Location = new System.Drawing.Point(0, 0);
            this.pControl.Name = "pControl";
            this.pControl.Size = new System.Drawing.Size(587, 518);
            this.pControl.TabIndex = 0;
            // 
            // gbSystemZpm
            // 
            this.gbSystemZpm.Controls.Add(this.pZpm);
            this.gbSystemZpm.Location = new System.Drawing.Point(20, 286);
            this.gbSystemZpm.Name = "gbSystemZpm";
            this.gbSystemZpm.Size = new System.Drawing.Size(542, 221);
            this.gbSystemZpm.TabIndex = 5;
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
            this.lvTrace.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTrace.HideSelection = false;
            this.lvTrace.Location = new System.Drawing.Point(149, 79);
            this.lvTrace.MultiSelect = false;
            this.lvTrace.Name = "lvTrace";
            this.lvTrace.OwnerDraw = true;
            this.lvTrace.Scrollable = false;
            this.lvTrace.Size = new System.Drawing.Size(289, 106);
            this.lvTrace.TabIndex = 88;
            this.lvTrace.TabStop = false;
            this.lvTrace.UseCompatibleStateImageBehavior = false;
            this.lvTrace.View = System.Windows.Forms.View.Details;
            this.lvTrace.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvTrace.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvTrace.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListView_ItemSelectionChanged_Disable);
            this.lvTrace.Enter += new System.EventHandler(this.SkipControl_Enter);
            this.lvTrace.Resize += new System.EventHandler(this.ListView_Resize);
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
            this.btnStart.Location = new System.Drawing.Point(438, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(89, 28);
            this.btnStart.TabIndex = 90;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(438, 46);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(89, 28);
            this.btnStop.TabIndex = 100;
            this.btnStop.Text = "Stop";
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
            this.ckbRunning.Enabled = false;
            this.ckbRunning.Location = new System.Drawing.Point(149, 16);
            this.ckbRunning.Name = "ckbRunning";
            this.ckbRunning.Size = new System.Drawing.Size(71, 19);
            this.ckbRunning.TabIndex = 31;
            this.ckbRunning.TabStop = false;
            this.ckbRunning.Text = "Running";
            this.ckbRunning.UseVisualStyleBackColor = false;
            this.ckbRunning.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // tbDescSystem
            // 
            this.tbDescSystem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDescSystem.Location = new System.Drawing.Point(20, 20);
            this.tbDescSystem.Multiline = true;
            this.tbDescSystem.Name = "tbDescSystem";
            this.tbDescSystem.ReadOnly = true;
            this.tbDescSystem.Size = new System.Drawing.Size(545, 47);
            this.tbDescSystem.TabIndex = 3;
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
            this.gbSystemSettings.TabIndex = 4;
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
            this.label7.Location = new System.Drawing.Point(269, 141);
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
            this.tbCurWindowPosY.Location = new System.Drawing.Point(226, 138);
            this.tbCurWindowPosY.Name = "tbCurWindowPosY";
            this.tbCurWindowPosY.Size = new System.Drawing.Size(37, 23);
            this.tbCurWindowPosY.TabIndex = 87;
            this.tbCurWindowPosY.TabStop = false;
            this.tbCurWindowPosY.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // tbCurWindowPosX
            // 
            this.tbCurWindowPosX.BackColor = System.Drawing.SystemColors.Control;
            this.tbCurWindowPosX.Location = new System.Drawing.Point(183, 138);
            this.tbCurWindowPosX.Name = "tbCurWindowPosX";
            this.tbCurWindowPosX.Size = new System.Drawing.Size(37, 23);
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
            this.tbWindowPosY.Location = new System.Drawing.Point(226, 107);
            this.tbWindowPosY.MaxLength = 5;
            this.tbWindowPosY.Name = "tbWindowPosY";
            this.tbWindowPosY.Size = new System.Drawing.Size(37, 23);
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
            this.tbWindowPosX.MaxLength = 5;
            this.tbWindowPosX.Name = "tbWindowPosX";
            this.tbWindowPosX.Size = new System.Drawing.Size(37, 23);
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
            this.btnCancelSettings.Click += new System.EventHandler(this.btnCancelSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(438, 46);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(89, 28);
            this.btnSaveSettings.TabIndex = 70;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnEditSettings
            // 
            this.btnEditSettings.Location = new System.Drawing.Point(438, 10);
            this.btnEditSettings.Name = "btnEditSettings";
            this.btnEditSettings.Size = new System.Drawing.Size(89, 28);
            this.btnEditSettings.TabIndex = 60;
            this.btnEditSettings.Text = "Edit";
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
            this.cbNetwork.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
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
            this.cbCurrentUser.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // ckbAutoStart
            // 
            this.ckbAutoStart.AutoSize = true;
            this.ckbAutoStart.Location = new System.Drawing.Point(152, 80);
            this.ckbAutoStart.Name = "ckbAutoStart";
            this.ckbAutoStart.Size = new System.Drawing.Size(81, 19);
            this.ckbAutoStart.TabIndex = 25;
            this.ckbAutoStart.Text = "Auto-Start";
            this.ckbAutoStart.UseVisualStyleBackColor = false;
            this.ckbAutoStart.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.ckbAutoStart.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.ckbAutoStart.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
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
            // SystemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SystemControl";
            this.Size = new System.Drawing.Size(587, 540);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.pBase.ResumeLayout(false);
            this.pControl.ResumeLayout(false);
            this.pControl.PerformLayout();
            this.gbSystemZpm.ResumeLayout(false);
            this.pZpm.ResumeLayout(false);
            this.pZpm.PerformLayout();
            this.gbSystemSettings.ResumeLayout(false);
            this.pSystemSettings.ResumeLayout(false);
            this.pSystemSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pControl;
        private GroupBoxEx gbSystemZpm;
        private System.Windows.Forms.Panel pZpm;
        private System.Windows.Forms.Label lblEventCount;
        private System.Windows.Forms.Label lblEvents;
        private System.Windows.Forms.ListView lvTrace;
        private System.Windows.Forms.ColumnHeader chPlayerId;
        private System.Windows.Forms.ColumnHeader chPower;
        private System.Windows.Forms.ColumnHeader chHeartrate;
        private System.Windows.Forms.ColumnHeader chEventTime;
        private System.Windows.Forms.Label lblEventsProcessed;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox ckbRunning;
        private System.Windows.Forms.TextBox tbDescSystem;
        private GroupBoxEx gbSystemSettings;
        private System.Windows.Forms.Panel pSystemSettings;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbCurWindowPosY;
        private System.Windows.Forms.TextBox tbCurWindowPosX;
        private System.Windows.Forms.Label lblCurWindow;
        private System.Windows.Forms.TextBox tbWindowPosY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbWindowPosX;
        private System.Windows.Forms.Label lblDefWindow;
        private System.Windows.Forms.Label lblZpm;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnEditSettings;
        private System.Windows.Forms.ComboBox cbNetwork;
        private System.Windows.Forms.ComboBox cbCurrentUser;
        private System.Windows.Forms.CheckBox ckbAutoStart;
        private System.Windows.Forms.Label lblNetwork;
        private System.Windows.Forms.Label lblCurrentUser;
    }
}
