using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTechniqueApp.ViewModels
{
    public class ApplicationSettingsServices
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        string rateStatusStr = "rate";
        string rateTimeStr = "rateDateTime";

        /// <summary>
        /// 获取 ApplicationSettings 的 rateDateTime 值，如果没有则返回 null。
        /// </summary>
        /// <returns></returns>
        public DateTime GetRateTime()
        {
            if (!settings.Contains(rateTimeStr))
                throw new NullReferenceException();
            return (DateTime)settings[rateTimeStr];
        }

        public void SetRateTimeAsNow()
        {
            settings[rateTimeStr] = DateTime.Now;
        }

        public void SetRateStatus(ApplicationSettingsStatus status)
        {
            settings[rateStatusStr] = status;
        }
        public ApplicationSettingsStatus GetRateStatus()
        {
            if (settings.Contains(rateStatusStr))
                return (ApplicationSettingsStatus)settings[rateStatusStr];
            return ApplicationSettingsStatus.NeverSet;
        }
    }
}
