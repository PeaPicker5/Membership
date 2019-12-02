using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class OfficerRecordsPresenter :IOfficerRecordsView
    {
        public IEnumerable<Officer> OfficerRecs { get; set; }
        private readonly IOfficeHeldRepository _officeRepository;
        private readonly IOfficerRecordsView _view;

        public OfficerRecordsPresenter(IOfficerRecordsView view)
        {
            _officeRepository = new OfficeHeldRepository();
            _view = view;
        }

        public void GetByMemberId(Guid memberId)
        {
            _view.OfficerRecs = _officeRepository.GetOfficersByMember(memberId);
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _officeRepository.GetOfficerYearsOnFile();
        }

        public IEnumerable<Office> LoadOfficesOnFile()
        {
            return _officeRepository.GetOfficesOnFile();
        }

        public void LoadOfficersForaTitle(int officeId)
        {
            var allRecs = _officeRepository.GetOfficersByTitle(officeId).ToList();

        }
    }
}
