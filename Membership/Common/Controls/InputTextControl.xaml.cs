using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Properties;

namespace Membership.Common.Controls
{
    /// <summary>
    /// Interaction logic for LabelTextBox.xaml
    /// </summary>
    public partial class InputTextControl : INotifyPropertyChanged
    {
        public InputTextControl()
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
            DependencyProperty.Register("Label", typeof(string), typeof(InputTextControl));


        public string TextBox
        {
            get { return (string)GetValue(TextBoxProperty); }
            set { 
                SetValue(TextBoxProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty TextBoxProperty =
            DependencyProperty.Register("TextBox", typeof(string), typeof(InputTextControl));

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}