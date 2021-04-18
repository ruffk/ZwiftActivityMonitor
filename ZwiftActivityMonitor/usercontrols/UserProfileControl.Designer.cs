
namespace ZwiftActivityMonitor
{
    partial class UserProfileControl
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
            this.tbEmailAddr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.pControl.SuspendLayout();
            this.gbUserProfiles.SuspendLayout();
            this.pUserProfiles.SuspendLayout();
            this.pWeightUomGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPowerThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // pBase
            // 
            this.pBase.Controls.Add(this.pControl);
            this.pBase.Size = new System.Drawing.Size(586, 509);
            // 
            // pControl
            // 
            this.pControl.Controls.Add(this.tbDescUsers);
            this.pControl.Controls.Add(this.gbUserProfiles);
            this.pControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pControl.Location = new System.Drawing.Point(0, 0);
            this.pControl.Name = "pControl";
            this.pControl.Size = new System.Drawing.Size(586, 509);
            this.pControl.TabIndex = 3;
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
            this.tbDescUsers.TabIndex = 3;
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
            this.gbUserProfiles.Size = new System.Drawing.Size(545, 428);
            this.gbUserProfiles.TabIndex = 4;
            this.gbUserProfiles.TabStop = false;
            this.gbUserProfiles.Text = "User Profiles";
            // 
            // pUserProfiles
            // 
            this.pUserProfiles.Controls.Add(this.label2);
            this.pUserProfiles.Controls.Add(this.tbEmailAddr);
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
            this.pUserProfiles.Size = new System.Drawing.Size(539, 406);
            this.pUserProfiles.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 292);
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
            this.clbCollectors.Location = new System.Drawing.Point(106, 262);
            this.clbCollectors.Name = "clbCollectors";
            this.clbCollectors.Size = new System.Drawing.Size(114, 90);
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
            this.pWeightUomGroup.TabIndex = 50;
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
            this.rbLbs.TabIndex = 55;
            this.rbLbs.TabStop = true;
            this.rbLbs.Text = "Lbs";
            this.rbLbs.UseVisualStyleBackColor = true;
            this.rbLbs.Enter += new System.EventHandler(this.UserProfiles_TooltipOnEnter);
            this.rbLbs.Leave += new System.EventHandler(this.UserProfiles_TooltipOnLeave);
            this.rbLbs.Validating += new System.ComponentModel.CancelEventHandler(this.UserProfiles_Validating);
            // 
            // tbWeight
            // 
            this.tbWeight.Location = new System.Drawing.Point(109, 166);
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
            this.lvUserProfiles.CausesValidation = false;
            this.lvUserProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chDefault,
            this.chWeight,
            this.chThreshold,
            this.chBlank});
            this.lvUserProfiles.FullRowSelect = true;
            this.lvUserProfiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvUserProfiles.HideSelection = false;
            this.lvUserProfiles.Location = new System.Drawing.Point(3, 3);
            this.lvUserProfiles.MultiSelect = false;
            this.lvUserProfiles.Name = "lvUserProfiles";
            this.lvUserProfiles.OwnerDraw = true;
            this.lvUserProfiles.Size = new System.Drawing.Size(385, 116);
            this.lvUserProfiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvUserProfiles.TabIndex = 10;
            this.lvUserProfiles.UseCompatibleStateImageBehavior = false;
            this.lvUserProfiles.View = System.Windows.Forms.View.Details;
            this.lvUserProfiles.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvUserProfiles.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvUserProfiles.SelectedIndexChanged += new System.EventHandler(this.lvUserProfiles_SelectedIndexChanged);
            this.lvUserProfiles.Resize += new System.EventHandler(this.ListView_Resize);
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
            this.chWeight.Width = 63;
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
            this.chBlank.Width = 20;
            // 
            // tbEmailAddr
            // 
            this.tbEmailAddr.Location = new System.Drawing.Point(109, 230);
            this.tbEmailAddr.MaxLength = 100;
            this.tbEmailAddr.Name = "tbEmailAddr";
            this.tbEmailAddr.PlaceholderText = "Enter email address";
            this.tbEmailAddr.Size = new System.Drawing.Size(265, 23);
            this.tbEmailAddr.TabIndex = 72;
            this.tbEmailAddr.Enter += new System.EventHandler(this.UserProfiles_TooltipOnEnter);
            this.tbEmailAddr.Leave += new System.EventHandler(this.UserProfiles_TooltipOnLeave);
            this.tbEmailAddr.Validating += new System.ComponentModel.CancelEventHandler(this.UserProfiles_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 15);
            this.label2.TabIndex = 123;
            this.label2.Text = "Email Address:";
            // 
            // UserProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UserProfileControl";
            this.Size = new System.Drawing.Size(586, 531);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.pBase.ResumeLayout(false);
            this.pControl.ResumeLayout(false);
            this.pControl.PerformLayout();
            this.gbUserProfiles.ResumeLayout(false);
            this.pUserProfiles.ResumeLayout(false);
            this.pUserProfiles.PerformLayout();
            this.pWeightUomGroup.ResumeLayout(false);
            this.pWeightUomGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPowerThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pControl;
        private System.Windows.Forms.TextBox tbDescUsers;
        private System.Windows.Forms.GroupBox gbUserProfiles;
        private System.Windows.Forms.Panel pUserProfiles;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckedListBox clbCollectors;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pWeightUomGroup;
        private System.Windows.Forms.RadioButton rbKgs;
        private System.Windows.Forms.RadioButton rbLbs;
        private System.Windows.Forms.TextBox tbWeight;
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
        private System.Windows.Forms.ColumnHeader chDefault;
        private System.Windows.Forms.ColumnHeader chWeight;
        private System.Windows.Forms.ColumnHeader chThreshold;
        private System.Windows.Forms.ColumnHeader chBlank;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbEmailAddr;
    }
}
