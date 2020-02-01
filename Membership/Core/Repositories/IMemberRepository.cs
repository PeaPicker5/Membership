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

        void InsertMemberRecord(Member memberRec);
        void UpdateMemberRecord(Member memberRec);
        bool DeleteMemberRecord(Member memberRec, 
            IEnumerable<DuesHistory> duesRecs,
            IEnumerable<Officer> officerRecs);

    }
}
