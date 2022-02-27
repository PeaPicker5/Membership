using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Properties;

namespace Membership.Common.Controls
{
    public partial class InputComboControl : INotifyPropertyChanged
    {
        public InputComboControl()
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
            DependencyProperty.Register("Label", typeof(string), typeof(InputComboControl));

        public ICollection<Tuple<Guid, string>> ComboBoxItems
        {
            get { return (ICollection<Tuple<Guid, string>>)GetValue(ComboBoxItemsProperty); }
            set { 
                SetValue(ComboBoxItemsProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty ComboBoxItemsProperty =
            DependencyProperty.Register("ComboBoxItems", typeof(ICollection<Tuple<Guid, string>>), typeof(InputComboControl));


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}