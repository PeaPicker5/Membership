using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Core.DataModels;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Meetings.Presenters;
using Membership.Core.Members.DataModels;
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
            set { SetValue(CurrentMeetingProperty, value);
                SetControlValues();
            }
        }
        public static readonly DependencyProperty CurrentMeetingProperty =
            DependencyProperty.Register("CurrentMeeting", typeof(Meeting),
                typeof(MeetingAttendance));

        public ICollection<SelectableItem> MemberCheckList
        {
            get => (ICollection<SelectableItem>)GetValue(MemberCheckListProperty);
            set { SetValue(MemberCheckListProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty MemberCheckListProperty =
            DependencyProperty.Register("MemberCheckList", typeof(ICollection<SelectableItem>),
                typeof(MeetingAttendance));

        public int AttendCount
        {
            get => (int)GetValue(AttendCountProperty);
            set { SetValue(AttendCountProperty, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty AttendCountProperty =
            DependencyProperty.Register("AttendCount", typeof(int),
                typeof(MeetingAttendance), new PropertyMetadata(0));

        public ICollection<SelectableItem> Attendees
        {
            get => (ICollection<SelectableItem>)GetValue(AttendeesProperty);
            set => SetValue(AttendeesProperty, value);
        }

        public static readonly DependencyProperty AttendeesProperty =
            DependencyProperty.Register("Attendees", typeof(ICollection<SelectableItem>),
                typeof(MeetingAttendance));


        public ICollection<Tuple<Guid,string>> InChargeList
        {
            get { return (ICollection<Tuple<Guid, string>>)GetValue(InChargeListProperty); }
            set { SetValue(InChargeListProperty, value); }
        }
        public static readonly DependencyProperty InChargeListProperty =
            DependencyProperty.Register("InChargeList", typeof(ICollection<Tuple<Guid, string>>),
                typeof(MeetingAttendance));


        #endregion

        public MeetingAttendance()
        {
            MemberCheckList = new ObservableCollection<SelectableItem>();
            Attendees = new ObservableCollection<SelectableItem>();
            InitializeComponent();
            _presenter = new MeetingAttendancePresenter(this);
        }

        private void SetControlValues()
        {
            InChargeList = _presenter.GetInChargeList(CurrentMeeting.MeetingDate.Year);
            _presenter.LoadMeetingAttendance(CurrentMeeting.MeetingId);
            AttendCount = MemberCheckList.Count(mem => mem.IsSelected);
        }
        private void MemberName_OnCheckChanged(object sender, RoutedEventArgs e)
        {
            var member = (SelectableItem) ((CheckBox) sender).DataContext;

            if (member.IsSelected)
            {
                if (member.CheckStatus == SelectableMember.enumCheckStatus.Original)
                    member.CheckStatus = SelectableMember.enumCheckStatus.Added;
                if (member.IsSelected && member.CheckStatus == SelectableMember.enumCheckStatus.Removed)
                    member.CheckStatus = SelectableMember.enumCheckStatus.Original;
                Attendees.Add(member);
            }
            else
            {
                if (!member.IsSelected && member.CheckStatus == SelectableMember.enumCheckStatus.Original)
                    member.CheckStatus = SelectableMember.enumCheckStatus.Removed;
                if (!member.IsSelected && member.CheckStatus == SelectableMember.enumCheckStatus.Added)
                    member.CheckStatus = SelectableMember.enumCheckStatus.Original;
                Attendees.Remove(member);
            }
            CurrentMeeting.MemberCount = MemberCheckList.Count(mem => mem.IsSelected);
        }



        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
