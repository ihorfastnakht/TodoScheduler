using Xamarin.Forms;
using System.IO;
using Windows.Storage;
using TodoScheduler.Services.PlatformServices;
using SQLite.Net;
using TodoScheduler.UWP.Services;
using SQLite.Net.Platform.WinRT;

[assembly: Dependency(typeof(UWPSqlConnectionService))]
namespace TodoScheduler.UWP.Services
{
    public class UWPSqlConnectionService : ISqliteConnectionService
    {
        private string database = "todo_scheduler.db3";

        public SQLiteConnection GetDatabaseConnection()
        {
            var connectionString = Path.Combine(ApplicationData.Current.LocalFolder.Path, database);
            var platform = new SQLitePlatformWinRT();

            return new SQLiteConnection(platform, connectionString, false);
        }
    }
}
