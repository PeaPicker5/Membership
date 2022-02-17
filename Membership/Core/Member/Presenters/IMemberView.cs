using System;
using System.Collections.Generic;
using Membership.Core.Member.DataModels;

namespace Membership.Core.Member.Presenters
{
    public interface IMemberView
    {
        ICollection<Tuple<int, string>> MemberTypeLookups { get; set; }
        ICollection<Tuple<Guid, string>> SponsorLookups { get; set; }
        ICollection<MemberRemoval> RemovalCodes { get; set; }
    }
}
