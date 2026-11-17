using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Repositories;
using Membership.Core.Repositories;

namespace Membership.Core.Members.Presenters
{
    public class MemberInfoPresenter
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberTypeRepository _memberTypeRepository;
        private readonly IFileStoreRepository _fileStoreRepository;


        private readonly IMemberView _view;

        public MemberInfoPresenter(IMemberView view)
        {
            _memberRepository = new MemberRepository();
            _memberTypeRepository = new MemberTypeRepository();
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
        public void UpdateMemberRecord(Members.DataModels.Member memberRec, byte[] fullResPicture)
        {
            memberRec.PageId = UpdateMembershipCardImage(memberRec.PageId, fullResPicture);
            _memberRepository.UpdateMemberRecord(memberRec);
        }
        public bool DeleteMemberRecord(Members.DataModels.Member memberRec)
        {
            // Repository removes the member's dues, offices, meeting attendance,
            // LOSAP history and picture in a single transaction.
            return _memberRepository.DeleteMemberRecord(memberRec);
        }

        public byte[] GetImageData(Guid fileStoreId)
        {
            return _fileStoreRepository.Get(fileStoreId)?.ImageData;
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
            // If the stored picture is missing, save the new one rather than dropping it.
            return _fileStoreRepository.UpdateImageRecord(imageId, image)
                ? imageId
                : InsertNewPicture("Membership Card", image);
        }
        private Guid DeleteExistingPicture(Guid imageId)
        {
            // Either the picture was deleted or it was already gone; the member no longer has one.
            _fileStoreRepository.DeleteImageRecord(imageId);
            return Guid.Empty;
        }

    }
}
