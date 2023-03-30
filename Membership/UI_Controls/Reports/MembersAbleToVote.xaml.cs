using System.Collections.Generic;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{

    public partial class MembersAbleToVote : IReportViewerView
    {

        private readonly ReportDatasetPresenter _presenter;

        public MembersAbleToVote()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => {
                UpdateTheReport();
            };
        }

        private IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>();
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

            var MemberList = new List<Member>();
            MemberList.AddRange(_presenter.MembersThanCanVote());
            var dSet1 = new ReportDataSource
            {
                Name = "MembersAbleToVote",
                Value = MemberList
            };
            retValue.Add(dSet1);

            var MeetingList = new List<Meeting>();
            MeetingList.AddRange(_presenter.GetLast11Meetings());
            var dSet2 = new ReportDataSource
            {
                Name = "Last11Meetings",
                Value = MeetingList
            };
            retValue.Add(dSet2);

            return retValue;
        }


        public string ReportName { get; set; }
        public IEnumerable<ReportParameter> ReportParams { get; set; }
        public IEnumerable<ReportDataSource> ReportDatasets { get; set; }
    }
}
