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
using System.Net;

namespace ZwiftActivityMonitorV2
{
    public partial class RideRecap : Syncfusion.WinForms.Controls.SfForm
    {
        private RideRecapMetrics mRideRecapMetrics;
        private UserProfile CurrentUserProfile { get { return ZAMsettings.Settings.CurrentUser; } }
        private string mRideRecapHtml;

        private readonly ILogger<RideRecap> Logger;

        public RideRecap(RideRecapMetrics rideRecapMetrics)
        {
            mRideRecapMetrics = rideRecapMetrics;

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

            mRideRecapHtml = Properties.Resources.RideRecap;

            StringBuilder xAxis = new StringBuilder("");
            StringBuilder yAxis = new StringBuilder("");

            if (this.mRideRecapMetrics.Power.Length > 0)
            {
                foreach (var power in this.mRideRecapMetrics.Power)
                {
                    xAxis.Append($"'{DurationEnum.Instance.GetText(power.DurationType)}',");
                    yAxis.Append($"{power.APwattsMax},");
                }
            }
            else
            {
                // no power values, build an empty chart
                foreach(var item in DurationEnum.Instance.GetValues())
                {
                    xAxis.Append($"'{item}',");
                }
            }

            string chartDef = "{type:'line',data:{labels:[xAxisValues],datasets:[{fill:true,lineTension:0,backgroundColor:'#000004',borderColor:'#000002',data:[yAxisValues],}]},options:{legend:{display:false},title:{display:true,text:'Peak Power',fontSize:20,fontFamily:'Segoe UI, sans-serif',fontColor:'#000003',},scales:{yAxes:[{ticks:{fontFamily:'Segoe UI, sans-serif',fontColor: '#000003'},scaleLabel:{display:true,labelString:'Watts',fontSize:16,fontFamily:'Segoe UI, sans-serif',fontColor:'#000003',},gridLines:{display:false,}}],xAxes:[{ticks:{fontFamily:'Segoe UI, sans-serif',fontColor: '#000003'},scaleLabel:{display:false,labelString:'Time',fontSize:16,fontFamily:'Segoe UI, sans-serif',fontColor:'#000003',},gridLines:{display:false,}}],}}}";
            chartDef = chartDef.Replace("xAxisValues", xAxis.ToString());
            chartDef = chartDef.Replace("yAxisValues", yAxis.ToString());

            // Define and encode the chart URL
            string urlStr = WebUtility.UrlEncode(chartDef);

            Debug.WriteLine(urlStr);

            mRideRecapHtml = mRideRecapHtml.Replace("EncodedChartUrl", urlStr);


            mRideRecapHtml = mRideRecapHtml.Replace("000001", $"{colorTable.FormBackground.R:X2}{ colorTable.FormBackground.G:X2}{ colorTable.FormBackground.B:X2}");
            mRideRecapHtml = mRideRecapHtml.Replace("000002", $"{colorTable.ActiveFormBorderColor.R:X2}{ colorTable.ActiveFormBorderColor.G:X2}{ colorTable.ActiveFormBorderColor.B:X2}");
            mRideRecapHtml = mRideRecapHtml.Replace("000003", $"{colorTable.FormTextColor.R:X2}{ colorTable.FormTextColor.G:X2}{ colorTable.FormTextColor.B:X2}");
            mRideRecapHtml = mRideRecapHtml.Replace("000004", $"{colorTable.ActiveTitleGradientEnd.R:X2}{ colorTable.ActiveTitleGradientEnd.G:X2}{ colorTable.ActiveTitleGradientEnd.B:X2}");

            //public TimeSpan Duration { get; set; }
            //public double DistanceKm { get; set; }
            //public double DistanceMi { get; set; }
            //public double AverageKph { get; set; }
            //public double AverageMph { get; set; }
            //public int APwatts { get; set; }
            //public double? APwattsPerKg { get; set; }
            //public int NPwatts { get; set; }
            //public double? NPwattsPerKg { get; set; }
            //public double? IntensityFactor { get; set; } // null if FTP not set
            //public int? TrainingStressScore { get; set; } // null if FTP not set

            mRideRecapHtml = mRideRecapHtml.Replace("Duration", $"{this.mRideRecapMetrics.Duration.ToString("hh\\:mm\\:ss")}");
            mRideRecapHtml = mRideRecapHtml.Replace("DistanceKm", $"{this.mRideRecapMetrics.DistanceKm:0.0}");
            mRideRecapHtml = mRideRecapHtml.Replace("DistanceMi", $"{this.mRideRecapMetrics.DistanceMi:0.0}");
            mRideRecapHtml = mRideRecapHtml.Replace("AverageKph", $"{this.mRideRecapMetrics.AverageKph:0.0}");
            mRideRecapHtml = mRideRecapHtml.Replace("AverageMph", $"{this.mRideRecapMetrics.AverageMph:0.0}");
            mRideRecapHtml = mRideRecapHtml.Replace("APwattsPerKg", $"{(this.mRideRecapMetrics.APwattsPerKg.HasValue ? this.mRideRecapMetrics.APwattsPerKg.Value.ToString("0.00") : "")}");
            mRideRecapHtml = mRideRecapHtml.Replace("APwatts", $"{this.mRideRecapMetrics.APwatts}");
            mRideRecapHtml = mRideRecapHtml.Replace("NPwattsPerKg", $"{(this.mRideRecapMetrics.NPwattsPerKg.HasValue ? this.mRideRecapMetrics.NPwattsPerKg.Value.ToString("0.00") : "")}");
            mRideRecapHtml = mRideRecapHtml.Replace("NPwatts", $"{this.mRideRecapMetrics.NPwatts}");
            mRideRecapHtml = mRideRecapHtml.Replace("IntensityFactor", $"{(this.mRideRecapMetrics.IntensityFactor.HasValue ? this.mRideRecapMetrics.IntensityFactor.Value.ToString(".00") : "")}");
            mRideRecapHtml = mRideRecapHtml.Replace("TrainingStressScore", $"{(this.mRideRecapMetrics.TrainingStressScore.HasValue ? this.mRideRecapMetrics.TrainingStressScore.Value : "")}");

            StringBuilder powerStr = new StringBuilder("");
            foreach (var power in this.mRideRecapMetrics.Power)
            {
                powerStr.AppendLine("<tr>");
                powerStr.AppendLine($"<td>{DurationEnum.Instance.GetText(power.DurationType)}</td>");
                powerStr.AppendLine($"<td>{power.APwattsMax}</td>");
                powerStr.AppendLine($"<td>{(power.APwattsKgMax.HasValue ? power.APwattsKgMax.Value.ToString("0.00") : "")}</td>");
                powerStr.AppendLine("</tr>");
            }

            mRideRecapHtml = mRideRecapHtml.Replace("RideRecapPower", powerStr.ToString());


            /*
            <tr>
                <td>1</td>
                <td>1:45:43</td>
                <td>34.5 km/h (25.3 mi/h)</td>
                <td>21.0 km (18.3 mi)</td>
                <td>245 (3.2 w/kg)</td>
                <td>05:45:43</td>
            </tr>
            */

            StringBuilder lapStr = new StringBuilder("");
            foreach (var lap in this.mRideRecapMetrics.Laps)
            {
                lapStr.AppendLine("<tr>");
                lapStr.AppendLine($"<td>{lap.LapNumber}</td>");
                lapStr.AppendLine($"<td>{lap.LapTime.ToString("hh\\:mm\\:ss")}</td>");
                lapStr.AppendLine($"<td>{lap.LapSpeedKph:0.0} km/h ({lap.LapSpeedMph:0.0} mi/h)</td>");
                lapStr.AppendLine($"<td>{lap.LapDistanceKm:0.0} km ({lap.LapDistanceMi:0.0} mi)</td>");
                lapStr.AppendLine($"<td>{lap.LapAPwatts} w{(lap.LapAPwattsPerKg.HasValue ? " (" + lap.LapAPwattsPerKg.Value.ToString("0.00") + " w/kg)" : "")}</td>");
                lapStr.AppendLine($"<td>{lap.TotalTime.ToString("hh\\:mm\\:ss")}</td>");
                lapStr.AppendLine("</tr>");
            }

            mRideRecapHtml = mRideRecapHtml.Replace("RideRecapLaps", lapStr.ToString());

            /*
            <tr>
                <td>1</td>
                <td>1:45:43</td>
                <td>34.5 km/h (25.3 mi/h)</td>
                <td>21.0 km (18.3 mi)</td>
                <td>05:45:43</td>
                <td>-01:43</td>
            </tr>
            */

            StringBuilder splitStr = new StringBuilder("");
            foreach (var split in this.mRideRecapMetrics.Splits)
            {
                string delta;
                //string deltaColorCode;
                if (split.DeltaTime.HasValue)
                {
                    TimeSpan std = (TimeSpan)split.DeltaTime;
                    bool negated = false;

                    if (std.TotalSeconds < 0)
                    {
                        std = std.Negate();
                        negated = true;
                    }

                    delta = $"{(negated ? "-" : "+")}{(std.Minutes > 0 ? std.ToString("m'@QT's'\"'").Replace("@QT", "\'") : std.ToString("s'\"'"))}";
                    //deltaColorCode = negated ? "00C000" : "C00000";

                }
                else
                {
                    delta = "No Goal";
                }

                splitStr.AppendLine("<tr>");
                splitStr.AppendLine($"<td>{split.SplitNumber}</td>");
                splitStr.AppendLine($"<td>{split.SplitTime.ToString("hh\\:mm\\:ss")}</td>");
                splitStr.AppendLine($"<td>{split.SplitSpeedKph:0.0} km/h ({split.SplitSpeedMph:0.0} mi/h)</td>");
                splitStr.AppendLine($"<td>{split.SplitDistanceKm:0.0} km ({split.SplitDistanceMi:0.0} mi)</td>");
                splitStr.AppendLine($"<td>{split.TotalTime.ToString("hh\\:mm\\:ss")}</td>");
                splitStr.AppendLine($"<td>{delta}</td>");
                splitStr.AppendLine("</tr>");
            }

            mRideRecapHtml = mRideRecapHtml.Replace("RideRecapSplits", splitStr.ToString());

            Logger.LogDebug($"{this.GetType()}::RideRecap_Load - \n{mRideRecapHtml}");

            this.webBrowser.BrowserControl.DocumentText = styleSheet + mRideRecapHtml;

            tsbEmail.ToolTipText = this.CurrentUserProfile.EmailAddress.Length > 0 ? "MailTo:" + this.CurrentUserProfile.EmailAddress : "Please set email address in your user profile to use this feature.";
        }

        private void tsbEmail_Click(object sender, EventArgs e)
        {
            if (this.CurrentUserProfile.EmailAddress.Length > 0)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("ZwiftActivityMonitor@gmail.com");
                    mail.To.Add(ZAMsettings.Settings.CurrentUser.EmailAddress);
                    mail.Subject = "Zwift Activity Monitor Ride Recap";
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("ZwiftActivityMonitor@gmail.com", ZAMsettings.Settings.EmailPassword);
                    SmtpServer.EnableSsl = true;

                    mail.Body = this.webBrowser.BrowserControl.DocumentText;

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
                }
            }

        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {
            this.webBrowser.BrowserControl.ShowPrintPreviewDialog();
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
