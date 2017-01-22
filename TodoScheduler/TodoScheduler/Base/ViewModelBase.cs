using System.Collections.Generic;

namespace TodoScheduler.Base
{
    public abstract class ViewModelBase : ObservableObject
    {

        #region fields & properties

        string _icon;
        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        string _header;
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        #endregion

        #region virtual

        public virtual void Init(Dictionary<string, object> parameters = null) { }
        public virtual void Appearing() { }
        public virtual void Disappearing() { }

        #endregion
    }
}
