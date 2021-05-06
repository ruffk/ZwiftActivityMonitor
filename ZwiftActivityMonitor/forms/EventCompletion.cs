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
using System.Net.Mail;

namespace ZwiftActivityMonitor
{
    public partial class EventCompletion : Form
    {
        private System.Drawing.Point m_offset;                      // for moving window
        private bool m_mouseDown;                                   // for moving window
        private int TimerTicks { get; set; }
        private Timer CountdownTimer { get; }
        private int SecondsUntilAutoClose { get; set; }

        public EventCompletion()
        {
            InitializeComponent();

            this.CountdownTimer = new();
            this.CountdownTimer.Interval = 1000;
            this.CountdownTimer.Tick += this.CountdownTimer_Tick;

            // This rounds the edges of the borderless window
            this.Region = System.Drawing.Region.FromHrgn(ZAMsettings.CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent
        }

        public void InitLapEventCompletion(LapDetailItem item)
        {
            this.lblTitle.Text = "Lap Completed";

            ucEventView.InitLapEventCompletion(item);

            this.SecondsUntilAutoClose = 7;
            btnClose.Text = this.SecondsUntilAutoClose.ToString();

            this.CountdownTimer.Enabled = true;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            this.TimerTicks++;

            this.SecondsUntilAutoClose--;
            btnClose.Text = this.SecondsUntilAutoClose.ToString();

            if (this.SecondsUntilAutoClose <= 0)
            {
                btnClose.PerformClick();
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            m_offset.X = e.X;
            m_offset.Y = e.Y;
            m_mouseDown = true;
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_mouseDown)
            {
                Point currentPos = this.PointToScreen(e.Location);
                this.Location = new Point(currentPos.X - m_offset.X, currentPos.Y - m_offset.Y);
            }
        }

        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            m_mouseDown = false;
        }

        private void btnAutoClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
