using CountdownApp.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownApp
{

    public class SzoozeTimes
    {
        public static string OneMinute = AppResources.OneMinute;
        public static string FiveMinutes = AppResources.FiveMinutes;
        public static string TenMinutes = AppResources.TenMinutes;
        public static string OneHour = AppResources.OneHour;
        public static string FourHours = AppResources.FourHours;


        public static string[] SzoozeTimeArray = new string[] 
        {
            OneMinute,
            FiveMinutes, 
            TenMinutes, 
            OneHour, 
            FourHours
        };

        public static int GetSeconds(string szoozeTime)
        {
            int value = 0;

            if (szoozeTime.Equals(AppResources.OneMinute))
            {
                value = 1 * 60;
            }
            else if (szoozeTime.Equals(AppResources.FiveMinutes))
            {
                value = 5 * 60;
            }
            else if (szoozeTime.Equals(AppResources.TenMinutes))
            {
                value = 10 * 60;
            }
            else if (szoozeTime.Equals(AppResources.OneHour))
            {
                value = 60 * 60;
            }
            else if (szoozeTime.Equals(AppResources.FourHours))
            {
                value = 4 * 60 * 60;
            }

            return value;
        }
    };

}
