using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownApp.Common
{
    public class AppCommon
    {
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        private static AppCommon instance;
        private int? preSound;
        private bool? isRepeat;
        private bool? isSecondSound;

        private const string PRESELECTEDTIME = "PreSelectedTime";
        private const string PRESOUND = "preSound";
        private const string ISREPEAT = "IsRepeat";
        private const string ISSECONDSOUND = "IsSecondSound";

        public static AppCommon Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppCommon();
                }
                return AppCommon.instance;
            }
        }

        /// <summary>
        /// 上次选择的时间
        /// </summary>
        //public TimeSpan? PreSelectedTime
        //{
        //    get
        //    {
        //        if (settings.Contains(PRESELECTEDTIME))
        //        {
        //            preSelectedTime = (TimeSpan)settings[PRESELECTEDTIME];
        //        }
        //        return preSelectedTime;
        //    }
        //    set
        //    {
        //        preSelectedTime = value;
        //        settings[PRESELECTEDTIME] = value;
        //    }
        //}

        /// <summary>
        /// 循环播放
        /// </summary>
        public bool? IsRepeat
        {
            get
            {
                if (settings.Contains(ISREPEAT))
                {
                    isRepeat = (bool)settings[ISREPEAT];
                }
                return isRepeat;
            }
            set
            {
                isRepeat = value;
                settings[ISREPEAT] = value;
            }
        }

        /// <summary>
        /// 秒音效
        /// </summary>
        public bool? IsSecondSound
        {
            get
            {
                if (settings.Contains(ISSECONDSOUND))
                {
                    isSecondSound = (bool)settings[ISSECONDSOUND];
                }
                return isSecondSound;
            }
            set
            {
                isSecondSound = value;
                settings[ISSECONDSOUND] = value;
            }
        }

        /// <summary>
        /// 上次选择的提示音
        /// </summary>
        public int? PreSound
        {
            get
            {
                if (settings.Contains(PRESOUND))
                {
                    preSound = (int)settings[PRESOUND];
                }
                return preSound;
            }
            set
            {
                preSound = value;
                settings[PRESOUND] = value;
            }
        }
    }
}
