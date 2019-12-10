using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.Members
{
    public partial class MemberInfo : IMemberView, INotifyPropertyChanged
    {
        public static IEnumerable<string> StateInits => new List<string>()
        {
            "AK","AL","AR","AZ","CA","CO","CT","DC","DE","FL","GA","HI","IA","ID","IL","IN","KS","KY","LA","MA","MD","ME","MI","MN","MO","MS","MT",
            "NC","ND","NE","NH","NJ","NM","NV","NY","OH","OK","OR","PA","RI","SC","SD","TN","TX","UT","VA","VT","WA","WI","WV","WY"
        };
        private readonly MemberInfoPresenter _presenter;

        #region Dependency Properties
        public Member MemberRec
        {
            get { return (Member)GetValue(MemberRecProperty); }
            set { SetValue(MemberRecProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty MemberRecProperty =
            DependencyProperty.Register("MemberRec", typeof(Member),
                typeof(MemberInfo));

        public ICollection<Tuple<int,string>> MemberTypeLookups 
        {
            get { return (ICollection<Tuple<int, string>>)GetValue(MemberTypeLookupsProperty); }
            set { SetValue(MemberTypeLookupsProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty MemberTypeLookupsProperty =
            DependencyProperty.Register("MemberTypeLookups", typeof(ICollection<Tuple<int, string>>),
                typeof(MemberInfo));

        public ICollection<Tuple<Guid,string>> MemberLookups
        {
            get { return (ICollection<Tuple<Guid,string>>)GetValue(MemberLookupsProperty); }
            set { SetValue(MemberLookupsProperty, value); }
        }
        public static readonly DependencyProperty MemberLookupsProperty =
            DependencyProperty.Register("MemberLookups", typeof(ICollection<Tuple<Guid,string>>),
                typeof(MemberInfo));



        #endregion

        public MemberInfo()
        {
            MemberLookups = new ObservableCollection<Tuple<Guid, string>>();
            MemberTypeLookups = new ObservableCollection<Tuple<int, string>>();
            InitializeComponent();
            _presenter = new MemberInfoPresenter(this);
            _presenter.LoadLookups();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }

        private void AddMemberButtonOnClick(object sender, RoutedEventArgs e)
        {

        }

        private void EditMemberButtonOnClick(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteMemberButtonOnClick(object sender, RoutedEventArgs e)
        {

        }
        private void DatePickerOnPreviewTextInput(object sender, TextCompositionEventArgs eventArgs)
        {
            eventArgs.Handled = eventArgs.Text.Any(
                character => !char.IsDigit(character) && character != '/' && character != '-');
        }

        private void StartDatePicker_OnSelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
