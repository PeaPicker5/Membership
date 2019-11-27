using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    public sealed class IntegerToVisibilityConverter :IValueConverter
    {

        public Visibility EqualsValue { get; set; }
        public Visibility NotEqualsValue { get; set; }
        public int ComparisonInteger { get; set; }

        public IntegerToVisibilityConverter()
        {
            // default values
            EqualsValue = Visibility.Visible;
            NotEqualsValue = Visibility.Collapsed;
            ComparisonInteger = 0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int)) return null;

            return (int) value == ComparisonInteger ? EqualsValue : NotEqualsValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, EqualsValue)) return true;

            if (Equals(value, NotEqualsValue)) return false;

            return null;
        }
    }
}
