
namespace ZwiftActivityMonitor
{
    partial class MainViewControl
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "20 Min",
            "8.88",
            "8.88",
            "8.88",
            "888"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "20 Min",
            "8.88",
            "8.88",
            "8.88",
            "888"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "20 Min",
            "8.88",
            "8.88",
            "8.88",
            "888"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Overall",
            "888",
            "888",
            "8.88",
            "88.8"}, -1);
            this.pnViewer = new System.Windows.Forms.Panel();
            this.lvViewer = new System.Windows.Forms.ListView();
            this.chDesc = new System.Windows.Forms.ColumnHeader();
            this.chAvg = new System.Windows.Forms.ColumnHeader();
            this.chAvgMax = new System.Windows.Forms.ColumnHeader();
            this.chFTP = new System.Windows.Forms.ColumnHeader();
            this.chHR = new System.Windows.Forms.ColumnHeader();
            this.chBlank = new System.Windows.Forms.ColumnHeader();
            this.pnOverall = new System.Windows.Forms.Panel();
            this.lvOverall = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.chBlank2 = new System.Windows.Forms.ColumnHeader();
            this.pnViewer.SuspendLayout();
            this.pnOverall.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnViewer
            // 
            this.pnViewer.Controls.Add(this.lvViewer);
            this.pnViewer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnViewer.Location = new System.Drawing.Point(0, 0);
            this.pnViewer.Name = "pnViewer";
            this.pnViewer.Size = new System.Drawing.Size(334, 98);
            this.pnViewer.TabIndex = 3;
            // 
            // lvViewer
            // 
            this.lvViewer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
            this.lvViewer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvViewer.CausesValidation = false;
            this.lvViewer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDesc,
            this.chAvg,
            this.chAvgMax,
            this.chFTP,
            this.chHR,
            this.chBlank});
            this.lvViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvViewer.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvViewer.ForeColor = System.Drawing.Color.White;
            this.lvViewer.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvViewer.HideSelection = false;
            this.lvViewer.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lvViewer.Location = new System.Drawing.Point(0, 0);
            this.lvViewer.MultiSelect = false;
            this.lvViewer.Name = "lvViewer";
            this.lvViewer.OwnerDraw = true;
            this.lvViewer.Scrollable = false;
            this.lvViewer.Size = new System.Drawing.Size(334, 98);
            this.lvViewer.TabIndex = 0;
            this.lvViewer.TabStop = false;
            this.lvViewer.UseCompatibleStateImageBehavior = false;
            this.lvViewer.View = System.Windows.Forms.View.Details;
            this.lvViewer.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvViewer.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvViewer.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListView_ItemSelectionChanged_Disable);
            this.lvViewer.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // chDesc
            // 
            this.chDesc.Text = "";
            this.chDesc.Width = 75;
            // 
            // chAvg
            // 
            this.chAvg.Text = "Avg";
            this.chAvg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chAvg.Width = 50;
            // 
            // chAvgMax
            // 
            this.chAvgMax.Text = "Avg (Max)";
            this.chAvgMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chAvgMax.Width = 85;
            // 
            // chFTP
            // 
            this.chFTP.Text = "FTP";
            this.chFTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chFTP.Width = 50;
            // 
            // chHR
            // 
            this.chHR.Text = "HR";
            this.chHR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chHR.Width = 54;
            // 
            // chBlank
            // 
            this.chBlank.Text = "";
            this.chBlank.Width = 20;
            // 
            // pnOverall
            // 
            this.pnOverall.Controls.Add(this.lvOverall);
            this.pnOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnOverall.Location = new System.Drawing.Point(0, 98);
            this.pnOverall.Margin = new System.Windows.Forms.Padding(0);
            this.pnOverall.Name = "pnOverall";
            this.pnOverall.Size = new System.Drawing.Size(334, 55);
            this.pnOverall.TabIndex = 4;
            // 
            // lvOverall
            // 
            this.lvOverall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
            this.lvOverall.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvOverall.CausesValidation = false;
            this.lvOverall.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.chBlank2});
            this.lvOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvOverall.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvOverall.ForeColor = System.Drawing.Color.White;
            this.lvOverall.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvOverall.HideSelection = false;
            this.lvOverall.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4});
            this.lvOverall.Location = new System.Drawing.Point(0, 0);
            this.lvOverall.MultiSelect = false;
            this.lvOverall.Name = "lvOverall";
            this.lvOverall.OwnerDraw = true;
            this.lvOverall.Scrollable = false;
            this.lvOverall.Size = new System.Drawing.Size(334, 55);
            this.lvOverall.TabIndex = 0;
            this.lvOverall.TabStop = false;
            this.lvOverall.UseCompatibleStateImageBehavior = false;
            this.lvOverall.View = System.Windows.Forms.View.Details;
            this.lvOverall.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvOverall.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvOverall.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListView_ItemSelectionChanged_Disable);
            this.lvOverall.Enter += new System.EventHandler(this.SkipControl_Enter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 75;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Avg";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 50;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "NP";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 85;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "IF";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 50;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "km/h";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 54;
            // 
            // chBlank2
            // 
            this.chBlank2.Text = "";
            this.chBlank2.Width = 20;
            // 
            // MainViewControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnOverall);
            this.Controls.Add(this.pnViewer);
            this.Name = "MainViewControl";
            this.Size = new System.Drawing.Size(334, 153);
            this.Load += new System.EventHandler(this.UserControlBase_Load);
            this.pnViewer.ResumeLayout(false);
            this.pnOverall.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnViewer;
        private System.Windows.Forms.ListView lvViewer;
        private System.Windows.Forms.ColumnHeader chDesc;
        private System.Windows.Forms.ColumnHeader chAvg;
        private System.Windows.Forms.ColumnHeader chAvgMax;
        private System.Windows.Forms.ColumnHeader chFTP;
        private System.Windows.Forms.ColumnHeader chHR;
        private System.Windows.Forms.Panel pnOverall;
        private System.Windows.Forms.ListView lvOverall;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader chBlank;
        private System.Windows.Forms.ColumnHeader chBlank2;
    }
}
