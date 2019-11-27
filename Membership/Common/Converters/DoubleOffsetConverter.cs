using System;
using System.Globalization;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    public sealed class DoubleOffsetConverter : IValueConverter
    {
        public double OffsetValue { get; set; }

        public DoubleOffsetConverter()
        {
            //default value
            OffsetValue = 0.0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as double?) - OffsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
