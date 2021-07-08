using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZwiftActivityMonitorV2
{
    public partial class SplashScreen : Form, Dapplo.Microsoft.Extensions.Hosting.WinForms.IWinFormsShell
    {
        private int m_tickCount;
        private Color ZAMguy1Color = Color.FromArgb(255, 255, 0);
        private Color ZAMguy2Color = Color.FromArgb(25, 255, 243);
        private Color ZAMguy3Color = Color.FromArgb(255, 0, 255);
        private Color ZAMguy4Color = Color.FromArgb(4, 255, 0);

        private MainForm mMainForm = new();

        public SplashScreen()
        {
            InitializeComponent();

            // This rounds the edges of the borderless window
            this.Region = System.Drawing.Region.FromHrgn(ZAMsettings.CreateRoundRectRgn(0, 0, Width, Height, 50, 50));
            //btnClose.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            this.timer1.Interval = 1000;
            this.timer1.Enabled = true;

            this.pbZamCyclist.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Tron1;
            //this.BackColor = this.ZAMguy1Color;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch(m_tickCount++)
            {
                case 0:
                    this.pbZamCyclist.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Tron1;
                    this.BackColor = this.ZAMguy1Color;
                    break;

                case 1:
                    this.pbZamCyclist.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Tron2;
                    this.BackColor = this.ZAMguy2Color;
                    break;
                case 2:
                    this.pbZamCyclist.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Tron3;
                    this.BackColor = this.ZAMguy3Color;
                    break;
                case 3:
                    this.pbZamCyclist.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Tron4;
                    this.BackColor = this.ZAMguy4Color;
                    break;
                //case 3:
                //    this.pbZamCyclist.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Tron1;
                //    this.BackColor = this.ZAMguy1Color;
                //    break;
                case 4:
                    this.timer1.Enabled = false;
                    
                    // Hide this window, then show the MainForm as a modal window.  Once it closes, close this one and program ends.
                    this.Hide();
                    Debug.WriteLine($"{this.GetType()}::timer1_Tick - ShowDialog MainForm");
                    mMainForm.ShowDialog(); // This will block until closed.
                    this.Close();
                    break;
            }
        }
    }
}
