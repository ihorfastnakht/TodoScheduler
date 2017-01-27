using System;
using System.Threading.Tasks;
using Acr.Notifications;
using System.Diagnostics;

namespace TodoScheduler.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        #region INotificationService implementation

        public async Task SendNotificationAsync(string title, string message, DateTime date)
        {
            await Task.Factory.StartNew(() => {
                var notification = new Notification() {
                    Title = title,
                    Message = message
                };

                notification.SetSchedule(date);
                notification.Vibrate = true;
                Notifications.Instance.Send(notification);
            });
        }

        #endregion
    }
}
