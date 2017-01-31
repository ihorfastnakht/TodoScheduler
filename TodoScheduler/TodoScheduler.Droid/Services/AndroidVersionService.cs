using Android.Content;
using TodoScheduler.Services.PlatformServices;
using Xamarin.Forms;
using TodoScheduler.Android.Services;

[assembly: Dependency(typeof(AndroidVersionService))]
namespace TodoScheduler.Android.Services
{
    public class AndroidVersionService : IAppVersionService
    {
        #region IAppVersionService implementation

        public string GetAppVersion()
        {
            Context context = Forms.Context;
            return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
        }

        #endregion
    }
}