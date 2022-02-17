using System.Collections.Generic;
using System.Linq;
using Membership.Core.Officers.Repositories;

namespace Membership.Core.Officers.Presenters
{
    public class OfficeAssignmentPresenter : IOfficeAssignmentView
    {
        public IEnumerable<DataModels.Officer> Commissioners { get; set; }
        public IEnumerable<DataModels.Officer> LineOfficers { get; set; }
        public IEnumerable<DataModels.Officer> OtherDistrictOfficers { get; set; }
        public IEnumerable<DataModels.Officer> BoardOfDirectors { get; set; }
        public IEnumerable<DataModels.Officer> TableOfficers { get; set; }
        public IEnumerable<DataModels.Officer> OtherAssocOfficers { get; set; }

        private readonly IOfficerRepository _officerRepository;
        private readonly IOfficeAssignmentView _view;

        public OfficeAssignmentPresenter(IOfficeAssignmentView view)
        {
            _officerRepository = new OfficerRepository();
            _view = view;
        }

        public void LoadOfficersForaYear(int year)
        {
            var allRecs = _officerRepository.GetOfficersByYear(year).ToList<DataModels.Officer>();

            _view.Commissioners = allRecs.Where(off => off.OfficeRec.GroupId == 1);
            _view.LineOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 2).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.OtherDistrictOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 3).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.BoardOfDirectors = allRecs.Where(off => off.OfficeRec.GroupId == 4);
            _view.TableOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 5).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.OtherAssocOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 6).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
        }

    }
}
