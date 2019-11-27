using System;
using System.Globalization;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public sealed class BoolToStringConverter : IValueConverter
    {
        public string TrueValue { get; set; }
        public string FalseValue { get; set; }

        public BoolToStringConverter()
        {
            //set default values
            TrueValue = string.Empty;
            FalseValue = string.Empty;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return null;
            }
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
            {
                return true;
            }
            if (Equals(value, FalseValue))
            {
                return false;
            }
            return null;
        }
    }
}
