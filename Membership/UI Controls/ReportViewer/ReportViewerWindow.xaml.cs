using System.Linq;
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
            ReportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            ReportViewer.RefreshReport();
        }


        private void LoadReportDatasets(string reportName)
        {
            ReportFileName = $"Membership.Reports.{reportName}.rdlc";

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
