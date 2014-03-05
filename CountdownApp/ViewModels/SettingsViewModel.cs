using CountdownApp.Managers;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private static SettingsViewModel instance;
        public static SettingsViewModel Instance
        {
            get { return instance ?? (instance = new SettingsViewModel()); }
        }

        private SettingsManager settingsManager;

        public bool IsRepeat
        {
            get { return settingsManager.IsRepeat; }
            set
            {
                settingsManager.IsRepeat = value;
                NotifyPropertyChanged(() => IsRepeat);
            }
        }

        public bool IsSecondSound
        {
            get { return settingsManager.IsSecondSound; }
            set
            {
                settingsManager.IsSecondSound = value;
                NotifyPropertyChanged(() => IsSecondSound);
            }
        }

        public bool IsAlwaysLight
        {
            get { return settingsManager.IsAlwaysLight; }
            set
            {
                settingsManager.IsAlwaysLight = value;
                NotifyPropertyChanged(() => IsAlwaysLight);

                if (settingsManager.IsAlwaysLight)
                {
                    PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
                }
                else
                {
                    PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
                }
            }
        }

        public bool IsSaveLastTime
        {
            get { return settingsManager.IsSaveLastTime; }
            set
            {
                settingsManager.IsSaveLastTime = value;
                NotifyPropertyChanged(() => IsSaveLastTime);
            }
        }

        public bool IsExitConfirm
        {
            get
            {
                return settingsManager.IsExitConfirm;
            }
            set
            {
                settingsManager.IsExitConfirm = value;
                NotifyPropertyChanged(() => IsExitConfirm);
            }
        }
        public TimeSpan DefaultTime
        {
            get { return settingsManager.DefaultTime; }
            set
            {
                if (settingsManager.DefaultTime == value)
                {
                    return;
                }
                settingsManager.DefaultTime = value;
                NotifyPropertyChanged(() => DefaultTime);
            }
        }

        private SettingsViewModel()
        {
            settingsManager = SettingsManager.Instance;
        }
    }
}
