using System.Collections.Generic;
using Membership.Core.Officers.DataModels;

namespace Membership.Core.Officers.Presenters
{
    public interface IOfficersByYearView
    {
        IEnumerable<Officer> Commissioners { get; set; }
        IEnumerable<Officer> LineOfficers { get; set; }
        IEnumerable<Officer> OtherDistrictOfficers { get; set; }

        IEnumerable<Officer> BoardOfDirectors { get; set; }
        IEnumerable<Officer> TableOfficers { get; set; }
        IEnumerable<Officer> OtherAssocOfficers { get; set; }
    }
}
