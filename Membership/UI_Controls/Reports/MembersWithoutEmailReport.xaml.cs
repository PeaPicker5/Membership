using System.Collections.Generic;
using System.Linq;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Membership.UI_Controls.ReportViewer;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class MembersWithoutEmailReport : ReportUserControlBase
    {
        private readonly ReportDatasetPresenter _presenter;

        public MembersWithoutEmailReport()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => UpdateTheReport();
        }

        protected override ReportViewerWindow Viewer => ReportControl;

        protected override IEnumerable<ReportDataSource> LoadDataSets()
        {
            var membersList = new List<Member>(
                _presenter.GetCurrentMembers().Where(x => string.IsNullOrEmpty(x.EmailAddress)));
            return new List<ReportDataSource>
            {
                new ReportDataSource { Name = "Members", Value = membersList }
            };
        }
    }
}
