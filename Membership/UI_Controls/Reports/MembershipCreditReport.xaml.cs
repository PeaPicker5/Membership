using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.Members.DataModels;
using Membership.Core.Officers.DataModels;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{

    public partial class MembershipCreditReport : IReportViewerView, INotifyPropertyChanged
    {

        private readonly ReportDatasetPresenter _presenter;
        public IEnumerable<int> YearsOnFile
        {
            get => (IEnumerable<int>)GetValue(YearsOnFileProperty);
            private set
            {
                SetValue(YearsOnFileProperty, value); OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty YearsOnFileProperty =
            DependencyProperty.Register(nameof(YearsOnFile), typeof(IEnumerable<int>),
                typeof(MembershipCreditReport));
        
        
        public MembershipCreditReport()
        {
            InitializeComponent();
            _presenter = new ReportDatasetPresenter();
            Loaded += (sender, args) => {
                SetupParameters();
            };
        }
        private void SetupParameters()
        {
            LoadYearsOnFile();
        }

        private const int initialYear = 2018;
        private void LoadYearsOnFile()
        {
            YearsOnFile = Enumerable.Range(initialYear, DateTime.Now.Year - initialYear + 1).Reverse().ToList();
            if (YearSelectionCombo.Items.Count > 0)
            {
                YearSelectionCombo.SelectedIndex = 0;
            }
        }

        private IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>();
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

            var query = $"SELECT * FROM VIEW_MembershipCredits WHERE (Year=0 or Year=2022) ";
            var dataTable = _presenter.GetRecords(query);
            retValue.Add(new ReportDataSource { Name = "Credits", Value = dataTable });


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
