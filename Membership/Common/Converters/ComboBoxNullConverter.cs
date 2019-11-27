using System;
using System.Globalization;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    /// <summary>
    /// Converts blank combobox items to -1 as a value. 
    /// I don't like using -1 to represent no selection, please update if there is a better solution.
    /// </summary>
    public class ComboBoxNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return -1;
            }
            return value;
        }
    }
}
