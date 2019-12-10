using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Membership.Core.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.ReportViewer
{
    public partial class ReportViewerWindow 
    {
        private readonly ReportViewerPresenter _presenter;

        #region Dependency Properties
        public string ReportName
        {
            get { return (string)GetValue(ReportNameProperty); }
            set { SetValue(ReportNameProperty, value); }
        }
        public static readonly DependencyProperty ReportNameProperty =
            DependencyProperty.Register("ReportName",
                typeof(string), typeof(ReportViewerWindow));
        public IEnumerable<ReportParameter> ReportParams
        {
            get { return (IEnumerable<ReportParameter>)GetValue(ReportParamsProperty); }
            set { SetValue(ReportParamsProperty, value); }
        }
        public static readonly DependencyProperty ReportParamsProperty =
            DependencyProperty.Register("ReportParams",
                typeof(IEnumerable<ReportParameter>), typeof(ReportViewerWindow));

        #endregion

        public ReportViewerWindow()
        {
            InitializeComponent();
            _presenter = new ReportViewerPresenter();
        }

        public void LoadReport()
        {
            var reportFileName = $"Membership.ReportDefinitions.{ReportName}.rdlc";
            ReportViewer.LocalReport.ReportEmbeddedResource = reportFileName;

            LoadReportDatasets();
            if (ReportParams.Any())
                ReportViewer.LocalReport.SetParameters(ReportParams);
            ReportViewer.SetDisplayMode(DisplayMode.PrintLayout);

            ReportViewer.RefreshReport();
        }

        private void LoadReportDatasets()
        {
            switch (ReportName.ToUpper())
            {
                case "DUESCARDS":
                case "DUESWARNING":
                case "DUESREMOVALNOTICE":
                case "DUESREMOVALLETTERS":
                    var membersThatOwe = _presenter.CurrentlyOweDues()
                                         .Where(rec => !rec.IsPaid)
                                         .Select(ml => ml.MemberId).ToList();
                    var dataset = new ReportDataSource
                    {
                        Name = "MemberRecord", 
                        Value = _presenter.GetMembersFromList(membersThatOwe)
                    };
                    ReportViewer.LocalReport.DataSources.Add(dataset);
                    break;
            }
        }

    }
}
