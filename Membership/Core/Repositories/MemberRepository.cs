using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using Dapper;
using Dapper.Contrib.Extensions;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public class MemberRepository : IMemberRepository
    {
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

            return false;
        }


    }
}
