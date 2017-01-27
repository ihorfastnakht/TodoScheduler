using System;
using System.Globalization;
using TodoScheduler.Enums;
using Xamarin.Forms;

namespace TodoScheduler.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var priority = (TodoPriority)value;

            var _color = string.Empty;

            switch (priority)
            {
                case TodoPriority.Low:
                    _color = "#FFFF00";
                    break;
                case TodoPriority.Normal:
                    _color = "#FF8000";
                    break;
                case TodoPriority.High:
                    _color = "#FF0000";
                    break;
            }
            return _color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
