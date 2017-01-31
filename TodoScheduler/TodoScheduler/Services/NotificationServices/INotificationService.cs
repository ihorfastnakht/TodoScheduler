using System;
using System.Threading.Tasks;
using TodoScheduler.Models;

namespace TodoScheduler.Services.NotificationServices
{
    public interface INotificationService
    {
        Task<string> SendNotificationAsync(string title, string message, DateTime date);
        Task CancelTodoNotificationAsync(TodoItem todo);
    }
}
