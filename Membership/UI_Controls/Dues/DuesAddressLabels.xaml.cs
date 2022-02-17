using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Annotations;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.ReportDefinitions
{
    public partial class DuesAddressLabels : INotifyPropertyChanged
    {
        private readonly ReportParametersPresenter _presenter;



        public int StartingLabel
        {
            get { return (int)GetValue(StartingLabelProperty); }
            set { SetValue(StartingLabelProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty StartingLabelProperty =
            DependencyProperty.Register("StartingLabel",
                typeof(int), typeof(DuesAddressLabels));


        public DuesAddressLabels()
        {
            InitializeComponent();
            _presenter = new ReportParametersPresenter();
            SetupParameters();
            UpdateTheReport();
        }

        private void SetupParameters()
        {
            StartingLabel = 1;
        }
        
        private IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter>
            {   
                new ReportParameter("StartingLabel", StartingLabel.ToString())
            };
        }

        private void UpdateReportOnClick(object sender, RoutedEventArgs e)
        {
            UpdateTheReport();
        }

        private void UpdateTheReport()
        {
            ReportControl.InitializeComponent();
            ReportControl.ReportName = "DuesAddressLabels";
            ReportControl.ReportParams = UpdateParameterValues();
            ReportControl.LoadReport();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
