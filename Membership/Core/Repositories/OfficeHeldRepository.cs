using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public class OfficeHeldRepository : IOfficeHeldRepository
    {
        private const string DbConnectionName = "MembershipDB";

        public IEnumerable<Officer> GetOfficesByMember(Guid memberId)
        {
            const string query = @"SELECT oa.*, ol.*
                                      FROM OFFICE_Assignments oa
                                      INNER JOIN OFFICE_List ol 
                                      ON oa.OfficeID = ol.OfficeID 
                                      WHERE oa.MemberId = @MemberId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var param = new {MemberId = memberId};
                var officers = connection.Query<Officer, Office, Officer>(query,
                    (term, office) =>{ term.OfficeRec = office; return term;},
                    param, splitOn: "OfficeId");

                return officers;
            }
        }

        public IEnumerable<Officer> GetOfficesByYear(int year)
        {
            const string query = "SELECT * FROM OfficeAssignments WHERE Year = @year";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<Officer>(query,new { Year = year }).ToList();
                return retVal;
            }
        }
        public IEnumerable<int> GetYearsOnFile()
        {
            var yearList = new List<int>();
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var yearRecs = connection.Query<DuesRecord>
                    ($"SELECT DISTINCT Year FROM OFFICE_Assignments").ToList();

                yearList.AddRange(yearRecs.OrderByDescending(y => y.Year).Select(y => y.Year));
                return yearList;
            }
        }

        public IEnumerable<Office> GetOfficesOnFile()
        {
            const string query = "SELECT DISTINCT ol.* FROM OFFICE_Assignments oa " +
                                 "INNER JOIN OFFICE_List ol ON oa.OfficeID = ol.OfficeID " +
                                 "Order by ol.OfficeId "  ;
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<Office>(query).ToList();
                return retVal;
            }

        }
    }
}
