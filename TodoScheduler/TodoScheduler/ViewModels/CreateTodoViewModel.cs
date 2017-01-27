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

namespace TodoScheduler.ViewModels
{
    public class CreateTodoViewModel : ViewModelBase
    {
        #region members

        readonly IDataService _dataService;
        readonly IDialogService _dialogService;
        readonly INotificationService _notificationService;

        #endregion

        #region fields & properties

        bool _isValid = false;
        public bool IsValid {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }

        IEnumerable<TagItem> _tags;
        public IEnumerable<TagItem> Tags {
            get { return _tags; }
            set { SetProperty(ref _tags, value); }
        }

        TagItem _selectedTag;
        public TagItem SelectedTag {
            get { return _selectedTag; }
            set { SetProperty(ref _selectedTag, value); }
        }

        string _title;
        public string Title {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
                IsValid = !string.IsNullOrWhiteSpace(Title);
            }
        }

        string _description;
        public string Description {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }


        bool _enableReminder = false;
        public bool EnableReminder {
            get { return _enableReminder; }
            set
            {
                if(SetProperty(ref _enableReminder, value))
                    ReminderDate = DueDate;
            }
        }

        TodoPriority _priority = TodoPriority.Low;
        public TodoPriority Priority {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
        }

        IEnumerable<TodoPriority> _priorityList = Enum.GetValues(typeof(TodoPriority)).Cast<TodoPriority>();
        public IEnumerable<TodoPriority> PriorityList
        {
            get { return _priorityList; }
            set { SetProperty(ref _priorityList, value); }
        }


        DateTime _dueDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,0,0,0);
        public DateTime DueDate {
            get { return _dueDate; }
            set {
                if (SetProperty(ref _dueDate, value))
                    ReminderDate = DueDate;
            }
        }

        DateTime _reminderDate;
        public DateTime ReminderDate {
            get { return _reminderDate; }
            set { SetProperty(ref _reminderDate, value); }
        }

        #endregion

        #region constructors

        public CreateTodoViewModel(IDataService dataService, IDialogService dialogService, 
            INotificationService notificationService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _notificationService = notificationService;
        }

        #endregion

        #region commands

        ICommand _selectDateCommand;
        public ICommand SelectDateCommand {
            get { return _selectDateCommand ?? new Command<string>(SelectDateCommandExecute); }
            set { SetProperty(ref _selectDateCommand, value); }
        }

        ICommand _selectTimeCommand;
        public ICommand SelectTimeCommand {
            get { return _selectTimeCommand ?? new Command<string>(SelectTimeCommandExecute); }
            set { SetProperty(ref _selectTimeCommand, value); }
        }

        ICommand _saveCommand;
        public ICommand SaveCommand {
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

        private async void SelectDateCommandExecute(string type)
        {
           
            var date = await _dialogService.ShowDateDialogAsync();

            if (date != null)
            {
                if (string.IsNullOrEmpty(type))
                    DueDate = date;
                else
                    ReminderDate = date;
            }
               
        }

        private async void SelectTimeCommandExecute(string type)
        {
            var time = await _dialogService.ShowTimeDialogAsync();

            if (time != null)
            {
                if (string.IsNullOrEmpty(type))
                    DueDate = new DateTime(DueDate.Year, DueDate.Month, DueDate.Day,
                        time.Hours, time.Minutes, time.Seconds);
                else
                    ReminderDate = new DateTime(ReminderDate.Year, ReminderDate.Month, ReminderDate.Day,
                        time.Hours, time.Minutes, time.Seconds);
            }
        }

        private async void SaveCommandExecute()
        {
            try
            {
                if (State == VmState.Busy)
                    return;

                State = VmState.Busy;

                TodoItem todo = new TodoItem() {
                    TagId = SelectedTag.Id,
                    Title = this.Title,
                    Description = this.Description == null ? "..." : this.Description,
                    Priority = this.Priority,
                    CreatedDate = DateTime.Now,
                    DueDate = this.DueDate,
                    IsCompleted = false
                }; 

                await Task.WhenAll(_dataService.CreateTodoItemAsync(todo),
                                   _dialogService.ShowToastMessageAsync("To-do has been created", TimeSpan.FromSeconds(2)),
                                   Navigation.CloseAsync());


                if (EnableReminder)
                    await _notificationService.SendNotificationAsync(todo.Title, todo.Description, ReminderDate);


                MessagingCenter.Send(this, "refresh_tags");
                MessagingCenter.Send(this, "refresh_todos");

            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorMessageAsync("Oops", ex.Message);
            }
            finally
            {
                State = VmState.Normal;
            }
        }

        private async void CancelCommandExecute() => await Navigation.CloseAsync();

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
                   //(TagItem)parameters["tag"],
                   //(TagItem)parameters["tag"]
                };
                SelectedTag = (TagItem)parameters["tag"];
                //SelectedTag = Tags.ToList()[0];
            }
        }

        #endregion
    }
}
