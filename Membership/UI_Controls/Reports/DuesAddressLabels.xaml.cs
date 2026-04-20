using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Membership.UI_Controls.ReportViewer;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class DuesAddressLabels : ReportUserControlBase, INotifyPropertyChanged
    {
        private readonly ReportDatasetPresenter _presenter;

        public int StartingLabel
        {
            get => (int)GetValue(StartingLabelProperty);
            set { SetValue(StartingLabelProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty StartingLabelProperty =
            DependencyProperty.Register(nameof(StartingLabel),
                typeof(int), typeof(DuesAddressLabels));

        public DuesAddressLabels()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => {
                StartingLabel = 1;
                UpdateTheReport();
            };
        }

        protected override ReportViewerWindow Viewer => ReportControl;

        protected override IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>
            {
                new ReportParameter("StartingLabel", StartingLabel.ToString())
            };
        }

        private void UpdateReportOnClick(object sender, RoutedEventArgs e) => UpdateTheReport();

        protected override IEnumerable<ReportDataSource> LoadDataSets()
        {
            var membersList = new List<Member>(ReportDatasetPresenter.GetBlankMembersForLabels(StartingLabel));
            var membersThatOwe = _presenter.CurrentlyOweDues()
                .Where(rec => !rec.IsPaid).Select(ml => ml.MemberId).ToList();
            membersList.AddRange(_presenter.GetMembersFromList(membersThatOwe));

            return new List<ReportDataSource>
            {
                new ReportDataSource { Name = "Members", Value = membersList }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
