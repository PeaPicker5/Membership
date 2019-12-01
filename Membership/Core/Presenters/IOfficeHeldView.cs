﻿using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IOfficeHeldView
    {
        IEnumerable<Officer> OfficerRecs { get; set; }
    }
}
