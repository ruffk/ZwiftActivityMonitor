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
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitorV2
{
    public partial class RideRecap : Syncfusion.WinForms.Controls.SfForm
    {
        private RideRecapMetrics m_rideRecapMetrics;
        //private bool m_hasEmailAddress;
        private readonly ILogger<RideRecap> Logger;

        public RideRecap()
        {
            //m_rideRecapMetrics = rideRecapMetrics;

            InitializeComponent();

            if (DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<RideRecap>();

            this.toolStrip.Renderer = new ToolStripProfessionalRendererEx();

            MSoffice2010ColorManager colorTable = ZAMappearance.ApplyColorTable(this);

            this.Icon = Properties.Resources.ZAMicon;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RideRecap_Load(object sender, EventArgs e)
        {
            /*
                #000001 BackColor
                #000002 ActiveFormBorderColor
                #000003 ForeColor
                #000004 ActiveTitleGradientEnd
            */
            MSoffice2010ColorManager colorTable = ZAMappearance.GetColorTable();

            this.pControls.BackColor = colorTable.FormBackground;
            this.pControls.ForeColor = colorTable.FormTextColor;
            this.toolStrip.BackColor = colorTable.FormBackground;

            string styleSheet = Properties.Resources.StyleSheet;

            styleSheet = styleSheet.Replace("000001", $"{colorTable.FormBackground.R:X2}{ colorTable.FormBackground.G:X2}{ colorTable.FormBackground.B:X2}");
            styleSheet = styleSheet.Replace("000002", $"{colorTable.ActiveFormBorderColor.R:X2}{ colorTable.ActiveFormBorderColor.G:X2}{ colorTable.ActiveFormBorderColor.B:X2}");
            styleSheet = styleSheet.Replace("000003", $"{colorTable.FormTextColor.R:X2}{ colorTable.FormTextColor.G:X2}{ colorTable.FormTextColor.B:X2}");
            styleSheet = styleSheet.Replace("000004", $"{colorTable.ActiveTitleGradientEnd.R:X2}{ colorTable.ActiveTitleGradientEnd.G:X2}{ colorTable.ActiveTitleGradientEnd.B:X2}");

            string rideRecap = Properties.Resources.RideRecap;

            Logger.LogDebug($"{this.GetType()}::RideRecap_Load - \n{rideRecap}");

            this.webBrowser.BrowserControl.DocumentText = styleSheet + rideRecap;

            //if (lblEmailAddr.Text.Length > 0)
            //{
            //    lblEmailAddr.Text = ZAMsettings.Settings.CurrentUser.EmailAddress;
            //    m_hasEmailAddress = true;
            //}
            //else
            //{
            //    lblEmailAddr.Text = "Please set email address in your user profile to use this feature.";
            //}
        }

        private void tsbEmail_Click(object sender, EventArgs e)
        {

        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {

        }

        //private void Launch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    var link = (LinkLabel)sender;
        //    link.LinkVisited = true;
        //    ProcessStartInfo psInfo = new ProcessStartInfo
        //    {
        //        FileName = link.Text,
        //        UseShellExecute = true
        //    };
        //    Process.Start(psInfo);
        //}



        //private void pbZamCyclist_Click(object sender, EventArgs e)
        //{
        //    if (m_hasEmailAddress)
        //    {
        //        try
        //        {
        //            this.Cursor = Cursors.WaitCursor;
        //            pbZamCyclist.Cursor = Cursors.WaitCursor;
        //            MailMessage mail = new MailMessage();
        //            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

        //            mail.From = new MailAddress("ZwiftActivityMonitor@gmail.com");
        //            mail.To.Add(ZAMsettings.Settings.CurrentUser.EmailAddress);
        //            mail.Subject = "Zwift Activity Monitor Ride Recap";
        //            SmtpServer.Port = 587;
        //            SmtpServer.Credentials = new System.Net.NetworkCredential("ZwiftActivityMonitor@gmail.com", "kfnggjsetsfxghky");
        //            SmtpServer.EnableSsl = true;

        //            StringBuilder s = new StringBuilder();
        //            s.Append("\n");
        //            s.Append($"Duration: {lblDuration.Text}\n");
        //            s.Append($"Distance: {lblDistance.Text}\n");
        //            s.Append($"Avg Speed: {lblAvgSpeed.Text}\n");
        //            s.Append($"Avg Power: {lblAvgPower.Text}\n");
        //            s.Append($"NP: {lblNp.Text}\n");
        //            s.Append($"IF: {lblIf.Text}\n");
        //            s.Append($"TSS: {lblTss.Text}\n");

        //            mail.Body = s.ToString();

        //            SmtpServer.Send(mail);

        //            MessageBox.Show($"Ride recap email sent to {ZAMsettings.Settings.CurrentUser.EmailAddress}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ride recap email failed. Reason: {ex.Message}", "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //        finally
        //        {
        //            this.Cursor = Cursors.Default;
        //            pbZamCyclist.Cursor = Cursors.Hand;
        //        }
        //    }
        //}
    }
}
