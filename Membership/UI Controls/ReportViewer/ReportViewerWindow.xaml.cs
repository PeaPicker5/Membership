using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Membership.Core.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.ReportViewer
{
    public partial class ReportViewerWindow : IReportViewerView
    {
        private readonly ReportViewerPresenter _presenter;

        public ReportViewerWindow()
        {
            InitializeComponent();
            _presenter = new ReportViewerPresenter(this);
        }

        public void LoadReport(string reportName)
        {
            LoadReportDatasets(reportName);

            ReportViewer.LocalReport.ReportEmbeddedResource = ReportFileName;
            ReportViewer.LocalReport.SetParameters(GetParameters());
            ReportViewer.SetDisplayMode(DisplayMode.PrintLayout);

            ReportViewer.RefreshReport();
        }

        private IEnumerable<ReportParameter> GetParameters()
        {
            var retVal = new List<ReportParameter>();
            retVal.Add(new ReportParameter("Signature", "Kevin Doran"));

            return retVal;
        }
        private void LoadReportDatasets(string reportName)
        {
            ReportFileName = $"Membership.ReportDefinitions.{reportName}.rdlc";

            switch (reportName.ToUpper())
            {
                case "DUESCARDS":
                case "DUESWARNING":
                    var membersThatOwe = _presenter.CurrentlyOweDues()
                                         .Where(rec => !rec.IsPaid)
                                         .Select(ml => ml.MemberId).ToList();
                    // MemberRecord Dataset
                    DatasetRecords = _presenter.GetMembersFromList(membersThatOwe);
                    var dataset = new ReportDataSource { Name = "MemberRecord", Value = DatasetRecords };
                    ReportViewer.LocalReport.DataSources.Add(dataset);
                    break;
            }
        }

        public string ReportFileName { get; set; }
        public string DatasetName { get; set; }
        public object DatasetRecords { get; set; }
    }
}
