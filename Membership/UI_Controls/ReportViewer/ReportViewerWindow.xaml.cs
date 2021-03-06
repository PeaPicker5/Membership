﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.ReportViewer
{
    public partial class ReportViewerWindow 
    {
        private readonly ReportViewerPresenter _presenter;

        #region Dependency Properties
        public string ReportName
        {
            get { return (string)GetValue(ReportNameProperty); }
            set { SetValue(ReportNameProperty, value); }
        }
        public static readonly DependencyProperty ReportNameProperty =
            DependencyProperty.Register("ReportName",
                typeof(string), typeof(ReportViewerWindow));
        public IEnumerable<ReportParameter> ReportParams
        {
            get { return (IEnumerable<ReportParameter>)GetValue(ReportParamsProperty); }
            set { SetValue(ReportParamsProperty, value); }
        }
        public static readonly DependencyProperty ReportParamsProperty =
            DependencyProperty.Register("ReportParams",
                typeof(IEnumerable<ReportParameter>), typeof(ReportViewerWindow));

        #endregion

        public ReportViewerWindow()
        {
            InitializeComponent();
            _presenter = new ReportViewerPresenter();
        }

        public void LoadReport()
        {
            var reportFileName = $"Membership.ReportDefinitions.{ReportName}.rdlc";
            ReportViewer.LocalReport.ReportEmbeddedResource = reportFileName;

            LoadReportDatasets();
            if (ReportParams.Any())
                ReportViewer.LocalReport.SetParameters(ReportParams);
            ReportViewer.SetDisplayMode(DisplayMode.PrintLayout);

            ReportViewer.RefreshReport();
        }

        private void LoadReportDatasets()
        {
            ReportViewer.LocalReport.DataSources.Clear();

            switch (ReportName.ToUpper())
            {
                case "DUESCARDS":
                case "DUESWARNING":
                case "DUESREMOVALNOTICE":
                case "DUESREMOVALLETTERS":
                case "DUESADDRESSLABELS":
                    var datasetValue = new List<Member>();

                    if (ReportName.ToUpper() == "DUESADDRESSLABELS")
                        datasetValue.AddRange(GetBlankMembersForLabels());
                    
                    var membersThatOwe = _presenter.CurrentlyOweDues().Where(rec => !rec.IsPaid).Select(ml => ml.MemberId).ToList();
                    datasetValue.AddRange(_presenter.GetMembersFromList(membersThatOwe));

                    var dataset = new ReportDataSource
                    {
                        Name = "MemberRecord",
                        Value = datasetValue
                    };
                    ReportViewer.LocalReport.DataSources.Add(dataset);
                    break;
            }
        }

        private IEnumerable<Member> GetBlankMembersForLabels()
        {
            var blankLabels = (from p in ReportParams where p.Name == "StartingLabel" select p.Values).FirstOrDefault();

            if (blankLabels == null) return null;
            var numOfLabels = Convert.ToInt32(blankLabels[0]);

            var returnVal = new List<Member>();
            for (var i = 1; i < numOfLabels; i++)
            {
                returnVal.Add(new Member());
            }
            return returnVal;
        }
    }
}
