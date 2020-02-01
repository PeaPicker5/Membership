using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IMemberSelectionView
    {
        ICollection<Member> Members { get; set; }

        IEnumerable<MemberType> MemberTypes { get; set; }
    }
}
