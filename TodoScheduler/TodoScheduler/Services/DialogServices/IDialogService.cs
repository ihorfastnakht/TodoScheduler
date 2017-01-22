using System;
using System.Threading.Tasks;

namespace TodoScheduler.Services.DialogServices
{
    public interface IDialogService
    {
        Task ShowToastMessageAsync(string message, TimeSpan showingTime);
        Task ShowErrorMessageAsync(string title, string message, string buttonText = "ok");
        Task<bool> ShowConfirmMessageAsync(string title, string message, string okButton = "ok", 
            string cancelButton = "cancel");
    }
}
