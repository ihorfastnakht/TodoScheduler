using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Infrastructure.Services.PlatformSpecificServices;
using TodoScheduler.Models;
using Xamarin.Forms;
//using SQLite;
//using SQLite.Net;

namespace TodoScheduler.Infrastructure.Services.DataServices
{
    public class SqliteDataService : IDataService
    {
        #region members

        private string _path;
        //private SQLiteConnection _connection;

        #endregion

        #region constructor

        public SqliteDataService()
        {
            _path = DependencyService.Get<IFileSystemService>().GetDataBasePath();
            //_connection = new SQLiteConnection(_path); 
        }

        #endregion


        #region IDataService implementation

        public Task CreateTagItemAsync(TagItem tagItem)
        {
            throw new NotImplementedException();
        }

        public Task CreateTodoItemAsync(TodoItem todoItem)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TagItem>> GetTagItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoItem>> GetTodoItemsAsync(TagItem tagItem)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoItem>> GetTodoItemsAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTagItemAsync(TagItem tagItem)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTodoItemAsync(TodoItem todoItem)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTagItemAsync(TagItem todoItem)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTodoItemAsync(TodoItem todoItem)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
