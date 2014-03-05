using CountdownApp.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownApp.Managers
{
    public class SettingsManager
    {
        readonly IsolatedStorageManager isolatedStorageManager;
        private bool isRepeat;
        private bool isSecondSound;
        private bool isSaveLastTime;
        private TimeSpan? lastTime;
        private TimeSpan defaultTime;
        private bool isExitConfirm;
        private bool isAlwaysLight;



        public bool IsRepeat
        {
            get { return isRepeat; }
            set
            {
                if (isRepeat == value)
                {
                    return;
                }
                Logger.Log("isRepeat: " + isRepeat);
                isRepeat = value;
                isolatedStorageManager.Set<bool>(Strings.ISREPEAT, isRepeat);
            }
        }

        public bool IsSecondSound
        {
            get { return isSecondSound; }
            set
            {
                if (isSecondSound == value)
                {
                    return;
                }
                Logger.Log("isSecondSound: " + isSecondSound);
                isSecondSound = value;
                isolatedStorageManager.Set<bool>(Strings.ISSECONDSOUND, isSecondSound);
            }
        }

        public bool IsSaveLastTime
        {
            get { return isSaveLastTime; }
            set
            {
                if (isSaveLastTime == value)
                {
                    return;
                }
                isSaveLastTime = value;
                isolatedStorageManager.Set<bool>(Strings.ISSAVELASTTIME, isSaveLastTime);
            }
        }

        public TimeSpan? LastTime
        {
            get { return lastTime; }
            set
            {
                if (lastTime == value)
                {
                    return;
                }
                lastTime = value;
                isolatedStorageManager.Set<TimeSpan?>(Strings.LASTTIME, lastTime);
            }
        }

        public bool IsExitConfirm
        {
            get { return isExitConfirm; }
            set
            {
                if (isExitConfirm == value)
                {
                    return;
                }
                isExitConfirm = value;
                isolatedStorageManager.Set<bool>(Strings.ISEXITCONFIRM, isExitConfirm);
            }
        }
        public bool IsAlwaysLight
        {
            get { return isAlwaysLight; }
            set
            {
                if (isAlwaysLight == value)
                {
                    return;
                }
                isAlwaysLight = value;
                isolatedStorageManager.Set<bool>(Strings.ISALWAYSLIGHT, isAlwaysLight);
            }
        }

        public TimeSpan DefaultTime
        {
            get { return defaultTime; }
            set
            {
                if (defaultTime == value)
                {
                    return;
                }
                defaultTime = value;
                isolatedStorageManager.Set<TimeSpan>(Strings.DEFAULTTIME, defaultTime);
            }
        }

        private static SettingsManager instance;

        public static SettingsManager Instance
        {
            get
            {
                return instance ?? (instance = new SettingsManager());
            }
        }

        public SettingsManager()
        {
            isolatedStorageManager = IsolatedStorageManager.Instance;

            InitSettings();

            isRepeat = isolatedStorageManager.Get<bool>(Strings.ISREPEAT);
            isSecondSound = isolatedStorageManager.Get<bool>(Strings.ISSECONDSOUND);
            isAlwaysLight = isolatedStorageManager.Get<bool>(Strings.ISALWAYSLIGHT);
            isSaveLastTime = isolatedStorageManager.Get<bool>(Strings.ISSAVELASTTIME);
            if (isSaveLastTime)
            {
                lastTime = isolatedStorageManager.Get<TimeSpan?>(Strings.LASTTIME);
            }
            isExitConfirm = isolatedStorageManager.Get<bool>(Strings.ISEXITCONFIRM);
            defaultTime = isolatedStorageManager.Get<TimeSpan>(Strings.DEFAULTTIME);
        }

        public void InitSettings()
        {
            if (isolatedStorageManager.Contains(Strings.ISSECONDSOUND))
            {
                return;
            }
            Logger.Log("InitSettings();");
            isolatedStorageManager.Set<bool>(Strings.ISSECONDSOUND, true);
            isolatedStorageManager.Set<bool>(Strings.ISEXITCONFIRM, true);
            isolatedStorageManager.Set<bool>(Strings.ISALWAYSLIGHT, true);
            isolatedStorageManager.Set<bool>(Strings.ISSAVELASTTIME, true);
            isolatedStorageManager.Set<TimeSpan>(Strings.DEFAULTTIME, new TimeSpan(0, 0, 10));
        }
    }
}
