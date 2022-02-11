using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Annotations;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.ReportDefinitions
{
    public partial class DuesNoticeReports : INotifyPropertyChanged
    {
        private readonly ReportParametersPresenter _presenter;
        public string ReportName { get; set; }

        public IEnumerable<Officer> TableOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("TableOfficers", typeof(IEnumerable<Officer>),
                typeof(DuesNoticeReports));

        public Officer SelectedOffice
        {
            get { return (Officer)GetValue(SelectedOfficeProperty); }
            set { SetValue(SelectedOfficeProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty SelectedOfficeProperty =
            DependencyProperty.Register("SelectedOffice", typeof(Officer),
                typeof(DuesNoticeReports));


        public DuesNoticeReports()
        {
            InitializeComponent();
            _presenter = new ReportParametersPresenter();

            Loaded += (sender, args) => {
                SetupParameters();
                UpdateTheReport();
            };
        }

        private void SetupParameters()
        {
            TableOfficers = _presenter.LoadTableOfficersForThisYear();

            var finSecRecord = TableOfficers.FirstOrDefault(x => x.OfficeRec.OfficeId == 24);
            SelectedOffice = finSecRecord;
        }

        private IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>
            {
                new ReportParameter("Signature", OfficerToSignText.Text),
                new ReportParameter("Office", OfficeTitleCombo.Text)
            };
        }

        private void UpdateReportOnClick(object sender, RoutedEventArgs e)
        {
            UpdateTheReport();
        }

        private void UpdateTheReport()
        {
            ReportControl.ReportName = ReportName; // "DuesRemovalNotice";
            ReportControl.ReportParams = UpdateParameterValues();
            ReportControl.LoadReport();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OfficeTitleCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OfficerToSignText.Text = ((Officer) OfficeTitleCombo.SelectedItem).MemberRec.FullName;
        }
    }
}
