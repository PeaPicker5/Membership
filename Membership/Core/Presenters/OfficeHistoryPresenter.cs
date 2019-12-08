using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class OfficeHistoryPresenter :IOfficeHistoryView
    {
        public IEnumerable<Officer> OfficerRecs { get; set; }
        private readonly IOfficeHeldRepository _officeRepository;
        private readonly IOfficeHistoryView _view;

        public OfficeHistoryPresenter(IOfficeHistoryView view)
        {
            _officeRepository = new OfficeHeldRepository();
            _view = view;
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _officeRepository.GetOfficerYearsOnFile();
        }

        public IEnumerable<Office> LoadOfficesOnFile()
        {
            return _officeRepository.GetOfficesOnFile();
        }

        public void LoadOfficersForaTitle(int officeId, int fromYear = 9999, int backToYear = 0)
        {
            _view.OfficerRecs = _officeRepository.GetOfficersByTitle(officeId)
                .Where(x => x.Year <= fromYear && x.Year >= backToYear)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.MemberRec.LastName)
                .ThenBy(x => x.MemberRec.FirstName)
                .ToList();
        }
    }
}
