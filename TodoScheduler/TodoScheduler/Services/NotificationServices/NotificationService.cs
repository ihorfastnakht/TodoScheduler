using System;
using System.Threading.Tasks;
using Acr.Notifications;
using System.Diagnostics;
using TodoScheduler.Models;

namespace TodoScheduler.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        #region INotificationService implementation

        public async Task<string> SendNotificationAsync(string title, string message, DateTime date)
        {
            return await Task.Factory.StartNew(() => {

                Notifications.Instance.Vibrate(2000);
                var notification = new Notification() {
                    Title = title,
                    Message = message,
                };

                notification.SetSchedule(date);
                notification.Vibrate = true;
                return Notifications.Instance.Send(notification);
            });
        }

        public async Task CancelTodoNotificationAsync(TodoItem todo)
        {
            await Task.Factory.StartNew(() =>
            {
                if (todo == null)
                    return;
                Notifications.Instance.Cancel(todo.ReminderId);
            });
        }

        #endregion
    }
}
