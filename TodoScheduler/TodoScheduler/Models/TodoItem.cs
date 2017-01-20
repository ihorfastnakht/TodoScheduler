using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Infrastructure.Base;

namespace TodoScheduler.Models
{
    public class TodoItem : ObservableObject
    {
        #region fields & properties

        string[] dependendProperties = new string[] { nameof(Status), nameof(RemainHours) };

        int _id;
        public int Id {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        int _tagId;
        public int TagId
        {
            get { return _tagId; }
            set { SetProperty(ref _tagId, value); }
        }

        string _title;
        public string Title {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        TodoPriority _priority = TodoPriority.Low;
        public TodoPriority Priority {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
        }

        DateTime? _createdTime;
        public DateTime? CreatedTime {
            get { return _createdTime; }
            set { SetProperty(ref _createdTime, value); }
        }

        DateTime? _dueTime;
        public DateTime? DueTime {
            get { return _dueTime; }
            set { SetProperty(ref _dueTime, value, dependendProperty: dependendProperties); }
        }

        bool _isCompleted = false;
        public bool IsCompleted {
            get { return _isCompleted; }
            set { SetProperty(ref _isCompleted, value, dependendProperty : dependendProperties); }
        }

        #endregion

        #region readonly properties

        public TodoStatus Status
        {
            get
            {
                if (IsCompleted) return TodoStatus.Completed;
                if (!IsCompleted && RemainHours <= 0) return TodoStatus.Failed;

                return TodoStatus.InProcess;
            }
        }

        public int RemainHours => DueTime == null ? -1 : DueTime.Value.Subtract(DateTime.Now).Minutes;

        #endregion
    }
}
