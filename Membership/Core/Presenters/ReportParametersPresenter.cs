using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;
using Microsoft.Reporting.WinForms;

namespace Membership.Core.Presenters
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
                .Where(x => x.OfficeRec.GroupId == 5)
                .OrderBy(x => x.OfficeRec.Order)
                .ToList();
        }


    }
}
