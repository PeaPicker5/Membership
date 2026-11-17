using System.Collections.Generic;
using Membership.Core.LOSAP.DataModels;

namespace Membership.Core.LOSAP.Presenters
{
    public interface ILosapRecordView
    {
        ICollection<LosapRecord> LosapRecs { get; set; }
    }
}
