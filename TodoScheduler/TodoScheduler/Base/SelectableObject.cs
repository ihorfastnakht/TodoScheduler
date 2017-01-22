namespace TodoScheduler.Base
{
    public class SelectableObject<T> : ObservableObject
    {
        T _item;
        public T Item {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        bool _isSelected = false;
        public bool IsSelected {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }   
    }
}
