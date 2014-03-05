using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CountdownApp.UserControls
{
    public partial class WindowsPhoneControlStartButton : UserControl
    {
        public WindowsPhoneControlStartButton()
        {
            InitializeComponent();
        }
        public event EventHandler<RoutedEventArgs> Click;
        private void StartButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }
    }
}
