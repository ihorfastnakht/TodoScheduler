using System;
using System.Globalization;
using TodoScheduler.Services.NavigationServices;
using Xamarin.Forms;

namespace TodoScheduler.Converters
{
    public class NavigationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NavigationService((INavigation)value);
        }
    }
}
