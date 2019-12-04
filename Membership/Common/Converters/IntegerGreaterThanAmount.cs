using System;
using System.Globalization;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(int), typeof(bool))]
    public class IntegerGreaterThanAmount : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) > Cutoff;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public int Cutoff { get; set; }
    }
}
