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

        public IEnumerable<OfficeHeld> GetOfficesByMember(Guid memberId)
        {
            const string query = @"SELECT oa.*, ol.*
                                      FROM OFFICE_Assignments oa
                                      INNER JOIN OFFICE_List ol 
                                      ON oa.OfficeID = ol.OfficeID 
                                      WHERE oa.MemberId = @MemberId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var param = new {MemberId = memberId};
                var officers = connection.Query<OfficeHeld, Office, OfficeHeld>(query,
                    (term, office) =>{ term.OfficeRec = office; return term;},
                    param, splitOn: "OfficeId");

                return officers;
            }
        }

        public IEnumerable<OfficeHeld> GetOfficesByYear(int year)
        {
            const string query = "SELECT * FROM OfficeAssignments WHERE Year = @year";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<OfficeHeld>(query,new { Year = year }).ToList();
                return retVal;
            }
        }
    }
}
