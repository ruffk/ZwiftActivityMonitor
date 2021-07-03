using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZwiftActivityMonitorV2
{
    /// <summary>
    /// Since the DataGridView is getting updated on non-gui threads, we're using a syncronized binding source to marshall the updates.  See link for details.
    /// https://stackoverflow.com/questions/32885552/update-elements-in-bindingsource-via-separate-task
    /// </summary>
    public class SyncBindingSource : BindingSource
    {
        private SynchronizationContext syncContext;
        public SyncBindingSource()
        {
            syncContext = SynchronizationContext.Current;
        }
        public SyncBindingSource(object dataSource, string dataMember) : base(dataSource, dataMember)
        {
            syncContext = SynchronizationContext.Current;
        }
        public SyncBindingSource(IContainer container) : base(container)
        {
            syncContext = SynchronizationContext.Current;
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (syncContext != null)
                syncContext.Send(_ => base.OnListChanged(e), null);
            else
                base.OnListChanged(e);
        }
    }
}
