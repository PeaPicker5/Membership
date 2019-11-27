using System;
using System.Collections.Generic;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class MemberSelectionPresenter :IMemberSelectionView
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

        public void Load()
        {
            _view.Members = _memberRepository.GetMembers();
            _view.MemberTypes = _memberTypeRepository.GetAllTypes();
        }

        public void GetByMemberId(Guid memberId)
        {
            var x = _memberRepository.Get(memberId);
        }

        public IEnumerable<Member> Members { get; set; }
        public IEnumerable<MemberType> MemberTypes { get; set; }
        public IEnumerable<Office> Offices { get; set; }
        public IEnumerable<DuesRecord> DuesRecords { get; set; }
    }
}
