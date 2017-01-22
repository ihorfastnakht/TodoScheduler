using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoScheduler.Services.NavigationServices
{
    public interface INavigationService
    {
        Task CloseAsync(NavType navType = NavType.Stack, bool animation = false);
        Task NavigateAsync(Type viewModelType, Dictionary<string, object> parameters = null, 
            NavType navType = NavType.Stack, bool animation = false);
    }

    public enum NavType
    {
        Stack = 0,
        Modal,
        RelpaceRoot
    }
}
