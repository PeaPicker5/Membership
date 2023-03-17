using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Repositories;
using Membership.Core.Officers.DataModels;
using Membership.Core.Officers.Repositories;
using Membership.Core.Repositories;
using Microsoft.Reporting.WinForms;

namespace Membership.Core.Reports.Presenters
{
    public class ReportParametersPresenter
    {
        public IEnumerable<ReportParameter> Parameters { get; set; }

        private readonly IOfficerRepository _officeRepository;
        private readonly IMemberRepository _memberRepository;

        public ReportParametersPresenter()
        {
            _officeRepository = new OfficerRepository();
            _memberRepository = new MemberRepository();
        }

        public IEnumerable<Officer> LoadTableOfficersForThisYear()
        {
            return _officeRepository.GetOfficersByYear(DateTime.Now.Year)
                .Where(x => x.OfficeRec.GroupId == 22)
                .OrderBy(x => x.OfficeRec.GroupOrder)
                .ToList();
        }

        public IEnumerable<Tuple<Guid,string>> GetMemberTuples()
        {
            return _memberRepository.GetMembers()
                .Select(mem => new Tuple<Guid, string>(mem.MemberId, mem.LFName))
                .OrderBy(mem => mem.Item2)
                .ToList();
        }

    }
}
