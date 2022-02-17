using System.Collections.Generic;
using System.Linq;
using Membership.Core.Members.Repositories;
using Membership.Core.Officers.Repositories;

namespace Membership.Core.Officers.Presenters
{
    public class EditOfficerPresenter : IEditOfficerView
    {

        public IEnumerable<DataModels.Office> OfficeRecs { get; set; }
        public IEnumerable<Members.DataModels.Member> MemberRecs { get; set; }
        private IEnumerable<DataModels.Office> _allOfficeRecs { get; set; }
        private IEnumerable<Members.DataModels.Member> _allMemberRecs { get; set; }
        private readonly IMemberRepository _memberRepository;
        private readonly IOfficerRepository _officeRepository;
        private readonly IEditOfficerView _view;

        public EditOfficerPresenter(IEditOfficerView view)
        {
            _memberRepository = new MemberRepository();
            _officeRepository = new OfficerRepository();
            _view = view;
            LoadAllOffices();
            LoadAllMembers();
        }
        private void LoadAllOffices()
        {
            _allOfficeRecs = _officeRepository.GetOfficesOnFile().ToList<DataModels.Office>();
        }
        private void LoadAllMembers()
        {
            _allMemberRecs = _memberRepository.GetMembers().ToList();
        }

        public void GetAllOfficeRecords(bool onlyCurrentOffices = false)
        {
            _view.OfficeRecs = onlyCurrentOffices
                ? _allOfficeRecs.Where(x => x.CurrentlyUsed).OrderBy(x => x.GroupId).ThenBy(x => x.GroupOrder).ToList()
                : _allOfficeRecs.OrderBy(x => x.GroupId).ThenBy(x => x.GroupOrder).ToList();
        }


        public void GetAllMemberRecords(bool onlyCurrentMembers = false)
        {
            _view.MemberRecs = onlyCurrentMembers
                ? _allMemberRecs.Where(x => x.IsCurrent).OrderBy(x => x.LFName).ToList()
                : _allMemberRecs.OrderBy(x => x.LFName).ToList();
        }
        public void GetCurrentActiveMembers()
        {
            _view.MemberRecs = _allMemberRecs.Where(x => x.IsActive).OrderBy(x => x.LFName).ToList();
        }


        public void InsertOfficerRecord(DataModels.Officer officerRec)
        {

            _officeRepository.InsertOfficerRecord(officerRec);
        }
        
        public void UpdateOfficerRecord(DataModels.Officer officerRec)
        {
            _officeRepository.UpdateOfficerRecord(officerRec);
        }

        public void DeleteOfficerRecord(DataModels.Officer officerRec)
        {
            _officeRepository.DeleteOfficerRecord(officerRec);
        }


        #region EDIT Office Control Calls
        public void LoadCurrentMembersWithAnOffice(int year, int officeType)
        {
            _view.MemberRecs = _memberRepository.GetMembersWithOffice(year, officeType)
                .OrderBy(x => x.LFName)
                .ToList();
        }

        #endregion

    }
}
