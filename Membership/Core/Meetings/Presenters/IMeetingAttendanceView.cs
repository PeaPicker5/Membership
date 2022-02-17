using System.Collections.Generic;

namespace Membership.Core.Meetings.Presenters
{
    public interface IMeetingAttendanceView
    {
        Meetings.DataModels.Meeting CurrentMeeting { get; set; }
        ICollection<Members.DataModels.Member> MemberList { get; set; }
        ICollection<Members.DataModels.Member> AdditionalMembers { get; set; }
    }
}
