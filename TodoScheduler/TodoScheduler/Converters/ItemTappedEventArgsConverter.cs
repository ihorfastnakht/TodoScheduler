using System;
using System.Globalization;
using Xamarin.Forms;

namespace TodoScheduler.Converters
{
    public class ItemTappedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = (ItemTappedEventArgs)value;
            if (eventArgs == null) return null;

            return eventArgs.Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
