using CountdownApp.Common;
using CountdownApp.Managers;
using CountdownApp.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CountdownApp.ViewModels
{
    /// <summary>
    /// 正在即时中 ViewModel逻辑。
    /// </summary>
    public class TimingViewModel : ViewModelBase, IDisposable
    {
        #region Private Fileds

        private PhoneApplicationPage page;
        public bool IsRepeat
        {
            get { return SettingsManager.Instance.IsRepeat; }
        }
        public bool IsSecondSound
        {
            get { return SettingsManager.Instance.IsSecondSound; }
        }
        readonly string secSoundFileUri = "Sounds/sec.wav";
        SoundService secondSoundService;
        readonly string repeatTimeOutFileUri = "Sounds/Alarm1.wav";
        SoundService repeatTimeOutService;
        private string showTime;
        Timer timer;
        
        #endregion

        #region Public Properties

        public string ShowTime
        {
            get { return showTime; }
            set
            {
                showTime = value;
                NotifyPropertyChanged("ShowTime");
            }
        }

        private bool isPausing;

        public bool IsPausing
        {
            get { return isPausing; }
            set
            {
                isPausing = value;
                NotifyPropertyChanged(() => IsPausing);
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                {
                    return;
                }
                name = value;
                NotifyPropertyChanged(() => Name);
            }
        }
        #endregion

        #region Events

        public static event Action TimeOut;
        /// <summary>
        /// 后台运行，时间到达。
        /// </summary>
        public event Action BackgroundTimeOut;
        private static object locker = new object();
        private DateTime inputCountdownTime;

        public DateTime InputCountdownTime
        {
            get { return inputCountdownTime; }
        }
        #endregion

        #region Public Methods

        public void Timekeeping(DateTime input)
        {
            bool reset = false;
            ResetTime(input);

            timer = new Timer(new TimerCallback(param =>
            {
                if (reset)
                {
                    ResetTime(input);
                    reset = false;
                    return;
                }

                if (IsSecondSound)
                {
                    ThreadPool.QueueUserWorkItem(stateInfo =>
                    {
                        secondSoundService.Play();
                    });
                }

                inputCountdownTime = inputCountdownTime.AddSeconds(-1);

                page.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ShowTime = inputCountdownTime.ToString("HH:mm:ss");
                }));

                if (ShowTime.Equals("00:00:01") || ShowTime.Equals("00:00:00"))
                {
                    if (IsRepeat)
                    {
                        ThreadPool.QueueUserWorkItem(stateInfo =>
                        {
                            repeatTimeOutService.Play();
                            Thread.Sleep(1000);
                            repeatTimeOutService.Stop();
                        });
                        reset = true;
                    }
                    else
                    {
                        Stop();
                        page.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (page.NavigationService.CanGoBack)
                                page.NavigationService.GoBack();

                            //时间到，取消订阅。
                            if (TimeOut != null) TimeOut();
                            UnSubscribe();

                        }));

                        if (ScheduledActionService.Find("alarm") != null)
                        {
                            ScheduledActionService.Remove("alarm");
                        }
                    }
                }
            }));
            timer.Change(1000, 1000);
        }

        private void ResetTime(DateTime input)
        {
            inputCountdownTime = input;
            page.Dispatcher.BeginInvoke(new Action(() =>
            {
                ShowTime = inputCountdownTime.ToString("HH:mm:ss");
            }));
            if (IsSecondSound)
            {
                ThreadPool.QueueUserWorkItem(stateInfo =>
                {
                    secondSoundService.Play();
                });
            }
        }

        #endregion

        #region Constructor
        PhoneApplicationService phoneApplicationService;
        public TimingViewModel()
        {
            secondSoundService = new SoundService(secSoundFileUri);
            repeatTimeOutService = new SoundService(repeatTimeOutFileUri);
            phoneApplicationService = App.Current.ApplicationLifetimeObjects.OfType<PhoneApplicationService>().First();
            Subscribe();
        }

        private void Subscribe()
        {
            Logger.Log("TimingViewModel Subscribe");
            phoneApplicationService.Activated += phoneApplicationService_Activated;
            phoneApplicationService.Deactivated += phoneApplicationService_Deactivated;
        }

        private void phoneApplicationService_Deactivated(object sender, DeactivatedEventArgs e)
        {
            if (!IsRepeat)
                PhoneApplicationService.Current.State["LeftTime"] = DateTime.Now;


        }
        private void phoneApplicationService_Activated(object sender, ActivatedEventArgs e)
        {
            Logger.Log("TimingViewModel phoneApplicationService_Activated()");
            if (PhoneApplicationService.Current.State.ContainsKey("LeftTime"))
            {
                DateTime deactivated = (DateTime)PhoneApplicationService.Current.State["LeftTime"];
                DateTime now = DateTime.Now;
                lock (locker)
                {
                    DateTime dateTimeTemp = DateTime.Parse(this.inputCountdownTime.ToString());
                    if (!IsPausing) // 暂停状态下，恢复应用，不计算已经过去的时间。
                    {
                        var secondsPasd = (int)(now - deactivated).TotalSeconds;
                        inputCountdownTime = dateTimeTemp.AddSeconds(secondsPasd * -1);
                    }
                    Logger.Log("this.inputCountdownTime.Day != dateTimeTemp.Day " + (this.inputCountdownTime.Day != dateTimeTemp.Day).ToString());
                    if (this.inputCountdownTime.Day != dateTimeTemp.Day)
                    {

                        inputCountdownTime = new DateTime(1990, 1, 1, 0, 0, 0);
                        Stop();

                        // 后台运行时间到，要取消订阅，否则再点击开始会造成重复订阅。
                        if (BackgroundTimeOut != null) BackgroundTimeOut();
                        UnSubscribe();

                        new Thread(() =>
                        {
                            page.Dispatcher.BeginInvoke(new Action(() => { page.NavigationService.GoBack(); }));
                        }).Start();
                    }
                    ShowTime = inputCountdownTime.ToString("HH:mm:ss");
                }
            }
        }

        public TimingViewModel(DateTime inputCountdownTime, string name, PhoneApplicationPage page)
            : this()
        {
            this.page = page;
            this.name = name;
            Timekeeping(inputCountdownTime);
        }

        #endregion

        #region Command

        private ICommand stopCommand;
        public ICommand StopCommand
        {
            get
            {
                if (stopCommand == null)
                {
                    stopCommand = new RelayCommand(param => Stop(),
                        param => true);
                }
                return stopCommand;
            }
        }

        private ICommand pauseCommand;

        public ICommand PauseCommand
        {
            get
            {
                if (pauseCommand == null)
                {
                    pauseCommand = new RelayCommand(param => Pause());
                }
                return pauseCommand;
            }
        }

        private void Pause()
        {
            if (timer != null)
            {
                timer.Change(int.MaxValue, int.MaxValue);
            }
            IsPausing = true;
        }
        private ICommand continueCommand;

        public ICommand ContinueCommand
        {
            get
            {
                if (continueCommand == null)
                {
                    continueCommand = new RelayCommand(param => Continue());
                }
                return continueCommand;
            }
        }

        private void Continue()
        {
            if (timer != null)
            {
                timer.Change(1000, 1000);
            }
            IsPausing = false;
        }

        #endregion

        #region Private Methods

        private void Stop()
        {
            if (timer != null)
            {
                timer.Change(int.MaxValue, int.MaxValue);
            }
            // IsPausing = false;
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void UnSubscribe()
        {
            Logger.Log("TimingViewModel UnSubscribe");
            phoneApplicationService.Activated -= phoneApplicationService_Activated;
            phoneApplicationService.Deactivated -= phoneApplicationService_Deactivated;
        }
    }
}
