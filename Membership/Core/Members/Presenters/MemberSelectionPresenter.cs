using System;
using System.Collections.Generic;
using Membership.Core.Dues.DataModels;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Repositories;
using Membership.Core.Officers.DataModels;

namespace Membership.Core.Members.Presenters
{
    public class MemberSelectionPresenter
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberTypeRepository _memberTypeRepository;
        private readonly IMemberSelectionView _view;

        public MemberSelectionPresenter(IMemberSelectionView view)
        {
            _memberRepository = new MemberRepository();
            _memberTypeRepository = new MemberTypeRepository();
            _view = view;
        }

        public void LoadMembers()
        {
            _view.Members = _memberRepository.GetMembers();
        }
        public void LoadMemberTypes()
        {
            _view.MemberTypes = _memberTypeRepository.GetAllTypes();
        }

        public void GetByMemberId(Guid memberId)
        {
            var x = _memberRepository.Get(memberId);
        }

    }
}
