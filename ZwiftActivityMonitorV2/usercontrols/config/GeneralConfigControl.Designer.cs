
namespace ZwiftActivityMonitorV2
{
    partial class GeneralConfigControl
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
            this.tbDescSystem = new System.Windows.Forms.TextBox();
            this.gbGeneralSettings = new ZwiftActivityMonitorV2.GroupBoxEx();
            this.pSystemSettings = new System.Windows.Forms.Panel();
            this.gbHotKeys = new ZwiftActivityMonitorV2.GroupBoxEx();
            this.label5 = new System.Windows.Forms.Label();
            this.tbResetLapsKeys = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbNewLapKeys = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSplitViewKeys = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLapViewKeys = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbActivityViewKeys = new System.Windows.Forms.TextBox();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnEditSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.pBase.SuspendLayout();
            this.pControl.SuspendLayout();
            this.gbGeneralSettings.SuspendLayout();
            this.pSystemSettings.SuspendLayout();
            this.gbHotKeys.SuspendLayout();
            this.SuspendLayout();
            // 
            // pBase
            // 
            this.pBase.Controls.Add(this.pControl);
            this.pBase.Size = new System.Drawing.Size(587, 518);
            // 
            // pControl
            // 
            this.pControl.Controls.Add(this.tbDescSystem);
            this.pControl.Controls.Add(this.gbGeneralSettings);
            this.pControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pControl.Location = new System.Drawing.Point(0, 0);
            this.pControl.Name = "pControl";
            this.pControl.Size = new System.Drawing.Size(587, 518);
            this.pControl.TabIndex = 0;
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
            this.tbDescSystem.Text = "Setup hotkeys to jump between views or start / reset laps.  Click on a key to con" +
    "figure and press the desired key combination.";
            this.tbDescSystem.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // gbGeneralSettings
            // 
            this.gbGeneralSettings.BorderColor = System.Drawing.SystemColors.ControlLight;
            this.gbGeneralSettings.Controls.Add(this.pSystemSettings);
            this.gbGeneralSettings.Location = new System.Drawing.Point(20, 73);
            this.gbGeneralSettings.Name = "gbGeneralSettings";
            this.gbGeneralSettings.Size = new System.Drawing.Size(548, 434);
            this.gbGeneralSettings.TabIndex = 4;
            this.gbGeneralSettings.TabStop = false;
            this.gbGeneralSettings.Text = "General Settings";
            this.gbGeneralSettings.TextColor = System.Drawing.Color.Empty;
            // 
            // pSystemSettings
            // 
            this.pSystemSettings.Controls.Add(this.gbHotKeys);
            this.pSystemSettings.Controls.Add(this.btnCancelSettings);
            this.pSystemSettings.Controls.Add(this.btnSaveSettings);
            this.pSystemSettings.Controls.Add(this.btnEditSettings);
            this.pSystemSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSystemSettings.Location = new System.Drawing.Point(3, 19);
            this.pSystemSettings.Name = "pSystemSettings";
            this.pSystemSettings.Size = new System.Drawing.Size(542, 412);
            this.pSystemSettings.TabIndex = 3;
            this.pSystemSettings.TabStop = true;
            // 
            // gbHotKeys
            // 
            this.gbHotKeys.BorderColor = System.Drawing.SystemColors.ControlLight;
            this.gbHotKeys.Controls.Add(this.label5);
            this.gbHotKeys.Controls.Add(this.tbResetLapsKeys);
            this.gbHotKeys.Controls.Add(this.label4);
            this.gbHotKeys.Controls.Add(this.tbNewLapKeys);
            this.gbHotKeys.Controls.Add(this.label3);
            this.gbHotKeys.Controls.Add(this.tbSplitViewKeys);
            this.gbHotKeys.Controls.Add(this.label2);
            this.gbHotKeys.Controls.Add(this.tbLapViewKeys);
            this.gbHotKeys.Controls.Add(this.label1);
            this.gbHotKeys.Controls.Add(this.tbActivityViewKeys);
            this.gbHotKeys.Location = new System.Drawing.Point(12, 10);
            this.gbHotKeys.Name = "gbHotKeys";
            this.gbHotKeys.Size = new System.Drawing.Size(361, 207);
            this.gbHotKeys.TabIndex = 81;
            this.gbHotKeys.TabStop = false;
            this.gbHotKeys.Text = "Hot Keys";
            this.gbHotKeys.TextColor = System.Drawing.Color.Empty;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Reset Laps:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbResetLapsKeys
            // 
            this.tbResetLapsKeys.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbResetLapsKeys.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbResetLapsKeys.Location = new System.Drawing.Point(90, 157);
            this.tbResetLapsKeys.Name = "tbResetLapsKeys";
            this.tbResetLapsKeys.Size = new System.Drawing.Size(241, 27);
            this.tbResetLapsKeys.TabIndex = 0;
            this.tbResetLapsKeys.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "New Lap:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbNewLapKeys
            // 
            this.tbNewLapKeys.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbNewLapKeys.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbNewLapKeys.Location = new System.Drawing.Point(90, 124);
            this.tbNewLapKeys.Name = "tbNewLapKeys";
            this.tbNewLapKeys.Size = new System.Drawing.Size(241, 27);
            this.tbNewLapKeys.TabIndex = 0;
            this.tbNewLapKeys.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Split View:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSplitViewKeys
            // 
            this.tbSplitViewKeys.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSplitViewKeys.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbSplitViewKeys.Location = new System.Drawing.Point(90, 58);
            this.tbSplitViewKeys.Name = "tbSplitViewKeys";
            this.tbSplitViewKeys.Size = new System.Drawing.Size(241, 27);
            this.tbSplitViewKeys.TabIndex = 0;
            this.tbSplitViewKeys.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Lap View:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbLapViewKeys
            // 
            this.tbLapViewKeys.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbLapViewKeys.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbLapViewKeys.Location = new System.Drawing.Point(90, 91);
            this.tbLapViewKeys.Name = "tbLapViewKeys";
            this.tbLapViewKeys.Size = new System.Drawing.Size(241, 27);
            this.tbLapViewKeys.TabIndex = 0;
            this.tbLapViewKeys.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Activity View:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbActivityViewKeys
            // 
            this.tbActivityViewKeys.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbActivityViewKeys.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbActivityViewKeys.Location = new System.Drawing.Point(90, 25);
            this.tbActivityViewKeys.Name = "tbActivityViewKeys";
            this.tbActivityViewKeys.Size = new System.Drawing.Size(241, 27);
            this.tbActivityViewKeys.TabIndex = 0;
            this.tbActivityViewKeys.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // GeneralConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "GeneralConfigControl";
            this.Size = new System.Drawing.Size(587, 540);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.pBase.ResumeLayout(false);
            this.pControl.ResumeLayout(false);
            this.pControl.PerformLayout();
            this.gbGeneralSettings.ResumeLayout(false);
            this.pSystemSettings.ResumeLayout(false);
            this.gbHotKeys.ResumeLayout(false);
            this.gbHotKeys.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pControl;
        private System.Windows.Forms.TextBox tbDescSystem;
        private ZwiftActivityMonitorV2.GroupBoxEx gbGeneralSettings;
        private System.Windows.Forms.Panel pSystemSettings;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnEditSettings;
        private ZwiftActivityMonitorV2.GroupBoxEx gbHotKeys;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbResetLapsKeys;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbNewLapKeys;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSplitViewKeys;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLapViewKeys;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbActivityViewKeys;
    }
}
