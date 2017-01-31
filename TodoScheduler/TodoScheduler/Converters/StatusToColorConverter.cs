using System;
using System.Globalization;
using TodoScheduler.Enums;
using Xamarin.Forms;

namespace TodoScheduler.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (TodoStatus)value;

            if (status == TodoStatus.Completed)
                return "#00CC00";
            if (status == TodoStatus.Postponed)
                return "#FF0000";

            return "#808080";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
