using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Membership.Core.Repositories
{

    public class ReportDataRepository : IReportDataRepository
    {
        private const string DbConnectionName = "MembershipDB";
        public ReportDataRepository() { }

        public DataTable GetRecordSet(string query)
        {
            DataTable table = new DataTable();
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                using (var reader = connection.ExecuteReader(query))
                {
                    table.Load(reader);
                }
            }
            return table;
        }
    }
}