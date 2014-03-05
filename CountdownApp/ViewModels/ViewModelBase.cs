using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CountdownApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // NotifyPropertyChanged(() => this.Items);
        public void NotifyPropertyChanged<T>(Expression<Func<T>> propertyName)
        {
            if (PropertyChanged != null)
            {
                var memberExpression = propertyName.Body as MemberExpression;
                if (memberExpression != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs(memberExpression.Member.Name));
                }
            }
        }
    }
}
