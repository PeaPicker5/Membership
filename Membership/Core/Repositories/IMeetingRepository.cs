using System;
using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public interface IMeetingRepository
    {
        Meeting Get(Guid meetingId);

        ICollection<Member> GetAttendanceList();
        void InsertMeeting(Meeting meetingRec);

        bool DeleteMeeting(Meeting meetingRec);
    }
}
