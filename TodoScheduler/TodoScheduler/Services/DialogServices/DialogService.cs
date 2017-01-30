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

        public async Task<DateTime> ShowDateDialogAsync()
        {
            return await await Task.Factory.StartNew(async () =>
            {
                DatePromptConfig dateCfg = new DatePromptConfig()
                {
                    MinimumDate = DateTime.Now,
                    OkText = "Ok",
                    IsCancellable = false
                };

                DatePromptResult result = await UserDialogs.Instance.DatePromptAsync(dateCfg);

                return result.SelectedDate;
            });
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
        public async Task<TimeSpan> ShowTimeDialogAsync(DateTime date)
        {
            return await await Task.Factory.StartNew(async () =>
            {
                TimePromptConfig timeCfg = new TimePromptConfig()
                {
                    MinuteInterval = 1,
                    Use24HourClock = true,
                    OkText = "Ok",
                    IsCancellable = false
                };

                TimePromptResult result = await UserDialogs.Instance.TimePromptAsync(timeCfg);

                if (date.Date <= DateTime.Now)
                {
                    if (result.SelectedTime <= DateTime.Now.TimeOfDay)
                        throw new Exception($"Selected time must be gather than now");
                }

                return result.SelectedTime;
            });
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
