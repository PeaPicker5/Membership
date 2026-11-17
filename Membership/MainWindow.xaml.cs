using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
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

        #region Screen switching

        // The ribbon button whose screen was shown last, and when that screen finished loading.
        private object _lastScreenButton;
        private DateTime _lastScreenLoadedAt = DateTime.MinValue;
        private bool _isLoadingScreen;

        /// <summary>
        /// Replaces the main content with the screen <paramref name="createScreen"/> builds.
        /// A double-click raises Click twice. The second click is ignored when:
        ///  - it is processed while a screen is still loading. "Loading" lasts until the window is
        ///    idle, because report screens build their report in their Loaded event (after this
        ///    method returns) and process pending window messages while doing it; or
        ///  - it comes from the same button within the system double-click time after loading.
        /// A later single click still reloads the screen.
        /// </summary>
        private void ShowScreen(object button, Func<UIElement> createScreen)
        {
            if (_isLoadingScreen) return;

            var sinceLastLoad = DateTime.Now - _lastScreenLoadedAt;
            if (ReferenceEquals(button, _lastScreenButton)
                && sinceLastLoad.TotalMilliseconds < System.Windows.Forms.SystemInformation.DoubleClickTime)
                return;

            _isLoadingScreen = true;
            try
            {
                var screen = createScreen();
                MainStack.Children.Clear();
                MainStack.Children.Add(screen);
                _lastScreenButton = button;
            }
            catch
            {
                _isLoadingScreen = false;
                throw;
            }

            // Finished once Loaded handlers and rendering have run and the window goes idle.
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                _isLoadingScreen = false;
                _lastScreenLoadedAt = DateTime.Now;
            }));
        }

        #endregion

        private void MembershipButton_Click(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () =>
            {
                var screen = new MemberSelection();
                screen.LoadMembers();
                return screen;
            });
        }

        #region LOSAP FUNCTIONS
        private void LosapListingButton_Click(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () =>
            {
                var screen = new LosapListing();
                screen.LoadMembers();
                return screen;
            });
        }

        private void LosapEntryButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () =>
            {
                var screen = new LosapCredit();
                screen.LoadMembers();
                return screen;
            });
        }

        #endregion

        #region  DUES FUNCTIONS

        private void DuesEmailListButtonOnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new DuesEmailList());
        }

        private void DuesHistoryButtonOnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () =>
            {
                var screen = new DuesHistory();
                screen.LoadMembers();
                return screen;
            });
        }

        private void DuesCollectionButtonOnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () =>
            {
                var screen = new DuesCollection();
                screen.LoadMembers();
                return screen;
            });
        }

        private void DuesCardsButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new DuesCards { ReportName = ReportNames.DuesCards });
        }

        private void DuesAddressLabelsButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new DuesAddressLabels { ReportName = ReportNames.DuesAddressLabels });
        }

        private void DuesWarningButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new DuesNoticeReports { ReportName = ReportNames.DuesWarning });
        }

        private void DuesRemovalNoticeButtonOnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new DuesNoticeReports { ReportName = ReportNames.DuesRemovalNotice });
        }

        private void DuesRemovalLettersButtonOnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new DuesNoticeReports { ReportName = ReportNames.DuesRemovalLetters });
        }
        #endregion

        #region OFFICER FUNCTIONS - On creation of new year, ask to close out previous year
        private void OfficeHistoryButtonOnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () =>
            {
                var screen = new OfficeHistory();
                screen.LoadRecords();
                return screen;
            });
        }

        private void OfficerAssignmentButtonOnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () =>
            {
                var screen = new OfficersByYear();
                screen.LoadYearsOnFile();
                return screen;
            });
        }
        #endregion

        private void MeetingButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new MeetingSelection());
        }

        #region REPORTS
        private void MembersAbleToVote_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new MembersAbleToVote { ReportName = ReportNames.MembersAbleToVote });
        }

        private void MemberDetails_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new MemberDetailsReport { ReportName = ReportNames.MemberDetails });
        }

        private void MemberPhoneList_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new MemberPhoneListReport { ReportName = ReportNames.MemberPhoneList });
        }

        private void MembersWithoutEmails_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new MembersWithoutEmailReport { ReportName = ReportNames.MembersWithoutEmail });
        }

        private void MembershipCreditReport_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new MembershipCreditReport { ReportName = ReportNames.MembershipCredits });
        }

        private void Letterhead_OnClick(object sender, RoutedEventArgs e)
        {
            ShowScreen(sender, () => new Letterhead { ReportName = ReportNames.Letterhead });
        }
        #endregion
    }
}
