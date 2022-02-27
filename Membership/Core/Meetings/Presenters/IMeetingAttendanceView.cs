using System.Collections.Generic;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Members.DataModels;

namespace Membership.Core.Meetings.Presenters
{
    public interface IMeetingAttendanceView
    {
        Meeting CurrentMeeting { get; set; }
        ICollection<SelectableMember> MemberCheckList { get; set; }
    }
}
