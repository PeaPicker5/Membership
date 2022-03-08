using System.Collections.Generic;
using Membership.Core.DataModels;
using Membership.Core.Meetings.DataModels;

namespace Membership.Core.Meetings.Presenters
{
    public interface IMeetingAttendanceView
    {
        Meeting CurrentMeeting { get; set; }
        ICollection<SelectableItem> MemberCheckList { get; set; }
    }
}
