using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Membership.Core.Members.DataModels;

namespace Membership.Core.Members.Repositories
{
    public class MemberTypeRepository : IMemberTypeRepository
    {
        private const string DbConnectionName = "MembershipDB";
        public MemberTypeRepository() { }

        public MemberType Get(int typeId)
        {
            const string query = "SELECT * FROM MEMBER_Type WHERE MemberTypeId = @MemberTypeId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<MemberType>(query, new {MemberTypeId = typeId}).ToList();
                return retVal[0];
            }
        }

        public IEnumerable<MemberType> GetAllTypes()
        {
            const string query = "SELECT * FROM MEMBER_Type";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<MemberType>(query).ToList();
                retVal.Add(new MemberType(0, "All Member Types", 0));
                return retVal.OrderBy(r => r.TypeId);
            }
        }

        public IEnumerable<MemberRemoval> GetRemovalCodes()
        {
            const string query = "SELECT * FROM MEMBER_RemovalCodes";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<MemberRemoval>(query).ToList();
                return retVal.OrderBy(r => r.ReasonID);
            }
        }
    }
}
