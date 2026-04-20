using System.Collections.Generic;
using System.Windows.Controls;
using Membership.UI_Controls.ReportViewer;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public abstract class ReportUserControlBase : UserControl
    {
        public string ReportName { get; set; }

        protected abstract ReportViewerWindow Viewer { get; }

        protected virtual IEnumerable<ReportParameter> UpdateParameterValues()
            => new List<ReportParameter>();

        protected virtual IEnumerable<ReportDataSource> LoadDataSets()
            => null;

        protected void UpdateTheReport()
        {
            Viewer.InitializeComponent();
            Viewer.ReportName = ReportName;
            Viewer.ReportParams = UpdateParameterValues();
            Viewer.ReportDatasets = LoadDataSets();
            Viewer.LoadReportControl();
        }
    }
}
