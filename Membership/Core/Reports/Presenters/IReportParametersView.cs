using System.Collections.Generic;
using Microsoft.Reporting.WinForms;

namespace Membership.Core.Reports.Presenters
{
    public interface IReportParametersView
    {
        IEnumerable<ReportParameter> Parameters { get; set; }
    }
}
