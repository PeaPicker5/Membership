using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Membership.Core.DataModels;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Members.DataModels;

namespace Membership.Core.Meetings.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private const string DbConnectionName = "MembershipDB";
        private const string MeetingQuery1 = @"SELECT ml.MeetingID, ml.MeetingDate, ml.Description, ml.Comments, ml.InCharge, ml.IsRegularScheduled, COUNT(mm.MemberId) as 'MemberCount' FROM MEETING_List ml LEFT OUTER JOIN MEETING_Members mm ON ml.Meetingid = mm.MeetingID  "; 
        private const string MeetingQuery2 = @"GROUP BY ml.MeetingID,ml.MeetingDate, ml.Description, ml.Comments, ml.InCharge, ml.IsRegularScheduled ";
        public MeetingRepository() { }
        public Meeting Get(Guid meetingId)
        {
            const string query = MeetingQuery1 + "WHERE MeetingId = @MeetingId " + MeetingQuery2;
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<Meeting>(query, new {MeetingId = meetingId}).FirstOrDefault();
            }
        }
        public IEnumerable<Meeting> GetMeetings()
        {
            const string query = MeetingQuery1 + MeetingQuery2;
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<Meeting>(query).ToList();
            }
        }


        public ICollection<SelectableItem> GetMeetingAttendees(Guid meetingId)
        {
            const string query = @"SELECT mm.MeetingID, mm.MemberId AS 'ItemId', 1 as 'IsSelected', 
                                      CASE
                                          WHEN ml.suffix IS NULL OR ml.Suffix = '' THEN ml.LastName + ', ' + ml.FirstName
                                          ELSE ml.LastName + ' ' + ml.suffix + ', ' + ml.FirstName
                                      END AS 'Name'
                                      FROM MEMBER_List ml 
                                      INNER JOIN MEETING_Members mm
                                      ON mm.MemberId = ml.MemberID     
                                      WHERE MeetingId = @MeetingId 
                                      ORDER BY Name";

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retValue = connection.Query<SelectableItem>(query, new { MeetingId = meetingId }).ToList();
                foreach (var itm in retValue)
                {
                    itm.IsSelected = true;
                    itm.CheckStatus = SelectableMember.enumCheckStatus.Original;
                }
                return retValue;
            }
        }
        public ICollection<SelectableItem> GetMeetingsAttended(Guid memberId)
        {
            const string query = @"SELECT ml.* 
                FROM MEETING_List ml
                INNER JOIN MEETING_Members mm 
                ON mm.MeetingID = ml.MeetingID
                WHERE mm.MemberId = @MemberId 
                    AND ml.MeetingDate > DATEADD(yy, -2, CONVERT(datetime, CONVERT(varchar, GETDATE(), 101)))
                ORDER BY MeetingDate";

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retValue = connection.Query<SelectableItem>(query, new { MemberId = memberId }).ToList();
                foreach (var itm in retValue)
                {
                    itm.IsSelected = true;
                    itm.CheckStatus = SelectableMember.enumCheckStatus.Original;
                }
                return retValue;
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

        private static IEnumerable<SelectableItem> ResetAttendeeCheckStatus(IEnumerable<SelectableItem> attendees)
        {
            var attendeeList = attendees.ToList();
            foreach (var mem in attendeeList.Where(x => x.CheckStatus == SelectableMember.enumCheckStatus.Added))
                mem.CheckStatus = SelectableMember.enumCheckStatus.Original;
            return attendeeList.Where(x => x.CheckStatus == SelectableMember.enumCheckStatus.Original).ToList();
        }

        private const string InsertMeetingSql =
            "INSERT INTO MEETING_List (MeetingID, MeetingDate, Description, Comments, InCharge, IsRegularScheduled) " +
            "VALUES (@MeetingId, @MeetingDate, @Description, @Comments, @InCharge, @IsRegularScheduled)";
        private const string UpdateMeetingSql =
            "UPDATE MEETING_List SET MeetingDate = @MeetingDate, Description = @Description, " +
            "Comments = @Comments, InCharge = @InCharge, IsRegularScheduled = @IsRegularScheduled " +
            "WHERE MeetingID = @MeetingId";
        private const string InsertAttendeeSql =
            "INSERT INTO MEETING_Members (MeetingID, MemberID) VALUES (@MeetingId, @MemberId)";
        private const string DeleteAttendeeSql =
            "DELETE FROM MEETING_Members WHERE MeetingID = @MeetingId AND MemberID = @MemberId";

        public void InsertMeeting(Meeting meetingRec, IEnumerable<SelectableItem>Attendees)
        {
            var attendeeRecs = Attendees
                .Select(member => new MeetingMember(meetingRec.MeetingId, member.ItemId))
                .ToList();

            using (var connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(InsertMeetingSql, meetingRec, transaction);
                    connection.Execute(InsertAttendeeSql, attendeeRecs, transaction);
                    transaction.Commit();
                }
            }
        }
        public void UpdateMeeting(Meeting meetingRec, IEnumerable<SelectableItem>Attendees)
        {
            var attendeeList = Attendees.ToList();
            var added = attendeeList
                .Where(x => x.CheckStatus == SelectableMember.enumCheckStatus.Added)
                .Select(x => new MeetingMember(meetingRec.MeetingId, x.ItemId)).ToList();
            var removed = attendeeList
                .Where(x => x.CheckStatus == SelectableMember.enumCheckStatus.Removed)
                .Select(x => new MeetingMember(meetingRec.MeetingId, x.ItemId)).ToList();

            using (var connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(UpdateMeetingSql, meetingRec, transaction);
                    connection.Execute(InsertAttendeeSql, added, transaction);
                    connection.Execute(DeleteAttendeeSql, removed, transaction);
                    transaction.Commit();
                }
            }
            ResetAttendeeCheckStatus(attendeeList);
        }
        public void DeleteMeeting(Meeting meetingRec)
        {
            // Remove all attendance rows for the meeting, then the meeting itself, all-or-nothing.
            const string query = "DELETE FROM MEETING_Members WHERE MeetingId = @MeetingId; " +
                                 "DELETE FROM MEETING_List WHERE MeetingId = @MeetingId;";
            using (var connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(query, new { meetingRec.MeetingId }, transaction);
                    transaction.Commit();
                }
            }
        }

    }
}
