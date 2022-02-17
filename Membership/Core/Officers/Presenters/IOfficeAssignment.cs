using System.Collections.Generic;

namespace Membership.Core.Officers.Presenters
{
    public interface IOfficeAssignmentView
    {
        IEnumerable<DataModels.Officer> Commissioners { get; set; }
        IEnumerable<DataModels.Officer> LineOfficers { get; set; }
        IEnumerable<DataModels.Officer> OtherDistrictOfficers { get; set; }

        IEnumerable<DataModels.Officer> BoardOfDirectors { get; set; }
        IEnumerable<DataModels.Officer> TableOfficers { get; set; }
        IEnumerable<DataModels.Officer> OtherAssocOfficers { get; set; }
    }
}
