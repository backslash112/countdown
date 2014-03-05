using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CountdownApp
{
    public partial class WindowsPhoneControlStopButton : UserControl
    {
        public WindowsPhoneControlStopButton()
        {
            InitializeComponent();
        }

        private void StopButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (StopClick != null)
            {
                StopClick(this, e);
            }
        }

        private void ContinueButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ContinueClick != null)
            {
                ContinueClick(this, e);
            }
        }

        public event EventHandler<RoutedEventArgs> StopClick;
        public event EventHandler<RoutedEventArgs> ContinueClick;
    }
}
