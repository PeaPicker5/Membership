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
        private readonly IOfficeHeldRepository _officeHeldRepository;
        private readonly IDuesRepository _duesRepository;

        private readonly IMemberView _view;
        public ICollection<Tuple<int, string>> MemberTypeLookups { get; set; }
        public ICollection<Tuple<Guid, string>> SponsorLookups { get; set; }
        public ICollection<MemberRemoval> RemovalCodes { get; set; }

        public MemberInfoPresenter(IMemberView view)
        {
            _memberRepository = new MemberRepository();
            _memberTypeRepository = new MemberTypeRepository();
            _officeHeldRepository = new OfficeHeldRepository();
            _duesRepository = new DuesRepository();
            _view = view;
        }


        public Member GetMember(Guid memberId)
        {
            return _memberRepository.Get(memberId);
        }

        public void LoadLookups()
        {
            _view.MemberTypeLookups.Clear();
            foreach (var stat in _memberTypeRepository.GetAllTypes().Where(x => x.TypeId > 0).OrderBy(x => x.TypeId))
            {
                _view.MemberTypeLookups.Add(new Tuple<int, string>(stat.TypeId, $"{stat.Description,-15}\t(Dues ${stat.DuesAmount,3}.00)"));
            }

            _view.SponsorLookups = _memberRepository.GetMembers().Select(mem => new Tuple<Guid, string>(mem.MemberId, mem.FullName)).ToList();
            
            _view.RemovalCodes = _memberTypeRepository.GetRemovalCodes().ToList();
        }

        public bool InsertMemberRecord(Member memberRec)
        {
            return _memberRepository.InsertMemberRecord(memberRec);
        }
        public bool UpdateMemberRecord(Member memberRec)
        {
            return _memberRepository.UpdateMemberRecord(memberRec);
        }
        public bool DeleteMemberRecord(Member memberRec)
        {
            //Need to delete records from DUES_History, MEETING_Attendance, and OFFICE_Assignments.
            var duesRecords = _duesRepository.GetDuesByMember(memberRec.MemberId);
            var officeRecords  = _officeHeldRepository.GetOfficersByMember(memberRec.MemberId);
//            var meetingRecords = _


            return (_memberRepository.DeleteMemberRecord(memberRec, duesRecords, officeRecords));
        }

    }
}
