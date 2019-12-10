using System;
using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IMemberView
    {
        ICollection<Tuple<int, string>> MemberTypeLookups { get; set; }
        ICollection<Tuple<Guid, string>> MemberLookups { get; set; }
        ICollection<MemberRemoval> RemovalCodes { get; set; }
    }
}
