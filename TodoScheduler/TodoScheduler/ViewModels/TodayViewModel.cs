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

        ICommand _selectHeaderCommand;
        public ICommand SelectHeaderCommand {
            get { return _selectHeaderCommand ?? new Command(SelectHeaderCommandExecute); }
            set { SetProperty(ref _selectHeaderCommand, value); }
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


        private void SelectHeaderCommandExecute(object item)
        {
            /*
            var selectable = (SelectableObject<Grouping<object, TodoItem>>)item;
            selectable.IsSelected = !selectable.IsSelected;

            if (!selectable.IsSelected)
            {
                selectable.Item.Clear();
            }
            else
            {

            }
            */
        }

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            Header = $"Today ({DateTime.Now.DayOfWeek}, {DateTime.Now.ToString("dd.MM.yyyy")})";
            LoadTodayTodos();
        }

        public override void Appearing()
        {
            base.Appearing();
            LoadTodayTodos();
        }

        #endregion
    }
}
