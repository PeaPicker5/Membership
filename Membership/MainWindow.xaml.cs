using System;
using System.Reflection;
using System.Windows;
using Membership.UI_Controls.Dues;
using Membership.UI_Controls.Meetings;
using Membership.UI_Controls.Members;
using Membership.UI_Controls.Offices;
using Membership.UI_Controls.Reports;
using DuesEmailList = Membership.UI_Controls.Reports.DuesEmailList;

namespace Membership
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = $"Association Membership - ver {Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void MembershipButton_Click(object sender, RoutedEventArgs e)
        {
            var ucMemberSelection = new MemberSelection();
            ucMemberSelection.LoadMembers();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucMemberSelection);
        }

        private void OrganizationButton_Click(object sender, RoutedEventArgs e)
        {

        }



        #region  DUES FUNCTIONS 

        private void DuesEmailListButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesEmailList = new DuesEmailList();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesEmailList);
        }


        private void DuesHistoryButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesHistory = new DuesHistory();
            ucDuesHistory.LoadMembers();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesHistory);
        }

        private void DuesCollectionButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesCollection = new DuesCollection();
            ucDuesCollection.LoadMembers();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesCollection);
        }

        private void DuesCardsButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesCards = new DuesCards();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesCards);
        }
        private void DuesAddressLabelsButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesCardReport = new DuesAddressLabels();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesCardReport);
        }

        private void DuesWarningButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesWarningReport = new DuesNoticeReports { ReportName = "DuesWarning" };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesWarningReport);
        }
        private void DuesRemovalNoticeButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesRemovalNoticeReport = new DuesNoticeReports {ReportName = "DuesRemovalNotice"};
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesRemovalNoticeReport);
        }

        private void DuesRemovalLettersButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesRemovalLettersReport = new DuesNoticeReports { ReportName = "DuesRemovalLetters" };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesRemovalLettersReport);
        }
        #endregion

        #region OFFICER FUNCTIONS - On creation of new year, ask to close out previous year
        private void OfficeHistoryButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucOfficeHistory = new OfficeHistory();
            ucOfficeHistory.LoadRecords();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucOfficeHistory);
        }
        private void OfficerAssignmentButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucOfficerRecords = new OfficersByYear();
            ucOfficerRecords.LoadYearsOnFile();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucOfficerRecords);
        }

        //private void AssignOfficerRecordsButtonOnClick(object sender, RoutedEventArgs e)
        //{
        //    var ucOfficeAssignment = new OfficeAssignment();
        //    ucOfficeAssignment.LoadYearsOnFile();
        //    MainStack.Children.Clear();
        //    MainStack.Children.Add(ucOfficeAssignment);
        //}
        #endregion
        private void ReportsButtonOnClick(object sender, RoutedEventArgs e)
        {

        }

        private void SysConfigButtonOnClick(object sender, RoutedEventArgs e)
        {

        }

        private void MeetingButton_OnClick(object sender, RoutedEventArgs e)
        {
            var ucMeetingSelection = new MeetingSelection();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucMeetingSelection);
        }

        private void MembersAbleToVoteOnClick(object sender, RoutedEventArgs e)
        {
            var ucAbleToVoteReport = new MembersAbleToVote();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucAbleToVoteReport);
        }

        private void MemberDetailsOnClick(object sender, RoutedEventArgs e)
        {
            var ucMemberDetailsReport = new MemberDetailsReport();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucMemberDetailsReport);
        }
    }
}
