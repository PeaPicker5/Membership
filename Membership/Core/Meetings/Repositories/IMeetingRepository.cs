using System;
using System.Collections.Generic;
using Membership.Core.DataModels;
using Membership.Core.Meetings.DataModels;

namespace Membership.Core.Meetings.Repositories
{
    public interface IMeetingRepository
    {
        Meeting Get(Guid meetingId);
        IEnumerable<Meeting> GetMeetings();
        ICollection<SelectableItem> GetMeetingAttendees(Guid meetingId);
        IEnumerable<int> GetMeetingYearsOnFile();

        void InsertMeeting(Meeting meetingRec, IEnumerable<SelectableItem> Attendees);
        void UpdateMeeting(Meeting meetingRec, IEnumerable<SelectableItem> Attendees);
        void DeleteMeeting(Meeting meetingRec);
    }
}
