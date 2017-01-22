using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoScheduler.Infrastructure.Enums;
using Xamarin.Forms;

namespace TodoScheduler.Infrastructure.Services.NavigationServices
{
    public class NavigationService : INavigationService
    {
        #region members

        private INavigation _navigation;

        #endregion

        #region constructor

        public NavigationService(INavigation navigation)
        {
            _navigation = navigation;
        }

        #endregion

        #region INavigationService implementation

        public async Task CloseAsync(NavType navType = NavType.Stack, bool animation = false)
        {
            switch (navType)
            {
                case NavType.Stack:
                    await _navigation.PopAsync(animation);
                    break;
                case NavType.Modal:
                    await _navigation.PopModalAsync(animation);
                    break;
                default:
                    throw new Exception("Navigation is not valid");
            }
        }

        public async Task NavigateAsync(Type viewModelType, Dictionary<string, object> parameters = null, 
            NavType navType = NavType.Stack, bool animation = false)
        {
            switch (navType)
            {
                case NavType.Stack:
                    await _navigation.PushAsync(App.ResolvePage(viewModelType, parameters), animated: animation);
                    break;
                case NavType.Modal:
                    throw new NotImplementedException();
                    break;
                case NavType.ReplaceRoot:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        #endregion
    }
}
