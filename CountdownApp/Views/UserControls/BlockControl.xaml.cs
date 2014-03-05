using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using CountdownApp.Resources;

namespace CountdownApp.Views
{
    public partial class BlockControl : UserControl
    {
        public BlockControl()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (App.IsTrial)
            {
                ShowTrialAlert();
            }
        }

        /// <summary>
        /// 显示试用版提示信息
        /// </summary>
        private void ShowTrialAlert()
        {
            if (MessageBox.Show(AppResources.DownloadFullVersionMsg, AppResources.DownloadFullVersionTitle, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                DownloadFullVersion();
            }
        }
        /// <summary>
        /// 去商店下载正式版
        /// </summary>
        private void DownloadFullVersion()
        {
            MarketplaceDetailTask marketPlaceDetailTask = new MarketplaceDetailTask();
            marketPlaceDetailTask.Show();
        }
   
    }
}
