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
        private int mtickCount;
        private DateTime mStartTime;
        private Color ZAMguy1Color = Color.FromArgb(255, 255, 0);
        private Color ZAMguy2Color = Color.FromArgb(25, 255, 243);
        private Color ZAMguy3Color = Color.FromArgb(255, 0, 255);
        private Color ZAMguy4Color = Color.FromArgb(4, 255, 0);

        private MainForm mMainForm = new();

        public SplashScreen()
        {
            InitializeComponent();

            this.mStartTime = DateTime.Now;

            // This rounds the edges of the borderless window
            this.Region = System.Drawing.Region.FromHrgn(ZAMsettings.CreateRoundRectRgn(0, 0, Width, Height, 50, 50));
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            this.pbZamCyclist.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Tron1;

            if (ZAMsettings.Settings.SplashScreenDurationSecs > 0)
            {
                this.timer1.Interval = 1000;
                this.timer1.Enabled = true;
            }
            else
            {
                this.LaunchMainForm();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine($"{this.GetType()}::timer1_Tick - {(DateTime.Now - mStartTime).TotalSeconds}");

            if ((DateTime.Now - mStartTime).TotalSeconds >= ZAMsettings.Settings.SplashScreenDurationSecs)
            {
                this.LaunchMainForm();
            }
            else
            {
                this.UpdateImage();
                mtickCount++;
                mtickCount %= 4;
            }
        }

        private void LaunchMainForm()
        {
            Debug.WriteLine($"{this.GetType()}::LaunchMainForm");
            this.timer1.Enabled = false;

            // Hide this window, then show the MainForm as a modal window.  Once it closes, close this one and program ends.
            this.Hide();
            mMainForm.ShowDialog(); // This will block until closed.
            this.Close();
        }

        private void UpdateImage()
        {
            switch (mtickCount)
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
            }
        }
    }
}
