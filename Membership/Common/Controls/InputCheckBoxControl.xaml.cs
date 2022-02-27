using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Properties;

namespace Membership.Common.Controls
{

    public partial class InputCheckBoxControl : INotifyPropertyChanged
    {
        public InputCheckBoxControl()
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
            DependencyProperty.Register("Label", typeof(string), typeof(InputCheckBoxControl));

        public bool IsChecked    
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { 
                SetValue(IsCheckedProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(InputCheckBoxControl));

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}