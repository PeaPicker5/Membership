using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Membership.UI_Controls.ReportViewer;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class MemberDetailsReport : ReportUserControlBase, INotifyPropertyChanged
    {
        private readonly ReportDatasetPresenter _presenter;

        public IEnumerable<Member> MemberLookups
        {
            get => (IEnumerable<Member>)GetValue(MemberLookupsProperty);
            private set { SetValue(MemberLookupsProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty MemberLookupsProperty =
            DependencyProperty.Register(nameof(MemberLookups), typeof(IEnumerable<Member>),
                typeof(MemberDetailsReport));

        public MemberDetailsReport()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => {
                MemberLookups = _presenter.GetAllMembers();
                MembersCombo.SelectedIndex = 187;
            };
        }

        protected override ReportViewerWindow Viewer => ReportControl;

        protected override IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>
            {
                new ReportParameter("MemberId", MembersCombo.SelectedValue.ToString())
            };
        }

        private void UpdateReportOnClick(object sender, RoutedEventArgs e) => UpdateTheReport();

        protected override IEnumerable<ReportDataSource> LoadDataSets()
        {
            var memberId = Guid.Parse(MembersCombo.SelectedValue.ToString());

            return new List<ReportDataSource>
            {
                new ReportDataSource { Name = "Member",      Value = new List<Member> { _presenter.GetMember(memberId) } },
                new ReportDataSource { Name = "OfficesHeld", Value = _presenter.GetRecords(
                    "SELECT * FROM VIEW_OfficeAssignments WHERE MemberId = @MemberId",
                    new { MemberId = memberId }) }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
