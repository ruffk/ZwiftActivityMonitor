using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZwiftActivityMonitorV2
{
    public interface IStatusViewer
    {
        public string DocumentText { get; set; }
    }
    public partial class StatusViewerControlEx : ViewerControlEx, IStatusViewer
    {
        public StatusViewerControlEx()
        {
            InitializeComponent();
        }

        public string DocumentText
        {
            get { return webBrowserControl.webBrowser.DocumentText; }
            set { webBrowserControl.webBrowser.DocumentText = value; }
        }
    }
}
