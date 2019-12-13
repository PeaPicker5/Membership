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
        IEnumerable<Member> GetMembers();

        bool InsertMemberRecord(Member memberRec);
        bool UpdateMemberRecord(Member memberRec);
        bool DeleteMemberRecord(Member memberRec, IEnumerable<DuesRecord> duesRecs,
            IEnumerable<Officer> officerRecs);
    }
}
