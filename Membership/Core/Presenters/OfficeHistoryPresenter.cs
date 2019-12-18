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
        private readonly IOfficerRepository _officerRepository;
        private readonly IOfficeHistoryView _view;

        public OfficeHistoryPresenter(IOfficeHistoryView view)
        {
            _officerRepository = new OfficerRepository();
            _view = view;
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _officerRepository.GetOfficerYearsOnFile();
        }

        public IEnumerable<Office> LoadOfficesOnFile()
        {
            return _officerRepository.GetOfficesOnFile();
        }

        public void LoadOfficersForaTitle(int officeId, int fromYear = 9999, int backToYear = 0)
        {
            _view.OfficerRecs = _officerRepository.GetOfficersByTitle(officeId)
                .Where(x => x.Year <= fromYear && x.Year >= backToYear)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.MemberRec.LastName)
                .ThenBy(x => x.MemberRec.FirstName)
                .ToList();
        }
    }
}
