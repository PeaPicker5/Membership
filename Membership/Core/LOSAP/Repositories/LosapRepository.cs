using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Membership.Core.Dues.DataModels;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.Members.DataModels;

namespace Membership.Core.LOSAP.Repositories
{
    public class LosapRepository : ILosapRepository
    {
        private const string DbConnectionName = "MembershipDB";
        private const string SelectQuery = @"SELECT losap.*, members.* 
            FROM LOSAP_History losap 
            INNER JOIN MEMBER_List members 
            ON losap.MemberID = members.MemberID ";
        private static LosapRecord GetOne(object param1, object param2, string whereClause)
        {
            var query = SelectQuery + whereClause;
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var param = new
                {
                    Param1 = param1,
                    Param2 = param2
                };
                return connection.Query<LosapRecord, Member, LosapRecord>(query,
                    (losapRec, person) =>
                    {
                        losapRec.MemberRec = person;
                        return losapRec;
                    }, param: param, splitOn: "MemberId").First();
            }
        }
        private static IEnumerable<LosapRecord> GetMany(object param1, string whereClause)
        {
            var query = SelectQuery + whereClause;
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var param = new
                {
                    Param1 = param1
                };
                return connection.Query<LosapRecord, Member, LosapRecord>(query,
                    (losapRec, person) =>
                    {
                        losapRec.MemberRec = person;
                        return losapRec;
                    }, param: param, splitOn: "MemberId");
            }
        }

        public ICollection<LosapRecord> GetLosapForActiveMembers()
        {
            const string whereClause = "WHERE members.MemberTypeId = @Param1 " +
                                       "AND members.DateDeceased IS NULL " +
                                       "AND members.DateRemoved IS NULL";
            return GetMany(1, whereClause).ToList();
        }


        public LosapRecord Get(Guid memberId, int year)
        {
            return GetOne(memberId, year, "WHERE losap.MemberId = @Param1 AND losap.Year = @Param2");
        }
        public ICollection<LosapRecord> GetLosapRecordsByMemberId(Guid memberId)
        {
            return GetMany(memberId, "WHERE losap.MemberId = @Param1").ToList();
        }
        public IEnumerable<LosapRecord> GetLosapByYear(int year)
        {
            return GetMany(year, "WHERE losap.Year = @Param1");
        }

        public IEnumerable<int> GetYearsOnFile()
        {
            var yearList = new List<int>();
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var yearRecs = connection.Query<LosapRecord>
                    ($"SELECT DISTINCT Year FROM LOSAP_History").ToList();

                yearList.AddRange(yearRecs.OrderByDescending(y => y.Year).Select(y => y.Year));
                return yearList;
            }
        }
        public IEnumerable<LosapRecord> CurrentActiveMembers()
        {
            const string query = "SELECT * FROM LOSAP_History";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var x = connection.Query<LosapRecord, Member, LosapRecord>(query,
                    (losapRec, person) =>
                    {
                        losapRec.MemberRec = person;
                        return losapRec;
                    },  splitOn: "MemberId");
                return x;
            }
        }

        public void InsertLosapCredits(IEnumerable<LosapRecord> losapCredits)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var isSuccess = connection.Insert(losapCredits);
            }
        }
        public void DeleteLosapCredits(IEnumerable<LosapRecord> losapCredits)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var isSuccess = connection.Delete(losapCredits);
            }
        }

        //public List<LosapRecord> MapFromData(IEnumerable<LosapRecord> losapRecords)
        //{
        //    return losapRecords.Select(rec => new Lo()
        //    {
        //        MemberId = rec.MemberRec.MemberId,
        //        Year = rec.Year,
        //        Amount = rec.Amount,
        //        Month = rec.Month
        //    }).ToList();
        //}

    }
}
