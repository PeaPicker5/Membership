using System.Collections.Generic;
using Membership.Core.Members.DataModels;

namespace Membership.Core.Members.Presenters
{
    public interface IMemberSelectionView
    {
        ICollection<Members.DataModels.Member> Members { get; set; }

        IEnumerable<MemberType> MemberTypes { get; set; }
    }
}
