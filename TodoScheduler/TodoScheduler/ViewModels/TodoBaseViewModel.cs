using System.Collections.Generic;
using TodoScheduler.Base;
using TodoScheduler.Enums;
using TodoScheduler.Models;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using TodoScheduler.Services.NotificationServices;

namespace TodoScheduler.ViewModels
{
    public abstract class TodoBaseViewModel : ViewModelBase
    {
        #region members

        TodoViewModelType _type;
        readonly IDataService _dataService;
        readonly IDialogService _dialogService;
        readonly INotificationService _notificationService;

        #endregion

        #region fields & properties

        private IEnumerable<TodoItem> _todoItems;
        public IEnumerable<TodoItem> TodoItems {
            get { return _todoItems; }
            set { SetProperty(ref _todoItems, value); }
        }

        #endregion

        public TodoBaseViewModel(IDataService dataService, IDialogService dialogService, INotificationService notificationService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _notificationService = notificationService;
        }


        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            if (parameters != null)
            {
                var type = (TodoViewModelType)parameters["type"];

                if (type == TodoViewModelType.Today)
                {
                    _type = TodoViewModelType.Today;
                }
                else
                {
                    _type = TodoViewModelType.Tomorrow;
                }
            }
            _type = TodoViewModelType.Schedule;
        }
    }
}
