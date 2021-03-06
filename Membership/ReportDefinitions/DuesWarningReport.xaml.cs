﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Annotations;
using Membership.Core.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.ReportDefinitions
{
    public partial class DuesWarningReport : INotifyPropertyChanged
    {
        private readonly ReportParametersPresenter _presenter;

        public DuesWarningReport()
        {
            InitializeComponent();
            _presenter = new ReportParametersPresenter();
            SetupParameters();
            UpdateTheReport();
        }

        private void SetupParameters()
        {
            var finSecRecord = _presenter.LoadTableOfficersForThisYear()
                .FirstOrDefault(x => x.OfficeRec.OfficeId == 13);

            if (finSecRecord != null)
                OfficerToSignText.Text = finSecRecord.MemberRec.FullName;
        }
        
        private IEnumerable<ReportParameter> UpdateParameterValues()
        {
            return new List<ReportParameter> { new ReportParameter("Signature", OfficerToSignText.Text) };
        }

        private void UpdateReportOnClick(object sender, RoutedEventArgs e)
        {
            UpdateTheReport();
        }

        private void UpdateTheReport()
        {
            ReportControl.ReportName = "DuesWarning";
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
