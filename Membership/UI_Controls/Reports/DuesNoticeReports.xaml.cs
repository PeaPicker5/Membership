using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Annotations;
using Membership.Core.Members.DataModels;
using Membership.Core.Officers.DataModels;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{
    public partial class DuesNoticeReports : IReportViewerView, INotifyPropertyChanged
    {
        private readonly ReportDatasetPresenter _presenter;
        public string ReportName { get; set; }
        public IEnumerable<ReportParameter> ReportParams { get; set; }
        public IEnumerable<ReportDataSource> ReportDatasets { get; set; }

        public IEnumerable<Officer> TableOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register(nameof(TableOfficers), typeof(IEnumerable<Officer>),
                typeof(DuesNoticeReports));

        public Officer SelectedOffice
        {
            get { return (Officer)GetValue(SelectedOfficeProperty); }
            set { SetValue(SelectedOfficeProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty SelectedOfficeProperty =
            DependencyProperty.Register(nameof(SelectedOffice), typeof(Officer),
                typeof(DuesNoticeReports));

        public DuesNoticeReports()
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
            TableOfficers = _presenter.LoadTableOfficersForThisYear();
            SelectedOffice = TableOfficers.FirstOrDefault(x => x.OfficeRec.OfficeId == 24);
        }

        private void OfficeTitleCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OfficerToSignText.Text = ((Officer) OfficeTitleCombo.SelectedItem).MemberRec.FullName;
        }
        private void UpdateReportOnClick(object sender, RoutedEventArgs e)
        {
            UpdateTheReport();
        }

        private IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>
            {
                new ReportParameter("Signature", OfficerToSignText.Text),
                new ReportParameter("Office", OfficeTitleCombo.Text)
            };
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
            var MembersList = new List<Member>();

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
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}