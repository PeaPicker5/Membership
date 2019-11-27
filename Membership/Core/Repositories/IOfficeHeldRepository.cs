using System;
using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public interface IOfficeHeldRepository
    {
        /// <summary>
        /// Gets the Office records assigned to a given member.
        /// </summary>
        /// <param name="memberId">The event identifier.</param>
        /// <returns></returns>
        IEnumerable<OfficeHeld> GetOfficesByMember(Guid memberId);

        /// <summary>
        /// Gets all Officers for a given year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        IEnumerable<OfficeHeld> GetOfficesByYear(int year);

    }
}
