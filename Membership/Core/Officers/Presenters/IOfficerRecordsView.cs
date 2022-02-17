using System.Collections.Generic;
using Membership.Core.Officers.DataModels;

namespace Membership.Core.Officers.Presenters
{
    public interface IOfficerRecordsView
    {
        IEnumerable<Officer> OfficerRecs { get; set; }

    }
}
