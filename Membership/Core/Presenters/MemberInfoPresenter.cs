using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class MemberInfoPresenter :IMemberView
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberTypeRepository _memberTypeRepository;
        private readonly IMemberView _view;

        public MemberInfoPresenter(IMemberView view)
        {
            _memberRepository = new MemberRepository();
            _memberTypeRepository = new MemberTypeRepository();
            _view = view;
        }


        public void LoadLookups()
        {
            _view.MemberTypeLookups.Clear();
            foreach (var stat in _memberTypeRepository.GetAllTypes().Where(x => x.TypeId > 0).OrderBy(x => x.TypeId))
            {
                _view.MemberTypeLookups.Add(new Tuple<int, string>(stat.TypeId, $"{stat.Description,-15}\t(Dues ${stat.DuesAmount,3}.00)"));
            }

            _view.MemberLookups = _memberRepository.GetMembers().Select(mem => new Tuple<Guid, string>(mem.MemberId, mem.FullName)).ToList();
            
            _view.RemovalCodes = _memberTypeRepository.GetRemovalCodes().ToList();
        }

        public ICollection<Tuple<int, string>> MemberTypeLookups { get; set; }
        public ICollection<Tuple<Guid, string>> MemberLookups { get; set; }
        public ICollection<MemberRemoval> RemovalCodes { get; set; }
    }
}
