using System;
using System.Windows.Threading;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using ZwiftPacketMonitor;

namespace ZwiftActivityMonitor
{
    public partial class SystemOptions : Form, IWinFormsShell
    {
        public SystemOptions()
        {
            InitializeComponent();
        }
    }
}
