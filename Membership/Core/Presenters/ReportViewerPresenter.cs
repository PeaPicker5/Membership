using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class ReportViewerPresenter
    {
        private readonly IDuesRepository _duesRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IReportDataRepository _reportDataRepository;

        public ReportViewerPresenter()
        {
            _duesRepository = new DuesRepository();
            _memberRepository = new MemberRepository();
            _reportDataRepository = new ReportDataRepository();
        }

        public IEnumerable<Member> GetMembersFromList(IEnumerable<Guid> memberIdList)
        {
            var retValue = new List<Member>();
            foreach (var itm in memberIdList)
            {
                retValue.AddRange(_memberRepository.GetMembers().Where(mem => itm == mem.MemberId));
            }
            return retValue;
        }
        public IEnumerable<DuesRecord> CurrentlyOweDues()
        {
            return _duesRepository.CurrentlyOweDues();
        }

        public DataTable GetRecords(string query)
        {
            return _reportDataRepository.GetRecordSet(query);
        }

    }
}
