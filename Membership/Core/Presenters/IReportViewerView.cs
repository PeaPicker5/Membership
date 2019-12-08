using System.Collections.Generic;
using Microsoft.Reporting.WinForms;

namespace Membership.Core.Presenters
{
    public interface IReportViewerView
    {
        string ReportName { get; set; }
        IEnumerable<ReportParameter> ReportParams { get; set; }
    }
}
