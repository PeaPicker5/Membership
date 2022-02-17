using System;
using System.Collections.Generic;
using Membership.Core.Members.DataModels;

namespace Membership.Core.Members.Presenters
{
    public interface IMemberView
    {
        ICollection<Tuple<int, string>> MemberTypeLookups { get; set; }
        ICollection<Tuple<Guid, string>> SponsorLookups { get; set; }
        ICollection<MemberRemoval> RemovalCodes { get; set; }
    }
}
