using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Base;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using TodoScheduler.Services.NotificationServices;

namespace TodoScheduler.ViewModels
{
    public class TodayViewModel : ViewModelBase
    {
        #region memebrs

        readonly IDataService _dataService;
        readonly IDialogService _dialogService;
        readonly INotificationService _notificationService;

        #endregion

        #region fields & properties



        #endregion

        #region constructor

        public TodayViewModel(IDataService dataService, IDialogService dialogService, INotificationService notificationService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _notificationService = notificationService;
        }

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            Header = $"Today ({DateTime.Now.DayOfWeek}, {DateTime.Now.ToString("dd.MM.yyyy")})";
        }

        #endregion
    }
}
