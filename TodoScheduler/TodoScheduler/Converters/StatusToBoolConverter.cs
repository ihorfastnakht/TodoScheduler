using System;
using System.Globalization;
using TodoScheduler.Enums;
using Xamarin.Forms;

namespace TodoScheduler.Converters
{
    public class StatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (TodoStatus)value;

            if (status == TodoStatus.Completed)
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
