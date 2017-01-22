using SQLite.Net;

namespace TodoScheduler.Services.PlatformServices
{
    public interface ISqliteConnectionService
    {
        SQLiteConnection GetDatabaseConnection();
    }
}
