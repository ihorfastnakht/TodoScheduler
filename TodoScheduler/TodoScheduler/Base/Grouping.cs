using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TodoScheduler.Base
{
    public class Grouping<TKey, TItem> : ObservableCollection<TItem>
    {
        public TKey Key { get; private set; }

        public Grouping(TKey _key, IEnumerable<TItem> _items)
        {
            Key = _key;

            foreach (var i in _items)
                this.Items.Add(i);
        }
    }
}
