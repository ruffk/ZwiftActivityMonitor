
namespace ZwiftActivityMonitorV2
{
    partial class SplitViewerControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgDetail = new ZwiftActivityMonitorV2.DataGridViewEx();
            this.dgSummary = new ZwiftActivityMonitorV2.DataGridViewEx();
            this.tlPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSummary)).BeginInit();
            this.tlPanel.SuspendLayout();
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
            // dgSummary
            // 
            this.dgSummary.AllowUserToAddRows = false;
            this.dgSummary.AllowUserToDeleteRows = false;
            this.dgSummary.AllowUserToResizeColumns = false;
            this.dgSummary.AllowUserToResizeRows = false;
            this.dgSummary.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgSummary.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgSummary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSummary.EnableHeadersVisualStyles = false;
            this.dgSummary.Location = new System.Drawing.Point(0, 97);
            this.dgSummary.Margin = new System.Windows.Forms.Padding(0);
            this.dgSummary.Name = "dgSummary";
            this.dgSummary.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgSummary.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgSummary.RowHeadersVisible = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgSummary.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgSummary.RowTemplate.Height = 25;
            this.dgSummary.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgSummary.ShowCellToolTips = false;
            this.dgSummary.ShowFocus = null;
            this.dgSummary.Size = new System.Drawing.Size(320, 50);
            this.dgSummary.TabIndex = 20;
            this.dgSummary.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            this.dgSummary.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewGradientHeader_CellPainting);
            // 
            // tlPanel
            // 
            this.tlPanel.ColumnCount = 1;
            this.tlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlPanel.Controls.Add(this.dgDetail, 0, 0);
            this.tlPanel.Controls.Add(this.dgSummary, 0, 1);
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
            // SplitViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tlPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SplitViewerControl";
            this.Size = new System.Drawing.Size(320, 147);
            this.Load += new System.EventHandler(this.ViewControl_Load);
            this.SizeChanged += new System.EventHandler(this.ViewControl_SizeChanged);
            this.Resize += new System.EventHandler(this.ViewControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSummary)).EndInit();
            this.tlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ZwiftActivityMonitorV2.DataGridViewEx dgDetail;
        private ZwiftActivityMonitorV2.DataGridViewEx dgSummary;
        private System.Windows.Forms.TableLayoutPanel tlPanel;
    }
}
