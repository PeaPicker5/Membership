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

        public IEnumerable<Member> GetMembers()
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


        public bool InsertMemberRecord(Member memberRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var isSuccess = connection.Insert(memberRec);
                return isSuccess > 0;
            }
        }
        public bool UpdateMemberRecord(Member memberRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var isSuccess = connection.Update(memberRec);
                return isSuccess;
            }
        }

        public bool DeleteMemberRecord(Member memberRec, IEnumerable<DuesRecord> duesRecs, IEnumerable<Officer> officerRecs)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMemberRecord(Member memberRec, IEnumerable<DuesHistory> duesHistoryRecs, 
                                        IEnumerable<Officer> officerRecs)
        {

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                bool isSuccess = true;
                //if (officerRecs.Any())
                //    isSuccess = connection.Delete(officerRecs);
                //if (isSuccess && duesHistoryRecs.Any())
                //    isSuccess = connection.Delete(duesRecs);
                //if (isSuccess)
                //    isSuccess = connection.Delete(memberRec);
                return isSuccess;
            }

        }


    }
}
