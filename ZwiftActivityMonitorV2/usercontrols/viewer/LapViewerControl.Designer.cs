
namespace ZwiftActivityMonitorV2
{
    partial class LapViewerControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgDetail = new ZwiftActivityMonitorV2.DataGridViewEx();
            this.tlPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pToolStrip = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbLap = new System.Windows.Forms.ToolStripButton();
            this.tsbReset = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.tlPanel.SuspendLayout();
            this.pToolStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgDetail
            // 
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.AllowUserToResizeColumns = false;
            this.dgDetail.AllowUserToResizeRows = false;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgDetail.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.Location = new System.Drawing.Point(0, 0);
            this.dgDetail.Margin = new System.Windows.Forms.Padding(0);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgDetail.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgDetail.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgDetail.RowTemplate.Height = 25;
            this.dgDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgDetail.ShowCellToolTips = false;
            this.dgDetail.ShowFocus = null;
            this.dgDetail.Size = new System.Drawing.Size(320, 97);
            this.dgDetail.TabIndex = 19;
            this.dgDetail.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dgDetail.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewGradientHeader_CellPainting);
            // 
            // tlPanel
            // 
            this.tlPanel.ColumnCount = 1;
            this.tlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPanel.Controls.Add(this.dgDetail, 0, 0);
            this.tlPanel.Controls.Add(this.pToolStrip, 0, 1);
            this.tlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tlPanel.Location = new System.Drawing.Point(0, 0);
            this.tlPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tlPanel.Name = "tlPanel";
            this.tlPanel.RowCount = 2;
            this.tlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlPanel.Size = new System.Drawing.Size(320, 147);
            this.tlPanel.TabIndex = 21;
            // 
            // pToolStrip
            // 
            this.pToolStrip.Controls.Add(this.lblStatus);
            this.pToolStrip.Controls.Add(this.toolStrip);
            this.pToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pToolStrip.Location = new System.Drawing.Point(3, 100);
            this.pToolStrip.Name = "pToolStrip";
            this.pToolStrip.Size = new System.Drawing.Size(314, 44);
            this.pToolStrip.TabIndex = 20;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.Location = new System.Drawing.Point(136, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(178, 44);
            this.lblStatus.TabIndex = 23;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLap,
            this.tsbReset});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(136, 44);
            this.toolStrip.TabIndex = 22;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbLap
            // 
            this.tsbLap.AutoSize = false;
            this.tsbLap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLap.Image = global::ZwiftActivityMonitorV2.Properties.Resources.stopwatch;
            this.tsbLap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLap.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.tsbLap.Name = "tsbLap";
            this.tsbLap.Size = new System.Drawing.Size(48, 48);
            this.tsbLap.Text = "Click to start a new lap";
            this.tsbLap.Click += new System.EventHandler(this.tsbLap_Click);
            // 
            // tsbReset
            // 
            this.tsbReset.AutoSize = false;
            this.tsbReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbReset.Image = global::ZwiftActivityMonitorV2.Properties.Resources.split;
            this.tsbReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReset.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.tsbReset.Name = "tsbReset";
            this.tsbReset.Size = new System.Drawing.Size(48, 48);
            this.tsbReset.Text = "Click to clear laps and restart";
            this.tsbReset.Click += new System.EventHandler(this.tsbReset_Click);
            // 
            // LapViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tlPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LapViewerControl";
            this.Size = new System.Drawing.Size(320, 147);
            this.Load += new System.EventHandler(this.ViewControl_Load);
            this.Resize += new System.EventHandler(this.ViewControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.tlPanel.ResumeLayout(false);
            this.pToolStrip.ResumeLayout(false);
            this.pToolStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ZwiftActivityMonitorV2.DataGridViewEx dgDetail;
        private System.Windows.Forms.TableLayoutPanel tlPanel;
        private System.Windows.Forms.Panel pToolStrip;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbLap;
        private System.Windows.Forms.ToolStripButton tsbReset;
    }
}
