using System;
using System.Globalization;
using System.Windows.Data;

namespace Membership.Common.Converters
{
    [ValueConversion(typeof(int), typeof(bool))]

    // XAML USAGE
    //
    //<DataGrid.Resources>
    //  <Style x:Key="CheckBoxStyle" TargetType="{x:Type CheckBox}" >
    //      <Style.Triggers>
    //          <DataTrigger Binding = "{Binding Path=Year,Converter={StaticResource IntegerGreaterThanAmount}}"
    //                                      Value="True">
    //               <Setter Property = "IsEnabled" Value="False"/>
    //          </DataTrigger>
    //      </Style.Triggers>
    //   </Style>
    //</DataGrid.Resources>-->

    //  <DataGridCheckBoxColumn MinWidth = "40" Header="PAID" Binding="{Binding Path=IsPaid}" 
    //                          ElementStyle="{StaticResource CheckBoxStyle}" />


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
