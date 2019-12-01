using System;
using System.Collections.Generic;
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
            _view.OfficerRecs = _officeRepository.GetOfficesByMember(memberId);
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _officeRepository.GetYearsOnFile();
        }

        public IEnumerable<Office> LoadOfficesOnFile()
        {
            return _officeRepository.GetOfficesOnFile();
        }
    }
}
