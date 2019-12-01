using System.Windows;
using Membership.UI_Controls.Dues;
using Membership.UI_Controls.Members;
using Membership.UI_Controls.Offices;
using Membership.UI_Controls.ReportViewer;
using DuesHistory = Membership.UI_Controls.Dues.DuesHistory;

namespace Membership
{
    public partial class MainWindow
    {
        public bool CanAddMember
        {
            get { return (bool)GetValue(CanAddMemberProperty); }
            set { SetValue(CanAddMemberProperty, value); }
        }
        public static readonly DependencyProperty CanAddMemberProperty =
            DependencyProperty.Register("CanAddMember", typeof(bool), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
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

        private void OfficerButton_Click(object sender, RoutedEventArgs e)
        {
            MainStack.Children.Clear();
            var ucOfficers = new CurrentOfficers();
            ucOfficers.LoadOfficers();
            MainStack.Children.Add(ucOfficers);
        }

        private void AddMemberButtonOnClick(object sender, RoutedEventArgs e)
        {

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
            var ucDuesCardReport = new ReportViewerWindow();
            ucDuesCardReport.LoadReport("DuesCards");
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesCardReport);
        }

        private void DuesWarningButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucDuesCardReport = new ReportViewerWindow();
            ucDuesCardReport.LoadReport("DuesWarning");
            MainStack.Children.Clear();
            MainStack.Children.Add(ucDuesCardReport);
        }


        private void OfficerHistoryButtonOnClick(object sender, RoutedEventArgs e)
        {

        }

        private void OfficerAssignButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucOfficerRecords = new OfficerRecords();
            ucOfficerRecords.Load();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucOfficerRecords);
        }

        private void ReportsButtonOnClick(object sender, RoutedEventArgs e)
        {

        }

        private void SysConfigButtonOnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
