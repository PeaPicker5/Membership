using System;
using System.Collections.Generic;
using Membership.Core.Dues.DataModels;
using Membership.Core.Officers.DataModels;

namespace Membership.Core.Members.Repositories
{
    public interface IMemberRepository
    {
        /// <summary>
        /// Gets the specified event.
        /// </summary>
        /// <param name="memberId">The event identifier.</param>
        /// <returns></returns>
        Members.DataModels.Member Get(Guid memberId);

        /// <summary>
        /// Gets all access events.
        /// </summary>
        /// <returns></returns>
        ICollection<Members.DataModels.Member> GetMembers();

        /// <summary>
        /// Get a list of members that hold an office for the given year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="officeType"></param>
        /// <returns></returns>
        IEnumerable<Members.DataModels.Member> GetMembersWithOffice(int year, int officeType);

        IEnumerable<Members.DataModels.Member> GetCurrentMembers();
        IEnumerable<Members.DataModels.Member> GetMembersThatCanVote();
        void InsertMemberRecord(Members.DataModels.Member memberRec);
        void UpdateMemberRecord(Members.DataModels.Member memberRec);
        bool DeleteMemberRecord(Members.DataModels.Member memberRec, 
            IEnumerable<DuesHistory> duesRecs,
            IEnumerable<Officer> officerRecs);

    }
}
