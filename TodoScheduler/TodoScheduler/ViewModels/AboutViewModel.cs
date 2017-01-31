using TodoScheduler.Base;
using Xamarin.Forms;
using TodoScheduler.Services.PlatformServices;
using System.Collections.Generic;

namespace TodoScheduler.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        #region fields & properties

        private readonly string appVersion = DependencyService.Get<IAppVersionService>().GetAppVersion();
        public string Text { get; set; }

        #endregion

        #region override

        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            Text = $@"
Application:

    Todo scheduler v{appVersion}

Description:

    Simple cross - platform todo application created with
    Xamarin Forms  and SQLite database technologies
                               
Plugins:

    - Arc.UserDialogs
    - Arc.Notifications
    - SQLite .NET-PCL
    - XLabs Forms
    - Iconize

Images and icons resources:

    - icon8.com
    - fontawesome.io";
        }

        #endregion
    }
}
