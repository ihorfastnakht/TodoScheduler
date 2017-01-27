using System;
using System.IO;
using SQLite.Net;
using TodoScheduler.Services.PlatformServices;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;
using TodoScheduler.Android.Services;

[assembly: Dependency(typeof(AndroidSqliteConnectionService))]
namespace TodoScheduler.Android.Services
{
    public class AndroidSqliteConnectionService : ISqliteConnectionService
    {
        private string database = "todo_scheduler.db3";

        public SQLiteConnection GetDatabaseConnection()
        {
            var connectionString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), database);
            var platform = new SQLitePlatformAndroid();

            return new SQLiteConnection(platform, connectionString, false);
        }
    }
}