using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Members.DataModels;

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
        public ICollection<Meeting> GetMeetings()
        {
            const string query = "SELECT * FROM MEETING_List ";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var listOfMeetings = connection.Query<Meeting>(query).ToList();
                foreach (var meet in listOfMeetings)
                {
                    meet.Attendees = GetMeetingAttendees(meet.MeetingId);
                }
                return listOfMeetings;
            }
        }
        private static ICollection<SelectableMember> GetMeetingAttendees(Guid meetingId)
        {
            const string query = @"SELECT ml.*, 1 as 'IsSelected'
                                    FROM MEMBER_List ml 
                                    INNER JOIN MEETING_Members mm
                                    ON mm.MemberId = ml.MemberID   
                                    WHERE MeetingId = @MeetingId";

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<SelectableMember>(query, new { MeetingId = meetingId}).ToList();
            }
        }
        public void InsertMeeting(Meeting meetingRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                foreach (var member in meetingRec.Attendees)
                    connection.Insert(new MeetingMember(meetingRec.MeetingId, member.MemberId));
                connection.Insert(meetingRec);
            }
        }
        public bool DeleteMeeting(Meeting meetingRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                foreach (var member in meetingRec.Attendees)
                    connection.Delete(new MeetingMember(meetingRec.MeetingId, member.MemberId));
                connection.Delete(meetingRec);
                return true;
            }
        }

        public IEnumerable<int> GetMeetingYearsOnFile()
        {
            const string query = "SELECT DISTINCT Year(MeetingDate) FROM MEETING_List ORDER BY Year(MeetingDate) DESC";

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<int>(query).ToList();
            }
        }

    }
}
