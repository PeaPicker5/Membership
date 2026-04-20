using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Membership.Core.Reports.Presenters;
using Membership.UI_Controls.ReportViewer;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class DuesCards : ReportUserControlBase
    {
        private readonly ReportDatasetPresenter _presenter;

        public DuesCards()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => {
                SetupParameters();
                UpdateTheReport();
            };
        }

        protected override ReportViewerWindow Viewer => ReportControl;

        private void SetupParameters()
        {
            var finSecRecord = _presenter.LoadTableOfficersForThisYear()
                .FirstOrDefault(x => x.OfficeRec.OfficeId == 13);

            if (finSecRecord != null)
                OfficerToSignText.Text = finSecRecord.MemberRec.FullName;
        }

        protected override IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>
            {
                new ReportParameter("Signature", OfficerToSignText.Text),
                new ReportParameter("FirstNotice", NoticeRadio.IsChecked == true
                                                        ? bool.TrueString : bool.FalseString)
            };
        }

        private void UpdateReportOnClick(object sender, RoutedEventArgs e) => UpdateTheReport();

        protected override IEnumerable<ReportDataSource> LoadDataSets()
        {
            return new List<ReportDataSource>
            {
                new ReportDataSource { Name = "Members", Value = _presenter.GetRecords("exec RPT_DuesCards3330") }
            };
        }
    }
}
