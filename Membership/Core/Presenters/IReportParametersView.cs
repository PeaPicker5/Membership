using System.Collections.Generic;
using Microsoft.Reporting.WinForms;

namespace Membership.Core.Presenters
{
    public interface IReportParametersView
    {
        IEnumerable<ReportParameter> Parameters { get; set; }
    }
}
