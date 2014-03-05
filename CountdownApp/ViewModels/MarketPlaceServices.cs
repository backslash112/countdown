using CountdownApp.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PomodoroTechniqueApp.ViewModels
{
    /// <summary>
    /// 用来提醒用户评价应用。
    /// 用户将会选择稍后评价，立刻评价或者永不提醒。
    /// </summary>
    public class MarketplaceReviewServices : IDisposable
    {
        #region Fileds

        Control control;
        ApplicationSettingsServices appSettings;

        #endregion Fileds

        #region Properties

        /// <summary>
        /// 配置时间
        /// </summary>

        private int rateDays;

        public int RateDays
        {
            get { return rateDays; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("RateDays must be lager than 0.");
                }
                rateDays = value;
            }
        }


        #endregion Properties

        #region Constractor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="control">View 对象</param>
        public MarketplaceReviewServices(Control control)
        {
            appSettings = new ApplicationSettingsServices();
            this.control = control;
        }

        #endregion Constractor

        #region Public Methods

        public void Run()
        {
            new Thread(new ParameterizedThreadStart(param =>
            {
                if (rateDays == 0)
                {
                    throw new ArgumentOutOfRangeException("Before call the Run Method, the RateDays property must not be 0.");
                }
                Thread.Sleep(1000);
                var settingStatus = appSettings.GetRateStatus();
                switch (settingStatus)
                {
                    case ApplicationSettingsStatus.NeverSet:
                        appSettings.SetRateStatus(ApplicationSettingsStatus.RateLater);
                        appSettings.SetRateTimeAsNow();
                        break;

                    case ApplicationSettingsStatus.RateLater:
                        CheckRateTime();
                        break;

                    case ApplicationSettingsStatus.RateNever:
                        break;

                    default:
                        break;
                }

            })).Start();
        }


        public void RateNow()
        {
            appSettings.SetRateStatus(ApplicationSettingsStatus.RateNever);
            ShowMarketplaceReview();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 显示提示框，询问用户是否评价此应用。
        /// </summary>
        private void ShowCustomMessageBox()
        {
            CheckBox checkBox = new CheckBox()
            {
                Content = AppResources.CheckBoxContent,
                Margin = new Thickness(0, 14, 0, -2)
            };

            TiltEffect.SetIsTiltEnabled(checkBox, true);

            CustomMessageBox customMessageBox = new CustomMessageBox()
            {
                Caption = AppResources.MsgBoxCaption,
                Message = AppResources.MsgBoxMsg,
                Content = checkBox,
                LeftButtonContent = AppResources.MsgBoxLeftBtn,
                RightButtonContent = AppResources.MsgBoxRightBtn

            };

            customMessageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                    case CustomMessageBoxResult.None:
                        appSettings.SetRateTimeAsNow();
                        appSettings.SetRateStatus(ApplicationSettingsStatus.RateLater);

                        if ((bool)checkBox.IsChecked)
                            appSettings.SetRateStatus(ApplicationSettingsStatus.RateNever);
                        break;

                    case CustomMessageBoxResult.RightButton:
                        appSettings.SetRateStatus(ApplicationSettingsStatus.RateNever);
                        ShowMarketplaceReview();
                        break;

                    default:
                        break;
                }
            };
            customMessageBox.Show();
        }

        /// <summary>
        /// 链接到应用商店
        /// </summary>
        private void ShowMarketplaceReview()
        {
            MarketplaceReviewTask marketplaceReview = new MarketplaceReviewTask();
            marketplaceReview.Show();
        }

        /// <summary>
        /// 是否为提醒时间
        /// 如果上一次时间在10秒之前，则应该提醒，则返回true
        /// </summary>
        /// <param name="settingRateTime"></param>
        /// <returns></returns>
        private bool IsRateTime(DateTime settingRateTime)
        {
//#if DEBUG
//            return settingRateTime.AddSeconds(30) < DateTime.Now;
//#endif
            return settingRateTime.AddDays(RateDays) < DateTime.Now;
        }

        /// <summary>
        /// 检查提醒时间
        /// </summary>
        private void CheckRateTime()
        {
            DateTime rateTime = appSettings.GetRateTime();
            if (IsRateTime(rateTime))
                control.Dispatcher.BeginInvoke(new Action(() => ShowCustomMessageBox()));
        }


        #endregion Private Methods

        #region IDispose Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion IDispose Members
    }
}
