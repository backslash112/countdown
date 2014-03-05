using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTechniqueApp.ViewModels
{
    /// <summary>
    /// 应用程序设置状态
    /// </summary>
    public enum ApplicationSettingsStatus
    {
        /// <summary>
        /// 从未设置
        /// </summary>
        NeverSet,
        /// <summary>
        /// 以后设置
        /// </summary>
        RateLater,
        /// <summary>
        /// 从不设置
        /// </summary>
        RateNever
    }
}
