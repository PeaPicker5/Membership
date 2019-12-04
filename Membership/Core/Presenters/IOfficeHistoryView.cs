using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IOfficeHistoryView
    {
        IEnumerable<Officer> OfficerRecs { get; set; }
    }
}
