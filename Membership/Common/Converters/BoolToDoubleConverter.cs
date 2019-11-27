using System;
using System.Globalization;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(bool), typeof(double))]
    public sealed class BoolToDoubleConverter : IValueConverter
    {
        public double TrueValue { get; set; }
        public double FalseValue { get; set; }

        public BoolToDoubleConverter()
        {
            //set defaults
            TrueValue = 1.0;
            FalseValue = 0.5;
        }

        public object Convert(object value, Type targeType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return null;

            return (bool) value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
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
