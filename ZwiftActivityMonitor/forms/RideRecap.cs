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
    public partial class RideRecap : Form
    {
        private System.Drawing.Point m_offset;                      // for moving window
        private bool m_mouseDown;                                   // for moving window
        private RideRecapMetrics m_rideRecapMetrics;
        private bool m_hasEmailAddress;

        public RideRecap(RideRecapMetrics rideRecapMetrics)
        {
            m_rideRecapMetrics = rideRecapMetrics;

            InitializeComponent();

            // This rounds the edges of the borderless window
            this.Region = System.Drawing.Region.FromHrgn(ZAMsettings.CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RideRecap_Load(object sender, EventArgs e)
        {
            lblDuration.Text = $"{m_rideRecapMetrics.Duration.Hours:0#}:{m_rideRecapMetrics.Duration.Minutes:0#}:{m_rideRecapMetrics.Duration.Seconds:0#}";
            lblDistance.Text = $"{m_rideRecapMetrics.DistanceMi:0.0} mi ({m_rideRecapMetrics.DistanceKm:0.0} km)";
            lblAvgSpeed.Text = $"{m_rideRecapMetrics.AverageMph:0.0} mi/hr ({m_rideRecapMetrics.AverageKph:0.0} km/hr)";
            lblAvgPower.Text = $"{m_rideRecapMetrics.OverallPower} watts";
            lblNp.Text = $"{m_rideRecapMetrics.NormalizedPower} watts";
            lblIf.Text = $"{(m_rideRecapMetrics.IntensityFactor.HasValue ? m_rideRecapMetrics.IntensityFactor.Value.ToString("#.00") : "N/A")}";
            lblTss.Text = $"{(m_rideRecapMetrics.TotalSufferScore.HasValue ? m_rideRecapMetrics.TotalSufferScore.Value : "N/A")}";

            if (lblEmailAddr.Text.Length > 0)
            {
                lblEmailAddr.Text = ZAMsettings.Settings.CurrentUser.EmailAddress;
                m_hasEmailAddress = true;
            }
            else
            {
                lblEmailAddr.Text = "Please set email address in your user profile to use this feature.";
            }
        }

        private void Launch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var link = (LinkLabel)sender;
            link.LinkVisited = true;
            ProcessStartInfo psInfo = new ProcessStartInfo
            {
                FileName = link.Text,
                UseShellExecute = true
            };
            Process.Start(psInfo);
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

        private void pbZamCyclist_Click(object sender, EventArgs e)
        {
            if (m_hasEmailAddress)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    pbZamCyclist.Cursor = Cursors.WaitCursor;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("ZwiftActivityMonitor@gmail.com");
                    mail.To.Add(ZAMsettings.Settings.CurrentUser.EmailAddress);
                    mail.Subject = "Zwift Activity Monitor Ride Recap";
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("ZwiftActivityMonitor@gmail.com", "kfnggjsetsfxghky");
                    SmtpServer.EnableSsl = true;

                    StringBuilder s = new StringBuilder();
                    s.Append("\n");
                    s.Append($"Duration: {lblDuration.Text}\n");
                    s.Append($"Distance: {lblDistance.Text}\n");
                    s.Append($"Avg Speed: {lblAvgSpeed.Text}\n");
                    s.Append($"Avg Power: {lblAvgPower.Text}\n");
                    s.Append($"NP: {lblNp.Text}\n");
                    s.Append($"IF: {lblIf.Text}\n");
                    s.Append($"TSS: {lblTss.Text}\n");

                    mail.Body = s.ToString();

                    SmtpServer.Send(mail);

                    MessageBox.Show($"Ride recap email sent to {ZAMsettings.Settings.CurrentUser.EmailAddress}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ride recap email failed. Reason: {ex.Message}", "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    pbZamCyclist.Cursor = Cursors.Hand;
                }
            }
        }
    }
}
