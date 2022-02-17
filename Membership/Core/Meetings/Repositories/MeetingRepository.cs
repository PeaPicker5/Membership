using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Membership.Core.DataModels;
using Membership.Core.Meetings.DataModels;

namespace Membership.Core.Meetings.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private const string DbConnectionName = "MembershipDB";
        public MeetingRepository() { }

        public Meeting Get(Guid meetingId)
        {
            const string query = "SELECT * FROM Meeting WHERE MeetingId = @MeetingId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var meeting = connection.Query<Meeting>(query, new {MeetingId = meetingId}).FirstOrDefault();
                meeting.Attendees = GetMeetingAttendees(meetingId);
                return meeting;
            }
        }

        private static IEnumerable<Guid> GetMeetingAttendees(Guid meetingId)
        {
            const string query = "SELECT MemberId FROM MEETING_Attendance WHERE MeetingId = @MeetingId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<Guid>(query, new { MeetingId = meetingId }).ToList();
            }
        }

        public ICollection<Member.DataModels.Member> GetAttendanceList()
        {
            var retValue = new List<Member.DataModels.Member>();
            const string query = "SELECT * FROM MEMBER_List WHERE Zip LIKE '14%'";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var members = connection.Query<Member.DataModels.Member>(query);
                retValue.AddRange(members.Where(x => x.IsCurrent).OrderBy(y => y.LFName));
                return retValue;
            }

        }
        public void InsertMeeting(Meeting meetingRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                foreach (var memberId in meetingRec.Attendees)
                    connection.Insert(new MeetingMember(meetingRec.MeetingId, memberId));
                connection.Insert(meetingRec);
            }
        }
        public bool DeleteMeeting(Meeting meetingRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                foreach (var memberId in meetingRec.Attendees)
                    connection.Delete(new MeetingMember(meetingRec.MeetingId, memberId));
                connection.Delete(meetingRec);
                return true;
            }
        }


    }
}
