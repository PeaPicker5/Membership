using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IMeetingAttendanceView
    {
        Meeting CurrentMeeting { get; set; }
        ICollection<Member> MemberList { get; set; }
        ICollection<Member> AdditionalMembers { get; set; }
    }
}
