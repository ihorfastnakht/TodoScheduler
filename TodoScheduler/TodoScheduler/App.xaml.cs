using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoScheduler.Infrastructure.Base;
using TodoScheduler.Infrastructure.Services.DataServices;
using TodoScheduler.Infrastructure.Services.UserDialogServices;
using TodoScheduler.ViewModels;
using Xamarin.Forms;

namespace TodoScheduler
{
    public partial class App : Application
    {
        private static IUnityContainer _container = new UnityContainer();

        public App()
        {
            InitializeComponent();
            RegisterTypes();
            MainPage = GetMainPage();
        }

        private static void RegisterTypes()
        {
            _container.RegisterInstance<IDataService>(new TestDataService());
            _container.RegisterInstance<IDialogService>(new DialogService());
        }

        private static Page GetMainPage()
        {
            var main = (MasterDetailPage)ResolvePage(typeof(AppViewModel));
            main.Master = ResolvePage(typeof(MenuViewModel));
            main.Detail = new NavigationPage(ResolvePage(typeof(TagsViewModel)));
            return main;
        }

        public static Page ResolvePage(Type viewModelType, Dictionary<string, object> parameters = null)
        {
            var pageTypeName = viewModelType.AssemblyQualifiedName
                .Replace("ViewModels", "Pages").Replace("ViewModel", "Page");

            var pageType = Type.GetType(pageTypeName);

            if (pageType == null)
                throw new Exception($"Page {pageType} not found");

            var page = (Page)_container.Resolve(pageType);
            var viewModel = (ViewModelBase)_container.Resolve(viewModelType);

            viewModel.Init(parameters);
            page.BindingContext = viewModel;

            return page;
        }
        public static void ResolveDetailPage(Type viewModelType, Dictionary<string, object> parameters = null)
        {
            MasterDetailPage masterPage = (MasterDetailPage)App.Current.MainPage;
            var page = (Page)ResolvePage(viewModelType, parameters);
            NavigationPage navPage = new NavigationPage(page);

            masterPage.Detail = navPage;

            if (Device.Idiom == TargetIdiom.Phone)
                masterPage.IsPresented = false;
            else
                masterPage.IsPresented = true;
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
