using System;
using System.Globalization;
using System.Windows.Data;

namespace Membership.Common.Converters
{

    public class MinimumDateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime && (DateTime)value < new DateTime(2, 1, 1))
            {
                return null;
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
