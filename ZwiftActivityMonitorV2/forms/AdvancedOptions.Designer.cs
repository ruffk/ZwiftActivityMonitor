﻿
namespace ZwiftActivityMonitorV2
{
    partial class AdvancedOptions
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
            this.gbZwiftPacketMonitor = new GroupBoxEx();
            this.rbSimulation = new System.Windows.Forms.RadioButton();
            this.tbPlayerId = new System.Windows.Forms.TextBox();
            this.lblPlayerId = new System.Windows.Forms.Label();
            this.lblBpm = new System.Windows.Forms.Label();
            this.lblEthernetDevice = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblHeartrate = new System.Windows.Forms.Label();
            this.rbFindByPlayerId = new System.Windows.Forms.RadioButton();
            this.rbFindByHeartRate = new System.Windows.Forms.RadioButton();
            this.lblEventsProcessed = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tbTargetHeartrate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbMonitorOthers = new System.Windows.Forms.CheckBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lvTrace = new System.Windows.Forms.ListView();
            this.chPlayerId = new System.Windows.Forms.ColumnHeader();
            this.chPower = new System.Windows.Forms.ColumnHeader();
            this.chHeartrate = new System.Windows.Forms.ColumnHeader();
            this.chEventTime = new System.Windows.Forms.ColumnHeader();
            this.gbZwiftPacketMonitor.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbZwiftPacketMonitor
            // 
            this.gbZwiftPacketMonitor.BackColor = System.Drawing.Color.Transparent;
            this.gbZwiftPacketMonitor.Controls.Add(this.rbSimulation);
            this.gbZwiftPacketMonitor.Controls.Add(this.tbPlayerId);
            this.gbZwiftPacketMonitor.Controls.Add(this.lblPlayerId);
            this.gbZwiftPacketMonitor.Controls.Add(this.lblBpm);
            this.gbZwiftPacketMonitor.Controls.Add(this.lblEthernetDevice);
            this.gbZwiftPacketMonitor.Controls.Add(this.label8);
            this.gbZwiftPacketMonitor.Controls.Add(this.lblHeartrate);
            this.gbZwiftPacketMonitor.Controls.Add(this.rbFindByPlayerId);
            this.gbZwiftPacketMonitor.Controls.Add(this.rbFindByHeartRate);
            this.gbZwiftPacketMonitor.Controls.Add(this.lblEventsProcessed);
            this.gbZwiftPacketMonitor.Controls.Add(this.lblStatus);
            this.gbZwiftPacketMonitor.Controls.Add(this.tbTargetHeartrate);
            this.gbZwiftPacketMonitor.Controls.Add(this.label5);
            this.gbZwiftPacketMonitor.Controls.Add(this.label4);
            this.gbZwiftPacketMonitor.Controls.Add(this.cbMonitorOthers);
            this.gbZwiftPacketMonitor.Controls.Add(this.btnStop);
            this.gbZwiftPacketMonitor.Controls.Add(this.btnStart);
            this.gbZwiftPacketMonitor.Controls.Add(this.label2);
            this.gbZwiftPacketMonitor.Controls.Add(this.label1);
            this.gbZwiftPacketMonitor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbZwiftPacketMonitor.Location = new System.Drawing.Point(12, 12);
            this.gbZwiftPacketMonitor.Name = "gbZwiftPacketMonitor";
            this.gbZwiftPacketMonitor.Size = new System.Drawing.Size(400, 238);
            this.gbZwiftPacketMonitor.TabIndex = 0;
            this.gbZwiftPacketMonitor.TabStop = false;
            this.gbZwiftPacketMonitor.Text = "Zwift Packet Monitor";
            // 
            // rbSimulation
            // 
            this.rbSimulation.AutoSize = true;
            this.rbSimulation.Location = new System.Drawing.Point(47, 193);
            this.rbSimulation.Name = "rbSimulation";
            this.rbSimulation.Size = new System.Drawing.Size(118, 19);
            this.rbSimulation.TabIndex = 20;
            this.rbSimulation.TabStop = true;
            this.rbSimulation.Text = "Power Simulation";
            this.rbSimulation.UseVisualStyleBackColor = true;
            // 
            // tbPlayerId
            // 
            this.tbPlayerId.Location = new System.Drawing.Point(276, 169);
            this.tbPlayerId.MaxLength = 7;
            this.tbPlayerId.Name = "tbPlayerId";
            this.tbPlayerId.Size = new System.Drawing.Size(63, 23);
            this.tbPlayerId.TabIndex = 18;
            // 
            // lblPlayerId
            // 
            this.lblPlayerId.AutoSize = true;
            this.lblPlayerId.Location = new System.Drawing.Point(215, 172);
            this.lblPlayerId.Name = "lblPlayerId";
            this.lblPlayerId.Size = new System.Drawing.Size(55, 15);
            this.lblPlayerId.TabIndex = 17;
            this.lblPlayerId.Text = "Player Id:";
            // 
            // lblBpm
            // 
            this.lblBpm.AutoSize = true;
            this.lblBpm.Location = new System.Drawing.Point(311, 146);
            this.lblBpm.Name = "lblBpm";
            this.lblBpm.Size = new System.Drawing.Size(65, 15);
            this.lblBpm.TabIndex = 16;
            this.lblBpm.Text = "(+- 2 bpm)";
            // 
            // lblEthernetDevice
            // 
            this.lblEthernetDevice.AutoSize = true;
            this.lblEthernetDevice.Location = new System.Drawing.Point(133, 74);
            this.lblEthernetDevice.Name = "lblEthernetDevice";
            this.lblEthernetDevice.Size = new System.Drawing.Size(30, 15);
            this.lblEthernetDevice.TabIndex = 15;
            this.lblEthernetDevice.Text = "Eth0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(35, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 15);
            this.label8.TabIndex = 14;
            this.label8.Text = "Ethernet Device:";
            // 
            // lblHeartrate
            // 
            this.lblHeartrate.AutoSize = true;
            this.lblHeartrate.Location = new System.Drawing.Point(205, 146);
            this.lblHeartrate.Name = "lblHeartrate";
            this.lblHeartrate.Size = new System.Drawing.Size(65, 15);
            this.lblHeartrate.TabIndex = 13;
            this.lblHeartrate.Text = "Heart Rate:";
            // 
            // rbFindByPlayerId
            // 
            this.rbFindByPlayerId.AutoSize = true;
            this.rbFindByPlayerId.Location = new System.Drawing.Point(47, 168);
            this.rbFindByPlayerId.Name = "rbFindByPlayerId";
            this.rbFindByPlayerId.Size = new System.Drawing.Size(113, 19);
            this.rbFindByPlayerId.TabIndex = 5;
            this.rbFindByPlayerId.TabStop = true;
            this.rbFindByPlayerId.Text = "Find by Player ID";
            this.rbFindByPlayerId.UseVisualStyleBackColor = true;
            // 
            // rbFindByHeartRate
            // 
            this.rbFindByHeartRate.AutoSize = true;
            this.rbFindByHeartRate.Location = new System.Drawing.Point(47, 144);
            this.rbFindByHeartRate.Name = "rbFindByHeartRate";
            this.rbFindByHeartRate.Size = new System.Drawing.Size(122, 19);
            this.rbFindByHeartRate.TabIndex = 4;
            this.rbFindByHeartRate.TabStop = true;
            this.rbFindByHeartRate.Text = "Find by Heart Rate";
            this.rbFindByHeartRate.UseVisualStyleBackColor = true;
            // 
            // lblEventsProcessed
            // 
            this.lblEventsProcessed.AutoSize = true;
            this.lblEventsProcessed.Location = new System.Drawing.Point(133, 59);
            this.lblEventsProcessed.Name = "lblEventsProcessed";
            this.lblEventsProcessed.Size = new System.Drawing.Size(13, 15);
            this.lblEventsProcessed.TabIndex = 10;
            this.lblEventsProcessed.Text = "0";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(133, 44);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(75, 15);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Not Running";
            // 
            // tbTargetHeartrate
            // 
            this.tbTargetHeartrate.Location = new System.Drawing.Point(276, 143);
            this.tbTargetHeartrate.Name = "tbTargetHeartrate";
            this.tbTargetHeartrate.Size = new System.Drawing.Size(29, 23);
            this.tbTargetHeartrate.TabIndex = 6;
            this.tbTargetHeartrate.Text = "125";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(6, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Manual Operation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(6, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Demo / Debugging Options";
            // 
            // cbMonitorOthers
            // 
            this.cbMonitorOthers.AutoSize = true;
            this.cbMonitorOthers.Location = new System.Drawing.Point(31, 119);
            this.cbMonitorOthers.Name = "cbMonitorOthers";
            this.cbMonitorOthers.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbMonitorOthers.Size = new System.Drawing.Size(326, 19);
            this.cbMonitorOthers.TabIndex = 3;
            this.cbMonitorOthers.Text = "Monitor Other Zwifters (So you don\'t have to ride to test)";
            this.cbMonitorOthers.UseVisualStyleBackColor = true;
            this.cbMonitorOthers.CheckedChanged += new System.EventHandler(this.cbMonitorOthers_CheckedChanged);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.SystemColors.Control;
            this.btnStop.Location = new System.Drawing.Point(315, 54);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(61, 25);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.Control;
            this.btnStart.Location = new System.Drawing.Point(315, 19);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(61, 25);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Events Processed:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClose.Location = new System.Drawing.Point(170, 379);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // lvTrace
            // 
            this.lvTrace.BackColor = System.Drawing.SystemColors.Control;
            this.lvTrace.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPlayerId,
            this.chPower,
            this.chHeartrate,
            this.chEventTime});
            this.lvTrace.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lvTrace.HideSelection = false;
            this.lvTrace.Location = new System.Drawing.Point(12, 256);
            this.lvTrace.MultiSelect = false;
            this.lvTrace.Name = "lvTrace";
            this.lvTrace.OwnerDraw = true;
            this.lvTrace.Scrollable = false;
            this.lvTrace.Size = new System.Drawing.Size(400, 106);
            this.lvTrace.TabIndex = 2;
            this.lvTrace.UseCompatibleStateImageBehavior = false;
            this.lvTrace.View = System.Windows.Forms.View.Details;
            this.lvTrace.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvTrace.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
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
            this.chPower.Width = 50;
            // 
            // chHeartrate
            // 
            this.chHeartrate.Name = "chHeartrate";
            this.chHeartrate.Text = "Heartrate";
            this.chHeartrate.Width = 63;
            // 
            // chEventTime
            // 
            this.chEventTime.Name = "chEventTime";
            this.chEventTime.Text = "Event Time";
            this.chEventTime.Width = 300;
            // 
            // AdvancedOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 412);
            this.Controls.Add(this.lvTrace);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbZwiftPacketMonitor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconSize = new System.Drawing.Size(32, 32);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Text = "Advanced Options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AdvancedOptions_FormClosed);
            this.Load += new System.EventHandler(this.AdvancedOptions_Load);
            this.Shown += new System.EventHandler(this.AdvancedOptions_Shown);
            this.BackColorChanged += new System.EventHandler(this.AdvancedOptions_BackColorChanged);
            this.ForeColorChanged += new System.EventHandler(this.AdvancedOptions_ForeColorChanged);
            this.VisibleChanged += new System.EventHandler(this.AdvancedOptions_VisibleChanged);
            this.gbZwiftPacketMonitor.ResumeLayout(false);
            this.gbZwiftPacketMonitor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBoxEx gbZwiftPacketMonitor;
        private System.Windows.Forms.CheckBox cbMonitorOthers;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblEventsProcessed;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox tbTargetHeartrate;
        private System.Windows.Forms.Label lblEthernetDevice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblHeartrate;
        private System.Windows.Forms.RadioButton rbFindByPlayerId;
        private System.Windows.Forms.RadioButton rbFindByHeartRate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblBpm;
        private System.Windows.Forms.ListView lvTrace;
        private System.Windows.Forms.ColumnHeader chPlayerId;
        private System.Windows.Forms.ColumnHeader chPower;
        private System.Windows.Forms.ColumnHeader chHeartrate;
        private System.Windows.Forms.ColumnHeader chEventTime;
        private System.Windows.Forms.Label lblPlayerId;
        private System.Windows.Forms.TextBox tbPlayerId;
        private System.Windows.Forms.RadioButton rbSimulation;
    }
}