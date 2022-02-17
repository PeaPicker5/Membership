using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.Meetings
{

    public partial class MeetingAttendance : IMeetingAttendanceView, INotifyPropertyChanged
    {
        private readonly MeetingAttendancePresenter _presenter;

        #region Dependency Properties
        public Meeting CurrentMeeting
        {
            get => (Meeting)GetValue(CurrentMeetingProperty);
            set => SetValue(CurrentMeetingProperty, value);
        }
        public static readonly DependencyProperty CurrentMeetingProperty =
            DependencyProperty.Register("CurrentMeeting", typeof(Meeting),
                typeof(MeetingAttendance));

        public ICollection<Member> MemberList
        {
            get => (ICollection<Member>)GetValue(MemberListProperty);
            set { SetValue(MemberListProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty MemberListProperty =
            DependencyProperty.Register("MemberListProperty", typeof(ICollection<Member>),
                typeof(MeetingAttendance));

        public ICollection<Member> AdditionalMembers
        {                           
            get => (ICollection<Member>)GetValue(AdditionalMembersProperty);
            set { SetValue(AdditionalMembersProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty AdditionalMembersProperty =
            DependencyProperty.Register("AdditionalMembers", typeof(ICollection<Member>),
                typeof(MeetingAttendance));


        #endregion

        public MeetingAttendance()
        {
            MemberList = new ObservableCollection<Member>();
            AdditionalMembers = new ObservableCollection<Member>();
            InitializeComponent();
            _presenter = new MeetingAttendancePresenter(this);
            _presenter.LoadAttendanceList();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private void MeetingGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void FilterCheckBoxOnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
