using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public partial class StatusViewerControlEx : ViewerControlEx
    {
        private string mDismissBtnBaseText;
        private int mStatusViewerDuration;

        private readonly ILogger<StatusViewerControlEx> Logger;

        private Form mParentForm;
        private bool mIsStatusInitialized;

        public static bool IsVisible { get; internal set; }

        public StatusViewerControlEx()
        {
            InitializeComponent();

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<StatusViewerControlEx>();

            this.mDismissBtnBaseText = this.btnAutoDismiss.Text;
        }

        public string DocumentText
        {
            get { return webBrowserControl.webBrowser.DocumentText; }
            set { webBrowserControl.webBrowser.DocumentText = value; }
        }

        private void btnAutoDismiss_Click(object sender, EventArgs e)
        {
            this.HideStatus();
        }

        private void HideStatus()
        {
            Logger.LogDebug($"{this.GetType()}::HideStatus");

            this.timer1.Enabled = false;
            this.Hide();

            Debug.Assert(this.mParentForm.Controls.Contains(this), $"Status control not found in form's controls.");
            this.mParentForm.Controls.Remove(this);

            IsVisible = false;
        }

        /// <summary>
        /// This needs to be called on the UI thread
        /// </summary>
        /// <param name="form"></param>
        public virtual void InitializeStatus(Form form)
        {
            Debug.Assert(!this.mIsStatusInitialized, $"InitializeStatus already called.");

            this.mParentForm = form;
            this.mParentForm.Controls.Add(this);

            MSoffice2010ColorManager colorTable = ZAMappearance.GetColorTable();

            this.tlPanel.BackColor = colorTable.FormBackground;
            this.pStatus.BackColor = colorTable.FormBackground;
            this.pStatus.ForeColor = colorTable.FormTextColor;

            this.mStatusViewerDuration = 10;
            this.btnAutoDismiss.Text = this.mDismissBtnBaseText;
            this.mIsStatusInitialized = true;
        }


        public void ShowStatus()
        {
            Debug.Assert(this.mIsStatusInitialized, $"InitializeStatus not called.");

            this.Dock = DockStyle.Fill;
            this.BringToFront();
            this.Show();

            this.timer1.Enabled = true;

            IsVisible = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.mStatusViewerDuration > 0)
            {
                if (--this.mStatusViewerDuration <= 0)
                {
                    this.HideStatus();
                }
                this.btnAutoDismiss.Text = $"{this.mDismissBtnBaseText} {this.mStatusViewerDuration}";
            }
        }
    }
}
