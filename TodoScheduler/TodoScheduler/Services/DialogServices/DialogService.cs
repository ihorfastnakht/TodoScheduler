using System;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace TodoScheduler.Services.DialogServices
{
    public class DialogService : IDialogService
    {
        #region IDialogService implementation

        public async Task<bool> ShowConfirmMessageAsync(string title, string message, string okButton = "ok", string cancelButton = "cancel")
        {
            ConfirmConfig config = new ConfirmConfig() {
                Title = title,
                Message = message,
                OkText = okButton,
                CancelText = cancelButton
            };

            return await UserDialogs.Instance.ConfirmAsync(config);
        }

        public async Task ShowErrorMessageAsync(string title, string message, string buttonText = "ok")
        {
            AlertConfig alert = new AlertConfig() {
                Title = title,
                Message = message,
                OkText = buttonText
            };

            await UserDialogs.Instance.AlertAsync(alert);
        }

        public async Task ShowToastMessageAsync(string message, TimeSpan showingTime)
        {
            await Task.Factory.StartNew(() => {
                ToastConfig toast = new ToastConfig(message) {
                    Duration = showingTime
                };

                UserDialogs.Instance.Toast(toast);
            });
        }

        #endregion
    }
}
