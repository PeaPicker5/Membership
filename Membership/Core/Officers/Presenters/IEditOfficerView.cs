using System.Collections.Generic;

namespace Membership.Core.Officers.Presenters
{
    public interface IEditOfficerView
    {
        IEnumerable<DataModels.Office> OfficeRecs { get; set; }
        IEnumerable<Members.DataModels.Member> MemberRecs { get; set; }
    }
}
