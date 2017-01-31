using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoScheduler.Base;
using TodoScheduler.Enums;
using TodoScheduler.Models;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using TodoScheduler.Services.NotificationServices;
using Xamarin.Forms;

namespace TodoScheduler.ViewModels
{
    public class TodayViewModel : ViewModelBase
    {
        #region memebrs

        protected readonly IDataService _dataService;
        protected readonly IDialogService _dialogService;
        protected readonly INotificationService _notificationService;
        public virtual string NoDataText { get; set; } = "You've already done for today";
        #endregion

        #region fields & properties

        IEnumerable<SelectableObject<Grouping<object, TodoItem>>> _groupedSelectableTodoItems;
        public IEnumerable<SelectableObject<Grouping<object, TodoItem>>> GroupedSelectableTodoItems
        {
            get { return _groupedSelectableTodoItems; }
            set { SetProperty(ref _groupedSelectableTodoItems, value); }
        }

        IEnumerable<Grouping<object, TodoItem>> _groupedTodoItems;
        public IEnumerable<Grouping<object, TodoItem>> GroupedTodoItems
        {
            get { return _groupedTodoItems; }
            set { SetProperty(ref _groupedTodoItems, value); }
        }

        #endregion

        #region constructor

        public TodayViewModel(IDialogService dialogService, IDataService dataService,
            INotificationService notificationService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _notificationService = notificationService;
        }

        #endregion

        #region commands

        ICommand _refreshCommand;
        public ICommand RefreshCommand {
            get { return _refreshCommand ?? new Command(() => LoadTodayTodos()); }
            set { SetProperty(ref _refreshCommand, value); }
        }

        ICommand _addTodoCommand;
        public ICommand AddTodoCommand {
            get { return _addTodoCommand ?? new Command(async () => await Navigation.NavigateAsync(typeof(CreateTodoViewModel))); }
            set { SetProperty(ref _addTodoCommand, value); }
        }

        ICommand _completeCommand;
        public ICommand CompleteCommand {
            get { return _completeCommand ?? new Command<TodoItem>(CompleteCommandExecute); }
            set { SetProperty(ref _completeCommand, value); }
        }


        ICommand _updateDueDateCommand;
        public ICommand UpdateDueDateCommand {
            get { return _updateDueDateCommand ?? new Command<TodoItem>(UpdateDueDateCommandExecute); }
            private set { SetProperty(ref _updateDueDateCommand, value); }
        }

        ICommand _removeTodoCommand;
        public ICommand RemoveTodoCommand {
            get { return _removeTodoCommand ?? new Command<TodoItem>(RemoveTodoCommandExecute); }
            set { SetProperty(ref _removeTodoCommand, value); }
        }

        #endregion

        #region private


        protected virtual async void LoadTodayTodos()
        {
            try
            {
                if (State == VmState.Busy) return;
                State = VmState.Busy;

                var todos = await _dataService.GetTodoItemsAsync(DateTime.Now);

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
        private async void CompleteCommandExecute(TodoItem todo)
        {
            try
            {
                if (todo == null) return;

                var result = await _dialogService.ShowConfirmMessageAsync("Confirm", $"Did you compeleted this todo?", "Yes", "Not yet");
                if (!result)
                    return;

                todo.IsCompleted = true;

                await _notificationService.CancelTodoNotificationAsync(todo);
                await _dataService.UpdateTodoItemAsync(todo);
                await _dialogService.ShowToastMessageAsync("Congratulation, todo has been completed", TimeSpan.FromSeconds(2));
                //Refresh todos after completed
                LoadTodayTodos();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
        }

        private async void RemoveTodoCommandExecute(TodoItem todo)
        {
            try
            {
                if (todo == null) return;

                var result = await _dialogService.ShowConfirmMessageAsync("Confirm", $"Remove this todo (it isn't complete)?");
                if (!result)
                    return;

                await _notificationService.CancelTodoNotificationAsync(todo);
                await _dataService.RemoveTodoItemAsync(todo);
                await _dialogService.ShowToastMessageAsync("Todo has been removed", TimeSpan.FromSeconds(2));
                LoadTodayTodos();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
        }

        private async void UpdateDueDateCommandExecute(TodoItem todo)
        {
            try
            {
                if (todo.IsCompleted)
                {
                    var result = await _dialogService.ShowConfirmMessageAsync("Confirm", $"This todo already marked as 'completed'. Do you want to update due date for this todo?", "Yes", "No");
                    if (!result)
                        return;

                    todo.IsCompleted = false;
                }

                if (todo == null) return;
                var date = await _dialogService.ShowDateDialogAsync();
                if (date == null) return;
                var time = await _dialogService.ShowTimeDialogAsync(date);

                if (time == null) return;

                todo.DueTime = new DateTime(date.Year, date.Month, date.Day,
                            time.Hours, time.Minutes, time.Seconds);


                await _notificationService.CancelTodoNotificationAsync(todo);
                todo.ReminderId = await _notificationService.SendNotificationAsync(todo.Title, todo.Description, todo.DueTime.Value);
                await _dataService.UpdateTodoItemAsync(todo);
                await _dialogService.ShowToastMessageAsync("Todo due date has been updated", TimeSpan.FromSeconds(2));
                LoadTodayTodos();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
        }

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            Header = $"Today ({Helpers.StringHelper.DayCutter(DateTime.Now.DayOfWeek.ToString())}, {DateTime.Now.ToString("dd.MM.yyyy")})";
            LoadTodayTodos();
        }

        public override void Appearing()
        {
            base.Appearing();
            LoadTodayTodos();
        }

        public override void Disappearing()
        {
            base.Disappearing();
            GroupedTodoItems = null;
        }

        #endregion
    }
}
