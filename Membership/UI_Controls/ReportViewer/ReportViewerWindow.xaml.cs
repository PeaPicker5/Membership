using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using Membership.Core.DataModels;
using Membership.Core.Meetings.DataModels;
using Membership.Core.Members.DataModels;
using Membership.Core.Reports.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.ReportViewer
{
    public partial class ReportViewerWindow : IReportViewerView 
    {
        private readonly ReportViewerPresenter _presenter;

        #region Dependency Properties
        public string ReportName
        {
            get { return (string)GetValue(ReportNameProperty); }
            set { SetValue(ReportNameProperty, value); }
        }
        public static readonly DependencyProperty ReportNameProperty =
            DependencyProperty.Register(nameof(ReportName),
                typeof(string), typeof(ReportViewerWindow));

        public IEnumerable<ReportParameter> ReportParams
        {
            get => (IEnumerable<ReportParameter>)GetValue(ReportParamsProperty);
            set => SetValue(ReportParamsProperty, value);
        }
        public static readonly DependencyProperty ReportParamsProperty =
            DependencyProperty.Register(nameof(ReportParams),
                typeof(IEnumerable<ReportParameter>), typeof(ReportViewerWindow));

        public IEnumerable<ReportDataSource> ReportDatasets { get; set; }
        #endregion

        public ReportViewerWindow()
        {
            InitializeComponent();
            _presenter = new ReportViewerPresenter();
        }

        public void LoadReportControl()
        {
            ReportViewer.LocalReport.DataSources.Clear();
            var reportFileName = $"Membership.ReportDefinitions.{ReportName}.rdlc";
            ReportViewer.LocalReport.ReportEmbeddedResource = reportFileName;

            ReportViewer.LocalReport.DataSources.Clear();
            foreach (var dSet in ReportDatasets)
            {
                ReportViewer.LocalReport.DataSources.Add(dSet);
            }
            if (ReportParams.Any())
                ReportViewer.LocalReport.SetParameters(ReportParams);
            ReportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            ReportViewer.RefreshReport();
//          ReportViewer.ZoomMode = ZoomMode.PageWidth;
        }

    }
}
