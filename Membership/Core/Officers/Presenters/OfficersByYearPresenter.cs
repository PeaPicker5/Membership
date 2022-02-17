using System.Collections.Generic;
using System.Linq;
using Membership.Core.Officers.DataModels;
using Membership.Core.Officers.Repositories;

namespace Membership.Core.Officers.Presenters
{
    public class OfficersByYearPresenter : IOfficersByYearView
    {
        public IEnumerable<Officer> Commissioners { get; set; }
        public IEnumerable<Officer> LineOfficers { get; set; }
        public IEnumerable<Officer> OtherDistrictOfficers { get; set; }
        public IEnumerable<Officer> BoardOfDirectors { get; set; }
        public IEnumerable<Officer> TableOfficers { get; set; }
        public IEnumerable<Officer> OtherAssocOfficers { get; set; }

        private readonly IOfficerRepository _officeRepository;
        private readonly IOfficersByYearView _view;

        public OfficersByYearPresenter(IOfficersByYearView view)
        {
            _officeRepository = new OfficerRepository();
            _view = view;
        }

        public void LoadOfficersForaYear(int year)
        {
            var allRecs = _officeRepository.GetOfficersByYear(year).ToList();

            _view.Commissioners = allRecs.Where(off => off.OfficeRec.GroupId == 11).OrderBy(n => n.MemberRec.LFName); 
            _view.LineOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 12).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.OtherDistrictOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 13).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.BoardOfDirectors = allRecs.Where(off => off.OfficeRec.GroupId == 21).OrderBy(n => n.MemberRec.LFName);
            _view.TableOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 22).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.OtherAssocOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 23).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
        }


    }
}
