using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
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

        private static ICollection<Tuple<Guid,string>> GetMeetingAttendees(Guid meetingId)
        {
            const string query = @"SELECT mm.memberId, ml.Lastname + ' ' + ml.Suffix + ', ' + ml.Firstname + ' ' + ml.MI as LFName  
                                    FROM MEETING_Members mm
                                    INNER JOIN MEMBER_List ml 
                                    ON mm.MemberId = ml.MemberID   
                                    WHERE MeetingId = @MeetingId";
            
            var retValue = new List<Tuple<Guid, string>>();
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            { 
                retValue = connection.Query<Guid, string, Tuple<Guid,string>>(query,Tuple.Create, new { MeetingId = meetingId }, splitOn: "*").ToList();    
                   
                return retValue;
            }

        }

        public ICollection<Meeting> GetMeetings()
        {
            const string query = "SELECT * FROM MEETING_List ";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var meetingRecs = connection.Query<Meeting>(query).ToList();
                foreach (var meet in meetingRecs)
                {
                    meet.Attendees = GetMeetingAttendees(meet.MeetingId);
                }
                return meetingRecs;
            }
        }
        public ICollection<Member> GetAttendanceList()
        {
            var retValue = new List<Member>();
            const string query = "SELECT * FROM MEMBER_List WHERE Zip LIKE '14%'";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var members = connection.Query<Member>(query);
                retValue.AddRange(members.Where(x => x.IsCurrent).OrderBy(y => y.LFName));
                return retValue;
            }

        }
        public void InsertMeeting(Meeting meetingRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                foreach (var memberTuple in meetingRec.Attendees)
                    connection.Insert(new MeetingMember(meetingRec.MeetingId, memberTuple.Item1));
                connection.Insert(meetingRec);
            }
        }
        public bool DeleteMeeting(Meeting meetingRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                foreach (var memberTuple in meetingRec.Attendees)
                    connection.Delete(new MeetingMember(meetingRec.MeetingId, memberTuple.Item1));
                connection.Delete(meetingRec);
                return true;
            }
        }


        public IEnumerable<int> GetYearsOnFile()
        {
            const string query = "SELECT DISTINCT Year(MeetingDate) FROM MEETING_List ORDER BY Year(MeetingDate) DESC";

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<int>(query).ToList();
            }
        }

    }
}
