using System;
using System.Globalization;
using Xamarin.Forms;

namespace TodoScheduler.Converters
{
    public class BoolToOn_OffConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? "ON" : "OFF";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
