using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoScheduler.Infrastructure.Base
{
    public class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region protected

        protected bool SetProperty<T>(ref T backedField, T value, [CallerMemberName] string propertyName = "",
            string[] dependendProperty = null)
        {
            if (EqualityComparer<T>.Default.Equals(backedField, value)) return false;

            if (dependendProperty != null)
            {
                foreach (var property in dependendProperty)
                    OnPropertyChanged(property);
            }

            backedField = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
