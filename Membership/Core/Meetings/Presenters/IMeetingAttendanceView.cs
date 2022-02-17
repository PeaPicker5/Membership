using System.Collections.Generic;

namespace Membership.Core.Meetings.Presenters
{
    public interface IMeetingAttendanceView
    {
        Meetings.DataModels.Meeting CurrentMeeting { get; set; }
        ICollection<Member.DataModels.Member> MemberList { get; set; }
        ICollection<Member.DataModels.Member> AdditionalMembers { get; set; }
    }
}
