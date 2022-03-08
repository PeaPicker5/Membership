using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dapper.Contrib.Extensions;
using Membership.Annotations;

namespace Membership.Core.Meetings.DataModels
{
    [Table("MEETING_List")]
    public class Meeting : INotifyPropertyChanged
    {
        public Meeting() { }
        public Meeting(Guid meetingId, DateTime meetingDate, string description, string comments, Guid inCharge, bool isRegularScheduled)
        {
            MeetingId = meetingId;
            MeetingDate = meetingDate;
            Description = description;
            Comments = comments;
            InCharge = inCharge;
            IsRegularScheduled = isRegularScheduled;
        }

        [ExplicitKey] public Guid MeetingId  { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public Guid InCharge { get; set; }
        public bool IsRegularScheduled { get; set; }


        private int _memberCount;

        [Computed] public int MemberCount
        {
            get => _memberCount;
            set { _memberCount = value; OnPropertyChanged();}
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
