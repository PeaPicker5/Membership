using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Presenters;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{

    public partial class MemberDetailsReport : IMemberView, INotifyPropertyChanged
    {

        private readonly ReportParametersPresenter _presenter;


        public IEnumerable<Tuple<Guid, string>> MemberLookups
        {
            get { return (IEnumerable<Tuple<Guid, string>>)GetValue(MemberLookupsProperty); }
            set { SetValue(MemberLookupsProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty MemberLookupsProperty =
            DependencyProperty.Register("MemberLookups", typeof(IEnumerable<Tuple<Guid, string>>),
                typeof(MemberDetailsReport));

        public MemberDetailsReport()
        {
            InitializeComponent();
            _presenter = new ReportParametersPresenter();
            Loaded += (sender, args) => {
                SetupParameters();
            };
        }
        private void SetupParameters()
        {
            MemberLookups = _presenter.GetMemberTuples();
            MembersCombo.SelectedIndex = 1;
        }

        private IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>
            {
                new ReportParameter("MemberId", MembersCombo.SelectedValue.ToString())
            };
        }

        private void UpdateReportOnClick(object sender, RoutedEventArgs e)
        {
            UpdateTheReport();
        }

        private void UpdateTheReport()
        {
            ReportControl.ReportName = "MemberDetails";
            ReportControl.ReportParams = UpdateParameterValues();
            ReportControl.LoadReport();
        }

        public ICollection<Tuple<int, string>> MemberTypeLookups { get; set; }
        public ICollection<Tuple<Guid, string>> SponsorLookups { get; set; }
        public ICollection<MemberRemoval> RemovalCodes { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
