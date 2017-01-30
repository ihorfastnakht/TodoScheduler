using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.NotificationServices;
using TodoScheduler.Services.DialogServices;
using TodoScheduler.Base;
using TodoScheduler.Models;
using System.Collections.ObjectModel;
using TodoScheduler.Enums;

namespace TodoScheduler.ViewModels
{
    public class ScheduleViewModel : TodayViewModel
    {

        #region constructor

        public ScheduleViewModel(IDialogService dialogService, IDataService dataService,
            INotificationService notificationService) : base(dialogService, dataService, notificationService)
        {
        }

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            Header = "Schedule";
        }
        protected async override void LoadTodayTodos()
        {
            //base.LoadTodayTodos();
            try
            {
                if (State == VmState.Busy) return;
                State = VmState.Busy;

                var todos = await _dataService.GetTodoItemsAsync();
                if (todos.Any())
                {
                    var groupByTag = from todoItem in todos
                                     where todoItem.Status == TodoStatus.InProcess
                                     orderby todoItem.Remain,
                                             todoItem.Priority descending
                                     group todoItem by todoItem.ParentTag
                                into grouped
                                     select new Grouping<object, TodoItem>(grouped.Key, grouped);
                    GroupedTodoItems = new ObservableCollection<Grouping<object, TodoItem>>(groupByTag);

                    State = VmState.Normal;
                }
                else
                    State = VmState.NoData;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
        }
        public override string NoDataText
        {
            get
            {
                return "There are no todos for any days";
            }

            set
            {
                base.NoDataText = value;
            }
        }
        #endregion
    }
}
