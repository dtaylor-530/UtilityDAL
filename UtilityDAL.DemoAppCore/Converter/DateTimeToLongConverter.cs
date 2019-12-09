using System;
using System.Globalization;
using System.Windows.Data;

namespace UtilityDAL.DemoApp
{
    public class DateTimeToLongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).Ticks;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new DateTime((long)value);
        }
    }
}