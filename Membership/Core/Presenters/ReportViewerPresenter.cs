using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class ReportViewerPresenter : IReportViewerView
    {
        private readonly IDuesRepository _duesRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IReportViewerView _view;

        public ReportViewerPresenter(IReportViewerView view)
        {
            _duesRepository = new DuesRepository();
            _memberRepository = new MemberRepository();
            _view = view;
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


        public string ReportFileName { get; set; }
        public string DatasetName { get; set; }
        public object DatasetRecords { get; set; }
    }
}
