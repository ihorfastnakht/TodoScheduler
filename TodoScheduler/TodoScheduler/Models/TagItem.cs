using SQLite.Net.Attributes;
using System.Collections.Generic;
using System.Linq;
using TodoScheduler.Base;
using TodoScheduler.Enums;

namespace TodoScheduler.Models
{
    [Table("Tags")]
    public class TagItem : ObservableObject
    {
        #region private members

        readonly string[] dependendProps = new string[] { nameof(HasItems), nameof(Total), nameof(Failed), nameof(Completed) };

        #endregion

        #region fields & properties

        int _id;
        [AutoIncrement, PrimaryKey]
        public int Id {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        string _title;
        [NotNull, MaxLength(20)]
        public string Title {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        string _hexcolor;
        public string HexColor {
            get { return _hexcolor; }
            set { SetProperty(ref _hexcolor, value); }
        }

        IEnumerable<TodoItem> _todoItems;
        [Ignore]
        public IEnumerable<TodoItem> TodoItems {
            get { return _todoItems; }
            set { SetProperty(ref _todoItems, value, dependendProperties: dependendProps); }
        }

        #endregion

        #region readonly properties
        //[Ignore]
        //public int TodoItemsCount => !HasItems ? 0 : TodoItems.Count();
        [Ignore]
        public bool HasItems => TodoItems?.Any() == true ? true : false;

        [Ignore]
        public string Total => !HasItems ? "-" : TodoItems.Count().ToString();
        [Ignore]
        public string Completed
        {
            get
            {
                if (!HasItems) return "-";
                return TodoItems.Where(t => t.Status == TodoStatus.Completed).Count().ToString();
            }
        }
        [Ignore]
        public string Failed
        {
            get
            {
                if (!HasItems) return "-";
                return TodoItems.Where(t => t.Status == TodoStatus.Failed).Count().ToString();
            }
        }

        #endregion
    }
}
