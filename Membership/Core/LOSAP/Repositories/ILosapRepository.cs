using System;
using System.Collections.Generic;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.Members.DataModels;

namespace Membership.Core.LOSAP.Repositories
{
    public interface ILosapRepository
    {

        ///// <summary>
        ///// Gets all Losap records for a given year.
        ///// </summary>
        ///// <param name="year"></param>
        ///// <returns></returns>
        IEnumerable<LosapRecord> GetLosapByYear(int year);

        ICollection<LosapRecord> GetLosapForActiveMembers();

        IEnumerable<int> GetYearsOnFile();

        void InsertLosapCredits(IEnumerable<LosapRecord> losapPayments);
        void DeleteLosapCredits(IEnumerable<LosapRecord> losapPayments);

        //List<DuesHistory> MapFromData(IEnumerable<DuesRecord> duesRecords);

    }
}
