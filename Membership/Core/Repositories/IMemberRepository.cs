using System;
using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public interface IMemberRepository
    {
        /// <summary>
        /// Gets the specified event.
        /// </summary>
        /// <param name="memberId">The event identifier.</param>
        /// <returns></returns>
        Member Get(Guid memberId);

        /// <summary>
        /// Gets all access events.
        /// </summary>
        /// <returns></returns>
        ICollection<Member> GetMembers();

        /// <summary>
        /// Get a list of members that hold an office for the given year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="officeType"></param>
        /// <returns></returns>
        IEnumerable<Member> GetMembersWithOffice(int year, int officeType);

        IEnumerable<Member> GetCurrentMembers();
        void InsertMemberRecord(Member memberRec);
        void UpdateMemberRecord(Member memberRec);
        bool DeleteMemberRecord(Member memberRec, 
            IEnumerable<DuesHistory> duesRecs,
            IEnumerable<Officer> officerRecs);

    }
}
