using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Models;
using SQLite.Net;
using TodoScheduler.Services.PlatformServices;
using Xamarin.Forms;
using System.Diagnostics;

namespace TodoScheduler.Services.DataServices
{
    public class SqliteDataService : IDataService
    {
        readonly SQLiteConnection _connection;

        #region constructor

        public SqliteDataService()
        {
            _connection = DependencyService.Get<ISqliteConnectionService>().GetDatabaseConnection();

            if (_connection == null)
                throw new ArgumentNullException("SqliteDataService: connection is null");
            
            //Create tables
            _connection.CreateTable<TagItem>();


            var tag = new TagItem() {
                Title = "Default",
                HexColor = "#7635EB"
            };

            if (!IsExistTag(tag))
                _connection.Insert(tag);
            
        }

        #endregion

        #region private

        private bool IsExistTag(TagItem tagItem)
        {
            var exist = _connection.Table<TagItem>().Where(t => t.Title == tagItem.Title).FirstOrDefault();
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

                _connection.Insert(tagItem);
            });
        }

        public async Task<IEnumerable<TagItem>> GetTagItemsAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                return _connection.Table<TagItem>().ToList();
            });
        }

        public async Task RemoveTagItemAsync(TagItem tagItem)
        {
            await Task.Factory.StartNew(() =>
            {
                if (tagItem == null)
                    throw new ArgumentNullException("TagItem is null");
                if (IsExistTag(tagItem))
                    throw new Exception($"Tag ({tagItem.Title}) not exist");

                _connection.Delete(tagItem);
            });
        }

        #endregion
    }
}
