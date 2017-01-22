using System.Threading.Tasks;

namespace TodoScheduler.Services.NotificationServices
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string title, string message);
    }
}
