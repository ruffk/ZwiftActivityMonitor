using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ZwiftActivityMonitorV2
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Conditionally updates a property only if the value has actually changed.  If so, NotifyPropertyChanged is also called.
        /// This is to save on updates to the DataGridView.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="valueToSet"></param>
        /// <param name="propertyName"></param>
        protected bool SetProperty<T>(ref T property, T valueToSet, [CallerMemberName] string propertyName = "")
        {
            if (property == null || !property.Equals(valueToSet))
            {
                //Debug.WriteLine($"SetProperty<T> NOT EQUAL - Name: {propertyName}, Type: {typeof(T)} Current: {property}, New: {valueToSet}");

                property = valueToSet;
                this.NotifyPropertyChanged(propertyName);
                return true;
            }
            //else Debug.WriteLine($"SetProperty<T> EQUAL - Name: {propertyName}, Type: {typeof(T)} Current: {property}, New: {valueToSet}");
            return false;
        }
    }
}
