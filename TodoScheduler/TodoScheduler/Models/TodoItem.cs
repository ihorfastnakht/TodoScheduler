using System;
using TodoScheduler.Base;
using SQLite.Net.Attributes;
using TodoScheduler.Enums;
using System.Diagnostics;

namespace TodoScheduler.Models
{
    [Table("TodoItems")]
    public class TodoItem : ObservableObject
    {
        #region private

        // updated properties

        readonly string[] dependendProps = new string[] { nameof(Status), nameof(RemainDisplay) };

        int _id;
        [AutoIncrement, PrimaryKey]
        public int Id {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        int _tagId;
        [NotNull]
        public int TagId {
            get { return _tagId; }
            set { SetProperty(ref _tagId, value); }
        }

        string _title;
        [NotNull, MaxLength(20)]
        public string Title {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        string _description;
        [NotNull, MaxLength(50)]
        public string Description {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        TodoPriority _priority = TodoPriority.Low;
        [NotNull]
        public TodoPriority Priority {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
        }

        DateTime? _createdDate;
        [NotNull]
        public DateTime? CreatedDate {
            get { return _createdDate; }
            set { SetProperty(ref _createdDate, value); }
        }

        DateTime? _dueDate;
        [NotNull]
        public DateTime? DueDate {
            get { return _dueDate; }
            set { SetProperty(ref _dueDate, value, dependendProperties: dependendProps); }
        }

        bool _isCompleted = false;
        [NotNull]
        public bool IsCompleted {
            get { return _isCompleted; }
            set { SetProperty(ref _isCompleted, value, dependendProperties: dependendProps); }
        }

        [Ignore]
        public TagItem TagItem { get; set; }
        #endregion

        #region readonly properties
        
        [Ignore]
        public TodoStatus Status {
            get
            {
                if (IsCompleted)
                    return TodoStatus.Completed;
                if (!IsCompleted && Remain <= 0)
                    return TodoStatus.Failed;

                return TodoStatus.InProcess;
            }
        }

        [Ignore]
        private double Remain => DueDate.HasValue 
            ? DueDate.Value.Subtract(DateTime.Now).TotalMinutes 
            : -1;

        [Ignore]
        public string RemainDisplay
        {
            get
            {
                var hours = Math.Round(Remain / 60);
                var minutes = Math.Round(Remain % 60);

                return $"{hours:00} hh {minutes:00} mm";
            }
        }

        #endregion
    }
}
