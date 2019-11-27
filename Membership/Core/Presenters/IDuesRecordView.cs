using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IDuesRecordView
    {
        ICollection<DuesRecord> DuesRecs { get; set; }
    }
}
