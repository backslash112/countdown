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
using System.Threading;
using System.Diagnostics;
using Microsoft.Phone.Scheduler;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Phone.BackgroundAudio;
using CountdownApp.Resources;
using CountdownApp.Models;
using CountdownApp.Common;

namespace CountdownApp
{
    public partial class TimingPage : PhoneApplicationPage
    {
        PhoneApplicationService phoneApplicationService =
            App.Current.ApplicationLifetimeObjects.OfType<PhoneApplicationService>().First();
        TimingViewModel timingViewModel;

        public TimingPage()
        {
            InitializeComponent();
            BuildApplicationBar();

            Subscribe();
            TimingViewModel.TimeOut += TimingViewModel_TimeOut;

        }

        void TimingViewModel_TimeOut()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            Logger.Log("TimingPage Subscribe");
            phoneApplicationService.Activated += phoneApplicationService_Activated;
            phoneApplicationService.Deactivated += phoneApplicationService_Deactivated;
        }

        void phoneApplicationService_Activated(object sender, ActivatedEventArgs e)
        {
            if (ScheduledActionService.Find("alarm") != null)
            {
                Logger.Log("ScheduledActionService.Remove(\"alarm\")");
                ScheduledActionService.Remove("alarm");
            }
        }

        private void phoneApplicationService_Deactivated(object sender, DeactivatedEventArgs e)
        {
            // 重复模式下，不添加到系统提示。
            if (timingViewModel.IsRepeat)
            {
                return;
            }
            Alarm alarm = new Alarm("alarm");
            var soundUri = SoundModel.GetSoundUriByID(soundId);
            if (string.IsNullOrEmpty(soundUri))
            {
                // alarm.sound 空白或滴
                alarm.Sound = new Uri("Sounds/BeepNone.wav", UriKind.Relative);
            }
            else
            {
                alarm.Sound = new Uri(soundUri, UriKind.Relative);
            }
            DateTime now = DateTime.Now;
            DateTime showTime = DateTime.Parse(timingViewModel.ShowTime);
            now = now.AddHours(showTime.Hour);
            now = now.AddMinutes(showTime.Minute);
            now = now.AddSeconds(showTime.Second);
            alarm.BeginTime = now;
            alarm.ExpirationTime = now.AddMinutes(5);

            alarm.Content = timingViewModel.InputCountdownTime.ToString("HH:mm:ss") + "\r\n" + timingViewModel.Name;
            try
            {
                if (alarm.BeginTime.Second <= 30 && alarm.BeginTime.Hour == DateTime.Now.Hour && alarm.BeginTime.Minute == DateTime.Now.Minute)
                {
                    alarm.BeginTime = alarm.BeginTime.AddMinutes(1);
                }

                if (ScheduledActionService.Find(alarm.Name) != null)
                {
                    ScheduledActionService.Remove(alarm.Name);
                    Logger.Log("ScheduledActionService.Replace(alarm);");
                }
                ScheduledActionService.Add(alarm);
                Logger.Log(" ScheduledActionService.Add(alarm);");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

        }

        // Helper function to build a localized ApplicationBar
        private void BuildApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            //ApplicationBar = new ApplicationBar();

            //// Create a new button and set the text value to the localized string from AppResources.
            //ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("Images/stop.png", UriKind.Relative));
            //appBarButton.Text = AppResources.StopMenu;
            //appBarButton.Click += AppBarBtnStop_Click_1;
            //ApplicationBar.Buttons.Add(appBarButton);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                string countdownTimeString = string.Empty;
                if (NavigationContext.QueryString.TryGetValue("countdownTime", out countdownTimeString))
                {
                    countdownTime = DateTime.Parse(countdownTimeString);
                }

                string soundIdString = string.Empty;
                if (NavigationContext.QueryString.TryGetValue("soundId", out soundIdString))
                {
                    soundId = int.Parse(soundIdString);
                }
                string name = string.Empty;
                NavigationContext.QueryString.TryGetValue("name", out name);
                // 从其他页面跳转过来之后，再创建ViewModel对象，因为 TimingViewModel 是有参构造函数。
                timingViewModel = new TimingViewModel(countdownTime, name, this);
                timingViewModel.BackgroundTimeOut += timingViewModel_BackgroundTimeOut;
                DataContext = timingViewModel;
            }
        }

        void timingViewModel_BackgroundTimeOut()
        {
            Unsubscribe();
        }
        int soundId;
        DateTime countdownTime = DateTime.Now;

        private void Unsubscribe()
        {
            Logger.Log("TimingPage Unsubscribe");
            phoneApplicationService.Activated -= phoneApplicationService_Activated;
            phoneApplicationService.Deactivated -= phoneApplicationService_Deactivated;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        ~TimingPage()
        {
        }

        private void OnContinueButton_Click(object sender, RoutedEventArgs e)
        {
            timingViewModel.ContinueCommand.Execute(null);
        }

        private void OnStopButton_Click(object sender, RoutedEventArgs e)
        {
            // UNDONE:停止倒计时
            timingViewModel.StopCommand.Execute(null);

            //取消订阅
            timingViewModel.UnSubscribe();

            // 返回 Main 界面
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }

            Unsubscribe();
        }

        private void OnPauseButton_Click(object sender, RoutedEventArgs e)
        {
            timingViewModel.PauseCommand.Execute(null);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.IsTrial) // 试用版，有广告，设置广告位显示。
            {
                sMAdBannerView.Visibility = Visibility.Visible;
            }
            else
            {
                sMAdBannerView.Visibility = Visibility.Collapsed;
            }
        }
    }
}