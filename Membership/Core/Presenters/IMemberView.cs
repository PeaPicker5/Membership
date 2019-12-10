using System;
using System.Collections.Generic;

namespace Membership.Core.Presenters
{
    public interface IMemberView
    {
        ICollection<Tuple<int, string>> MemberTypeLookups { get; set; }
        ICollection<Tuple<Guid, string>> MemberLookups { get; set; }
    }
}
