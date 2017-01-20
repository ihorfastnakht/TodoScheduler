using System.Collections.Generic;
using TodoScheduler.Infrastructure.Services.NavigationServices;

namespace TodoScheduler.Infrastructure.Base
{
    public abstract class ViewModelBase : ObservableObject
    {
        #region fields & properties

        INavigationService _navigation;
        public INavigationService Navigation {
            get { return _navigation; }
            set { SetProperty(ref _navigation, value); }
        }

        string _icon;
        public string Icon {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        string _header;
        public string Header {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }


        #endregion

        #region virtual

        public virtual void Init(Dictionary<string, object>[] parameters = null) { }
        public virtual void Appearing() { }
        public virtual void Disappearing() { }
         
        #endregion
    }
}
