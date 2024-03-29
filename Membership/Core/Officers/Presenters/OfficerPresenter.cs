﻿using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.Officers.DataModels;
using Membership.Core.Officers.Repositories;

namespace Membership.Core.Officers.Presenters
{
    public class OfficerPresenter :IOfficeHistoryView
    {
        public IEnumerable<Officer> OfficerRecs { get; set; }
        private readonly IOfficerRepository _officeRepository;
        private readonly IOfficeHistoryView _view;

        public OfficerPresenter(IOfficeHistoryView view)
        {
            _officeRepository = new OfficerRepository();
            _view = view;
        }

        public void GetByMemberId(Guid memberId)
        {
            _view.OfficerRecs = _officeRepository.GetOfficersByMember(memberId).OrderBy(fd => fd.FromDate);
        }

    }
}
