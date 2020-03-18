using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IEditOfficerView
    {
        IEnumerable<Office> OfficeRecs { get; set; }
        IEnumerable<Member> MemberRecs { get; set; }
    }
}
