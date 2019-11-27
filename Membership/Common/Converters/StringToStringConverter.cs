using System;
using System.Globalization;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public sealed class StringToStringConverter : IValueConverter
    {
        public string EqualsValue { get; set; }
        public string ComparisonString { get; set; }

        public StringToStringConverter()
        {
            // default values
            EqualsValue = string.Empty;
            ComparisonString = string.Empty;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return null;
            
            return value.ToString() == ComparisonString ? EqualsValue : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return value.ToString() == EqualsValue ? ComparisonString : value.ToString();
        }
    }
}
