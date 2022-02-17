using System.Collections.Generic;
using Membership.Core.Dues.DataModels;

namespace Membership.Core.Dues.Presenters
{
    public interface IDuesRecordView
    {
        ICollection<DuesRecord> DuesRecs { get; set; }
    }
}
