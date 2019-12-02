using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class OfficerRecordsByYearPresenter :IOfficerRecordsByYearView
    {
        public IEnumerable<Officer> Commissioners { get; set; }
        public IEnumerable<Officer> LineOfficers { get; set; }
        public IEnumerable<Officer> OtherDistrictOfficers { get; set; }
        public IEnumerable<Officer> BoardOfDirectors { get; set; }
        public IEnumerable<Officer> TableOfficers { get; set; }
        public IEnumerable<Officer> OtherAssocOfficers { get; set; }

        private readonly IOfficeHeldRepository _officeRepository;
        private readonly IOfficerRecordsByYearView _view;

        public OfficerRecordsByYearPresenter(IOfficerRecordsByYearView view)
        {
            _officeRepository = new OfficeHeldRepository();
            _view = view;
        }

        public void LoadOfficersForaYear(int year)
        {
            var allRecs = _officeRepository.GetOfficersByYear(year).ToList();

            _view.Commissioners = allRecs.Where(off => off.OfficeRec.GroupId == 1);
            _view.LineOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 2);
            _view.OtherDistrictOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 3);
            _view.BoardOfDirectors = allRecs.Where(off => off.OfficeRec.GroupId == 4);
            _view.TableOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 5);
            _view.OtherAssocOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 6);
        }
    }
}
