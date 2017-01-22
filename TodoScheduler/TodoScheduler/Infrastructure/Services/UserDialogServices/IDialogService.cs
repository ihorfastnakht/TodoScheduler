using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TodoScheduler.Infrastructure.Services.UserDialogServices
{
    public interface IDialogService
    {
        Task ShowErrorMessage(string title, string message, string buttonText);
        Task ShowPopup(string message);
        Task<bool> ShowMessageWithConfirmation(string title, string message, string okText = "ok", string cancelText = "cancel");
    }
}
