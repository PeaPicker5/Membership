using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Properties;

namespace Membership.Common.Controls
{
    public sealed partial class InputDateControl : INotifyPropertyChanged
    {
        public InputDateControl()
        {
            InitializeComponent();
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
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
            get => (string)GetValue(DtpTextProperty);
            set { 
                SetValue(DtpTextProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty DtpTextProperty =
            DependencyProperty.Register("DtpText", typeof(string), typeof(InputDateControl));


        
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}