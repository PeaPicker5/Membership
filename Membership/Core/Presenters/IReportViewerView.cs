using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Presenters
{
    public interface IReportViewerView
    {
        string ReportFileName { get; set; }
        string DatasetName { get; set; }
        object DatasetRecords { get; set; }
    }
}
