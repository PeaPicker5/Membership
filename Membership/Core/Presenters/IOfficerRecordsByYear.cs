using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IOfficerRecordsByYearView
    {
        IEnumerable<Officer> Commissioners { get; set; }
        IEnumerable<Officer> LineOfficers { get; set; }
        IEnumerable<Officer> OtherDistrictOfficers { get; set; }

        IEnumerable<Officer> BoardOfDirectors { get; set; }
        IEnumerable<Officer> TableOfficers { get; set; }
        IEnumerable<Officer> OtherAssocOfficers { get; set; }
    }
}
