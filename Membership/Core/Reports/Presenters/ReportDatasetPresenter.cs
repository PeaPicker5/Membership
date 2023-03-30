using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Navigation;
using Membership.Core.Dues.DataModels;
using Membership.Core.Dues.Repositories;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Meetings.Repositories;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Repositories;
using Membership.Core.Officers.DataModels;
using Membership.Core.Officers.Repositories;
using Membership.Core.Reports.Repositories;

namespace Membership.Core.Reports.Presenters
{
    public class ReportDatasetPresenter
    {
        private readonly IDuesRepository _duesRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IOfficerRepository _officerRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IReportDataRepository _reportDataRepository;

        public ReportDatasetPresenter()
        {
            _duesRepository = new DuesRepository();
            _memberRepository = new MemberRepository();
            _officerRepository = new OfficerRepository();
            _meetingRepository = new MeetingRepository();
            _reportDataRepository = new ReportDataRepository();
        }
        
        
        #region ***  MEMBER related datasets  ***
        public Member GetMember(Guid memberId)
        {
            return _memberRepository.Get(memberId);
        }

        public IEnumerable<Member> GetMembersFromList(IEnumerable<Guid> memberIdList)
        {
            var retValue = new List<Members.DataModels.Member>();
            foreach (var itm in memberIdList)
            {
                retValue.AddRange(_memberRepository.GetMembers().Where(mem => itm == mem.MemberId));
            }
            return retValue;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _memberRepository.GetMembers();
        }
        public IEnumerable<Member> GetCurrentMembers()
        {
            return _memberRepository.GetCurrentMembers();
        }
        public IEnumerable<Member> MembersThanCanVote()
        {
            return _memberRepository.GetMembersThatCanVote();
        }

        public static IEnumerable<Member> GetBlankMembersForLabels(int startingLabel)
        {
            var returnVal = new List<Member>();
            for (var i = 1; i < startingLabel; i++)
            {
                returnVal.Add(new Member());
            }
            return returnVal;
        }






        #endregion

        #region ***  DUES related datasets  ***
        public IEnumerable<DuesRecord> CurrentlyOweDues()
        {
            return _duesRepository.CurrentlyOweDues();
        }

        #endregion

        #region ***  MEETING related datasaets  ***
        public IEnumerable<Meeting> GetLast11Meetings()
        {
            return _meetingRepository.GetMeetings()
                .Where(m => m.MeetingDate > DateTime.Today.AddMonths(-11) && m.IsRegularScheduled)
                .OrderBy(m => m.MeetingDate);
        }

        #endregion


        public IEnumerable<Officer> LoadTableOfficersForThisYear()
        {
            return _officerRepository.GetOfficersByYear(DateTime.Now.Year)
                .Where(x => x.OfficeRec.GroupId == 22)
                .OrderBy(x => x.OfficeRec.GroupOrder)
                .ToList();
        }


        public DataTable GetRecords(string query)
        {
            return _reportDataRepository.GetRecordSet(query);
        }

    }
}
