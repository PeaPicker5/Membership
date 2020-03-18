using System;
using System.Linq;
using System.Collections.Generic;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class EditOfficerPresenter : IEditOfficerView
    {
        private const int DistrictOnly = 0;
        private const int AssocOnly = 1;

        public IEnumerable<Office> OfficeRecs { get; set; }
        public IEnumerable<Member> MemberRecs { get; set; }
        private readonly IMemberRepository _memberRepository;
        private readonly IOfficerRepository _officeRepository;
        private readonly IEditOfficerView _view;

        public EditOfficerPresenter(IEditOfficerView view)
        {
            _memberRepository = new MemberRepository();
            _officeRepository = new OfficerRepository();
            _view = view;
        }

        public void LoadOfficesForaType(int officeType, bool currentOfficesOnly)
        {
            _view.OfficeRecs = _officeRepository.GetAssignableOffices();

            if (currentOfficesOnly)
                _view.OfficeRecs = _view.OfficeRecs.Where(x => x.CurrentlyUsed == 1);

            _view.OfficeRecs = officeType == DistrictOnly 
                ? _view.OfficeRecs.Where(x => x.GroupId < 20) 
                : _view.OfficeRecs.Where(x => x.GroupId >= 20);

            _view.OfficeRecs = _view.OfficeRecs
                .OrderBy(x => x.GroupId)
                .ThenBy(x => x.GroupOrder);
        }

        public void LoadActiveMembers()
        {
            _view.MemberRecs = _memberRepository.GetMembers()
                .Where(x => x.IsActive)
                .OrderBy(x => x.LFName)
                .ToList();
        }

        public void LoadCurrentMembers()
        {
            _view.MemberRecs = _memberRepository.GetMembers()
                .Where(x => x.IsCurrent)
                .OrderBy(x => x.LFName)
                .ToList();
        }
    }
}
