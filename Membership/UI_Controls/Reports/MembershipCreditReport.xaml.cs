using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.Reports.Presenters;
using Membership.UI_Controls.ReportViewer;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class MembershipCreditReport : ReportUserControlBase, INotifyPropertyChanged
    {
        private readonly ReportDatasetPresenter _presenter;
        private const int InitialYear = 2018;

        public IEnumerable<int> YearsOnFile
        {
            get => (IEnumerable<int>)GetValue(YearsOnFileProperty);
            private set { SetValue(YearsOnFileProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty YearsOnFileProperty =
            DependencyProperty.Register(nameof(YearsOnFile), typeof(IEnumerable<int>),
                typeof(MembershipCreditReport));

        public MembershipCreditReport()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => {
                YearsOnFile = Enumerable.Range(InitialYear, DateTime.Now.Year - InitialYear + 1).Reverse().ToList();
                if (YearSelectionCombo.Items.Count > 0)
                    YearSelectionCombo.SelectedIndex = 0;
            };
        }

        protected override ReportViewerWindow Viewer => ReportControl;

        private void UpdateReportOnClick(object sender, RoutedEventArgs e) => UpdateTheReport();

        protected override IEnumerable<ReportDataSource> LoadDataSets()
        {
            var year = (int)YearSelectionCombo.SelectedItem;
            return new List<ReportDataSource>
            {
                new ReportDataSource { Name = "Credits", Value = _presenter.GetRecords(
                    "SELECT * FROM VIEW_MembershipCredits WHERE (Year = 0 OR Year = @Year)",
                    new { Year = year }) }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
