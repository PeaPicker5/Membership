using System.Collections.Generic;

namespace Membership.Core.Officers.Presenters
{
    public interface IEditOfficerView
    {
        IEnumerable<DataModels.Office> OfficeRecs { get; set; }
        IEnumerable<Member.DataModels.Member> MemberRecs { get; set; }
    }
}
