using System.Collections.Generic;
using System.Linq;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{

    public partial class Letterhead : IReportViewerView
    {
        private readonly ReportDatasetPresenter _presenter;

        public string ReportName { get; set; }
        public IEnumerable<ReportParameter> ReportParams { get; set; }
        public IEnumerable<ReportDataSource> ReportDatasets { get; set; }

        public Letterhead()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => {
                UpdateTheReport();
            };
        }

        private void UpdateTheReport()
        {
            ReportControl.InitializeComponent();
            ReportControl.ReportName = ReportName;
            ReportControl.ReportParams = new List<ReportParameter>();
            ReportControl.LoadReportControl();
        }

    }
}
