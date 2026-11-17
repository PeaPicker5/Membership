using System;
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

        // When the Membership List last finished loading. Used to ignore the second click of a double-click.
        private DateTime _membershipListLoadedAt = DateTime.MinValue;

        private void MembershipButton_Click(object sender, RoutedEventArgs e)
        {
            // A double-click raises Click twice. The second click is processed right after the first
            // finishes loading, so ignore it if it arrives within the system double-click time.
            var sinceLastLoad = DateTime.Now - _membershipListLoadedAt;
            if (sinceLastLoad.TotalMilliseconds < System.Windows.Forms.SystemInformation.DoubleClickTime
                && MainStack.Children.Count == 1 && MainStack.Children[0] is MemberSelection)
                return;

            var ucMemberSelection = new MemberSelection();
            ucMemberSelection.LoadMembers();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucMemberSelection);
            _membershipListLoadedAt = DateTime.Now;
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
            var ucLosapCredit = new LosapCredit();
            ucLosapCredit.LoadMembers();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucLosapCredit);
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
        #endregion


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
