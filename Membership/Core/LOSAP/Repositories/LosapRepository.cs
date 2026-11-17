using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Membership.Core.LOSAP.Repositories
{
    public class LosapRepository : ILosapRepository
    {
        private const string DbConnectionName = "MembershipDB";

        public IDictionary<Guid, int> GetCreditedYearCounts()
        {
            const string query = "SELECT MemberID, COUNT(*) AS Years " +
                                 "FROM LOSAP_History WHERE LosapCredit = 1 GROUP BY MemberID";
            using (var connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<CreditCount>(query)
                    .ToDictionary(r => r.MemberId, r => r.Years);
            }
        }

        private class CreditCount
        {
            public Guid MemberId { get; set; }
            public int Years { get; set; }
        }

        public ISet<Guid> GetCreditedMemberIds(int year)
        {
            const string query = "SELECT MemberID FROM LOSAP_History WHERE Year = @Year AND LosapCredit = 1";
            using (var connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return new HashSet<Guid>(connection.Query<Guid>(query, new { Year = year }));
            }
        }

        public IEnumerable<int> GetYearsOnFile()
        {
            const string query = "SELECT DISTINCT Year FROM LOSAP_History ORDER BY Year DESC";
            using (var connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<int>(query).ToList();
            }
        }

        public void SaveCredits(int year, IEnumerable<Guid> membersToCredit, IEnumerable<Guid> membersToUncredit)
        {
            // Mark the row credited, adding it if the member has no row for that year yet.
            const string creditSql =
                "UPDATE LOSAP_History SET LosapCredit = 1 WHERE MemberID = @MemberId AND Year = @Year; " +
                "IF @@ROWCOUNT = 0 " +
                "INSERT INTO LOSAP_History (MemberID, Year, LosapCredit) VALUES (@MemberId, @Year, 1);";
            // Keep the row (the year was reviewed) but stop it counting toward LOSAP.
            const string uncreditSql =
                "UPDATE LOSAP_History SET LosapCredit = 0 WHERE MemberID = @MemberId AND Year = @Year;";

            var credit = membersToCredit.Select(id => new { MemberId = id, Year = year }).ToList();
            var uncredit = membersToUncredit.Select(id => new { MemberId = id, Year = year }).ToList();

            using (var connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(creditSql, credit, transaction);
                    connection.Execute(uncreditSql, uncredit, transaction);
                    transaction.Commit();
                }
            }
        }
    }
}
