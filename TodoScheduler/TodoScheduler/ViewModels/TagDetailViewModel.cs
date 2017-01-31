using System;
using System.Collections.Generic;
using System.Windows.Input;
using TodoScheduler.Base;
using TodoScheduler.Enums;
using TodoScheduler.Models;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using TodoScheduler.Services.NotificationServices;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TodoScheduler.ViewModels
{
    public class TagDetailViewModel : ViewModelBase
    {
        #region members

        readonly IDataService _dataService;
        readonly IDialogService _dialogService;
        readonly INotificationService _notificationService;

        #endregion

        #region fileds & properties

        IEnumerable<TodoItem> _todoItems;
        public IEnumerable<TodoItem> TodoItems
        {
            get { return _todoItems; }
            set { SetProperty(ref _todoItems, value); }
        }

        TagItem _tag;
        public TagItem Tag
        {
            get { return _tag; }
            set { SetProperty(ref _tag, value, needComapare: false); }
        }

        IEnumerable<Grouping<object, TodoItem>> _groupedTodoItems;
        public IEnumerable<Grouping<object, TodoItem>> GroupedTodoItems
        {
            get { return _groupedTodoItems; }
            set { SetProperty(ref _groupedTodoItems, value); }
        }

        #endregion

        #region constructor

        public TagDetailViewModel(IDataService dataService, IDialogService dialogService,
            INotificationService notificationService)

        {
            _dataService = dataService;
            _dialogService = dialogService;
            _notificationService = notificationService;
        }

        #endregion

        #region commands

        ICommand _addTodoCommand;
        public ICommand AddTodoCommand
        {
            get { return _addTodoCommand ?? new Command(AddTodoCommandExecute); }
            private set { SetProperty(ref _addTodoCommand, value); }
        }

        ICommand _removeTodoCommand;
        public ICommand RemoveTodoCommand
        {
            get { return _removeTodoCommand ?? new Command<TodoItem>(RemoveTodoCommandExecute); }
            set { SetProperty(ref _removeTodoCommand, value); }
        }

        ICommand _completeTodoCommand;
        public ICommand CompleteTodoCommand
        {
            get { return _completeTodoCommand ?? new Command<TodoItem>(CompleteTodoCommandExecute); }
            private set { SetProperty(ref _completeTodoCommand, value); }
        }

        ICommand _updateDueDateCommand;
        public ICommand UpdateDueDateCommand
        {
            get { return _updateDueDateCommand ?? new Command<TodoItem>(UpdateDueDateCommandExecute); }
            private set { SetProperty(ref _updateDueDateCommand, value); }
        }

        ICommand _groupCommand;
        public ICommand GroupCommand
        {
            get { return _groupCommand ?? new Command<string>((p) => GroupCommandExecute(p)); }
            private set { SetProperty(ref _groupCommand, value); }
        }

        ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get { return _refreshCommand ?? new Command(() => RefreshCommandExecute()); }
            private set { SetProperty(ref _refreshCommand, value); }
        }

        #endregion

        #region private

        private async void AddTodoCommandExecute()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() { ["tag"] = Tag };
            await Navigation.NavigateAsync(typeof(CreateTodoViewModel), parameters);
        }

        private async void RemoveTodoCommandExecute(TodoItem todo)
        {
            try
            {
                if (todo == null) return;

                var result = await _dialogService.ShowConfirmMessageAsync("Confirm", $"Do You want to remove this todo?");
                if (!result)
                    return;

                await _notificationService.CancelTodoNotificationAsync(todo);
                await _dataService.RemoveTodoItemAsync(todo);
                await _dialogService.ShowToastMessageAsync("Todo has been removed", TimeSpan.FromSeconds(2));

                RefreshCommandExecute();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
        }

        private async void CompleteTodoCommandExecute(TodoItem todo)
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

                RefreshCommandExecute();
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
                    var result = await _dialogService.ShowConfirmMessageAsync("Confirm", $"This todo already marked as 'completed'. Do You want to update due date for this todo?", "Yes", "No");
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

                RefreshCommandExecute();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
        }

        private async void GroupCommandExecute(string groupType)
        {
            await Task.Factory.StartNew(() =>
            {
                if (groupType == "date")
                {
                    var groupedByDate = from todoItem in TodoItems
                                        orderby todoItem.Remain
                                        group todoItem by todoItem.DueTime.Value.ToString("dd.MM.yyyy")
                                        into grouped
                                        select new Grouping<object, TodoItem>(grouped.Key, grouped);
                    GroupedTodoItems = new ObservableCollection<Grouping<object, TodoItem>>(groupedByDate);
                }

                if (groupType == "status")
                {
                    var groupedByStatus = from todoItem in TodoItems
                                          orderby todoItem.Status, todoItem.Remain
                                          group todoItem by todoItem.Status
                                          into grouped
                                          select new Grouping<object, TodoItem>(grouped.Key, grouped);
                    GroupedTodoItems = new ObservableCollection<Grouping<object, TodoItem>>(groupedByStatus);
                }
                if (groupType == "priority")
                {
                    var groupedByStatus = from todoItem in TodoItems
                                          orderby todoItem.Priority descending, todoItem.Remain
                                          group todoItem by todoItem.Priority
                                          into grouped
                                          select new Grouping<object, TodoItem>(grouped.Key, grouped);
                    GroupedTodoItems = new ObservableCollection<Grouping<object, TodoItem>>(groupedByStatus);
                }
            });
        }

        private async void RefreshCommandExecute()
        {
            try
            {
                if (State == VmState.Busy)
                    return;
                State = VmState.Busy;
                TodoItems = null;
#if DEBUG
                await Task.Delay(500);
#endif
                var items = await _dataService.GetTodoItemsAsync(Tag);

                if (items.Any())
                {
                    TodoItems = items;
                    State = VmState.Normal;
                    GroupCommandExecute("date");
                }
                else
                    State = VmState.NoData;
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
            if (parameters != null)
            {
                if (!parameters.ContainsKey("tag"))
                    return;

                var tag = (TagItem)parameters["tag"];
                Header = $"{tag.Title} todo items";

                if (!tag.HasItems)
                    State = VmState.NoData;
                else
                    State = VmState.Normal;

                Tag = tag;     
                TodoItems = Tag.TodoItems;

                //default
                GroupCommandExecute("date");
            }
        }

        public override void Appearing()
        {
            base.Appearing();
            RefreshCommandExecute();
        }

        public override void Disappearing()
        {
            base.Disappearing();
            TodoItems = null;
        }

        #endregion
    }
}
