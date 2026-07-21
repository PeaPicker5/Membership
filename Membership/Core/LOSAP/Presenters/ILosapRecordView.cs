using System.Collections.Generic;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.Members.DataModels;

namespace Membership.Core.LOSAP.Presenters
{
    public interface ILosapRecordView
    {
        ICollection<LosapRecord> LosapRecs { get; set; }
        ICollection<Member> MemberRecs { get; set; }
    }
}
