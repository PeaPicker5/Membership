using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
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
                var retVal = connection.Query<Member>(query,new {MemberId = memberId}).ToList();
                return retVal[0];
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
            const int MeetingsNeeded = 5;
            var query = "SELECT * FROM MEMBER_List ml " +
                        "INNER JOIN VIEW_MembersAbleToVote v " +
                        "ON ml.MemberId = v.MemberId " +
                        $"WHERE v.RecCount >= {MeetingsNeeded} " +
                        "ORDER BY ml.Lastname, ml.FirstName";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<Member>(query).ToList();
                return retVal;
            }
        }


        private IEnumerable<Member> GetSelectedMemberRecords(string clause, DynamicParameters param)
        {
            const string query = "SELECT ml.* " +
                                 "FROM MEMBER_List ml " +
                                 "INNER JOIN MEMBER_Type mt ON ml.MemberTypeId = mt.TypeId " +
                                 "LEFT OUTER JOIN OFFICE_Assignments oa ON oa.MemberId = oa.MemberId ";

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<Member>(query + clause, param).Distinct().ToList();
                return retVal;
            }

        }

        public IEnumerable<Member> GetMembersWithOffice(int year, int officeType)
        {
            var whereClause = officeType == DistrictOnly
                ? "WHERE oa.Year = @year AND oa.OfficeId < 20 "
                : "WHERE oa.Year = @year AND oa.OfficeId >= 20 ";
            var parameter = new DynamicParameters(); parameter.Add("Year", year);
            return GetSelectedMemberRecords(whereClause, parameter);
        }
        public IEnumerable<Member> GetCurrentMembers()
        {
            var members = GetMembers();
            return members.Where(x => x.IsCurrent);
        }

        public void InsertMemberRecord(Member memberRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Insert(memberRec);
            }
        }
        public void UpdateMemberRecord(Member memberRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Update(memberRec);
            }
        }

        public bool DeleteMemberRecord(Member memberRec, 
                                       IEnumerable<DuesHistory> duesHistoryRecs, 
                                       IEnumerable<Officer> officerRecs)
        {

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                foreach (var officeRec in officerRecs)
                    connection.Delete(officeRec); 
                foreach (var duesRec in duesHistoryRecs)
                    connection.Delete(duesRec);
                if (memberRec.PageId != Guid.Empty)
                    connection.Delete(new ImageRec(memberRec.PageId, string.Empty, null));
                connection.Delete(memberRec);
                return true;
            }
        }


    }
}
