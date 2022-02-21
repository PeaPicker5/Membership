using System;
using System.Collections.Generic;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Members.DataModels;

namespace Membership.Core.Meetings.Repositories
{
    public interface IMeetingRepository
    {
        Meeting Get(Guid meetingId);
        ICollection<Meeting> GetMeetings();
        ICollection<Member> GetAttendanceList();
        
        void InsertMeeting(Meeting meetingRec);

        bool DeleteMeeting(Meeting meetingRec);

        IEnumerable<int> GetYearsOnFile();
    }
}
