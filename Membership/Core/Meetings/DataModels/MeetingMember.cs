using System;
using Dapper.Contrib.Extensions;

namespace Membership.Core.Meetings.DataModels
{
    [Table("MEETING_Attendance")]
    public class MeetingMember
    {
        public MeetingMember() { }
        public MeetingMember(Guid meetingId, Guid memberId)
        {
            MeetingId = meetingId;
            MemberId = memberId;
        }

        [ExplicitKey] public Guid MeetingId { get; set; }
        public Guid MemberId { get; set; }
    }
}
