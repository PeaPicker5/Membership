using System;
using System.Collections.Generic;
using Membership.Core.DataModels;
using Membership.Core.Meetings.DataModels;

namespace Membership.Core.Meetings.Repositories
{
    public interface IMeetingRepository
    {
        Meeting Get(Guid meetingId);

        ICollection<Members.DataModels.Member> GetAttendanceList();
        void InsertMeeting(Meeting meetingRec);

        bool DeleteMeeting(Meeting meetingRec);
    }
}
