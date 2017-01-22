using System;
using System.Threading.Tasks;
using Acr.Notifications;

namespace TodoScheduler.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        #region INotificationService implementation

        public async Task SendNotificationAsync(string title, string message, DateTime date, TimeSpan time)
        {
            await Task.Factory.StartNew(() => {
                var notification = new Notification() {
                    Title = title,
                    Message = message,
                    Date = date,
                    When = time
                };

                Notifications.Instance.Send(notification);
            });
        }

        #endregion
    }
}
