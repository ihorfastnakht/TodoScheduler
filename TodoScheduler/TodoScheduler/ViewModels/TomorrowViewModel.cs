using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TodoScheduler.Base;
using TodoScheduler.Enums;
using TodoScheduler.Models;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using TodoScheduler.Services.NotificationServices;
using TodoScheduler.Helpers;

namespace TodoScheduler.ViewModels
{
    public class TomorrowViewModel : TodayViewModel
    {
        #region constructor

        public TomorrowViewModel(IDialogService dialogService, IDataService dataService, 
            INotificationService notificationService) 
            : base(dialogService, dataService, notificationService)
        {
        }

        #endregion

        #region override

        protected override async void LoadTodayTodos()
        {
            try
            {
                if (State == VmState.Busy) return;
                State = VmState.Busy;

                var todos = await _dataService.GetTodoItemsAsync(DateTime.Now.AddDays(1));

                var groupByTag = from todoItem in todos
                                 where todoItem.Status == TodoStatus.InProcess || todoItem.Status == TodoStatus.Postponed
                                 orderby todoItem.Remain,
                                         todoItem.Priority descending
                                 group todoItem by todoItem.ParentTag
                                 into grouped
                                 select new Grouping<object, TodoItem>(grouped.Key, grouped);
                if (groupByTag.Any())
                {
                    GroupedTodoItems = new ObservableCollection<Grouping<object, TodoItem>>(groupByTag);
                    State = VmState.Normal;
                }
                else
                {
                    State = VmState.NoData;
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
        }

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            Header = $"Tomorrow ({StringHelper.DayCutter(DateTime.Now.AddDays(1).DayOfWeek.ToString())}, {DateTime.Now.AddDays(1).ToString("dd.MM.yyyy")})";
        }

        public override string NoDataText
        {
            get
            {
                return "There are no todos for tomorrow";
            }
            set
            {
                base.NoDataText = value;
            }
        }
        #endregion
    }
}
