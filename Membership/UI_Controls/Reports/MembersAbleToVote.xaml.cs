using System.Collections.Generic;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Membership.UI_Controls.ReportViewer;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class MembersAbleToVote : ReportUserControlBase
    {
        private readonly ReportDatasetPresenter _presenter;

        public MembersAbleToVote()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => UpdateTheReport();
        }

        protected override ReportViewerWindow Viewer => ReportControl;

        protected override IEnumerable<ReportDataSource> LoadDataSets()
        {
            return new List<ReportDataSource>
            {
                new ReportDataSource { Name = "MembersAbleToVote", Value = new List<Member>(_presenter.MembersThanCanVote()) },
                new ReportDataSource { Name = "Last11Meetings",    Value = new List<Meeting>(_presenter.GetLast11Meetings()) }
            };
        }
    }
}
