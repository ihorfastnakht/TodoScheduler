using System;
using Xamarin.Forms;
using TodoScheduler.iOS.Services;
using TodoScheduler.Services.PlatformServices;
using SQLite.Net;
using System.IO;
using SQLite.Net.Platform.XamarinIOS;

[assembly: Dependency(typeof(IOSSqliteConnectionService))]

namespace TodoScheduler.iOS.Services
{
    public class IOSSqliteConnectionService : ISqliteConnectionService
    {
        private string database = "todo_scheduler.db3";

        public SQLiteConnection GetDatabaseConnection()
        {
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(docPath, "..", "Library");
            var connectionString = Path.Combine(libraryPath, database);

            var platform = new SQLitePlatformIOS();

            return new SQLiteConnection(platform, connectionString, false);
        }
    }
}
