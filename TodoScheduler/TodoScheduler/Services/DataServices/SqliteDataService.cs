using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoScheduler.Models;
using SQLite.Net;
using TodoScheduler.Services.PlatformServices;
using Xamarin.Forms;

namespace TodoScheduler.Services.DataServices
{
    public class SqliteDataService : IDataService
    {
        #region members

        readonly static object locker = new object();
        readonly SQLiteConnection _database;

        #endregion
        #region constructor

        public SqliteDataService()
        {
            _database = DependencyService.Get<ISqliteConnectionService>().GetDatabaseConnection();
            
            _database.CreateTable<TagItem>();
            _database.CreateTable<TodoItem>();

            Init();
        }

        #endregion

        #region private

        private void Init()
        {
            var tag = new TagItem() { Title = "Personal", HexColor = "#7635EB" };
            if (!IsTagExists(tag))
                _database.Insert(tag);
        }

        private bool IsTagExists(TagItem tag)
        {
            var exist = _database.Table<TagItem>()
                                 .Where(t => t.Title.ToLower() == tag.Title.ToLower())
                                 .FirstOrDefault();

            return exist == null ? false : true;
        }

        private bool IsTodoExists(TodoItem todo)
        {
            var exist = _database.Table<TodoItem>()
                                 .Where(t => t.Id == todo.Id)
                                 .FirstOrDefault();

            return exist == null ? false : true;
        }

        #endregion

        #region IDataService implementation

        /// <summary>
        /// Insert new tag into Tags table
        /// if not exists
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task CreateTagItemAsync(TagItem tag)
        {
            await Task.Factory.StartNew(() =>
            {
                if (tag == null)
                    throw new ArgumentNullException("TagItem is null");
                if (IsTagExists(tag))
                    throw new Exception($"Tag '{tag.Title}' already existed");

                lock (locker)
                {
                    _database.Insert(tag);
                }
            });
        }

        /// <summary>
        /// Insert new todo item into 
        /// TodoItems table
        /// </summary>
        /// <param name="todo"></param>
        /// <returns></returns>
        public async Task CreateTodoItemAsync(TodoItem todo)
        {
            await Task.Factory.StartNew(() =>
            {
                if (todo == null)
                    throw new ArgumentNullException("TodoItem is null");

                lock (locker)
                {
                    _database.Insert(todo);
                }
            });
        }

        /// <summary>
        /// Get all tags from Tags table
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TagItem>> GetTagItemsAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                var tags = _database.Table<TagItem>().ToList();
                var todos = _database.Table<TodoItem>().ToList();

                foreach (var tag in tags)
                {
                    foreach (var todo in todos)
                    {
                        if (todo.TagId == tag.Id)
                            todo.ParentTag = tag;
                    }

                    tag.TodoItems = todos.Where(t => t.TagId == tag.Id)
                                         .OrderBy(t => t.DueTime);
                                         //.OrderByDescending(t => t.Status);
                }

                return tags;
            });
        }

        /// <summary>
        /// Get all todo items from 
        /// TodoItems table
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                var tags = _database.Table<TagItem>().ToList();
                var todos = _database.Table<TodoItem>().ToList();

                foreach (var todo in todos)
                    todo.ParentTag = tags.Where(t => t.Id == todo.TagId).FirstOrDefault();

                return todos;
            });
        }

        /// <summary>
        /// Get all todo items from 
        /// TodoItems table by Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(TagItem tag)
        {
            return await Task.Factory.StartNew(() =>
            {
                var todos = _database.Table<TodoItem>()
                                     .ToList()
                                     .Where(t => t.TagId == tag.Id);

                foreach (var todo in todos)
                    todo.ParentTag = tag;

                return todos;
            });
        }

        /// <summary>
        /// Get all todo items 
        /// from TodoItems table
        /// by DueDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(DateTime date)
        {
            return await Task.Factory.StartNew(() =>
            {
                var tags = _database.Table<TagItem>().ToList();
                var todos = _database.Table<TodoItem>()
                                     .ToList()
                                     .Where(t => t.DueTime.Value.Date == date.Date
                                            && t.Status == Enums.TodoStatus.InProcess);

                foreach (var todo in todos)
                    todo.ParentTag = tags.Where(t => t.Id == todo.TagId).FirstOrDefault();

                return todos;
            });
        }

        /// <summary>
        /// Remove tag from 
        /// Tags table if it exist
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task RemoveTagItemAsync(TagItem tag)
        {
            await Task.Factory.StartNew(() =>
            {
                if (tag == null)
                    throw new ArgumentNullException("TagItem is null");
                if (!IsTagExists(tag))
                    throw new Exception($"Tag '{tag.Title}' not existed");
                if (tag.Title.ToLower() == "personal")
                    throw new Exception($"Sorry, but personal tag couldn't be removed");

                lock (locker)
                {
                    var relatedTodos = _database.Table<TodoItem>().Where(t => t.TagId == tag.Id);

                    foreach (var item in relatedTodos)
                        _database.Delete(item);

                    _database.Delete(tag);
                }
            });
        }

        /// <summary>
        /// Remove todo from TodoItems
        /// table if it exist
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task RemoveTodoItemAsync(TodoItem todo)
        {
            await Task.Factory.StartNew(() =>
            {
                if (todo == null)
                    throw new ArgumentNullException("TodoItem is null");
                if (!IsTodoExists(todo))
                    throw new Exception($"Todo with 'id: {todo.Id}' not existed");

                lock (locker)
                {
                    _database.Delete(todo);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="todo"></param>
        /// <returns></returns>
        public async Task UpdateTodoItemAsync(TodoItem todo)
        {
            await Task.Factory.StartNew(() =>
            {
                if (todo == null)
                    throw new ArgumentNullException("TodoItem is null");
                if (!IsTodoExists(todo))
                    throw new Exception($"Todo with 'id: {todo.Id}' not existed");

                lock (locker)
                {
                    _database.InsertOrReplace(todo);
                }
            });
        }

        #endregion
    }
}
