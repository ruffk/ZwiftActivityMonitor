
namespace ZwiftActivityMonitor
{
    partial class RideRecap
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
            this.pbZamCyclist = new System.Windows.Forms.PictureBox();
            this.pnTitle = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblDistance = new System.Windows.Forms.Label();
            this.lblAvgSpeed = new System.Windows.Forms.Label();
            this.lblAvgPower = new System.Windows.Forms.Label();
            this.lblNp = new System.Windows.Forms.Label();
            this.lblIf = new System.Windows.Forms.Label();
            this.lblTss = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblEmailAddr = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbZamCyclist)).BeginInit();
            this.pnTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbZamCyclist
            // 
            this.pbZamCyclist.BackColor = System.Drawing.Color.Transparent;
            this.pbZamCyclist.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbZamCyclist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbZamCyclist.Image = global::ZwiftActivityMonitor.Properties.Resources.Tron2;
            this.pbZamCyclist.Location = new System.Drawing.Point(15, 266);
            this.pbZamCyclist.Name = "pbZamCyclist";
            this.pbZamCyclist.Size = new System.Drawing.Size(70, 70);
            this.pbZamCyclist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbZamCyclist.TabIndex = 3;
            this.pbZamCyclist.TabStop = false;
            this.pbZamCyclist.Click += new System.EventHandler(this.pbZamCyclist_Click);
            // 
            // pnTitle
            // 
            this.pnTitle.BackColor = System.Drawing.Color.White;
            this.pnTitle.Controls.Add(this.btnClose);
            this.pnTitle.Controls.Add(this.lblTitle);
            this.pnTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTitle.Location = new System.Drawing.Point(0, 0);
            this.pnTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pnTitle.Name = "pnTitle";
            this.pnTitle.Size = new System.Drawing.Size(489, 26);
            this.pnTitle.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnClose.Location = new System.Drawing.Point(460, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(29, 26);
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(489, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Ride Recap";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseMove);
            this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(129, 73);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(91, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Duration:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(127, 102);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(93, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "Distance:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(111, 131);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(109, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = "Avg Speed:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(112, 160);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(108, 24);
            this.label7.TabIndex = 14;
            this.label7.Text = "Avg Power:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(180, 189);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(40, 24);
            this.label1.TabIndex = 51;
            this.label1.Text = "NP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(189, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 24);
            this.label2.TabIndex = 52;
            this.label2.Text = "IF:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(172, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 24);
            this.label6.TabIndex = 53;
            this.label6.Text = "TSS:";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.BackColor = System.Drawing.Color.Transparent;
            this.lblDuration.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDuration.Location = new System.Drawing.Point(232, 73);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDuration.Size = new System.Drawing.Size(86, 24);
            this.lblDuration.TabIndex = 54;
            this.lblDuration.Text = "Duration";
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.BackColor = System.Drawing.Color.Transparent;
            this.lblDistance.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDistance.Location = new System.Drawing.Point(232, 102);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblDistance.Size = new System.Drawing.Size(88, 24);
            this.lblDistance.TabIndex = 55;
            this.lblDistance.Text = "Distance";
            // 
            // lblAvgSpeed
            // 
            this.lblAvgSpeed.AutoSize = true;
            this.lblAvgSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblAvgSpeed.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblAvgSpeed.Location = new System.Drawing.Point(232, 131);
            this.lblAvgSpeed.Name = "lblAvgSpeed";
            this.lblAvgSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblAvgSpeed.Size = new System.Drawing.Size(104, 24);
            this.lblAvgSpeed.TabIndex = 56;
            this.lblAvgSpeed.Text = "Avg Speed";
            // 
            // lblAvgPower
            // 
            this.lblAvgPower.AutoSize = true;
            this.lblAvgPower.BackColor = System.Drawing.Color.Transparent;
            this.lblAvgPower.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblAvgPower.Location = new System.Drawing.Point(232, 160);
            this.lblAvgPower.Name = "lblAvgPower";
            this.lblAvgPower.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblAvgPower.Size = new System.Drawing.Size(103, 24);
            this.lblAvgPower.TabIndex = 57;
            this.lblAvgPower.Text = "Avg Power";
            // 
            // lblNp
            // 
            this.lblNp.AutoSize = true;
            this.lblNp.BackColor = System.Drawing.Color.Transparent;
            this.lblNp.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNp.Location = new System.Drawing.Point(232, 189);
            this.lblNp.Name = "lblNp";
            this.lblNp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblNp.Size = new System.Drawing.Size(35, 24);
            this.lblNp.TabIndex = 58;
            this.lblNp.Text = "NP";
            // 
            // lblIf
            // 
            this.lblIf.AutoSize = true;
            this.lblIf.BackColor = System.Drawing.Color.Transparent;
            this.lblIf.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblIf.Location = new System.Drawing.Point(232, 218);
            this.lblIf.Name = "lblIf";
            this.lblIf.Size = new System.Drawing.Size(26, 24);
            this.lblIf.TabIndex = 59;
            this.lblIf.Text = "IF";
            // 
            // lblTss
            // 
            this.lblTss.AutoSize = true;
            this.lblTss.BackColor = System.Drawing.Color.Transparent;
            this.lblTss.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTss.Location = new System.Drawing.Point(232, 247);
            this.lblTss.Name = "lblTss";
            this.lblTss.Size = new System.Drawing.Size(43, 24);
            this.lblTss.TabIndex = 60;
            this.lblTss.Text = "TSS";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(91, 321);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label8.Size = new System.Drawing.Size(48, 17);
            this.label8.TabIndex = 61;
            this.label8.Text = "Send to:";
            // 
            // lblEmailAddr
            // 
            this.lblEmailAddr.AutoSize = true;
            this.lblEmailAddr.BackColor = System.Drawing.Color.Transparent;
            this.lblEmailAddr.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEmailAddr.Location = new System.Drawing.Point(134, 321);
            this.lblEmailAddr.Name = "lblEmailAddr";
            this.lblEmailAddr.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblEmailAddr.Size = new System.Drawing.Size(179, 17);
            this.lblEmailAddr.TabIndex = 62;
            this.lblEmailAddr.Text = "Please configure your email address";
            // 
            // RideRecap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::ZwiftActivityMonitor.Properties.Resources.Enjoy_Fitness_Logo_Short_25_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(489, 351);
            this.Controls.Add(this.lblEmailAddr);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblTss);
            this.Controls.Add(this.lblIf);
            this.Controls.Add(this.lblNp);
            this.Controls.Add(this.lblAvgPower);
            this.Controls.Add(this.lblAvgSpeed);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pnTitle);
            this.Controls.Add(this.pbZamCyclist);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RideRecap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RideRecap";
            this.Load += new System.EventHandler(this.RideRecap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbZamCyclist)).EndInit();
            this.pnTitle.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbZamCyclist;
        private System.Windows.Forms.Panel pnTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.Label lblAvgSpeed;
        private System.Windows.Forms.Label lblAvgPower;
        private System.Windows.Forms.Label lblNp;
        private System.Windows.Forms.Label lblIf;
        private System.Windows.Forms.Label lblTss;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblEmailAddr;
    }
}