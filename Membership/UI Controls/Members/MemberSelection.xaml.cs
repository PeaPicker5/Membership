using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.Members
{

    public partial class MemberSelection : IMemberSelectionView, INotifyPropertyChanged
    {
        private readonly MemberSelectionPresenter _presenter;

        #region Dependency Properties
        public Member SelectedMember
        {
            get { return (Member)GetValue(SelectedMemberProperty); }
            set { SetValue(SelectedMemberProperty, value); }
        }
        public static readonly DependencyProperty SelectedMemberProperty =
            DependencyProperty.Register("SelectedMember", typeof(Member), 
                typeof(MemberSelection));

        public IEnumerable<Member> Members
        {
            get { return (IEnumerable<Member>)GetValue(MembersProperty); }
            set { SetValue(MembersProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty MembersProperty =
            DependencyProperty.Register("MembersProperty", typeof(IEnumerable<Member>), typeof(MemberSelection));

        public IEnumerable<MemberType> MemberTypes
        {
            get { return (IEnumerable<MemberType>)GetValue(MemberTypesProperty); }
            set { SetValue(MemberTypesProperty, value); }
        }
        public static readonly DependencyProperty MemberTypesProperty =
            DependencyProperty.Register("MemberTypes", typeof(IEnumerable<MemberType>), typeof(MemberSelection));

        public IEnumerable<Member> FilteredMembers
        {
            get { return (IEnumerable<Member>)GetValue(FilteredMembersProperty); }
            set
            {
                SetValue(FilteredMembersProperty, value); 
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty FilteredMembersProperty =
            DependencyProperty.Register("FilteredMembers", typeof(IEnumerable<Member>), typeof(MemberSelection));


        #endregion

        public MemberSelection()
        {
            FilteredMembers = new ObservableCollection<Member>();
            InitializeComponent();
            _presenter = new MemberSelectionPresenter(this);
            _presenter.LoadMemberTypes();
        }

        public void LoadMembers()
        {
            _presenter.LoadMembers();
            FilteredMembers = Members;
            if (SelectedMember == null)
                MemberSelectionGrid.SelectedIndex = 0;
        }

        private void MemberGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = (DataGrid) sender;
            if (!(dg.SelectedItem is Member selectedMemberRec)) return;
            SelectedMember = (Member)dg.SelectedItem;
            DuesPaidControl.LoadDuesPaidRecords(selectedMemberRec.MemberId);
            OfficesHeldControl.LoadOfficeRecords(selectedMemberRec.MemberId);
            MemberInfoControl.ResetControl(selectedMemberRec.MemberId);
        }


        private void ApplyFilterSettings()
        {
            FilteredMembers = Members;
            if (FilterFirstName.Text != "" || FilterLastName.Text != "")
                FilteredMembers = Members.Where(x =>
                    x.FirstName.IndexOf(FilterFirstName.Text, StringComparison.OrdinalIgnoreCase) >= 0 &&
                    x.LastName.IndexOf(FilterLastName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            if (FilterCurrent.IsChecked == true)
                FilteredMembers = FilteredMembers.Where(y => y.IsCurrent);
            if (FilterStatus.SelectedIndex > 0)
                FilteredMembers = FilteredMembers.Where(y => y.MemberTypeId == (int) FilterStatus.SelectedValue);
            if (FilterObligated.Text.Length == 4)
            {
                var obligationYear = Convert.ToInt32(FilterObligated.Text);
                if (obligationYear >= 1915 && obligationYear < 2100)
                    FilteredMembers = FilteredMembers.Where(y =>
                        y.DateObligated != null && y.DateObligated.Value.Year == obligationYear);
            }
        }
        private void FilterTextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterSettings();
        }
        private void FilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilterSettings();
        }
        private void FilterCheckBoxOnClick(object sender, RoutedEventArgs e)
        {
            ApplyFilterSettings();
        }

        private void FilterObligatedTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilterSettings();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void MemberInfoControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (!MemberInfoControl.IsEditing) return;
            if (MessageBox.Show("Do you want to save your changes before leaving?", "Save or Cancel", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                MemberInfoControl.SaveMemberRecord();
            else
                MemberInfoControl.CancelMemberRecord();
        }

        private void MemberInfoControl_OnOnMemberUpdated(object sender, RoutedEventArgs e)
        {
            LoadMembers();
            ApplyFilterSettings();
        }
        private void MemberInfoControl_OnOnMemberDeleted(object sender, RoutedEventArgs e)
        {
            SelectedMember = null;
            LoadMembers();
            ApplyFilterSettings();
        }

        private void ClearSelectionOnClick(object sender, RoutedEventArgs e)
        {
            FilterCurrent.IsChecked = true;
            FilterLastName.Text = "";
            FilterFirstName.Text = "";
            FilterStatus.SelectedIndex = 0;
            FilterObligated.Text = "";
            ApplyFilterSettings();
        }

    }
}
