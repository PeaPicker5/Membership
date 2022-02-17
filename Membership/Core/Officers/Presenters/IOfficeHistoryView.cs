using System.Collections.Generic;

namespace Membership.Core.Officers.Presenters
{
    public interface IOfficeHistoryView
    {
        IEnumerable<DataModels.Officer> OfficerRecs { get; set; }
    }
}
