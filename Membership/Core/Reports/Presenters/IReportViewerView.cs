using System.Collections.Generic;
using Microsoft.Reporting.WinForms;

namespace Membership.Core.Reports.Presenters
{
    public interface IReportViewerView
    {
        string ReportName { get; set; }
        IEnumerable<ReportParameter> ReportParams { get; set; }
        IEnumerable<ReportDataSource> ReportDatasets { get; set; }
    }
}
