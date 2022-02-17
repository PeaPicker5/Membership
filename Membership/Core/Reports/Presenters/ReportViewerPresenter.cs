using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Membership.Core.Dues.DataModels;
using Membership.Core.Dues.Repositories;
using Membership.Core.Members.Repositories;
using Membership.Core.Reports.Repositories;

namespace Membership.Core.Reports.Presenters
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

        public IEnumerable<Members.DataModels.Member> GetMembersFromList(IEnumerable<Guid> memberIdList)
        {
            var retValue = new List<Members.DataModels.Member>();
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
