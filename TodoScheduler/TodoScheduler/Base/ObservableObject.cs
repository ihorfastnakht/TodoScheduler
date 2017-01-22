using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoScheduler.Base
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region protected

        protected bool SetProperty<T>(ref T backedField, T value, [CallerMemberName] string propertyName = "",
            string[] dependendProperties = null)
        {
            if (EqualityComparer<T>.Default.Equals(backedField, value)) return false;

            if (dependendProperties != null)
            {
                foreach (var property in dependendProperties)
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
