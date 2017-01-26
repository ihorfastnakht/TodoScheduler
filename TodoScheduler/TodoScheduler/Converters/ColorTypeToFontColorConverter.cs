using System;
using System.Globalization;
using TodoScheduler.Enums;
using Xamarin.Forms;

namespace TodoScheduler.Converters
{
    public class ColorTypeToFontColorConverter : IValueConverter
    {
        private const string WHITE_FONT = "#FFFFFF";
        private const string BLACK_FONT = "#000000";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorType = Data.ColorFactory.GetColorTypeByHexValue(value.ToString());

            switch (colorType.ColorType)
            {
                case ColorType.Light:
                    return BLACK_FONT;
                case ColorType.Dark:
                    return WHITE_FONT;
                default:
                    return BLACK_FONT;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
