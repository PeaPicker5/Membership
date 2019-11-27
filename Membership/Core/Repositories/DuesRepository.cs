﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public class DuesRepository : IDuesRepository
    {
        private const string DbConnectionName = "MembershipDB";
        private const string SelectQuery = @"SELECT dues.*, members.* FROM DUES_History dues " +
                                            "INNER JOIN MEMBER_List members " +
                                            "ON dues.MemberID = members.MemberID ";

        private static DuesRecord GetOne(object param1, object param2, string whereClause)
        {
            var query = SelectQuery + whereClause;
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var param = new
                {
                    Param1 = param1,
                    Param2 = param2
                };
                return connection.Query<DuesRecord, Member, DuesRecord>(query,
                    (duesRec, person) =>
                    {
                        duesRec.MemberRec = person;
                        return duesRec;
                    }, param, splitOn: "MemberId").First();
            }
        }
        private static IEnumerable<DuesRecord> GetMany(object param1, string whereClause)
        {
            var query = SelectQuery + whereClause;
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var param = new
                {
                    Param1 = param1
                };
                return connection.Query<DuesRecord, Member, DuesRecord>(query,
                    (duesRec, person) =>
                    {
                        duesRec.MemberRec = person;
                        return duesRec;
                    }, param, splitOn: "MemberId");
            }
        }
        
        public DuesRecord Get(Guid memberId, int year)
        {
            return GetOne(memberId, year, "WHERE dues.MemberId = @Param1 AND dues.Year = @Param2");
        }
        public ICollection<DuesRecord> GetDuesByMember(Guid memberId)
        {
            return GetMany(memberId, "WHERE dues.MemberId = @Param1").ToList();
        }
        public IEnumerable<DuesRecord> GetDuesByYear(int year)
        {
            return GetMany(year, "WHERE dues.Year = @Param1");
        }
        public IEnumerable<int> GetYearsOnFile()
        {
            var yearList = new List<int>();
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var yearRecs = connection.Query<DuesRecord>
                    ($"SELECT DISTINCT Year FROM DUES_History").ToList();

                yearList.AddRange(yearRecs.OrderByDescending(y => y.Year).Select(y => y.Year));
                return yearList;
            }
        }
        public IEnumerable<DuesRecord> CurrentlyOweDues()
        {
            const string query = "SELECT * FROM VIEW_CurrentDues ORDER BY Lastname, Firstname";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var x = connection.Query<DuesRecord, Member, DuesRecord>(query,
                    (duesRec, person) =>
                    {
                        duesRec.MemberRec = person;
                        return duesRec;
                    },  splitOn: "MemberId");
                return x;
            }
        }

        public void UpdateDuesPayments(ICollection<DuesHistory> duesPayments)
        {

            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {

                var isSuccess = connection.Insert(duesPayments);
            }

        }
    }
}
