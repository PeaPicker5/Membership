using System;
using System.Collections.Generic;
using Membership.Core.Dues.DataModels;
using Membership.Core.Officers.DataModels;

namespace Membership.Core.Member.Repositories
{
    public interface IMemberRepository
    {
        /// <summary>
        /// Gets the specified event.
        /// </summary>
        /// <param name="memberId">The event identifier.</param>
        /// <returns></returns>
        Member.DataModels.Member Get(Guid memberId);

        /// <summary>
        /// Gets all access events.
        /// </summary>
        /// <returns></returns>
        ICollection<Member.DataModels.Member> GetMembers();

        /// <summary>
        /// Get a list of members that hold an office for the given year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="officeType"></param>
        /// <returns></returns>
        IEnumerable<Member.DataModels.Member> GetMembersWithOffice(int year, int officeType);

        IEnumerable<Member.DataModels.Member> GetCurrentMembers();
        void InsertMemberRecord(Member.DataModels.Member memberRec);
        void UpdateMemberRecord(Member.DataModels.Member memberRec);
        bool DeleteMemberRecord(Member.DataModels.Member memberRec, 
            IEnumerable<DuesHistory> duesRecs,
            IEnumerable<Officer> officerRecs);

    }
}
