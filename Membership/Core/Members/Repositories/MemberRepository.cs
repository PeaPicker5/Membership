using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Membership.Core.DataModels;
using Membership.Core.Dues.DataModels;
using Membership.Core.Members.DataModels;
using Membership.Core.Officers.DataModels;

namespace Membership.Core.Members.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private const int DistrictOnly = 0;
        private const int AssocOnly = 1;

        private const string DbConnectionName = "MembershipDB";
        public MemberRepository() {}

        public Member Get(Guid memberId)
        {
            const string query = "SELECT * FROM MEMBER_List WHERE MemberId = @MemberId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                // Returns null when the member is not on file.
                return connection.QueryFirstOrDefault<Member>(query, new { MemberId = memberId });
            }
        }

        public ICollection<Member> GetMembers()
        {
            const string query = "SELECT ml.*, mt.Description as [Status], mt.DuesAmount " +
                                 "FROM MEMBER_List ml " +
                                 "INNER JOIN MEMBER_Type mt ON ml.MemberTypeId = mt.TypeId " +
                                 "ORDER BY Lastname";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<Member>(query).ToList();
                return retVal;
            }
        }

        public IEnumerable<Member> GetMembersThatCanVote()
        {
            const string query = "SELECT * FROM MEMBER_List ml " +
                                 "INNER JOIN VIEW_MembersAbleToVote v " +
                                 "ON ml.MemberId = v.MemberId " +
                                 "WHERE v.RecCount >= @MeetingsNeeded " +
                                 "ORDER BY ml.Lastname, ml.FirstName";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<Member>(query,
                    new { MeetingsNeeded = Helper.MeetingsNeededToVote }).ToList();
                return retVal;
            }
        }


        public IEnumerable<Member> GetMembersWithOffice(int year, int officeType)
        {
            // EXISTS (rather than a join) returns each member once, even if they hold several offices.
            var officeFilter = officeType == DistrictOnly ? "oa.OfficeId < 20" : "oa.OfficeId >= 20";
            var query = "SELECT ml.* FROM MEMBER_List ml " +
                        "WHERE EXISTS (SELECT 1 FROM OFFICE_Assignments oa " +
                        "WHERE oa.MemberId = ml.MemberId AND oa.Year = @Year AND " + officeFilter + ")";

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<Member>(query, new { Year = year }).ToList();
            }
        }
        public IList<Tuple<Guid, string>> GetSponsorLookups()
        {
            // Only the two columns the dropdown needs, instead of every column of every member.
            const string query = "SELECT MemberId, FirstName, LastName " +
                                 "FROM MEMBER_List ORDER BY LastName, FirstName";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<Member>(query)
                    .Select(m => new Tuple<Guid, string>(m.MemberId, m.FullName))
                    .ToList();
            }
        }

        public IEnumerable<Member> GetCurrentMembers()
        {
            var members = GetMembers();
            return members.Where(x => x.IsCurrent);
        }

        public void InsertMemberRecord(Member memberRec)
        {
            const string query =
                "INSERT INTO MEMBER_List (MemberID, MemberTypeID, IsActive, FireFighterNum, Lastname, Firstname, " +
                "MI, Suffix, Comment, EMailAddress, SponsorID1, SponsorID2, SponsorID3, Address1, Address2, " +
                "City, State, ZIP, Phone, DateObligated, DateRemoved, RemovalReasonId, DateOfBirth, DateDeceased, " +
                "PageId, PageThumb, LegacyLosapYears) " +
                "VALUES (@MemberId, @MemberTypeId, @IsActive, @FireFighterNum, @LastName, @FirstName, " +
                "@MI, @Suffix, @Comment, @EmailAddress, @SponsorId1, @SponsorId2, @SponsorId3, @Address1, @Address2, " +
                "@City, @State, @ZIP, @Phone, @DateObligated, @DateRemoved, @RemovalReasonId, @DateOfBirth, @DateDeceased, " +
                "@PageId, @PageThumb, @LegacyLosapYears)";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Execute(query, memberRec);
            }
        }
        public void UpdateMemberRecord(Member memberRec)
        {
            const string query =
                "UPDATE MEMBER_List SET MemberTypeID = @MemberTypeId, IsActive = @IsActive, " +
                "FireFighterNum = @FireFighterNum, Lastname = @LastName, Firstname = @FirstName, MI = @MI, " +
                "Suffix = @Suffix, Comment = @Comment, EMailAddress = @EmailAddress, SponsorID1 = @SponsorId1, " +
                "SponsorID2 = @SponsorId2, SponsorID3 = @SponsorId3, Address1 = @Address1, Address2 = @Address2, " +
                "City = @City, State = @State, ZIP = @ZIP, Phone = @Phone, DateObligated = @DateObligated, " +
                "DateRemoved = @DateRemoved, RemovalReasonId = @RemovalReasonId, DateOfBirth = @DateOfBirth, " +
                "DateDeceased = @DateDeceased, PageId = @PageId, PageThumb = @PageThumb, " +
                "LegacyLosapYears = @LegacyLosapYears " +
                "WHERE MemberID = @MemberId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Execute(query, memberRec);
            }
        }

        /// <summary>
        /// Deletes a member and every record that references them, all-or-nothing.
        /// </summary>
        public bool DeleteMemberRecord(Member memberRec)
        {
            const string query =
                "DELETE FROM OFFICE_Assignments WHERE MemberId = @MemberId; " +
                "DELETE FROM DUES_History WHERE MemberId = @MemberId; " +
                "DELETE FROM MEETING_Members WHERE MemberId = @MemberId; " +
                "DELETE FROM LOSAP_History WHERE MemberId = @MemberId; " +
                "DELETE FROM SYS_FileStore WHERE ImageId = @PageId AND @PageId <> @EmptyId; " +
                "DELETE FROM MEMBER_List WHERE MemberId = @MemberId;";

            using (var connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(query, new
                    {
                        memberRec.MemberId,
                        memberRec.PageId,
                        EmptyId = Guid.Empty
                    }, transaction);
                    transaction.Commit();
                    return true;
                }
            }
        }


    }
}
