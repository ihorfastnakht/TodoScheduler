using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Infrastructure.Base;

namespace TodoScheduler.Models
{
    public class TagItem : ObservableObject
    {
        #region fields & properties

        string[] dependendProperties = new string[] { nameof(HasTodoItems), nameof(TodoItemsCount) };

        int _id;
        public int Id {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        string _title;
        public string Title {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        string _tagColor;
        public string TagColor {
            get { return _tagColor; }
            set { SetProperty(ref _tagColor, value); }
        }

        IEnumerable<TodoItem> _todoItems;
        public IEnumerable<TodoItem> TodoItems {
            get { return _todoItems; }
            set { SetProperty(ref _todoItems, value, dependendProperty: dependendProperties); }
        }

        #endregion

        #region readonly properties

        public bool HasTodoItems => TodoItems == null ? false : true;
        public int TodoItemsCount => HasTodoItems == true ? TodoItems.Count() : 0;

        #endregion
    }
}
