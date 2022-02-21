using System.Collections.Generic;
using Membership.Core.Meetings.DataModels;

namespace Membership.Core.Meetings.Presenters
{
    public interface IMeetingSelectionView
    {
        ICollection<Meeting> MeetingList { get; set; }
    }
}
