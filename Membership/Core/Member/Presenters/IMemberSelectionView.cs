using System.Collections.Generic;
using Membership.Core.Member.DataModels;

namespace Membership.Core.Member.Presenters
{
    public interface IMemberSelectionView
    {
        ICollection<Member.DataModels.Member> Members { get; set; }

        IEnumerable<MemberType> MemberTypes { get; set; }
    }
}
