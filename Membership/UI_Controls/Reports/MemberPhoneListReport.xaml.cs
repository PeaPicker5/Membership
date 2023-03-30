using System.Collections.Generic;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{

    public partial class MemberPhoneListReport : IReportViewerView
    {
        private readonly ReportDatasetPresenter _presenter;

        public string ReportName { get; set; }
        public IEnumerable<ReportParameter> ReportParams { get; set; }
        public IEnumerable<ReportDataSource> ReportDatasets { get; set; }

        public MemberPhoneListReport()
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
            ReportControl.ReportDatasets = LoadDataSets();
            ReportControl.LoadReportControl();
        }

        private IEnumerable<ReportDataSource> LoadDataSets()
        {
            var retValue = new List<ReportDataSource>();
            var MembersList = new List<Member>();

            MembersList.AddRange(_presenter.GetCurrentMembers());
            var dSet = new ReportDataSource
            {
                Name = "Members",
                Value = MembersList
            };
            retValue.Add(dSet);

            return retValue;
        }
    }
}
