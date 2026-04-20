using System;
using System.Collections.Generic;
using Membership.Core.LOSAP.DataModels;

namespace Membership.Core.LOSAP.Repositories
{
    public interface ILosapRepository
    {
        ///// <summary>
        ///// Gets the Losap record for a given member for a given year.
        ///// </summary>
        ///// <param name="memberId">The event identifier.</param>
        ///// <param name="year"></param>
        ///// <returns></returns>
        LosapRecord Get(Guid memberId, int year);

        ///// <summary>
        ///// Gets all Losap records for a given member.
        ///// </summary>
        ///// <param name="memberId">The event identifier.</param>
        ///// <returns></returns>
        ICollection<LosapRecord> GetLosapRecordsByMemberId(Guid memberId);

        ///// <summary>
        ///// Gets all Losap records for a given year.
        ///// </summary>
        ///// <param name="year"></param>
        ///// <returns></returns>
        IEnumerable<LosapRecord> GetLosapByYear(int year);

        ICollection<LosapRecord> GetLosapForActiveMembers();

        IEnumerable<int> GetYearsOnFile();

        IEnumerable<LosapRecord> CurrentActiveMembers();

        void InsertLosapCredits(IEnumerable<LosapRecord> losapPayments);
        void DeleteLosapCredits(IEnumerable<LosapRecord> losapPayments);

        //List<DuesHistory> MapFromData(IEnumerable<DuesRecord> duesRecords);

    }
}
