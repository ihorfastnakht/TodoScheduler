using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TodoScheduler.Base;
using TodoScheduler.Enums;
using TodoScheduler.Models;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using TodoScheduler.Services.NotificationServices;
using TodoScheduler.Extensions;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TodoScheduler.ViewModels
{
    public class CreateTodoViewModel : ViewModelBase
    {
        #region members

        readonly IDataService _dataService;
        readonly IDialogService _dialogService;
        readonly INotificationService _notificationService;
        readonly string reminder_on = "_reminder.png";
        readonly string reminder_off = "_no_reminder.png";

        #endregion

        #region fields & properties

        bool _isValid = false;
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (SetProperty(ref _isValid, value))
                    ((Command)SaveCommand).CanExecute(value);
            }
        }

        IEnumerable<TagItem> _tags;
        public IEnumerable<TagItem> Tags
        {
            get { return _tags; }
            set { SetProperty(ref _tags, value); }
        }

        TagItem _selectedTag;
        public TagItem SelectedTag
        {
            get { return _selectedTag; }
            set { SetProperty(ref _selectedTag, value, needComapare: false); }
        }

        string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
                Validation();
            }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        bool _enableReminder = false;
        public bool EnableReminder
        {
            get { return _enableReminder; }
            set
            {
                if (SetProperty(ref _enableReminder, value))
                {
                    if (value)
                        ReminderIcon = reminder_on;
                    else
                        ReminderIcon = reminder_off;
                    OnPropertyChanged(nameof(ReminderIcon));
                }
            }
        }

        public string ReminderIcon { get; private set; }

        TodoPriority _priority = TodoPriority.Low;
        public TodoPriority Priority
        {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
        }

        IEnumerable<TodoPriority> _priorityList = Enum.GetValues(typeof(TodoPriority)).Cast<TodoPriority>();
        public IEnumerable<TodoPriority> PriorityList
        {
            get { return _priorityList; }
            set { SetProperty(ref _priorityList, value); }
        }

        DateTime? _dueTime = null;
        public DateTime? DueTime
        {
            get { return _dueTime; }
            set
            {
                SetProperty(ref _dueTime, value);
                Validation();
            }
        }

        #endregion

        #region constructors

        public CreateTodoViewModel(IDataService dataService, IDialogService dialogService,
            INotificationService notificationService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _notificationService = notificationService;

            ReminderIcon = reminder_off;
        }

        #endregion

        #region commands

        ICommand _selectDueDateCommand;
        public ICommand SelectDueDateCommand
        {
            get { return _selectDueDateCommand ?? new Command(SelectDueDateCommandExecute); }
            set { SetProperty(ref _selectDueDateCommand, value); }
        }

        ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? new Command(() => SaveCommandExecute(), () => IsValid); }
            set { SetProperty(ref _saveCommand, value); }
        }

        ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return _cancelCommand ?? new Command(CancelCommandExecute); }
            set { SetProperty(ref _cancelCommand, value); }
        }

        #endregion

        #region private

        private async void SelectDueDateCommandExecute()
        {
            try
            {
                var date = await _dialogService.ShowDateDialogAsync();
                if (date != null)
                {
                    var time = await _dialogService.ShowTimeDialogAsync(date);

                    if (time != null)
                        DueTime = new DateTime(date.Year, date.Month, date.Day,
                            time.Hours, time.Minutes, time.Seconds);
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
        }

        private async void SaveCommandExecute()
        {
            try
            {
                if (State == VmState.Busy)
                    return;

                State = VmState.Busy;

                TodoItem todo = new TodoItem()
                {
                    TagId = SelectedTag.Id,
                    Title = this.Title,
                    Description = this.Description == null ? "..." : this.Description,
                    Priority = this.Priority,
                    CreatedTime = DateTime.Now,
                    DueTime = this.DueTime.Value,
                    IsCompleted = false
                };

                await _dataService.CreateTodoItemAsync(todo);
                await _dialogService.ShowToastMessageAsync($"Todo has been created", TimeSpan.FromSeconds(2));

                if (EnableReminder)
                    await _notificationService.SendNotificationAsync(todo.Title, todo.Description, DueTime.Value);

                MessagingCenter.Send(this, "refresh_tags");
                await Navigation.CloseAsync();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
            finally
            {
                //State = VmState.Normal;
            }
        }

        private async void CancelCommandExecute() => await Navigation.CloseAsync();

        private void Validation()
        {
            if (string.IsNullOrEmpty(Title) || DueTime == null)
                IsValid = false;
            else
                IsValid = true;
        }

        #endregion

        #region override

        public async override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            //load all tag items
            //form dataservice
            if (parameters == null)
            {
                Tags = await _dataService.GetTagItemsAsync();
            }
            else
            {
                if (!parameters.ContainsKey("tag"))
                    throw new Exception("Parameter (tag) not found");

                Tags = new List<TagItem>{
                   (TagItem)parameters["tag"],
                };
                SelectedTag = (TagItem)parameters["tag"];
            }
        }

        #endregion
    }
}
