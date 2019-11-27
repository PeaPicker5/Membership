using System;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(PhysicalAddress), typeof(string))]
    public class StringToPhysicalAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return PhysicalAddress.Parse(value?.ToString()) ?? PhysicalAddress.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }
}
