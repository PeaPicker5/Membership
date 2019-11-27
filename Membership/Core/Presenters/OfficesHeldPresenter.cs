using System;
using System.Collections.Generic;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class OfficesHeldPresenter :IOfficeHeldView
    {
        public IEnumerable<OfficeHeld> OfficerRecs { get; set; }
        private readonly IOfficeHeldRepository _officeRepository;
        private readonly IOfficeHeldView _view;

        public OfficesHeldPresenter(IOfficeHeldView view)
        {
            _officeRepository = new OfficeHeldRepository();
            _view = view;
        }

        public void GetByMemberId(Guid memberId)
        {
            _view.OfficerRecs = _officeRepository.GetOfficesByMember(memberId);
        }

    }
}
