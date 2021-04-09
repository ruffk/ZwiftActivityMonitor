
namespace ZwiftActivityMonitor
{
    partial class StatisticsControl
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
            this.gbCollectors = new System.Windows.Forms.GroupBox();
            this.pStatistics = new System.Windows.Forms.Panel();
            this.pFtpGroup = new System.Windows.Forms.Panel();
            this.rbFtpHide = new System.Windows.Forms.RadioButton();
            this.rbFtpWkg = new System.Windows.Forms.RadioButton();
            this.rbFtpWatts = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.pAvgMaxGroup = new System.Windows.Forms.Panel();
            this.rbAvgMaxHide = new System.Windows.Forms.RadioButton();
            this.rbAvgMaxWkg = new System.Windows.Forms.RadioButton();
            this.rbAvgMaxWatts = new System.Windows.Forms.RadioButton();
            this.pAvgGroup = new System.Windows.Forms.Panel();
            this.rbAvgHide = new System.Windows.Forms.RadioButton();
            this.rbAvgWkg = new System.Windows.Forms.RadioButton();
            this.rbAvgWatts = new System.Windows.Forms.RadioButton();
            this.btnStatsCancel = new System.Windows.Forms.Button();
            this.btnStatsSave = new System.Windows.Forms.Button();
            this.btnStatsEdit = new System.Windows.Forms.Button();
            this.tbDuration = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lvCollectors = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chAverage = new System.Windows.Forms.ColumnHeader();
            this.chAverageMax = new System.Windows.Forms.ColumnHeader();
            this.chFtp = new System.Windows.Forms.ColumnHeader();
            this.chBlank = new System.Windows.Forms.ColumnHeader();
            this.tbDescStatistics = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.pControl.SuspendLayout();
            this.gbCollectors.SuspendLayout();
            this.pStatistics.SuspendLayout();
            this.pFtpGroup.SuspendLayout();
            this.pAvgMaxGroup.SuspendLayout();
            this.pAvgGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBase
            // 
            this.pBase.Controls.Add(this.pControl);
            this.pBase.Size = new System.Drawing.Size(586, 476);
            // 
            // pControl
            // 
            this.pControl.Controls.Add(this.gbCollectors);
            this.pControl.Controls.Add(this.tbDescStatistics);
            this.pControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pControl.Location = new System.Drawing.Point(0, 0);
            this.pControl.Name = "pControl";
            this.pControl.Size = new System.Drawing.Size(586, 476);
            this.pControl.TabIndex = 1;
            // 
            // gbCollectors
            // 
            this.gbCollectors.Controls.Add(this.pStatistics);
            this.gbCollectors.Location = new System.Drawing.Point(20, 73);
            this.gbCollectors.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.gbCollectors.Name = "gbCollectors";
            this.gbCollectors.Size = new System.Drawing.Size(545, 391);
            this.gbCollectors.TabIndex = 12;
            this.gbCollectors.TabStop = false;
            this.gbCollectors.Text = "Moving Average Collectors";
            // 
            // pStatistics
            // 
            this.pStatistics.Controls.Add(this.pFtpGroup);
            this.pStatistics.Controls.Add(this.label4);
            this.pStatistics.Controls.Add(this.pAvgMaxGroup);
            this.pStatistics.Controls.Add(this.pAvgGroup);
            this.pStatistics.Controls.Add(this.btnStatsCancel);
            this.pStatistics.Controls.Add(this.btnStatsSave);
            this.pStatistics.Controls.Add(this.btnStatsEdit);
            this.pStatistics.Controls.Add(this.tbDuration);
            this.pStatistics.Controls.Add(this.label5);
            this.pStatistics.Controls.Add(this.label6);
            this.pStatistics.Controls.Add(this.lblDuration);
            this.pStatistics.Controls.Add(this.lvCollectors);
            this.pStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pStatistics.Location = new System.Drawing.Point(3, 19);
            this.pStatistics.Name = "pStatistics";
            this.pStatistics.Size = new System.Drawing.Size(539, 369);
            this.pStatistics.TabIndex = 0;
            // 
            // pFtpGroup
            // 
            this.pFtpGroup.Controls.Add(this.rbFtpHide);
            this.pFtpGroup.Controls.Add(this.rbFtpWkg);
            this.pFtpGroup.Controls.Add(this.rbFtpWatts);
            this.pFtpGroup.Location = new System.Drawing.Point(128, 330);
            this.pFtpGroup.Name = "pFtpGroup";
            this.pFtpGroup.Size = new System.Drawing.Size(187, 28);
            this.pFtpGroup.TabIndex = 40;
            // 
            // rbFtpHide
            // 
            this.rbFtpHide.AutoSize = true;
            this.rbFtpHide.Location = new System.Drawing.Point(125, 3);
            this.rbFtpHide.Name = "rbFtpHide";
            this.rbFtpHide.Size = new System.Drawing.Size(50, 19);
            this.rbFtpHide.TabIndex = 44;
            this.rbFtpHide.TabStop = true;
            this.rbFtpHide.Text = "Hide";
            this.rbFtpHide.UseVisualStyleBackColor = true;
            this.rbFtpHide.Enter += new System.EventHandler(this.Statistics_TooltipOnEnter);
            this.rbFtpHide.Leave += new System.EventHandler(this.Statistics_TooltipOnLeave);
            this.rbFtpHide.Validating += new System.ComponentModel.CancelEventHandler(this.Statistics_Validating);
            // 
            // rbFtpWkg
            // 
            this.rbFtpWkg.AutoSize = true;
            this.rbFtpWkg.Location = new System.Drawing.Point(64, 3);
            this.rbFtpWkg.Name = "rbFtpWkg";
            this.rbFtpWkg.Size = new System.Drawing.Size(55, 19);
            this.rbFtpWkg.TabIndex = 42;
            this.rbFtpWkg.TabStop = true;
            this.rbFtpWkg.Text = "W/Kg";
            this.rbFtpWkg.UseVisualStyleBackColor = true;
            this.rbFtpWkg.Enter += new System.EventHandler(this.Statistics_TooltipOnEnter);
            this.rbFtpWkg.Leave += new System.EventHandler(this.Statistics_TooltipOnLeave);
            this.rbFtpWkg.Validating += new System.ComponentModel.CancelEventHandler(this.Statistics_Validating);
            // 
            // rbFtpWatts
            // 
            this.rbFtpWatts.AutoSize = true;
            this.rbFtpWatts.Location = new System.Drawing.Point(3, 3);
            this.rbFtpWatts.Name = "rbFtpWatts";
            this.rbFtpWatts.Size = new System.Drawing.Size(55, 19);
            this.rbFtpWatts.TabIndex = 41;
            this.rbFtpWatts.TabStop = true;
            this.rbFtpWatts.Text = "Watts";
            this.rbFtpWatts.UseVisualStyleBackColor = true;
            this.rbFtpWatts.Enter += new System.EventHandler(this.Statistics_TooltipOnEnter);
            this.rbFtpWatts.Leave += new System.EventHandler(this.Statistics_TooltipOnLeave);
            this.rbFtpWatts.Validating += new System.ComponentModel.CancelEventHandler(this.Statistics_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 335);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 15);
            this.label4.TabIndex = 19;
            this.label4.Text = "FTP Units:";
            // 
            // pAvgMaxGroup
            // 
            this.pAvgMaxGroup.Controls.Add(this.rbAvgMaxHide);
            this.pAvgMaxGroup.Controls.Add(this.rbAvgMaxWkg);
            this.pAvgMaxGroup.Controls.Add(this.rbAvgMaxWatts);
            this.pAvgMaxGroup.Location = new System.Drawing.Point(128, 296);
            this.pAvgMaxGroup.Name = "pAvgMaxGroup";
            this.pAvgMaxGroup.Size = new System.Drawing.Size(187, 28);
            this.pAvgMaxGroup.TabIndex = 30;
            // 
            // rbAvgMaxHide
            // 
            this.rbAvgMaxHide.AutoSize = true;
            this.rbAvgMaxHide.Location = new System.Drawing.Point(125, 3);
            this.rbAvgMaxHide.Name = "rbAvgMaxHide";
            this.rbAvgMaxHide.Size = new System.Drawing.Size(50, 19);
            this.rbAvgMaxHide.TabIndex = 34;
            this.rbAvgMaxHide.TabStop = true;
            this.rbAvgMaxHide.Text = "Hide";
            this.rbAvgMaxHide.UseVisualStyleBackColor = true;
            this.rbAvgMaxHide.Enter += new System.EventHandler(this.Statistics_TooltipOnEnter);
            this.rbAvgMaxHide.Leave += new System.EventHandler(this.Statistics_TooltipOnLeave);
            this.rbAvgMaxHide.Validating += new System.ComponentModel.CancelEventHandler(this.Statistics_Validating);
            // 
            // rbAvgMaxWkg
            // 
            this.rbAvgMaxWkg.AutoSize = true;
            this.rbAvgMaxWkg.Location = new System.Drawing.Point(64, 3);
            this.rbAvgMaxWkg.Name = "rbAvgMaxWkg";
            this.rbAvgMaxWkg.Size = new System.Drawing.Size(55, 19);
            this.rbAvgMaxWkg.TabIndex = 32;
            this.rbAvgMaxWkg.TabStop = true;
            this.rbAvgMaxWkg.Text = "W/Kg";
            this.rbAvgMaxWkg.UseVisualStyleBackColor = true;
            this.rbAvgMaxWkg.Enter += new System.EventHandler(this.Statistics_TooltipOnEnter);
            this.rbAvgMaxWkg.Leave += new System.EventHandler(this.Statistics_TooltipOnLeave);
            this.rbAvgMaxWkg.Validating += new System.ComponentModel.CancelEventHandler(this.Statistics_Validating);
            // 
            // rbAvgMaxWatts
            // 
            this.rbAvgMaxWatts.AutoSize = true;
            this.rbAvgMaxWatts.Location = new System.Drawing.Point(3, 3);
            this.rbAvgMaxWatts.Name = "rbAvgMaxWatts";
            this.rbAvgMaxWatts.Size = new System.Drawing.Size(55, 19);
            this.rbAvgMaxWatts.TabIndex = 31;
            this.rbAvgMaxWatts.TabStop = true;
            this.rbAvgMaxWatts.Text = "Watts";
            this.rbAvgMaxWatts.UseVisualStyleBackColor = true;
            this.rbAvgMaxWatts.Enter += new System.EventHandler(this.Statistics_TooltipOnEnter);
            this.rbAvgMaxWatts.Leave += new System.EventHandler(this.Statistics_TooltipOnLeave);
            this.rbAvgMaxWatts.Validating += new System.ComponentModel.CancelEventHandler(this.Statistics_Validating);
            // 
            // pAvgGroup
            // 
            this.pAvgGroup.Controls.Add(this.rbAvgHide);
            this.pAvgGroup.Controls.Add(this.rbAvgWkg);
            this.pAvgGroup.Controls.Add(this.rbAvgWatts);
            this.pAvgGroup.Location = new System.Drawing.Point(128, 262);
            this.pAvgGroup.Name = "pAvgGroup";
            this.pAvgGroup.Size = new System.Drawing.Size(187, 28);
            this.pAvgGroup.TabIndex = 20;
            // 
            // rbAvgHide
            // 
            this.rbAvgHide.AutoSize = true;
            this.rbAvgHide.Location = new System.Drawing.Point(125, 3);
            this.rbAvgHide.Name = "rbAvgHide";
            this.rbAvgHide.Size = new System.Drawing.Size(50, 19);
            this.rbAvgHide.TabIndex = 24;
            this.rbAvgHide.TabStop = true;
            this.rbAvgHide.Text = "Hide";
            this.rbAvgHide.UseVisualStyleBackColor = true;
            this.rbAvgHide.Enter += new System.EventHandler(this.Statistics_TooltipOnEnter);
            this.rbAvgHide.Leave += new System.EventHandler(this.Statistics_TooltipOnLeave);
            this.rbAvgHide.Validating += new System.ComponentModel.CancelEventHandler(this.Statistics_Validating);
            // 
            // rbAvgWkg
            // 
            this.rbAvgWkg.AutoSize = true;
            this.rbAvgWkg.Location = new System.Drawing.Point(64, 3);
            this.rbAvgWkg.Name = "rbAvgWkg";
            this.rbAvgWkg.Size = new System.Drawing.Size(55, 19);
            this.rbAvgWkg.TabIndex = 22;
            this.rbAvgWkg.TabStop = true;
            this.rbAvgWkg.Text = "W/Kg";
            this.rbAvgWkg.UseVisualStyleBackColor = true;
            this.rbAvgWkg.Enter += new System.EventHandler(this.Statistics_TooltipOnEnter);
            this.rbAvgWkg.Leave += new System.EventHandler(this.Statistics_TooltipOnLeave);
            this.rbAvgWkg.Validating += new System.ComponentModel.CancelEventHandler(this.Statistics_Validating);
            // 
            // rbAvgWatts
            // 
            this.rbAvgWatts.AutoSize = true;
            this.rbAvgWatts.Location = new System.Drawing.Point(3, 3);
            this.rbAvgWatts.Name = "rbAvgWatts";
            this.rbAvgWatts.Size = new System.Drawing.Size(55, 19);
            this.rbAvgWatts.TabIndex = 21;
            this.rbAvgWatts.TabStop = true;
            this.rbAvgWatts.Text = "Watts";
            this.rbAvgWatts.UseVisualStyleBackColor = true;
            this.rbAvgWatts.Enter += new System.EventHandler(this.Statistics_TooltipOnEnter);
            this.rbAvgWatts.Leave += new System.EventHandler(this.Statistics_TooltipOnLeave);
            this.rbAvgWatts.Validating += new System.ComponentModel.CancelEventHandler(this.Statistics_Validating);
            // 
            // btnStatsCancel
            // 
            this.btnStatsCancel.Location = new System.Drawing.Point(435, 80);
            this.btnStatsCancel.Name = "btnStatsCancel";
            this.btnStatsCancel.Size = new System.Drawing.Size(89, 28);
            this.btnStatsCancel.TabIndex = 70;
            this.btnStatsCancel.Text = "Cancel";
            this.btnStatsCancel.UseVisualStyleBackColor = true;
            this.btnStatsCancel.Click += new System.EventHandler(this.btnStatsCancel_Click);
            // 
            // btnStatsSave
            // 
            this.btnStatsSave.Location = new System.Drawing.Point(435, 46);
            this.btnStatsSave.Name = "btnStatsSave";
            this.btnStatsSave.Size = new System.Drawing.Size(89, 28);
            this.btnStatsSave.TabIndex = 60;
            this.btnStatsSave.Text = "Save";
            this.btnStatsSave.UseVisualStyleBackColor = true;
            this.btnStatsSave.Click += new System.EventHandler(this.btnStatsSave_Click);
            // 
            // btnStatsEdit
            // 
            this.btnStatsEdit.Location = new System.Drawing.Point(435, 12);
            this.btnStatsEdit.Name = "btnStatsEdit";
            this.btnStatsEdit.Size = new System.Drawing.Size(89, 28);
            this.btnStatsEdit.TabIndex = 50;
            this.btnStatsEdit.Text = "Edit";
            this.btnStatsEdit.UseVisualStyleBackColor = true;
            this.btnStatsEdit.Click += new System.EventHandler(this.btnStatsEdit_Click);
            // 
            // tbDuration
            // 
            this.tbDuration.BackColor = System.Drawing.SystemColors.Control;
            this.tbDuration.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDuration.Location = new System.Drawing.Point(127, 239);
            this.tbDuration.Name = "tbDuration";
            this.tbDuration.PlaceholderText = "Duration";
            this.tbDuration.ReadOnly = true;
            this.tbDuration.Size = new System.Drawing.Size(59, 16);
            this.tbDuration.TabIndex = 0;
            this.tbDuration.TabStop = false;
            this.tbDuration.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 299);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Average (max) Units:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 267);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 15);
            this.label6.TabIndex = 3;
            this.label6.Text = "Average Units:";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(47, 239);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(56, 15);
            this.lblDuration.TabIndex = 2;
            this.lblDuration.Text = "Duration:";
            // 
            // lvCollectors
            // 
            this.lvCollectors.BackColor = System.Drawing.SystemColors.Control;
            this.lvCollectors.CausesValidation = false;
            this.lvCollectors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chAverage,
            this.chAverageMax,
            this.chFtp,
            this.chBlank});
            this.lvCollectors.FullRowSelect = true;
            this.lvCollectors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCollectors.HideSelection = false;
            this.lvCollectors.Location = new System.Drawing.Point(3, 3);
            this.lvCollectors.MultiSelect = false;
            this.lvCollectors.Name = "lvCollectors";
            this.lvCollectors.OwnerDraw = true;
            this.lvCollectors.Size = new System.Drawing.Size(312, 225);
            this.lvCollectors.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvCollectors.TabIndex = 10;
            this.lvCollectors.UseCompatibleStateImageBehavior = false;
            this.lvCollectors.View = System.Windows.Forms.View.Details;
            this.lvCollectors.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvCollectors.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvCollectors.SelectedIndexChanged += new System.EventHandler(this.lvCollectors_SelectedIndexChanged);
            this.lvCollectors.Resize += new System.EventHandler(this.ListView_Resize);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 50;
            // 
            // chAverage
            // 
            this.chAverage.Text = "Average";
            this.chAverage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chAverage.Width = 55;
            // 
            // chAverageMax
            // 
            this.chAverageMax.Text = "Average (Max)";
            this.chAverageMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chAverageMax.Width = 95;
            // 
            // chFtp
            // 
            this.chFtp.Text = "FTP";
            this.chFtp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chFtp.Width = 55;
            // 
            // chBlank
            // 
            this.chBlank.Text = "";
            this.chBlank.Width = 52;
            // 
            // tbDescStatistics
            // 
            this.tbDescStatistics.BackColor = System.Drawing.SystemColors.Control;
            this.tbDescStatistics.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDescStatistics.Location = new System.Drawing.Point(20, 20);
            this.tbDescStatistics.Multiline = true;
            this.tbDescStatistics.Name = "tbDescStatistics";
            this.tbDescStatistics.ReadOnly = true;
            this.tbDescStatistics.Size = new System.Drawing.Size(545, 40);
            this.tbDescStatistics.TabIndex = 11;
            this.tbDescStatistics.TabStop = false;
            this.tbDescStatistics.Text = "Configure moving average collectors.  This allows customization of how the units " +
    "are displayed for each time interval.";
            this.tbDescStatistics.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // StatisticsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "StatisticsControl";
            this.Size = new System.Drawing.Size(586, 498);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.pBase.ResumeLayout(false);
            this.pControl.ResumeLayout(false);
            this.pControl.PerformLayout();
            this.gbCollectors.ResumeLayout(false);
            this.pStatistics.ResumeLayout(false);
            this.pStatistics.PerformLayout();
            this.pFtpGroup.ResumeLayout(false);
            this.pFtpGroup.PerformLayout();
            this.pAvgMaxGroup.ResumeLayout(false);
            this.pAvgMaxGroup.PerformLayout();
            this.pAvgGroup.ResumeLayout(false);
            this.pAvgGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pControl;
        private System.Windows.Forms.GroupBox gbCollectors;
        private System.Windows.Forms.Panel pStatistics;
        private System.Windows.Forms.Panel pFtpGroup;
        private System.Windows.Forms.RadioButton rbFtpHide;
        private System.Windows.Forms.RadioButton rbFtpWkg;
        private System.Windows.Forms.RadioButton rbFtpWatts;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pAvgMaxGroup;
        private System.Windows.Forms.RadioButton rbAvgMaxHide;
        private System.Windows.Forms.RadioButton rbAvgMaxWkg;
        private System.Windows.Forms.RadioButton rbAvgMaxWatts;
        private System.Windows.Forms.Panel pAvgGroup;
        private System.Windows.Forms.RadioButton rbAvgHide;
        private System.Windows.Forms.RadioButton rbAvgWkg;
        private System.Windows.Forms.RadioButton rbAvgWatts;
        private System.Windows.Forms.Button btnStatsCancel;
        private System.Windows.Forms.Button btnStatsSave;
        private System.Windows.Forms.Button btnStatsEdit;
        private System.Windows.Forms.TextBox tbDuration;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.ListView lvCollectors;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chAverage;
        private System.Windows.Forms.ColumnHeader chAverageMax;
        private System.Windows.Forms.ColumnHeader chFtp;
        private System.Windows.Forms.ColumnHeader chBlank;
        private System.Windows.Forms.TextBox tbDescStatistics;
    }
}
