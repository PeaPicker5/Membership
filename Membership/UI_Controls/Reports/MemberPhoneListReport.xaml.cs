using System.Collections.Generic;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Membership.UI_Controls.ReportViewer;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class MemberPhoneListReport : ReportUserControlBase
    {
        private readonly ReportDatasetPresenter _presenter;

        public MemberPhoneListReport()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => UpdateTheReport();
        }

        protected override ReportViewerWindow Viewer => ReportControl;

        protected override IEnumerable<ReportDataSource> LoadDataSets()
        {
            var membersList = new List<Member>(_presenter.GetCurrentMembers());
            return new List<ReportDataSource>
            {
                new ReportDataSource { Name = "Members", Value = membersList }
            };
        }
    }
}
