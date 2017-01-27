using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace TodoScheduler.Android
{
    [Activity(Label = "TodoScheduler", Icon = "@drawable/icon", Theme = "@style/MainTheme", 
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());

            UserDialogs.Init(() => (Activity)Forms.Context);

            LoadApplication(new App());
        }

        //HACK:
        //Fixed: Java.Lang.IllegalStateException: Activity has been destroyed
        protected override void OnDestroy()
        {
            try
            {
                base.OnDestroy();
            }
            catch
            {
            }
        }
    }
}

