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
            var datasetValueMembers = new List<Member>();
            var datasetValueMeetings = new List<Meeting>();
            var dataset = new ReportDataSource();

            switch (ReportName.ToUpper())
            {
                case "DUESCARDS":
                    DataTable datasetTable = _presenter.GetRecords("exec RPT_DuesCards3330");

                    var dataset1 = new ReportDataSource
                    {
                        Name = "Members",
                        Value = datasetTable
                    };
                    ReportViewer.LocalReport.DataSources.Add(dataset1);

                    break;
                case "DUESWARNING":
                case "DUESREMOVALNOTICE":
                case "DUESREMOVALLETTERS":
                case "DUESADDRESSLABELS":

                    if (ReportName.ToUpper() == "DUESADDRESSLABELS")
                        datasetValueMembers.AddRange(GetBlankMembersForLabels());

                    var membersThatOwe = _presenter.CurrentlyOweDues().Where(rec => !rec.IsPaid).Select(ml => ml.MemberId).ToList();
                    //                   var membersThatOwe = _presenter.CurrentlyOweDues().Where(rec => !rec.IsPaid).Select(ml => ml.MemberId).ToList();
                    datasetValueMembers.AddRange(_presenter.GetMembersFromList(membersThatOwe));

                    dataset.Name = "MemberRecord";
                    dataset.Value = datasetValueMembers;
                    ReportViewer.LocalReport.DataSources.Add(dataset);
                    break;
                case "MEMBERSABLETOVOTE":

                    datasetValueMembers.AddRange(_presenter.MembersThanCanVote());
                    dataset.Name = "MembersAbleToVote";
                    dataset.Value = datasetValueMembers;
                    ReportViewer.LocalReport.DataSources.Add(dataset);
                    dataset = new ReportDataSource();
                    datasetValueMeetings.AddRange(_presenter.GetLast11Meetings());
                    dataset.Name = "Last11Meetings";
                    dataset.Value = datasetValueMeetings;
                    ReportViewer.LocalReport.DataSources.Add(dataset);
                    break;
                case "MEMBERDETAILS":

                    var memberId = (from p in ReportParams where p.Name == "MemberID" select p.Values).FirstOrDefault().ToString();
                    datasetValueMembers.Add(_presenter.GetMember(Guid.Parse(memberId)));
                    dataset.Name = "MemberInfo";
                    dataset.Value = datasetValueMembers;
                    ReportViewer.LocalReport.DataSources.Add(dataset);

 //                   dataset = new ReportDataSource();
 ////                   datasetValueMeetings.AddRange(_presenter.GetOffices());
 //                   dataset.Name = "OfficerInfo";
 //                   dataset.Value = datasetValueMeetings;
 //                   ReportViewer.LocalReport.DataSources.Add(dataset);
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
