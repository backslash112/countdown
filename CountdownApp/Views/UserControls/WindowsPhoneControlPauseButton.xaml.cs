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
    public partial class WindowsPhoneControlPauseButton : UserControl
    {
        public WindowsPhoneControlPauseButton()
        {
            InitializeComponent();
        }
        public event EventHandler<RoutedEventArgs> Click;

        private void PauseButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }
    }
}
