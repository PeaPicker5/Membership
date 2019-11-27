using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Common;
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

        public Member MemberRec
        {
            get { return (Member)GetValue(MemberRecProperty); }
            set
            {
                SetValue(MemberRecProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty MemberRecProperty =
            DependencyProperty.Register("MemberRec", typeof(Member), typeof(MemberInfo));


        public MemberInfo()
        {
            InitializeComponent();
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
    }
}
