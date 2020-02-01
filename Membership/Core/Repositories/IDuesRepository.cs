using System;
using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public interface IDuesRepository
    {
        /// <summary>
        /// Gets the dues record for a given memner for a given year.
        /// </summary>
        /// <param name="memberId">The event identifier.</param>
        /// <param name="year"></param>
        /// <returns></returns>
        DuesRecord Get(Guid memberId, int year);

        /// <summary>
        /// Gets all dues records for a given member.
        /// </summary>
        /// <param name="memberId">The event identifier.</param>
        /// <returns></returns>
        ICollection<DuesRecord> GetDuesRecordByMemberId(Guid memberId);

        /// <summary>
        /// Gets all dues records for a given year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        IEnumerable<DuesRecord> GetDuesByYear(int year);


        IEnumerable<int> GetYearsOnFile();

        IEnumerable<DuesRecord> CurrentlyOweDues();

        void InsertDuesPayments(IEnumerable<DuesRecord> duesPayments);
        void DeleteDuesPayments(IEnumerable<DuesRecord> duesPayments);

        List<DuesHistory> MapFromData(IEnumerable<DuesRecord> duesRecords);

    }
}
