﻿using System.Windows;
using Membership.UI_Controls.Dues;
using Membership.UI_Controls.Members;
using Membership.UI_Controls.Offices;
using Membership.UI_Controls.ReportViewer;

namespace Membership
{
    public partial class MainWindow
    {
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


        private void OfficeHistoryButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucOfficeHistory = new OfficeHistory();
            ucOfficeHistory.LoadRecords();
            MainStack.Children.Clear();
            MainStack.Children.Add(ucOfficeHistory);
        }

        private void OfficerAssignButtonOnClick(object sender, RoutedEventArgs e)
        {
            var ucOfficerRecords = new OfficersByYear();
            ucOfficerRecords.LoadYearsOnFile();
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
