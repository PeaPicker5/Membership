using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
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

        public void InsertMeeting(Meeting meetingRec, IEnumerable<SelectableItem>Attendees)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Insert(meetingRec);
                foreach (var member in Attendees)
                    connection.Insert(new MeetingMember(meetingRec.MeetingId, member.ItemId));
            }
        }
        public void UpdateMeeting(Meeting meetingRec, IEnumerable<SelectableItem>Attendees)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Update(meetingRec);
                var attendeeList = Attendees.ToList();
                foreach (var member in attendeeList.Where(x => x.CheckStatus != SelectableMember.enumCheckStatus.Original))
                {
                    var meetingMemberRec = new MeetingMember(meetingRec.MeetingId, member.ItemId);
                    if (member.CheckStatus == SelectableMember.enumCheckStatus.Added)
                        connection.Insert(meetingMemberRec);
                    else if (member.CheckStatus == SelectableMember.enumCheckStatus.Removed)
                        connection.Delete(meetingMemberRec);
                }
                Attendees = ResetAttendeeCheckStatus(attendeeList);
            }
        }
        public void DeleteMeeting(Meeting meetingRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Delete(new MeetingMember(meetingRec.MeetingId, Guid.Empty));
                connection.Delete(meetingRec);
            }
        }

    }
}
