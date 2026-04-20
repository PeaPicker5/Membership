using System.Collections.Generic;
using Membership.Core.LOSAP.DataModels;

namespace Membership.Core.LOSAP.Presenters
{
    public interface ILosapListingView
    {
        ICollection<LosapRecord> LosapRecs { get; set; }
    }
}
