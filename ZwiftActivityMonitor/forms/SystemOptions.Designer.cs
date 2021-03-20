
namespace ZwiftActivityMonitor
{
    partial class SystemOptions
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("HelloWorld");
            this.gbSystemSettings = new System.Windows.Forms.GroupBox();
            this.pSystemSettings = new System.Windows.Forms.Panel();
            this.lblCurrentUser = new System.Windows.Forms.Label();
            this.lblNetwork = new System.Windows.Forms.Label();
            this.ckbAutoStart = new System.Windows.Forms.CheckBox();
            this.cbCurrentUser = new System.Windows.Forms.ComboBox();
            this.cbNetwork = new System.Windows.Forms.ComboBox();
            this.btnEditSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.gbUserProfiles = new System.Windows.Forms.GroupBox();
            this.pUserProfiles = new System.Windows.Forms.Panel();
            this.lvUserProfiles = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chDefault = new System.Windows.Forms.ColumnHeader();
            this.lblName = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.nWeight = new System.Windows.Forms.NumericUpDown();
            this.nPowerThreshold = new System.Windows.Forms.NumericUpDown();
            this.ckbDefault = new System.Windows.Forms.CheckBox();
            this.btnEditProfile = new System.Windows.Forms.Button();
            this.btnAddProfile = new System.Windows.Forms.Button();
            this.btnRemoveProfile = new System.Windows.Forms.Button();
            this.btnSaveProfile = new System.Windows.Forms.Button();
            this.btnCancelProfile = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ckbRunning = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.gbSystemSettings.SuspendLayout();
            this.pSystemSettings.SuspendLayout();
            this.gbUserProfiles.SuspendLayout();
            this.pUserProfiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPowerThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSystemSettings
            // 
            this.gbSystemSettings.Controls.Add(this.pSystemSettings);
            this.gbSystemSettings.Location = new System.Drawing.Point(12, 27);
            this.gbSystemSettings.Name = "gbSystemSettings";
            this.gbSystemSettings.Size = new System.Drawing.Size(548, 174);
            this.gbSystemSettings.TabIndex = 0;
            this.gbSystemSettings.TabStop = false;
            this.gbSystemSettings.Text = "System Settings";
            // 
            // pSystemSettings
            // 
            this.pSystemSettings.Controls.Add(this.button2);
            this.pSystemSettings.Controls.Add(this.button1);
            this.pSystemSettings.Controls.Add(this.ckbRunning);
            this.pSystemSettings.Controls.Add(this.label2);
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
            this.pSystemSettings.Size = new System.Drawing.Size(542, 152);
            this.pSystemSettings.TabIndex = 0;
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.AutoSize = true;
            this.lblCurrentUser.Location = new System.Drawing.Point(28, 17);
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(76, 15);
            this.lblCurrentUser.TabIndex = 0;
            this.lblCurrentUser.Text = "Current User:";
            // 
            // lblNetwork
            // 
            this.lblNetwork.AutoSize = true;
            this.lblNetwork.Location = new System.Drawing.Point(49, 49);
            this.lblNetwork.Name = "lblNetwork";
            this.lblNetwork.Size = new System.Drawing.Size(55, 15);
            this.lblNetwork.TabIndex = 1;
            this.lblNetwork.Text = "Network:";
            // 
            // ckbAutoStart
            // 
            this.ckbAutoStart.AutoSize = true;
            this.ckbAutoStart.Location = new System.Drawing.Point(205, 80);
            this.ckbAutoStart.Name = "ckbAutoStart";
            this.ckbAutoStart.Size = new System.Drawing.Size(81, 19);
            this.ckbAutoStart.TabIndex = 2;
            this.ckbAutoStart.Text = "Auto-Start";
            this.ckbAutoStart.UseVisualStyleBackColor = true;
            // 
            // cbCurrentUser
            // 
            this.cbCurrentUser.FormattingEnabled = true;
            this.cbCurrentUser.Location = new System.Drawing.Point(110, 14);
            this.cbCurrentUser.Name = "cbCurrentUser";
            this.cbCurrentUser.Size = new System.Drawing.Size(203, 23);
            this.cbCurrentUser.TabIndex = 6;
            // 
            // cbNetwork
            // 
            this.cbNetwork.FormattingEnabled = true;
            this.cbNetwork.Location = new System.Drawing.Point(110, 46);
            this.cbNetwork.Name = "cbNetwork";
            this.cbNetwork.Size = new System.Drawing.Size(203, 23);
            this.cbNetwork.TabIndex = 7;
            // 
            // btnEditSettings
            // 
            this.btnEditSettings.Location = new System.Drawing.Point(438, 10);
            this.btnEditSettings.Name = "btnEditSettings";
            this.btnEditSettings.Size = new System.Drawing.Size(89, 28);
            this.btnEditSettings.TabIndex = 8;
            this.btnEditSettings.Text = "Edit";
            this.btnEditSettings.UseVisualStyleBackColor = true;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(438, 46);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(89, 28);
            this.btnSaveSettings.TabIndex = 9;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.Location = new System.Drawing.Point(438, 82);
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(89, 28);
            this.btnCancelSettings.TabIndex = 10;
            this.btnCancelSettings.Text = "Cancel";
            this.btnCancelSettings.UseVisualStyleBackColor = true;
            // 
            // gbUserProfiles
            // 
            this.gbUserProfiles.Controls.Add(this.pUserProfiles);
            this.gbUserProfiles.Location = new System.Drawing.Point(15, 256);
            this.gbUserProfiles.Name = "gbUserProfiles";
            this.gbUserProfiles.Size = new System.Drawing.Size(545, 258);
            this.gbUserProfiles.TabIndex = 1;
            this.gbUserProfiles.TabStop = false;
            this.gbUserProfiles.Text = "User Profiles";
            // 
            // pUserProfiles
            // 
            this.pUserProfiles.Controls.Add(this.btnCancelProfile);
            this.pUserProfiles.Controls.Add(this.btnSaveProfile);
            this.pUserProfiles.Controls.Add(this.btnRemoveProfile);
            this.pUserProfiles.Controls.Add(this.btnAddProfile);
            this.pUserProfiles.Controls.Add(this.btnEditProfile);
            this.pUserProfiles.Controls.Add(this.ckbDefault);
            this.pUserProfiles.Controls.Add(this.nPowerThreshold);
            this.pUserProfiles.Controls.Add(this.nWeight);
            this.pUserProfiles.Controls.Add(this.tbName);
            this.pUserProfiles.Controls.Add(this.label1);
            this.pUserProfiles.Controls.Add(this.lblWeight);
            this.pUserProfiles.Controls.Add(this.lblName);
            this.pUserProfiles.Controls.Add(this.lvUserProfiles);
            this.pUserProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pUserProfiles.Location = new System.Drawing.Point(3, 19);
            this.pUserProfiles.Name = "pUserProfiles";
            this.pUserProfiles.Size = new System.Drawing.Size(539, 236);
            this.pUserProfiles.TabIndex = 0;
            // 
            // lvUserProfiles
            // 
            this.lvUserProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chDefault});
            this.lvUserProfiles.FullRowSelect = true;
            this.lvUserProfiles.HideSelection = false;
            this.lvUserProfiles.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvUserProfiles.Location = new System.Drawing.Point(3, 3);
            this.lvUserProfiles.Name = "lvUserProfiles";
            this.lvUserProfiles.Size = new System.Drawing.Size(375, 116);
            this.lvUserProfiles.TabIndex = 1;
            this.lvUserProfiles.UseCompatibleStateImageBehavior = false;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 150;
            // 
            // chDefault
            // 
            this.chDefault.Text = "Default";
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
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(53, 168);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(48, 15);
            this.lblWeight.TabIndex = 3;
            this.lblWeight.Text = "Weight:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Power Threshold:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(109, 134);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(176, 23);
            this.tbName.TabIndex = 5;
            // 
            // nWeight
            // 
            this.nWeight.Location = new System.Drawing.Point(109, 166);
            this.nWeight.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nWeight.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nWeight.Name = "nWeight";
            this.nWeight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nWeight.Size = new System.Drawing.Size(59, 23);
            this.nWeight.TabIndex = 6;
            this.nWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nWeight.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
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
            this.nPowerThreshold.Size = new System.Drawing.Size(59, 23);
            this.nPowerThreshold.TabIndex = 7;
            this.nPowerThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nPowerThreshold.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // ckbDefault
            // 
            this.ckbDefault.AutoSize = true;
            this.ckbDefault.Location = new System.Drawing.Point(314, 136);
            this.ckbDefault.Name = "ckbDefault";
            this.ckbDefault.Size = new System.Drawing.Size(64, 19);
            this.ckbDefault.TabIndex = 8;
            this.ckbDefault.Text = "Default";
            this.ckbDefault.UseVisualStyleBackColor = true;
            // 
            // btnEditProfile
            // 
            this.btnEditProfile.Location = new System.Drawing.Point(435, 12);
            this.btnEditProfile.Name = "btnEditProfile";
            this.btnEditProfile.Size = new System.Drawing.Size(89, 28);
            this.btnEditProfile.TabIndex = 9;
            this.btnEditProfile.Text = "Edit";
            this.btnEditProfile.UseVisualStyleBackColor = true;
            // 
            // btnAddProfile
            // 
            this.btnAddProfile.Location = new System.Drawing.Point(435, 46);
            this.btnAddProfile.Name = "btnAddProfile";
            this.btnAddProfile.Size = new System.Drawing.Size(89, 28);
            this.btnAddProfile.TabIndex = 10;
            this.btnAddProfile.Text = "Add";
            this.btnAddProfile.UseVisualStyleBackColor = true;
            // 
            // btnRemoveProfile
            // 
            this.btnRemoveProfile.Location = new System.Drawing.Point(435, 80);
            this.btnRemoveProfile.Name = "btnRemoveProfile";
            this.btnRemoveProfile.Size = new System.Drawing.Size(89, 28);
            this.btnRemoveProfile.TabIndex = 11;
            this.btnRemoveProfile.Text = "Remove";
            this.btnRemoveProfile.UseVisualStyleBackColor = true;
            // 
            // btnSaveProfile
            // 
            this.btnSaveProfile.Location = new System.Drawing.Point(435, 114);
            this.btnSaveProfile.Name = "btnSaveProfile";
            this.btnSaveProfile.Size = new System.Drawing.Size(89, 28);
            this.btnSaveProfile.TabIndex = 12;
            this.btnSaveProfile.Text = "Save";
            this.btnSaveProfile.UseVisualStyleBackColor = true;
            // 
            // btnCancelProfile
            // 
            this.btnCancelProfile.Location = new System.Drawing.Point(435, 148);
            this.btnCancelProfile.Name = "btnCancelProfile";
            this.btnCancelProfile.Size = new System.Drawing.Size(89, 28);
            this.btnCancelProfile.TabIndex = 13;
            this.btnCancelProfile.Text = "Cancel";
            this.btnCancelProfile.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(246, 533);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Network Monitor:";
            // 
            // ckbRunning
            // 
            this.ckbRunning.AutoSize = true;
            this.ckbRunning.Location = new System.Drawing.Point(110, 80);
            this.ckbRunning.Name = "ckbRunning";
            this.ckbRunning.Size = new System.Drawing.Size(71, 19);
            this.ckbRunning.TabIndex = 13;
            this.ckbRunning.Text = "Running";
            this.ckbRunning.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(110, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 28);
            this.button1.TabIndex = 14;
            this.button1.Text = "Stop";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(205, 106);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 28);
            this.button2.TabIndex = 15;
            this.button2.Text = "Start";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SystemOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 568);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbUserProfiles);
            this.Controls.Add(this.gbSystemSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Options";
            this.gbSystemSettings.ResumeLayout(false);
            this.pSystemSettings.ResumeLayout(false);
            this.pSystemSettings.PerformLayout();
            this.gbUserProfiles.ResumeLayout(false);
            this.pUserProfiles.ResumeLayout(false);
            this.pUserProfiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPowerThreshold)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSystemSettings;
        private System.Windows.Forms.Panel pSystemSettings;
        private System.Windows.Forms.ComboBox cbNetwork;
        private System.Windows.Forms.ComboBox cbCurrentUser;
        private System.Windows.Forms.CheckBox ckbAutoStart;
        private System.Windows.Forms.Label lblNetwork;
        private System.Windows.Forms.Label lblCurrentUser;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnEditSettings;
        private System.Windows.Forms.GroupBox gbUserProfiles;
        private System.Windows.Forms.Panel pUserProfiles;
        private System.Windows.Forms.NumericUpDown nPowerThreshold;
        private System.Windows.Forms.NumericUpDown nWeight;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ListView lvUserProfiles;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chDefault;
        private System.Windows.Forms.CheckBox ckbDefault;
        private System.Windows.Forms.CheckBox ckbRunning;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancelProfile;
        private System.Windows.Forms.Button btnSaveProfile;
        private System.Windows.Forms.Button btnRemoveProfile;
        private System.Windows.Forms.Button btnAddProfile;
        private System.Windows.Forms.Button btnEditProfile;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}