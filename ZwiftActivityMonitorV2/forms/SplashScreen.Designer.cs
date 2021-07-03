
namespace ZwiftActivityMonitorV2
{
    partial class SplashScreen
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
            this.components = new System.ComponentModel.Container();
            this.pbEnjoyFitness = new System.Windows.Forms.PictureBox();
            this.pbZamCyclist = new System.Windows.Forms.PictureBox();
            this.lblProductVersion = new System.Windows.Forms.Label();
            this.pSponsor = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbEnjoyFitness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbZamCyclist)).BeginInit();
            this.pSponsor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbEnjoyFitness
            // 
            this.pbEnjoyFitness.BackColor = System.Drawing.Color.White;
            this.pbEnjoyFitness.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbEnjoyFitness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbEnjoyFitness.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Enjoy_Fitness_Logo_Short;
            this.pbEnjoyFitness.Location = new System.Drawing.Point(0, 0);
            this.pbEnjoyFitness.Name = "pbEnjoyFitness";
            this.pbEnjoyFitness.Size = new System.Drawing.Size(640, 283);
            this.pbEnjoyFitness.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbEnjoyFitness.TabIndex = 0;
            this.pbEnjoyFitness.TabStop = false;
            // 
            // pbZamCyclist
            // 
            this.pbZamCyclist.BackColor = System.Drawing.Color.White;
            this.pbZamCyclist.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Tron1;
            this.pbZamCyclist.Location = new System.Drawing.Point(15, 155);
            this.pbZamCyclist.Name = "pbZamCyclist";
            this.pbZamCyclist.Size = new System.Drawing.Size(100, 100);
            this.pbZamCyclist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbZamCyclist.TabIndex = 3;
            this.pbZamCyclist.TabStop = false;
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
            // pSponsor
            // 
            this.pSponsor.Controls.Add(this.label3);
            this.pSponsor.Controls.Add(this.label1);
            this.pSponsor.Controls.Add(this.label2);
            this.pSponsor.Controls.Add(this.pbZamCyclist);
            this.pSponsor.Controls.Add(this.pbEnjoyFitness);
            this.pSponsor.Location = new System.Drawing.Point(12, 12);
            this.pSponsor.Name = "pSponsor";
            this.pSponsor.Size = new System.Drawing.Size(640, 283);
            this.pSponsor.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(387, 255);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Join the Zwift Activity Monitor Users FB group";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(387, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 24);
            this.label2.TabIndex = 15;
            this.label2.Text = "Zwift Activity Monitor";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(582, 231);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 14);
            this.label3.TabIndex = 17;
            this.label3.Text = "PRO";
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61)))));
            this.ClientSize = new System.Drawing.Size(664, 307);
            this.Controls.Add(this.pSponsor);
            this.Controls.Add(this.lblProductVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AboutForm";
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbEnjoyFitness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbZamCyclist)).EndInit();
            this.pSponsor.ResumeLayout(false);
            this.pSponsor.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbEnjoyFitness;
        private System.Windows.Forms.PictureBox pbZamCyclist;
        private System.Windows.Forms.Label lblProductVersion;
        private System.Windows.Forms.Panel pSponsor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}