using System;
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

        // MEMBER_Type rarely changes, so it is read once per run.
        private static readonly Lazy<IDictionary<int, MemberType>> TypesById =
            new Lazy<IDictionary<int, MemberType>>(LoadTypes);

        private static IDictionary<int, MemberType> LoadTypes()
        {
            const string query = "SELECT TypeId, Description, DuesAmount FROM MEMBER_Type";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<MemberType>(query).ToDictionary(t => t.TypeId);
            }
        }

        /// <summary>
        /// Description of a member type as stored in MEMBER_Type, or "Unknown" if it isn't on file.
        /// </summary>
        public static string GetDescription(int typeId)
        {
            return TypesById.Value.TryGetValue(typeId, out var type) ? type.Description : "Unknown";
        }

        /// <summary>Annual dues for a member type, or 0 if the type isn't on file.</summary>
        public static float GetDuesAmount(int typeId)
        {
            return TypesById.Value.TryGetValue(typeId, out var type) ? type.DuesAmount : 0;
        }

        public MemberType Get(int typeId)
        {
            const string query = "SELECT * FROM MEMBER_Type WHERE TypeId = @TypeId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                // Returns null when the type is not on file.
                return connection.QueryFirstOrDefault<MemberType>(query, new { TypeId = typeId });
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
                return retVal.OrderBy(r => r.ReasonId);
            }
        }
    }
}
