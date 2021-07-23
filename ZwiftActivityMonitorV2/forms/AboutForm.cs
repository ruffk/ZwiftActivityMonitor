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
    public partial class AboutForm : Form
    {
        private System.Drawing.Point m_offset;                      // for moving window
        private bool m_mouseDown;                                   // for moving window

        public AboutForm()
        {
            InitializeComponent();

            string[] proSuperScript = { "\u1D3E", "\u1D3F", "\u1D3C" };
            this.lblName.Text += $" {proSuperScript[0]}{proSuperScript[1]}{proSuperScript[2]}";


            // This rounds the edges of the borderless window
            this.Region = System.Drawing.Region.FromHrgn(ZAMsettings.CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblProductVersion.Text = version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
        }

        private void pbEnjoyFitness_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psInfo = new ProcessStartInfo
            {
                FileName = linkProjectSponsor.Text,
                UseShellExecute = true
            };
            Process.Start(psInfo);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
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
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
