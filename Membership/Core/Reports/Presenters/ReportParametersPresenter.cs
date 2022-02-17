using System;
using System.Collections.Generic;
using System.Linq;
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

        public ReportParametersPresenter()
        {
            _officeRepository = new OfficerRepository();
        }

        public IEnumerable<Officer> LoadTableOfficersForThisYear()
        {
            return _officeRepository.GetOfficersByYear(DateTime.Now.Year)
                .Where(x => x.OfficeRec.GroupId == 22)
                .OrderBy(x => x.OfficeRec.GroupOrder)
                .ToList();
        }


    }
}
