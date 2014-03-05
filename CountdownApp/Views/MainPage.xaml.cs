using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CountdownApp.Resources;
using CountdownApp.ViewModels;
using CountdownApp.Models;
using CountdownApp.Common;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.IO;
using System.Threading;
using Microsoft.Phone.Tasks;
using System.Diagnostics;
using CountdownApp.Managers;
using Windows.Phone.Devices.Notification;
using PomodoroTechniqueApp.ViewModels;

namespace CountdownApp
{
    public partial class CountdownMainPage : PhoneApplicationPage
    {
        MarketplaceReviewServices marketplaceReviewServices;
        public CountdownMainPage()
        {
            InitializeComponent();
            BuildApplicationBar();

            this.DataContext = new MainPageViewModel();

            TimingViewModel.TimeOut += OnTimeOut;

            marketplaceReviewServices =
               new MarketplaceReviewServices(this);
            marketplaceReviewServices.RateDays = 5;
#if DEBUG
            marketplaceReviewServices.RateDays = 1;
#endif
            marketplaceReviewServices.Run();

            LoadDefaultSettings();
        }

        /// <summary>
        /// 加载默认设置
        /// </summary>
        private void LoadDefaultSettings()
        {
            Logger.Log("UserIdleDetectionMode: " + PhoneApplicationService.Current.UserIdleDetectionMode);
            if (SettingsManager.Instance.IsAlwaysLight)
            {
                Logger.Log("Is Always Light.");
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
            LoadDefaultTime();

            if (SettingsManager.Instance.IsSaveLastTime)
            {
                LoadLastTime();
            }
        }

        private void LoadDefaultTime()
        {
            TimePickerCountdownTime.Value = SettingsManager.Instance.DefaultTime;
        }

        private void LoadLastTime()
        {
            if (SettingsManager.Instance.LastTime != null)
            {
                TimePickerCountdownTime.Value = SettingsManager.Instance.LastTime;
            }
        }

        /// <summary>
        /// 播放提示音对象
        /// </summary>
        SoundEffectInstance effectInstance;
        /// <summary>
        /// 振动对象
        /// </summary>
        VibrationDevice vibrationDevice;
        /// <summary>
        /// 振动线程
        /// </summary>
        Thread vibrationThread;
        /// <summary>
        /// 循环播放提示音
        /// </summary>
        bool loopPlay = false;
        void OnTimeOut()
        {
            ShowCustomMessageBox();
            loopPlay = true;

            if (soundId == -1) // 振动
            {
                vibrationDevice = VibrationDevice.GetDefault();
                vibrationThread = new Thread(() =>
                {
                    for (int i = 0; i < 15; i++)
                    {
                        vibrationDevice.Vibrate(TimeSpan.FromSeconds(1));
                        Thread.Sleep(3000);
                    }
                });
                vibrationThread.Start();
            }
            else
            {
                Ring();
            }
        }
        // 弹出框体现 方法
        private void ShowCustomMessageBox()
        {

            Grid grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.Margin = new Thickness(12, 15, 10, 30);

            TextBlock txtbTime = new TextBlock();
            txtbTime.Text = TimePickerCountdownTime.ValueString;
            Grid.SetRow(txtbTime, 0);

            TextBlock txtbName = new TextBlock();
            txtbName.Text = TextBoxName.Text;
            txtbName.TextWrapping = TextWrapping.Wrap;
            txtbName.TextTrimming = TextTrimming.WordEllipsis;
            Grid.SetRow(txtbName, 1);
            ListPicker listPicker = new ListPicker()
            {
                Header = AppResources.CustomerMessageBoxHeader,
                ItemsSource = SzoozeTimes.SzoozeTimeArray,
                Margin = new Thickness(0, 42, 14, 18)
            };
            Grid.SetRow(listPicker, 2);

            grid.Children.Add(txtbName);
            grid.Children.Add(txtbTime);
            grid.Children.Add(listPicker);

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = AppResources.ApplicationTitle,
                Content = grid,
                LeftButtonContent = AppResources.CustomerMessageBoxLeftButton,
                RightButtonContent = AppResources.CustomerMessageBoxRightButton,
                IsFullScreen = false
            };

            listPicker.SelectionChanged += listPicker_SelectionChanged;
            messageBox.Dismissed += messageBox_Dismissed;
            messageBox.Margin = new Thickness(0, -35, 0, 0);
            messageBox.Show();
        }

        // Szooze 列表变更事件 处理方法
        void listPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedSzoozeTime = (sender as ListPicker).SelectedItem.ToString();
        }

        // 用户选择的 Szooze
        string selectedSzoozeTime;

        // 提示框返回结果 处理方法
        void messageBox_Dismissed(object sender, DismissedEventArgs e)
        {
            if (effectInstance != null)
            {
                // 暂停提示音
                effectInstance.Stop();
            }

            if (vibrationThread != null)
            {
                // 关闭振动线程
                vibrationThread.Abort();
                // 暂停振动
                vibrationDevice.Cancel();
            }

            loopPlay = false;

            // 处理用户点击选择的按钮
            switch (e.Result)
            {
                case CustomMessageBoxResult.LeftButton:
                    var szoozeSeconds = SzoozeTimes.GetSeconds(selectedSzoozeTime);
                    var newSzoozeTime = DateTime.Parse("00:00:00").AddSeconds(szoozeSeconds);
                    new Thread(() =>
                    {
                        Thread.Sleep(500);
                        this.Dispatcher.BeginInvoke(() =>
                        {
                            NavigateToTimingPage(newSzoozeTime, TextBoxName.Text);
                        });
                    }).Start();
                    break;

                default:
                    break;
            }
        }
        private void NavigateToTimingPage(DateTime time, string name)
        {
            NavigationService.Navigate(new Uri("/Views/TimingPage.xaml?countdownTime=" + time +
            "&soundId=" + soundId + "&name=" + name, UriKind.Relative));
        }

        // 提示音提醒 方法
        private void Ring()
        {
            Stream stream = TitleContainer.OpenStream(SoundModel.GetSoundUriByID(soundId));
            effectInstance = SoundEffect.FromStream(stream).CreateInstance();
            FrameworkDispatcher.Update();
            int times = 20;
            new Thread(new ParameterizedThreadStart(param =>
            {
                do
                {
                    if (effectInstance.State.Equals(SoundState.Stopped))
                    {
                        effectInstance.Play();

                        times--;
                    }
                    Thread.Sleep(2000);

                    if (times == 0)
                    {
                        break;
                    }

                    if (loopPlay == false)
                    {
                        break;
                    }
                } while (true);
            })).Start();
        }

        private int soundId = SoundModel.Sounds.First().ID;

        // Helper function to build a localized ApplicationBar
        private void BuildApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton settingsButton = new ApplicationBarIconButton(new Uri("Images/feature.settings.png", UriKind.Relative));
            settingsButton.Text = AppResources.SettingsText;
            settingsButton.Click += OnSettingsButton_Click;
            ApplicationBar.Buttons.Add(settingsButton);

            ApplicationBarMenuItem rateMenu = new ApplicationBarMenuItem(AppResources.RateUs);
            rateMenu.Click += RateMenuItem_Click;
            ApplicationBar.MenuItems.Add(rateMenu);

            ApplicationBarMenuItem contactUsMenu = new ApplicationBarMenuItem(AppResources.ContactUs);
            contactUsMenu.Click += contactUsMenu_Click;
            ApplicationBar.MenuItems.Add(contactUsMenu); 
            
            if (App.IsTrial)　// 试用版，有广告，添加购买完整版菜单
            {
                ApplicationBarMenuItem buyMenu = new ApplicationBarMenuItem(AppResources.BuyFullVersion);
                buyMenu.Click += BuyMenuItem_Click;
                ApplicationBar.MenuItems.Add(buyMenu);
            }
        }
        private void OnSettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings.xaml", UriKind.Relative));
        }

        void contactUsMenu_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(AppResources.SendEmailContentText, AppResources.SendEmailTitleText, MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                EmailComposeTask emailComposeTask = new EmailComposeTask();
                emailComposeTask.To = "countdownapp@outlook.com";
                emailComposeTask.Show();
            }
        }

        private void BuyMenuItem_Click(object sender, EventArgs e)
        {
            MarketplaceDetailTask marketPlaceDetailTask = new MarketplaceDetailTask();
            marketPlaceDetailTask.Show();
        }
        private void RateMenuItem_Click(object sender, EventArgs e)
        {
            marketplaceReviewServices.RateNow();
        }

        private void ImgPlay_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image img = sender as Image;
            soundId = int.Parse(img.Tag.ToString());
            (this.DataContext as MainPageViewModel).Play(soundId);

            e.Handled = true;
        }

        private void SoundsListPicker_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as MainPageViewModel).Stop();

            soundId = (this.SoundsListPicker.SelectedItem as SoundModel).ID;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimePickerCountdownTime.Value.ToString().Equals("00:00:00"))
            {
                MessageBox.Show(AppResources.ValidateTimeMsg, AppResources.ErrorTitleText, MessageBoxButton.OK);
                return;
            }
            NavigateToTimingPage(DateTime.Parse(TimePickerCountdownTime.ValueString), TextBoxName.Text);
            SettingsManager.Instance.LastTime = TimePickerCountdownTime.Value;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (SettingsManager.Instance.IsExitConfirm)
            {
                if (MessageBox.Show(AppResources.ExitConfirmMsg, AppResources.ExitText, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    this.ClearBackEntries();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }
        private void ClearBackEntries()
        {
            while (NavigationService.BackStack != null & NavigationService.BackStack.Count() > 0)
            {
                NavigationService.RemoveBackEntry();
            }
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //if (App.IsTrial)
            //{
            //    // BlockControl.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    // BlockControl.Visibility = Visibility.Collapsed;
            //}
        }

    }
}