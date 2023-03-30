using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{

    public partial class MemberDetailsReport : IReportViewerView, INotifyPropertyChanged
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
                SetupParameters();
            };
        }
        private void SetupParameters()
        {
            MemberLookups = _presenter.GetAllMembers();
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
            ReportControl.InitializeComponent();
            ReportControl.ReportName = ReportName;
            ReportControl.ReportParams = UpdateParameterValues();
            ReportControl.ReportDatasets = LoadDataSets();
            ReportControl.LoadReportControl();
        }

        private IEnumerable<ReportDataSource> LoadDataSets()
        {
            var retValue = new List<ReportDataSource>();
            var MembersList = new List<Member>
            {
                _presenter.GetMember(Guid.Parse(MembersCombo.SelectedValue.ToString())) 
            };

            var dSet = new ReportDataSource
            {
                Name = "Member",
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

        public string ReportName { get; set; }
        public IEnumerable<ReportParameter> ReportParams { get; set; }
        public IEnumerable<ReportDataSource> ReportDatasets { get; set; }
    }
}
