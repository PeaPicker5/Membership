using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class MemberInfoPresenter :IMemberView
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberTypeRepository _memberTypeRepository;
        private readonly IOfficerRepository _officerRepository;
        private readonly IDuesRepository _duesRepository;
        private readonly IFileStoreRepository _fileStoreRepository;


        private readonly IMemberView _view;
        public ICollection<Tuple<int, string>> MemberTypeLookups { get; set; }
        public ICollection<Tuple<Guid, string>> SponsorLookups { get; set; }
        public ICollection<MemberRemoval> RemovalCodes { get; set; }

        public MemberInfoPresenter(IMemberView view)
        {
            _memberRepository = new MemberRepository();
            _memberTypeRepository = new MemberTypeRepository();
            _officerRepository = new OfficerRepository();
            _duesRepository = new DuesRepository();
            _fileStoreRepository = new FileStoreRepository();
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

        public void InsertMemberRecord(Member memberRec, byte[] fullResPicture)
        {
            if (fullResPicture != null)
                memberRec.PageId = InsertNewPicture("Membership Card", fullResPicture);
            _memberRepository.InsertMemberRecord(memberRec);
        }
        public void UpdateMemberRecord(Member memberRec, byte[] fullResPicture)
        {
            memberRec.PageId = UpdateMembershipCardImage(memberRec.PageId, fullResPicture);
            _memberRepository.UpdateMemberRecord(memberRec);
        }
        public bool DeleteMemberRecord(Member memberRec)
        {
            _fileStoreRepository.DeleteImageRecord(memberRec.PageId);

            var duesRecords = _duesRepository.GetDuesRecordByMemberId(memberRec.MemberId);
            var duesHistory = _duesRepository.MapFromData(duesRecords);
//            _duesRepository.DeleteDuesPayments(duesRecords);

            var officerRecords = _officerRepository.GetOfficersByMember(memberRec.MemberId);
            //_officerRepository.dele
            //Need to delete records from DUES_History, MEETING_Attendance, and OFFICE_Assignments.
            //var meetingRecords = _

            //return false;
            return _memberRepository.DeleteMemberRecord(memberRec, duesHistory, officerRecords);
        }

        public byte[] GetImageData(Guid fileStoreId)
        {
            return _fileStoreRepository.Get(fileStoreId).ImageData;
        }


        private Guid UpdateMembershipCardImage(Guid imageId, byte[] image)
        {
            // new picture added
            if (imageId == Guid.Empty && image != null) return InsertNewPicture("Membership Card", image);

            // picture was either updated or ignored.
            if (imageId != Guid.Empty && image != null) return UpdateExistingPicture(imageId, image);

            // picture was cleared out
            if (imageId != Guid.Empty && image == null) return DeleteExistingPicture(imageId);

            return Guid.Empty;
        }

        private Guid InsertNewPicture(string description, byte[] image)
        {
            var img = new ImageRec(Guid.NewGuid(), description, image);
            _fileStoreRepository.InsertImageRecord(img);
            return img.ImageId;
        }
        private Guid UpdateExistingPicture(Guid imageId, byte[] image)
        {
            return _fileStoreRepository.UpdateImageRecord(imageId, image) ? imageId : Guid.Empty;
        }
        private Guid DeleteExistingPicture(Guid imageId)
        {
            return _fileStoreRepository.DeleteImageRecord(imageId) ? Guid.Empty : imageId;
        }

    }
}
