using System;
using System.Collections.Generic;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class OfficesHeldPresenter :IOfficeHistoryView
    {
        public IEnumerable<Officer> OfficerRecs { get; set; }
        private readonly IOfficeHeldRepository _officeRepository;
        private readonly IOfficeHistoryView _view;

        public OfficesHeldPresenter(IOfficeHistoryView view)
        {
            _officeRepository = new OfficeHeldRepository();
            _view = view;
        }

        public void GetByMemberId(Guid memberId)
        {
            _view.OfficerRecs = _officeRepository.GetOfficersByMember(memberId);
        }

    }
}
