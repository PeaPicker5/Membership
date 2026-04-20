using System.Reflection;
using System.Windows;
using Membership.Core.Reports;
using Membership.UI_Controls.Dues;
using Membership.UI_Controls.LOSAP;
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

        #region LOSAP FUNCTIONS
        private void LosapListingButton_Click(object sender, RoutedEventArgs e)
        {
            var ucLosapListing = new LosapListing();
            ucLosapListing.LoadMembers();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucLosapListing);
        }

        private void LosapEntryButton_OnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesCollection = new DuesCollection();
            ucDuesCollection.LoadMembers();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesCollection);
        }

        #endregion

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

        private void DuesCardsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesCards = new DuesCards { ReportName = ReportNames.DuesCards };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesCards);
        }
        private void DuesAddressLabelsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesCardReport = new DuesAddressLabels { ReportName = ReportNames.DuesAddressLabels };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesCardReport);
        }

        private void DuesWarningButton_OnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesWarningReport = new DuesNoticeReports { ReportName = ReportNames.DuesWarning };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesWarningReport);
        }
        private void DuesRemovalNoticeButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesRemovalNoticeReport = new DuesNoticeReports { ReportName = ReportNames.DuesRemovalNotice };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesRemovalNoticeReport);
        }

        private void DuesRemovalLettersButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesRemovalLettersReport = new DuesNoticeReports { ReportName = ReportNames.DuesRemovalLetters };
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

        private void MembersAbleToVote_OnClick(object sender, RoutedEventArgs e)
        {
            var ucAbleToVoteReport = new MembersAbleToVote { ReportName = ReportNames.MembersAbleToVote };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucAbleToVoteReport);
        }

        private void MemberDetails_OnClick(object sender, RoutedEventArgs e)
        {
            var ucMemberDetailsReport = new MemberDetailsReport { ReportName = ReportNames.MemberDetails };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucMemberDetailsReport);
        }

        private void MemberPhoneList_OnClick(object sender, RoutedEventArgs e)
        {
            var ucMemberPhoneListReport = new MemberPhoneListReport { ReportName = ReportNames.MemberPhoneList };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucMemberPhoneListReport);
        }

        private void MembersWithoutEmails_OnClick(object sender, RoutedEventArgs e)
        {
            var ucMembersWithoutEmailReport = new MembersWithoutEmailReport { ReportName = ReportNames.MembersWithoutEmail };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucMembersWithoutEmailReport);
        }

        private void MembershipCreditReport_OnClick(object sender, RoutedEventArgs e)
        {
            var ucMembershipCreditReport = new MembershipCreditReport { ReportName = ReportNames.MembershipCredits };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucMembershipCreditReport);
        }

        private void Letterhead_OnClick(object sender, RoutedEventArgs e)
        {
            var ucLetterhead = new Letterhead { ReportName = ReportNames.Letterhead };
            MainStack.Children.Clear();
            MainStack.Children.Add(ucLetterhead);

        }

    }
}
