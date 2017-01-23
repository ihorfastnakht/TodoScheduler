using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoScheduler.Base;
using TodoScheduler.Services.DataServices;
using TodoScheduler.Services.DialogServices;
using TodoScheduler.Services.NotificationServices;
using TodoScheduler.ViewModels;
using Xamarin.Forms;

namespace TodoScheduler
{
    public partial class App : Application
    {
        private readonly static IUnityContainer _container = new UnityContainer();

        public App()
        {
            InitializeComponent();
            RegisterTypes();
            MainPage = GetStartUpPage();
        }


        public static Page ResolvePage(Type viewModelType, Dictionary<string, object> parameters = null)
        {
            var pageTypeName = viewModelType.AssemblyQualifiedName.Replace("ViewModels", "Pages").Replace("ViewModel", "Page");
            var pageType = Type.GetType(pageTypeName);
            if (pageType == null)
                throw new Exception();

            var page = (Page)_container.Resolve(pageType);
            var viewModel = (ViewModelBase)_container.Resolve(viewModelType);

            viewModel.Init(parameters);
            page.BindingContext = viewModel;

            return page;
        }
        public static void ResolveDetailPage(Type viewModelType, Dictionary<string, object> parameters = null)
        {
            var masterDetail = (MasterDetailPage)App.Current.MainPage;
            var navigationPage = new NavigationPage(ResolvePage(viewModelType, parameters));
            masterDetail.Detail = navigationPage;

            if (Device.Idiom == TargetIdiom.Phone)
                masterDetail.IsPresented = false;
            else
                masterDetail.IsPresented = true;
        }

        public static Page GetStartUpPage()
        {
            MasterDetailPage mainPage = new MasterDetailPage();
            mainPage.Master = ResolvePage(typeof(MenuViewModel));
            mainPage.Detail = new NavigationPage(ResolvePage(typeof(TagsViewModel)));

            return mainPage;
        }

        private static void RegisterTypes()
        {
            _container.RegisterType<IDataService, SqliteDataService>();
            _container.RegisterType<INotificationService, NotificationService>();
            _container.RegisterType<IDialogService, DialogService>();
        }

        #region application state handlers

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #endregion
    }
}
