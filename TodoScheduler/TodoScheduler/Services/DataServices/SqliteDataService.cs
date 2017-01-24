using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoScheduler.Models;
using SQLite.Net;
using TodoScheduler.Services.PlatformServices;
using Xamarin.Forms;
using TodoScheduler.Enums;

namespace TodoScheduler.Services.DataServices
{
    public class SqliteDataService : IDataService
    {
        readonly SQLiteConnection _database;

        #region constructor

        public SqliteDataService()
        {
            _database = DependencyService.Get<ISqliteConnectionService>().GetDatabaseConnection();

            if (_database == null)
                throw new ArgumentNullException("SqliteDataService: connection is null");
            
            //Create tables
            _database.CreateTable<TagItem>();
            _database.CreateTable<TodoItem>();

            var tag = new TagItem() {
                Title = "Default",
                HexColor = "#7635EB"
            };

            if (!IsExistTag(tag))
                _database.Insert(tag);
        }

        #endregion

        #region private

        private bool IsExistTag(TagItem tagItem)
        {
            var exist = _database.Table<TagItem>().Where(t => t.Title == tagItem.Title).FirstOrDefault();
            return exist == null ? false : true;
        }
        private bool IsExistTodo(TodoItem todoItem)
        {
            var exist = _database.Table<TodoItem>().Where(t => t.Id == todoItem.Id && t.TagId == todoItem.TagId).FirstOrDefault();
            return exist == null ? false : true;
        }

        #endregion


        #region IDataService implementation

        public async Task CreateTagItemAsync(TagItem tagItem)
        {
            await Task.Factory.StartNew(() => {
                if (tagItem == null)
                    throw new ArgumentNullException("TagItem is null");
                if (IsExistTag(tagItem))
                    throw new Exception($"Tag ({tagItem.Title}) already existed");

                _database.Insert(tagItem);
            });
        }
        public async Task<IEnumerable<TagItem>> GetTagItemsAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                var tags = _database.Table<TagItem>().ToList();
                var todos = _database.Table<TodoItem>().ToList();

                foreach (var tag in tags) {
                    //TODO: Add order
                    tag.TodoItems = todos.Where(t => t.TagId == tag.Id).ToList();
                }

                return tags;
            });
        }
        public async Task RemoveTagItemAsync(TagItem tagItem)
        {
            await Task.Factory.StartNew(() =>
            {
                if (tagItem == null)
                    throw new ArgumentNullException("TagItem is null");
                if (!IsExistTag(tagItem))
                    throw new Exception($"Tag ({tagItem.Title}) not exist");

                _database.Delete(tagItem);
            });
        }
        public async Task CreateTodoItemAsync(TodoItem todoItem)
        {
            await Task.Factory.StartNew(() =>
            {
                if (todoItem == null)
                    throw new ArgumentNullException("TodoItem is null");

                _database.Insert(todoItem);
            });
        }
        public async Task RemoveTodoItemAsync(TodoItem todoItem)
        {
            await Task.Factory.StartNew(() =>
            {
                if (todoItem == null)
                    throw new ArgumentNullException("TodoItem is null");
                if (!IsExistTodo(todoItem))
                    throw new Exception($"Todo item with Id:{todoItem.Id} not exist");

                _database.Delete(todoItem);
            });
        }
        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                return _database.Table<TodoItem>()
                                  .Where(t => t.Status == TodoStatus.InProcess)
                                  .OrderBy(t => t.DueDate.Value.Date)
                                  .OrderByDescending(t => t.Priority)
                                  .ToList();
            });
        }
        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(DateTime dueDate)
        {
            var items = await GetTodoItemsAsync();
            return items.Where(t => t.DueDate.Value == dueDate).ToList();
        }
        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(TagItem tagItem)
        {
            var items = await GetTodoItemsAsync();
            return items.Where(t => t.TagId == tagItem.Id).ToList();
        }

        #endregion
    }
}
