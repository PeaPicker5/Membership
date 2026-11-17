using System;
using System.Collections.Generic;

namespace Membership.Core.LOSAP.Repositories
{
    /// <summary>
    /// LOSAP_History access. A year counts toward LOSAP only when its row has LosapCredit = 1.
    /// </summary>
    public interface ILosapRepository
    {
        /// <summary>Credited years per member (members with none are absent).</summary>
        IDictionary<Guid, int> GetCreditedYearCounts();

        /// <summary>Members credited for the given year.</summary>
        ISet<Guid> GetCreditedMemberIds(int year);

        /// <summary>Years that have any LOSAP_History rows, newest first.</summary>
        IEnumerable<int> GetYearsOnFile();

        /// <summary>
        /// Credits <paramref name="membersToCredit"/> and un-credits <paramref name="membersToUncredit"/>
        /// for <paramref name="year"/>, all-or-nothing.
        /// </summary>
        void SaveCredits(int year, IEnumerable<Guid> membersToCredit, IEnumerable<Guid> membersToUncredit);
    }
}
