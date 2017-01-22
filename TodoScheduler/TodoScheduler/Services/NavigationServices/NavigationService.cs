using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TodoScheduler.Services.NavigationServices
{
    public class NavigationService : INavigationService
    {
        #region members

        readonly INavigation _navigation;

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
                case NavType.Stack: await _navigation.PopAsync(animation); break;
                case NavType.Modal: await _navigation.PopModalAsync(animation); break;
                //TODO: Implement this
                case NavType.RelpaceRoot:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new Exception("NavigationService: Invalid navigation route");
            }
        }

        public async Task NavigateAsync(Type viewModelType, Dictionary<string, object> parameters = null, 
            NavType navType = NavType.Stack, bool animation = false)
        {
            //TODO Implement this
            switch (navType)
            {
                case NavType.Stack:
                    throw new NotImplementedException();
                    break;
                case NavType.Modal:
                    throw new NotImplementedException();
                    break;
                case NavType.RelpaceRoot:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new Exception("NavigationService: Invalid navigation route");
            }
        }

        #endregion
    }
}
