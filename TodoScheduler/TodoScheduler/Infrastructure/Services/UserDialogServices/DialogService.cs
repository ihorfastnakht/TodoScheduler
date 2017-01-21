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

        public async Task ShowMessage(string title, string message)
        {
            await Task.Factory.StartNew(() => UserDialogs.Instance.Alert(message, title, "ok"));
        }

        #endregion
    }
}
