using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CountdownApp.ViewModels;

namespace CountdownApp
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            DataContext = SettingsViewModel.Instance;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.IsTrial)
            {
                BlockControl.Visibility = Visibility.Visible;
            }
            else
            {
                BlockControl.Visibility = Visibility.Collapsed;
            }
        }
    }
}