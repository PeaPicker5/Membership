using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private const string DbConnectionName = "MembershipDB";
        public MemberRepository() {}

        public Member Get(Guid memberId)
        {
            const string query = "SELECT * FROM Members WHERE MemberId = @MemberId";
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

    }
}
