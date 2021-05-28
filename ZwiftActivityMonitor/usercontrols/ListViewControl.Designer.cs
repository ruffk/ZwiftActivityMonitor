
namespace ZwiftActivityMonitor
{
    partial class ListViewControl
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
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "Detail Row"}, -1);
            this.pHeader = new System.Windows.Forms.Panel();
            this.lvHeader = new System.Windows.Forms.ListView();
            this.chFirst = new System.Windows.Forms.ColumnHeader();
            this.chHeader1 = new System.Windows.Forms.ColumnHeader();
            this.pDetail = new System.Windows.Forms.Panel();
            this.lvDetail = new System.Windows.Forms.ListView();
            this.chDetail1 = new System.Windows.Forms.ColumnHeader();
            this.chDetail2 = new System.Windows.Forms.ColumnHeader();
            this.pHeader.SuspendLayout();
            this.pDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // pHeader
            // 
            this.pHeader.Controls.Add(this.lvHeader);
            this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeader.Location = new System.Drawing.Point(0, 0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(314, 25);
            this.pHeader.TabIndex = 0;
            // 
            // lvHeader
            // 
            this.lvHeader.BackColor = System.Drawing.SystemColors.Control;
            this.lvHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvHeader.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFirst,
            this.chHeader1});
            this.lvHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHeader.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvHeader.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lvHeader.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvHeader.HideSelection = false;
            this.lvHeader.Location = new System.Drawing.Point(0, 0);
            this.lvHeader.MultiSelect = false;
            this.lvHeader.Name = "lvHeader";
            this.lvHeader.OwnerDraw = true;
            this.lvHeader.Size = new System.Drawing.Size(314, 25);
            this.lvHeader.TabIndex = 1;
            this.lvHeader.TabStop = false;
            this.lvHeader.UseCompatibleStateImageBehavior = false;
            this.lvHeader.View = System.Windows.Forms.View.Details;
            this.lvHeader.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvHeader.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvHeader.Resize += new System.EventHandler(this.ListView_Resize_HideHorizontalScrollBar);
            // 
            // chFirst
            // 
            this.chFirst.Text = "";
            this.chFirst.Width = 0;
            // 
            // chHeader1
            // 
            this.chHeader1.Text = "Title or Column Headings";
            this.chHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chHeader1.Width = 314;
            // 
            // pDetail
            // 
            this.pDetail.Controls.Add(this.lvDetail);
            this.pDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDetail.Location = new System.Drawing.Point(0, 25);
            this.pDetail.Name = "pDetail";
            this.pDetail.Size = new System.Drawing.Size(314, 49);
            this.pDetail.TabIndex = 1;
            // 
            // lvDetail
            // 
            this.lvDetail.BackColor = System.Drawing.SystemColors.Control;
            this.lvDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvDetail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDetail1,
            this.chDetail2});
            this.lvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDetail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvDetail.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lvDetail.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvDetail.HideSelection = false;
            this.lvDetail.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
            this.lvDetail.Location = new System.Drawing.Point(0, 0);
            this.lvDetail.MultiSelect = false;
            this.lvDetail.Name = "lvDetail";
            this.lvDetail.OwnerDraw = true;
            this.lvDetail.Size = new System.Drawing.Size(314, 49);
            this.lvDetail.TabIndex = 2;
            this.lvDetail.TabStop = false;
            this.lvDetail.UseCompatibleStateImageBehavior = false;
            this.lvDetail.View = System.Windows.Forms.View.Details;
            this.lvDetail.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
            this.lvDetail.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.Listview_DrawSubItem);
            this.lvDetail.SelectedIndexChanged += new System.EventHandler(this.lvDetail_SelectedIndexChanged);
            this.lvDetail.DoubleClick += new System.EventHandler(this.lvDetail_DoubleClick);
            // 
            // chDetail1
            // 
            this.chDetail1.Text = "";
            this.chDetail1.Width = 0;
            // 
            // chDetail2
            // 
            this.chDetail2.Text = "Detail Column Headings";
            this.chDetail2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chDetail2.Width = 314;
            // 
            // ListViewControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pDetail);
            this.Controls.Add(this.pHeader);
            this.Name = "ListViewControl";
            this.Size = new System.Drawing.Size(314, 74);
            this.pHeader.ResumeLayout(false);
            this.pDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pHeader;
        private System.Windows.Forms.Panel pDetail;
        private System.Windows.Forms.ColumnHeader chFirst;
        private System.Windows.Forms.ColumnHeader chHeader1;
        private System.Windows.Forms.ColumnHeader chDetail1;
        private System.Windows.Forms.ColumnHeader chDetail2;
        public System.Windows.Forms.ListView lvHeader;
        public System.Windows.Forms.ListView lvDetail;
    }
}
