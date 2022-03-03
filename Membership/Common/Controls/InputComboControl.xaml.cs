using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Properties;

namespace Membership.Common.Controls
{
    public sealed partial class InputComboControl : INotifyPropertyChanged
    {
        public InputComboControl()
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
            DependencyProperty.Register("Label", typeof(string), typeof(InputComboControl));

        public ICollection<Tuple<Guid, string>> ComboBoxItems
        {
            get => (ICollection<Tuple<Guid, string>>)GetValue(ComboBoxItemsProperty);
            set { 
                SetValue(ComboBoxItemsProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty ComboBoxItemsProperty =
            DependencyProperty.Register("ComboBoxItems", typeof(ICollection<Tuple<Guid, string>>), typeof(InputComboControl));

        public Guid SelectedItemGuid
        {
            get => (Guid)GetValue(SelectedItemGuidProperty);
            set => SetValue(SelectedItemGuidProperty, value);
        }
        public static readonly DependencyProperty SelectedItemGuidProperty =
            DependencyProperty.Register("SelectedItemGuid", typeof(Guid), typeof(InputComboControl));


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            if (comboBox.SelectedIndex < 0)
                SelectedItemGuid = Guid.Empty;
            else
                SelectedItemGuid = (Guid)comboBox.SelectedValue;
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}