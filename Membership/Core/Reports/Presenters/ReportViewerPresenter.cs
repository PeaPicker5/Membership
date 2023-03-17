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
using Membership.Core.Officers.Repositories;
using Membership.Core.Reports.Repositories;

namespace Membership.Core.Reports.Presenters
{
    public class ReportViewerPresenter
    {
        private readonly IDuesRepository _duesRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IOfficerRepository _officerRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IReportDataRepository _reportDataRepository;

        public ReportViewerPresenter()
        {
            _duesRepository = new DuesRepository();
            _memberRepository = new MemberRepository();
            _officerRepository = new OfficerRepository();
            _meetingRepository = new MeetingRepository();
            _reportDataRepository = new ReportDataRepository();
        }

        public IEnumerable<Members.DataModels.Member> GetMembersFromList(IEnumerable<Guid> memberIdList)
        {
            var retValue = new List<Members.DataModels.Member>();
            foreach (var itm in memberIdList)
            {
                retValue.AddRange(_memberRepository.GetMembers().Where(mem => itm == mem.MemberId));
            }
            return retValue;
        }
        public Member GetMember(Guid memberId)
        {
            return _memberRepository.Get(memberId);
        }

        public IEnumerable<DuesRecord> CurrentlyOweDues()
        {
            return _duesRepository.CurrentlyOweDues();
        }

        public IEnumerable<Member> MembersThanCanVote()
        {
            return _memberRepository.GetMembersThatCanVote();
        }

        public IEnumerable<Meeting> GetLast11Meetings()
        {
            return _meetingRepository.GetMeetings()
                .Where(m => m.MeetingDate > DateTime.Today.AddMonths(-11) && m.IsRegularScheduled)
                .OrderBy(m => m.MeetingDate);
        }

        public DataTable GetRecords(string query)
        {
            return _reportDataRepository.GetRecordSet(query);
        }

    }
}
