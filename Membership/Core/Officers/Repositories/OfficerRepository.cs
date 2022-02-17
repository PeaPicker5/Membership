using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Membership.Core.Dues.DataModels;
using Membership.Core.Officers.DataModels;

namespace Membership.Core.Officers.Repositories
{
    public class OfficerRepository : IOfficerRepository
    {
        private const string DbConnectionName = "MembershipDB";

        public IEnumerable<int> GetOfficerYearsOnFile()
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

        public IEnumerable<Office> GetAssignableOffices()
        {
            const string query = "SELECT * FROM OFFICE_List";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<Office>(query).ToList();
                return retVal;
            }
        }

        public IEnumerable<Office> GetOfficesOnFile()
        {
            const string query = "SELECT DISTINCT ol.* FROM OFFICE_Assignments oa " +
                                 "INNER JOIN OFFICE_List ol ON oa.OfficeID = ol.OfficeID " +
                                 "Order by ol.OfficeId ";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var retVal = connection.Query<Office>(query).ToList();
                return retVal;
            }
        }
        private IEnumerable<Office> GetSelectedOfficeRecords(string clause = "", DynamicParameters param = null)
        {
            const string query = "SELECT DISTINCT ol.* FROM OFFICE_Assignments oa " +
                                 "INNER JOIN OFFICE_List ol ON oa.OfficeID = ol.OfficeID ";

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Query<Office>(query + clause, param).ToList();
            }

        }

        private IEnumerable<Officer> GetSelectedOfficerRecords(string clause, DynamicParameters param)
        {
            const string query = "SELECT  oa.*, ml.*, ol.* " +
                                 "FROM Office_Assignments oa " +
                                 "INNER JOIN MEMBER_List ml ON oa.MemberID = ml.MemberID " +
                                 "INNER JOIN OFFICE_List ol on oa.OfficeID = ol.OfficeID ";

            List<Officer> officerRecs = null;
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                officerRecs = connection.Query<Officer, Member.DataModels.Member, Office, Officer>(query + clause,
                    (officer, member, office) =>
                    {
                        officer.MemberRec = member;
                        officer.OfficeRec = office;
                        return officer;
                    },param, splitOn: "MemberId,OfficeId", commandType: CommandType.Text).ToList();
            }

            return officerRecs;
        }

        public IEnumerable<Officer> GetOfficersByYear(int year)
        {
            var whereClause = "WHERE oa.Year = @year";
            var parameter = new DynamicParameters();
            parameter.Add("Year", year);
            return GetSelectedOfficerRecords(whereClause, parameter);
        }
        public IEnumerable<Officer> GetOfficersByTitle(int officeId)
        {
            var whereClause = "WHERE oa.OfficeId = @officeId";
            var parameter = new DynamicParameters();
            parameter.Add("OfficeId", officeId);

            return GetSelectedOfficerRecords(whereClause, parameter);
        }
        public IEnumerable<Officer> GetOfficersByMember(Guid memberId)
        {
            var whereClause = "WHERE oa.MemberId = @memberId ORDER BY oa.Year";
            var parameter = new DynamicParameters();
            parameter.Add("MemberId", memberId);
            return GetSelectedOfficerRecords(whereClause, parameter);
        }

        public IEnumerable<Office> GetOfficesByYear(int year)
        {
            var whereClause = "WHERE oa.Year = @year";
            var parameter = new DynamicParameters();
            parameter.Add("Year", year);
            return GetSelectedOfficeRecords(whereClause, parameter);
        }
        private bool IsDuplicateRecord(Officer officerRec)
        {
            var whereClause = "WHERE oa.MemberId = @MemberId " +
                                     " AND oa.OfficeId = @OfficeId " +
                                     " AND oa.Year = @Year " +
                                     " AND oa.FromDate = @FromDate " +
                                     " AND oa.ToDate = @ToDate ";

            var parameter = new DynamicParameters();
            parameter.Add("MemberId", officerRec.MemberId);
            parameter.Add("OfficeId", officerRec.OfficeId);
            parameter.Add("Year", officerRec.Year);
            parameter.Add("FromDate", officerRec.FromDate);
            parameter.Add("ToDate", officerRec.ToDate);
            var recOnFile = GetSelectedOfficeRecords(whereClause, parameter);
            return recOnFile.Any();
        }

        public void InsertOfficerRecord(Officer officerRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Insert(officerRec);
            }
        }
        public void UpdateOfficerRecord(Officer officerRec)
        {
            if (IsDuplicateRecord(officerRec)) return;
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Update(officerRec);
            }
        }

        public void DeleteOfficerRecord(Officer officerRec)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                connection.Delete(officerRec);
            }
        }


    }
}
