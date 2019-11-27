using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IOfficeHeldView
    {
        IEnumerable<OfficeHeld> OfficerRecs { get; set; }
    }
}
