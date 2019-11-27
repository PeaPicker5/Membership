using System;
using System.Globalization;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(string), typeof(bool))]
    internal class ScriptParameterToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool)value ? "1" : "0";
        }
    }
}
