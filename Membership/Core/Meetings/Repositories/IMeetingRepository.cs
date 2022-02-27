using System;
using System.Collections.Generic;
using Membership.Core.Meetings.DataModels;

namespace Membership.Core.Meetings.Repositories
{
    public interface IMeetingRepository
    {
        Meeting Get(Guid meetingId);
        ICollection<Meeting> GetMeetings();
        IEnumerable<int> GetMeetingYearsOnFile();

        void InsertMeeting(Meeting meetingRec);

        bool DeleteMeeting(Meeting meetingRec);
    }
}
