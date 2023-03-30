using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class DuesCards : IReportViewerView
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

        private void SetupParameters()
        {
            var finSecRecord = _presenter.LoadTableOfficersForThisYear()
                .FirstOrDefault(x => x.OfficeRec.OfficeId == 13);

            if (finSecRecord != null)
                OfficerToSignText.Text = finSecRecord.MemberRec.FullName;
        }
        
        private IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>
            {   
                new ReportParameter("Signature", OfficerToSignText.Text),
                new ReportParameter("FirstNotice", NoticeRadio.IsChecked==true
                                                            ? bool.TrueString : bool.FalseString )
            };
        }

        private void UpdateReportOnClick(object sender, RoutedEventArgs e)
        {
            UpdateTheReport();
        }

        private void UpdateTheReport()
        {
            ReportControl.InitializeComponent();
            ReportControl.ReportName = ReportName;
            ReportControl.ReportParams = UpdateParameterValues();
            ReportControl.ReportDatasets = LoadDataSets();
            ReportControl.LoadReportControl();
        }

        private IEnumerable<ReportDataSource> LoadDataSets()
        {
            var retValue = new List<ReportDataSource>();
            var datasetTable = _presenter.GetRecords("exec RPT_DuesCards3330");
            var dSet = new ReportDataSource
            {
                Name = "Members",
                Value = datasetTable
            };
            retValue.Add(dSet);
            return retValue;
        }


        public string ReportName { get; set; }
        public IEnumerable<ReportParameter> ReportParams { get; set; }
        public IEnumerable<ReportDataSource> ReportDatasets { get; set; }
    }
}
