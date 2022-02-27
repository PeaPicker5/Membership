using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Properties;

namespace Membership.Common.Controls
{
    /// <summary>
    /// Interaction logic for LabelTextBox.xaml
    /// </summary>
    public partial class InputDateControl : INotifyPropertyChanged
    {
        public InputDateControl()
        {
            InitializeComponent();
        }


        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set
            {
                SetValue(LabelProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(InputDateControl));


        public string DtpText    
        {
            get { return (string)GetValue(DtpTextProperty); }
            set { 
                SetValue(DtpTextProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty DtpTextProperty =
            DependencyProperty.Register("DtpText", typeof(string), typeof(InputDateControl));

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}