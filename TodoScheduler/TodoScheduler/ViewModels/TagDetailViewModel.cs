using System;
using System.Collections.Generic;
using System.Collections;
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
using Microsoft.Practices.ObjectBuilder2;

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
        public IEnumerable<TodoItem> TodoItems {
            get { return _todoItems; }
            set { SetProperty(ref _todoItems, value); }
        }

        TagItem _tag;
        public TagItem Tag {
            get { return _tag; }
            set { SetProperty(ref _tag, value, needComapare: false); }
        }

        IEnumerable<Grouping<object, TodoItem>> _groupedTodoItems;
        public IEnumerable<Grouping<object, TodoItem>> GroupedTodoItems {
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

            MessagingCenter.Subscribe<CreateTodoViewModel>(this, "refresh", (p) => RefreshCommandExecute());
        }

        #endregion

        #region commands

        ICommand _addTodoCommand;
        public ICommand AddTodoCommand {
            get { return _addTodoCommand ?? new Command(AddTodoCommandExecute); }
            set { SetProperty(ref _addTodoCommand, value); }
        }

        ICommand _groupCommand;
        public ICommand GroupCommand {
            get { return _groupCommand ?? new Command<string>((p) => GroupCommandExecute(p), (p) => { return State != VmState.Busy || State != VmState.NoData; }); }
            set { SetProperty(ref _groupCommand, value); }
        }

        ICommand _refreshCommand;
        public ICommand RefreshCommand {
            get { return _refreshCommand ?? new Command(() => RefreshCommandExecute(), () => { return State != VmState.Busy || State != VmState.NoData; }); }
            set { SetProperty(ref _refreshCommand, value); }
        }

        #endregion

        #region private

        private async void AddTodoCommandExecute()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() { ["tag"] = Tag };
            await Navigation.NavigateAsync(typeof(CreateTodoViewModel), parameters);
        }

        private async void GroupCommandExecute(string groupType)
        {
            await Task.Factory.StartNew(() =>
            {
                if (groupType == "date")
                {
                    var groupedByDate = from todoItem in TodoItems
                                        orderby todoItem.DueDate.Value
                                        group todoItem by todoItem.DueDate.Value.ToString("dd.MM.yyyy")
                                        into grouped
                                        select new Grouping<object, TodoItem>(grouped.Key, grouped);
                    GroupedTodoItems = new ObservableCollection<Grouping<object, TodoItem>>(groupedByDate);
                }
                if (groupType == "status")
                {
                    var groupedByStatus = from todoItem in TodoItems
                                          orderby todoItem.DueDate.Value
                                          group todoItem by todoItem.Status
                                          into grouped
                                          select new Grouping<object, TodoItem>(grouped.Key, grouped);
                    GroupedTodoItems = new ObservableCollection<Grouping<object, TodoItem>>(groupedByStatus);
                }
                if (groupType == "priority")
                {
                    var groupedByStatus = from todoItem in TodoItems
                                          orderby todoItem.DueDate.Value descending
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

                var items = await _dataService.GetTodoItemsAsync(Tag);

                TodoItems = items;

                State = VmState.Normal;

                GroupCommandExecute("date");

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
                Header = $"{tag.Title} to-do items";

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

        #endregion
    }
}
