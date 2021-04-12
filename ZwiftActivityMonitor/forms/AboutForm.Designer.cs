
namespace ZwiftActivityMonitor
{
    partial class AboutForm
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
            this.pbEnjoyFitness = new System.Windows.Forms.PictureBox();
            this.linkProjectWebsite = new System.Windows.Forms.LinkLabel();
            this.linkProjectSponsor = new System.Windows.Forms.LinkLabel();
            this.pbZamCyclist = new System.Windows.Forms.PictureBox();
            this.pnTitle = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProductVersion = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.linkLatestReleases = new System.Windows.Forms.LinkLabel();
            this.pSponsor = new System.Windows.Forms.Panel();
            this.pHeading = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbEnjoyFitness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbZamCyclist)).BeginInit();
            this.pnTitle.SuspendLayout();
            this.pSponsor.SuspendLayout();
            this.pHeading.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbEnjoyFitness
            // 
            this.pbEnjoyFitness.BackColor = System.Drawing.Color.White;
            this.pbEnjoyFitness.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEnjoyFitness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbEnjoyFitness.Image = global::ZwiftActivityMonitor.Properties.Resources.Enjoy_Fitness_Logo_red;
            this.pbEnjoyFitness.Location = new System.Drawing.Point(0, 21);
            this.pbEnjoyFitness.Name = "pbEnjoyFitness";
            this.pbEnjoyFitness.Size = new System.Drawing.Size(431, 270);
            this.pbEnjoyFitness.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbEnjoyFitness.TabIndex = 0;
            this.pbEnjoyFitness.TabStop = false;
            this.pbEnjoyFitness.Click += new System.EventHandler(this.pbEnjoyFitness_Click);
            // 
            // linkProjectWebsite
            // 
            this.linkProjectWebsite.AutoSize = true;
            this.linkProjectWebsite.Location = new System.Drawing.Point(113, 120);
            this.linkProjectWebsite.Name = "linkProjectWebsite";
            this.linkProjectWebsite.Size = new System.Drawing.Size(254, 15);
            this.linkProjectWebsite.TabIndex = 1;
            this.linkProjectWebsite.TabStop = true;
            this.linkProjectWebsite.Text = "https://github.com/ruffk/ZwiftActivityMonitor";
            this.linkProjectWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkProjectWebsite_LinkClicked);
            // 
            // linkProjectSponsor
            // 
            this.linkProjectSponsor.AutoSize = true;
            this.linkProjectSponsor.Location = new System.Drawing.Point(113, 170);
            this.linkProjectSponsor.Name = "linkProjectSponsor";
            this.linkProjectSponsor.Size = new System.Drawing.Size(152, 15);
            this.linkProjectSponsor.TabIndex = 2;
            this.linkProjectSponsor.TabStop = true;
            this.linkProjectSponsor.Text = "https://enjoyfitnessjax.com";
            this.linkProjectSponsor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkProjectSponsor_LinkClicked);
            // 
            // pbZamCyclist
            // 
            this.pbZamCyclist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbZamCyclist.Image = global::ZwiftActivityMonitor.Properties.Resources.Tron2;
            this.pbZamCyclist.Location = new System.Drawing.Point(28, 37);
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
            this.pnTitle.Size = new System.Drawing.Size(455, 26);
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
            this.btnClose.Location = new System.Drawing.Point(426, 0);
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
            this.lblTitle.Size = new System.Drawing.Size(455, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "About";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseMove);
            this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(113, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "Zwift Activity Monitor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(113, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "Version";
            // 
            // lblProductVersion
            // 
            this.lblProductVersion.AutoSize = true;
            this.lblProductVersion.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblProductVersion.Location = new System.Drawing.Point(179, 73);
            this.lblProductVersion.Name = "lblProductVersion";
            this.lblProductVersion.Size = new System.Drawing.Size(0, 21);
            this.lblProductVersion.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Project Website:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Latest Releases:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Project Sponsor:";
            // 
            // linkLatestReleases
            // 
            this.linkLatestReleases.AutoSize = true;
            this.linkLatestReleases.Location = new System.Drawing.Point(113, 145);
            this.linkLatestReleases.Name = "linkLatestReleases";
            this.linkLatestReleases.Size = new System.Drawing.Size(300, 15);
            this.linkLatestReleases.TabIndex = 11;
            this.linkLatestReleases.TabStop = true;
            this.linkLatestReleases.Text = "https://github.com/ruffk/ZwiftActivityMonitor/releases";
            this.linkLatestReleases.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLatestReleases_LinkClicked);
            // 
            // pSponsor
            // 
            this.pSponsor.Controls.Add(this.pbEnjoyFitness);
            this.pSponsor.Controls.Add(this.pHeading);
            this.pSponsor.Location = new System.Drawing.Point(12, 220);
            this.pSponsor.Name = "pSponsor";
            this.pSponsor.Size = new System.Drawing.Size(431, 291);
            this.pSponsor.TabIndex = 12;
            // 
            // pHeading
            // 
            this.pHeading.Controls.Add(this.label6);
            this.pHeading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeading.Location = new System.Drawing.Point(0, 0);
            this.pHeading.Name = "pHeading";
            this.pHeading.Size = new System.Drawing.Size(431, 21);
            this.pHeading.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(239, 21);
            this.label6.TabIndex = 7;
            this.label6.Text = "Developed in collaboration with:";
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = true;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(359, 517);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(86, 25);
            this.btnOk.TabIndex = 13;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(455, 551);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.pSponsor);
            this.Controls.Add(this.linkLatestReleases);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblProductVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnTitle);
            this.Controls.Add(this.pbZamCyclist);
            this.Controls.Add(this.linkProjectSponsor);
            this.Controls.Add(this.linkProjectWebsite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AboutForm";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbEnjoyFitness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbZamCyclist)).EndInit();
            this.pnTitle.ResumeLayout(false);
            this.pSponsor.ResumeLayout(false);
            this.pHeading.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbEnjoyFitness;
        private System.Windows.Forms.LinkLabel linkProjectWebsite;
        private System.Windows.Forms.LinkLabel linkProjectSponsor;
        private System.Windows.Forms.PictureBox pbZamCyclist;
        private System.Windows.Forms.Panel pnTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProductVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkLatestReleases;
        private System.Windows.Forms.Panel pSponsor;
        private System.Windows.Forms.Panel pHeading;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnOk;
    }
}