using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public sealed class StringToVisibilityConverter : IValueConverter
    {
        public Visibility EqualsValue { get; set; }
        public Visibility NotEqualsValue { get; set; }
        public string ComparisonString { get; set; }

        public StringToVisibilityConverter()
        {
            // default values
            EqualsValue = Visibility.Visible;
            NotEqualsValue = Visibility.Collapsed;
            ComparisonString = string.Empty;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return NotEqualsValue;
            return value.ToString() == ComparisonString ? EqualsValue : NotEqualsValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
