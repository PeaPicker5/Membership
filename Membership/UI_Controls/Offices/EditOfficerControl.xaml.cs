using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.DataModels;
using Membership.Core.Members.DataModels;
using Membership.Core.Officers.DataModels;
using Membership.Core.Officers.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.Offices
{

    public sealed partial class EditOfficerControl : IEditOfficerView, INotifyPropertyChanged
    {
        private readonly EditOfficerPresenter _presenter;

        public int YearOfTerm
        {
            get { return (int)GetValue(YearOfTermProperty); }
            set { SetValue(YearOfTermProperty, value); }
        }
        public static readonly DependencyProperty YearOfTermProperty =
            DependencyProperty.Register("YearOfTerm", typeof(int),
                typeof(EditOfficerControl));


        public static IEnumerable<string> Months1_12 => DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;

        public Officer currentOfficerRec => new Officer(
                                    (Member)MemberCombo.SelectedItem,
                                    (Office)OfficeCombo.SelectedItem, YearOfTerm,
                             DateTime.Parse($"{FromMonth.SelectedIndex + 1}-1-{YearOfTerm}"),
                               DateTime.Parse($"{ToMonth.SelectedIndex + 1}-1-{YearOfTerm}"));


        #region Dependency Properties

        public IEnumerable<Office> OfficeRecs
        {
            get { return (IEnumerable<Office>)GetValue(OfficeRecsProperty); }
            set { SetValue(OfficeRecsProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty OfficeRecsProperty =
            DependencyProperty.Register("OfficeRecs", typeof(IEnumerable<Office>),typeof(EditOfficerControl));

        public IEnumerable<Member> MemberRecs
        {
            get { return (IEnumerable<Member>)GetValue(MemberRecsProperty); }
            set { SetValue(MemberRecsProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty MemberRecsProperty =
            DependencyProperty.Register("MemberRecs", typeof(IEnumerable<Member>), typeof(EditOfficerControl));
        #endregion


        public EditOfficerControl()
        {
            MemberRecs = new ObservableCollection<Member>();
            OfficeRecs = new ObservableCollection<Office>();

            InitializeComponent();
            _presenter = new EditOfficerPresenter(this);
            LoadOfficeRecords();
            LoadMemberRecords();
        }

        public void ClearControlValues()
        {
            OfficeCombo.SelectedIndex = -1;
            MemberCombo.SelectedIndex = -1;
            FromMonth.SelectedIndex = -1;
            ToMonth.SelectedIndex = -1;
            SetCurrentValue(IsEditProtectedProperty, true);
            SetCurrentValue(IsDeleteProtectedProperty, true);
        }
        public void SetControlValues(Officer currentOfficer)
        {
            if (currentOfficer == null) return;
            SetCurrentValue(IsEditProtectedProperty, false);
            SetCurrentValue(IsDeleteProtectedProperty, false);

            OfficeCombo.SelectedValue = currentOfficer.OfficeId;
            if (OfficeCombo.SelectedIndex == -1)
            {
                FilterCurrentOffices.IsChecked = false;
                LoadOfficeRecords();
                OfficeCombo.SelectedValue = currentOfficer.OfficeId;
            }
            MemberCombo.SelectedValue = currentOfficer.MemberId;
            if (MemberCombo.SelectedIndex == -1)
            {
                FilterCurrentMembers.IsChecked = false;
                LoadMemberRecords();
                MemberCombo.SelectedValue = currentOfficer.MemberId;
            }
            FromMonth.SelectedIndex = currentOfficer.FromMonth-1;
            ToMonth.SelectedIndex = currentOfficer.ToMonth-1;
        }

        private void CurrentOfficesOnlyCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            LoadOfficeRecords();
        }
        private void CurrentMembersOnlyCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            LoadMemberRecords();
        }

        private void LoadOfficeRecords()
        {
            _presenter.GetAllOfficeRecords(FilterCurrentOffices.IsChecked == true);
        }
        private void LoadMemberRecords()
        {
            _presenter.GetAllMemberRecords(FilterCurrentMembers.IsChecked == true);
        }


        #region MENU BUTTON Events (Add, Edit, Remove, Save, Cancel)
        private void OfficerControl_OnAddItem(object sender, RoutedEventArgs e)
        {
            IsAdding = true;
            IsEditing = true;
            OfficeCombo.SelectedIndex = 0;
            MemberCombo.SelectedIndex = -1;
            FromMonth.SelectedIndex = 0;
            ToMonth.SelectedIndex = 11;
        }
        private void OfficerControl_OnEditItem(object sender, RoutedEventArgs e)
        {
            IsEditing = true;
        }
        private void OfficerControl_OnRemoveItem(object sender, RoutedEventArgs e)
        {
            _presenter.DeleteOfficerRecord(currentOfficerRec);
            ClearControlValues();
            UpdateListOfOfficers();
        }
        private void OfficerControl_OnSaveItem(object sender, RoutedEventArgs e)
        {
            if (IsAdding)
                _presenter.InsertOfficerRecord(currentOfficerRec);
            else if (IsEditing)
                _presenter.UpdateOfficerRecord(currentOfficerRec);
            UpdateListOfOfficers();
            IsAdding = false;
            IsEditing = false;
        }
        private void OfficerControl_OnCancelItem(object sender, RoutedEventArgs e)
        {
            ClearControlValues();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OfficeCombo_OnDropDownClosed(object sender, EventArgs e)
        {
            MemberCombo.SelectedIndex = 0;
        }



        #region Events to send back to the parent control


        public delegate void NotifyParameterDelegate();
        public event NotifyParameterDelegate RecordUpdated;
        private void UpdateListOfOfficers()
        {
            //bubble the event up to the parent
            RecordUpdated?.Invoke();
        }

        #endregion






    }
}
