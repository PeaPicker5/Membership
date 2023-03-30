using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class DuesAddressLabels : IReportViewerView, INotifyPropertyChanged
    {
        private readonly ReportDatasetPresenter _presenter;
        public string ReportName { get; set; }
        public IEnumerable<ReportParameter> ReportParams { get; set; }
        public IEnumerable<ReportDataSource> ReportDatasets { get; set; }

        public int StartingLabel
        {
            get => (int)GetValue(StartingLabelProperty);
            set { SetValue(StartingLabelProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty StartingLabelProperty =
            DependencyProperty.Register(nameof(StartingLabel),
                typeof(int), typeof(DuesAddressLabels));


        public DuesAddressLabels()
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
            StartingLabel = 1;
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
        private IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>
            {   
                new ReportParameter("StartingLabel", StartingLabel.ToString())
            };
        }

        private IEnumerable<ReportDataSource> LoadDataSets()
        {
            var retValue = new List<ReportDataSource>();
            var MembersList = new List<Member>();

            MembersList.AddRange(ReportDatasetPresenter.GetBlankMembersForLabels(StartingLabel));
            var membersThatOwe = _presenter.CurrentlyOweDues().Where(rec => !rec.IsPaid).Select(ml => ml.MemberId).ToList();
            MembersList.AddRange(_presenter.GetMembersFromList(membersThatOwe));

            var dSet = new ReportDataSource
            {
                Name = "Members",
                Value = MembersList
            };
            retValue.Add(dSet);

            return retValue;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
