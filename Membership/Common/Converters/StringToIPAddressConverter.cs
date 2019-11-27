using System;
using System.Globalization;
using System.Net;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(IPAddress), typeof(string))]
    public class StringToIPAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IPAddress address;
            IPAddress.TryParse((string)value, out address);
            return address ?? IPAddress.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
