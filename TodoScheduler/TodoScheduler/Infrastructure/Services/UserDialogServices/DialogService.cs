using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace TodoScheduler.Infrastructure.Services.UserDialogServices
{
    public class DialogService : IDialogService
    {
        #region IDialogService implementation

        public async Task ShowErrorMessage(string title, string message, string buttonText)
        {
            await Task.Factory.StartNew(() => UserDialogs.Instance.Alert(message, title, buttonText));
        }

        public async Task<bool> ShowMessageWithConfirmation(string title, string message, string okText = "ok", string cancelText = "cancel")
        {
            return await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
            {
                Title = title,
                Message = message,
                OkText = okText,
                CancelText = cancelText
            });
        }

        public async Task ShowPopup(string message)
        {
            await Task.Factory.StartNew(() => UserDialogs.Instance.Toast(message, TimeSpan.FromSeconds(2)));
        }

        #endregion
    }
}
