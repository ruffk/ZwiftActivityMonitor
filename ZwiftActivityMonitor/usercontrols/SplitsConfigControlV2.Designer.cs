
namespace ZwiftActivityMonitor
{
    partial class SplitsConfigControlV2
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tbDescSystem = new System.Windows.Forms.TextBox();
            this.gbSplits = new System.Windows.Forms.GroupBox();
            this.pSplits = new System.Windows.Forms.Panel();
            this.btnSplitEdit = new System.Windows.Forms.Button();
            this.gbSplitGoals = new System.Windows.Forms.GroupBox();
            this.ckbCustomized = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvSplits = new ZwiftActivityMonitor.SplitsConfigControlV2.DataGridViewExtended();
            this.lblGoalSpeed = new System.Windows.Forms.Label();
            this.lblGoalSpeedValue = new System.Windows.Forms.Label();
            this.dtpGoalTime = new System.Windows.Forms.DateTimePicker();
            this.tbGoalDistance = new System.Windows.Forms.TextBox();
            this.lblGoalDistance = new System.Windows.Forms.Label();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnEditSettings = new System.Windows.Forms.Button();
            this.lblGoalTime = new System.Windows.Forms.Label();
            this.ckbCalculateGoal = new System.Windows.Forms.CheckBox();
            this.cbSplitUom = new System.Windows.Forms.ComboBox();
            this.tbSplitDistance = new System.Windows.Forms.TextBox();
            this.lblSplitsEvery = new System.Windows.Forms.Label();
            this.ckbShowSplits = new System.Windows.Forms.CheckBox();
            this.lblGoalDistanceUom = new System.Windows.Forms.Label();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDeleteRow = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.gbSplits.SuspendLayout();
            this.pSplits.SuspendLayout();
            this.gbSplitGoals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSplits)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBase
            // 
            this.pBase.Controls.Add(this.gbSplits);
            this.pBase.Controls.Add(this.tbDescSystem);
            this.pBase.Size = new System.Drawing.Size(587, 518);
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
            this.tbDescSystem.TabIndex = 4;
            this.tbDescSystem.TabStop = false;
            this.tbDescSystem.Text = "Configure splits display options.  Splits will appear whenever the split distance" +
    " is traveled.  Set an optional goal to see if your pace is on-track.  Customize " +
    "goals if required.";
            this.tbDescSystem.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // gbSplits
            // 
            this.gbSplits.Controls.Add(this.pSplits);
            this.gbSplits.Location = new System.Drawing.Point(20, 73);
            this.gbSplits.Name = "gbSplits";
            this.gbSplits.Size = new System.Drawing.Size(548, 442);
            this.gbSplits.TabIndex = 5;
            this.gbSplits.TabStop = false;
            this.gbSplits.Text = "Splits";
            // 
            // pSplits
            // 
            this.pSplits.Controls.Add(this.btnSplitEdit);
            this.pSplits.Controls.Add(this.gbSplitGoals);
            this.pSplits.Controls.Add(this.lblGoalSpeed);
            this.pSplits.Controls.Add(this.lblGoalSpeedValue);
            this.pSplits.Controls.Add(this.dtpGoalTime);
            this.pSplits.Controls.Add(this.tbGoalDistance);
            this.pSplits.Controls.Add(this.lblGoalDistance);
            this.pSplits.Controls.Add(this.btnCancelSettings);
            this.pSplits.Controls.Add(this.btnSaveSettings);
            this.pSplits.Controls.Add(this.btnEditSettings);
            this.pSplits.Controls.Add(this.lblGoalTime);
            this.pSplits.Controls.Add(this.ckbCalculateGoal);
            this.pSplits.Controls.Add(this.cbSplitUom);
            this.pSplits.Controls.Add(this.tbSplitDistance);
            this.pSplits.Controls.Add(this.lblSplitsEvery);
            this.pSplits.Controls.Add(this.ckbShowSplits);
            this.pSplits.Controls.Add(this.lblGoalDistanceUom);
            this.pSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplits.Location = new System.Drawing.Point(3, 19);
            this.pSplits.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pSplits.Name = "pSplits";
            this.pSplits.Size = new System.Drawing.Size(542, 420);
            this.pSplits.TabIndex = 0;
            // 
            // btnSplitEdit
            // 
            this.btnSplitEdit.Location = new System.Drawing.Point(439, 120);
            this.btnSplitEdit.Name = "btnSplitEdit";
            this.btnSplitEdit.Size = new System.Drawing.Size(89, 28);
            this.btnSplitEdit.TabIndex = 140;
            this.btnSplitEdit.Text = "Edit Splits";
            this.btnSplitEdit.UseVisualStyleBackColor = true;
            this.btnSplitEdit.Click += new System.EventHandler(this.btnSplitEdit_Click);
            // 
            // gbSplitGoals
            // 
            this.gbSplitGoals.Controls.Add(this.ckbCustomized);
            this.gbSplitGoals.Controls.Add(this.label1);
            this.gbSplitGoals.Controls.Add(this.dgvSplits);
            this.gbSplitGoals.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbSplitGoals.Location = new System.Drawing.Point(34, 157);
            this.gbSplitGoals.Name = "gbSplitGoals";
            this.gbSplitGoals.Size = new System.Drawing.Size(494, 241);
            this.gbSplitGoals.TabIndex = 80;
            this.gbSplitGoals.TabStop = false;
            this.gbSplitGoals.Text = "Split Goals";
            // 
            // ckbCustomized
            // 
            this.ckbCustomized.AutoCheck = false;
            this.ckbCustomized.AutoSize = true;
            this.ckbCustomized.Location = new System.Drawing.Point(394, 216);
            this.ckbCustomized.Name = "ckbCustomized";
            this.ckbCustomized.Size = new System.Drawing.Size(89, 19);
            this.ckbCustomized.TabIndex = 0;
            this.ckbCustomized.TabStop = false;
            this.ckbCustomized.Text = "Customized";
            this.ckbCustomized.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 15);
            this.label1.TabIndex = 141;
            this.label1.Text = "* Right-click row to delete goal.";
            // 
            // dgvSplits
            // 
            this.dgvSplits.AllowUserToDeleteRows = false;
            this.dgvSplits.AllowUserToResizeColumns = false;
            this.dgvSplits.AllowUserToResizeRows = false;
            this.dgvSplits.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvSplits.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSplits.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSplits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSplits.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvSplits.EnableHeadersVisualStyles = false;
            this.dgvSplits.Location = new System.Drawing.Point(14, 22);
            this.dgvSplits.Logger = null;
            this.dgvSplits.MultiSelect = false;
            this.dgvSplits.Name = "dgvSplits";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSplits.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvSplits.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSplits.RowTemplate.Height = 25;
            this.dgvSplits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvSplits.ShowCellToolTips = false;
            this.dgvSplits.Size = new System.Drawing.Size(463, 195);
            this.dgvSplits.TabIndex = 90;
            this.dgvSplits.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSplits_CellMouseClick);
            this.dgvSplits.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSplits_CellValidated);
            this.dgvSplits.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvSplits_CellValidating);
            this.dgvSplits.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSplits_RowEnter);
            this.dgvSplits.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSplits_RowLeave);
            this.dgvSplits.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSplits_RowValidated);
            this.dgvSplits.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvSplits_RowValidating);
            // 
            // lblGoalSpeed
            // 
            this.lblGoalSpeed.Location = new System.Drawing.Point(12, 130);
            this.lblGoalSpeed.Name = "lblGoalSpeed";
            this.lblGoalSpeed.Size = new System.Drawing.Size(108, 15);
            this.lblGoalSpeed.TabIndex = 95;
            this.lblGoalSpeed.Text = "Goal Speed km/h:";
            this.lblGoalSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblGoalSpeedValue
            // 
            this.lblGoalSpeedValue.AutoSize = true;
            this.lblGoalSpeedValue.Location = new System.Drawing.Point(122, 130);
            this.lblGoalSpeedValue.Name = "lblGoalSpeedValue";
            this.lblGoalSpeedValue.Size = new System.Drawing.Size(28, 15);
            this.lblGoalSpeedValue.TabIndex = 96;
            this.lblGoalSpeedValue.Text = "88.8";
            // 
            // dtpGoalTime
            // 
            this.dtpGoalTime.CustomFormat = "HH:mm:ss";
            this.dtpGoalTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGoalTime.Location = new System.Drawing.Point(125, 100);
            this.dtpGoalTime.Name = "dtpGoalTime";
            this.dtpGoalTime.ShowUpDown = true;
            this.dtpGoalTime.Size = new System.Drawing.Size(77, 23);
            this.dtpGoalTime.TabIndex = 125;
            this.dtpGoalTime.Value = new System.DateTime(2021, 5, 10, 23, 59, 59, 0);
            this.dtpGoalTime.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.dtpGoalTime.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.dtpGoalTime.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // tbGoalDistance
            // 
            this.tbGoalDistance.Location = new System.Drawing.Point(270, 100);
            this.tbGoalDistance.MaxLength = 5;
            this.tbGoalDistance.Name = "tbGoalDistance";
            this.tbGoalDistance.Size = new System.Drawing.Size(39, 23);
            this.tbGoalDistance.TabIndex = 75;
            this.tbGoalDistance.Text = "88888";
            this.tbGoalDistance.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbGoalDistance.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbGoalDistance.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblGoalDistance
            // 
            this.lblGoalDistance.AutoSize = true;
            this.lblGoalDistance.Location = new System.Drawing.Point(209, 103);
            this.lblGoalDistance.Name = "lblGoalDistance";
            this.lblGoalDistance.Size = new System.Drawing.Size(55, 15);
            this.lblGoalDistance.TabIndex = 112;
            this.lblGoalDistance.Text = "Distance:";
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.Location = new System.Drawing.Point(439, 83);
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(89, 28);
            this.btnCancelSettings.TabIndex = 130;
            this.btnCancelSettings.Text = "Cancel";
            this.btnCancelSettings.UseVisualStyleBackColor = true;
            this.btnCancelSettings.Click += new System.EventHandler(this.btnCancelSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(439, 47);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(89, 28);
            this.btnSaveSettings.TabIndex = 120;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnEditSettings
            // 
            this.btnEditSettings.Location = new System.Drawing.Point(439, 11);
            this.btnEditSettings.Name = "btnEditSettings";
            this.btnEditSettings.Size = new System.Drawing.Size(89, 28);
            this.btnEditSettings.TabIndex = 110;
            this.btnEditSettings.Text = "Edit";
            this.btnEditSettings.UseVisualStyleBackColor = true;
            this.btnEditSettings.Click += new System.EventHandler(this.btnEditSettings_Click);
            // 
            // lblGoalTime
            // 
            this.lblGoalTime.AutoSize = true;
            this.lblGoalTime.Location = new System.Drawing.Point(57, 103);
            this.lblGoalTime.Name = "lblGoalTime";
            this.lblGoalTime.Size = new System.Drawing.Size(63, 15);
            this.lblGoalTime.TabIndex = 6;
            this.lblGoalTime.Text = "Goal Time:";
            // 
            // ckbCalculateGoal
            // 
            this.ckbCalculateGoal.AutoSize = true;
            this.ckbCalculateGoal.Location = new System.Drawing.Point(34, 76);
            this.ckbCalculateGoal.Name = "ckbCalculateGoal";
            this.ckbCalculateGoal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ckbCalculateGoal.Size = new System.Drawing.Size(133, 19);
            this.ckbCalculateGoal.TabIndex = 40;
            this.ckbCalculateGoal.Text = "Calculate Split Goals";
            this.ckbCalculateGoal.UseVisualStyleBackColor = true;
            this.ckbCalculateGoal.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.ckbCalculateGoal.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.ckbCalculateGoal.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // cbSplitUom
            // 
            this.cbSplitUom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSplitUom.FormattingEnabled = true;
            this.cbSplitUom.Location = new System.Drawing.Point(161, 41);
            this.cbSplitUom.Name = "cbSplitUom";
            this.cbSplitUom.Size = new System.Drawing.Size(47, 23);
            this.cbSplitUom.TabIndex = 30;
            this.cbSplitUom.SelectionChangeCommitted += new System.EventHandler(this.cbSplitUom_SelectionChangeCommitted);
            this.cbSplitUom.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.cbSplitUom.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.cbSplitUom.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // tbSplitDistance
            // 
            this.tbSplitDistance.Location = new System.Drawing.Point(125, 41);
            this.tbSplitDistance.MaxLength = 3;
            this.tbSplitDistance.Name = "tbSplitDistance";
            this.tbSplitDistance.Size = new System.Drawing.Size(26, 23);
            this.tbSplitDistance.TabIndex = 20;
            this.tbSplitDistance.Text = "888";
            this.tbSplitDistance.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.tbSplitDistance.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.tbSplitDistance.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblSplitsEvery
            // 
            this.lblSplitsEvery.AutoSize = true;
            this.lblSplitsEvery.Location = new System.Drawing.Point(51, 44);
            this.lblSplitsEvery.Name = "lblSplitsEvery";
            this.lblSplitsEvery.Size = new System.Drawing.Size(69, 15);
            this.lblSplitsEvery.TabIndex = 2;
            this.lblSplitsEvery.Text = "Splits Every:";
            // 
            // ckbShowSplits
            // 
            this.ckbShowSplits.AutoSize = true;
            this.ckbShowSplits.Location = new System.Drawing.Point(34, 17);
            this.ckbShowSplits.Name = "ckbShowSplits";
            this.ckbShowSplits.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ckbShowSplits.Size = new System.Drawing.Size(86, 19);
            this.ckbShowSplits.TabIndex = 10;
            this.ckbShowSplits.Text = "Show Splits";
            this.ckbShowSplits.UseVisualStyleBackColor = true;
            this.ckbShowSplits.Enter += new System.EventHandler(this.SystemSettings_TooltipOnEnter);
            this.ckbShowSplits.Leave += new System.EventHandler(this.SystemSettings_TooltipOnLeave);
            this.ckbShowSplits.Validating += new System.ComponentModel.CancelEventHandler(this.SystemSettings_Validating);
            // 
            // lblGoalDistanceUom
            // 
            this.lblGoalDistanceUom.AutoSize = true;
            this.lblGoalDistanceUom.Location = new System.Drawing.Point(309, 103);
            this.lblGoalDistanceUom.Name = "lblGoalDistanceUom";
            this.lblGoalDistanceUom.Size = new System.Drawing.Size(24, 15);
            this.lblGoalDistanceUom.TabIndex = 114;
            this.lblGoalDistanceUom.Text = "km";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDeleteRow});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(134, 26);
            // 
            // tsmiDeleteRow
            // 
            this.tsmiDeleteRow.Name = "tsmiDeleteRow";
            this.tsmiDeleteRow.Size = new System.Drawing.Size(133, 22);
            this.tsmiDeleteRow.Text = "Delete Row";
            this.tsmiDeleteRow.Click += new System.EventHandler(this.tsmiDeleteRow_Click);
            // 
            // SplitsConfigControlNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SplitsConfigControlNew";
            this.Size = new System.Drawing.Size(587, 540);
            this.Controls.SetChildIndex(this.pBase, 0);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.pBase.ResumeLayout(false);
            this.pBase.PerformLayout();
            this.gbSplits.ResumeLayout(false);
            this.pSplits.ResumeLayout(false);
            this.pSplits.PerformLayout();
            this.gbSplitGoals.ResumeLayout(false);
            this.gbSplitGoals.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSplits)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescSystem;
        private System.Windows.Forms.GroupBox gbSplits;
        private System.Windows.Forms.Panel pSplits;
        private System.Windows.Forms.Label lblSplitsEvery;
        private System.Windows.Forms.CheckBox ckbShowSplits;
        private System.Windows.Forms.TextBox tbSplitDistance;
        private System.Windows.Forms.Label lblGoalTime;
        private System.Windows.Forms.CheckBox ckbCalculateGoal;
        private System.Windows.Forms.ComboBox cbSplitUom;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnEditSettings;
        private System.Windows.Forms.Label lblGoalSpeedValue;
        private System.Windows.Forms.Label lblGoalSpeed;
        private System.Windows.Forms.Label lblGoalDistanceUom;
        private System.Windows.Forms.TextBox tbGoalDistance;
        private System.Windows.Forms.Label lblGoalDistance;
        private System.Windows.Forms.DateTimePicker dtpGoalTime;
        private System.Windows.Forms.Button btnSplitEdit;
        private System.Windows.Forms.GroupBox gbSplitGoals;
        //private System.Windows.Forms.DataGridView dgvSplits;
        private DataGridViewExtended dgvSplits;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteRow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckbCustomized;
    }
}
