using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Membership.Common.Controls;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.Offices
{

    public sealed partial class EditOfficerControl : IEditOfficerView, INotifyPropertyChanged
    {
        private const int DistrictOnly = 0;
        private const int AssocOnly = 1;
        private readonly EditOfficerPresenter _presenter;
        private int _selectedOfficeType = 0;


        public string[] Months1_12 => DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;

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


        public ICollection<string> FromDateOptions
        {
            get { return (ICollection<string>)GetValue(FromDateOptionsProperty); }
            set { SetValue(FromDateOptionsProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty FromDateOptionsProperty =
            DependencyProperty.Register("FromDateOptions", typeof(ICollection<string>), typeof(EditOfficerControl));


        public EditOfficerControl()
        {
            MemberRecs = new ObservableCollection<Member>();
            OfficeRecs = new ObservableCollection<Office>();

            InitializeComponent();

            _presenter = new EditOfficerPresenter(this);
            SelectedYear.Value = DateTime.Now.Year;
            FromMonth.ItemsSource = Months1_12;
            ToMonth.ItemsSource = Months1_12;
            LoadOfficeRecords();
        }

        public void Load()
        {
            if (ItemData == null) return;
            if (ItemData.OfficeRec.GroupId < 20)
                DistrictOfficerRadio.IsChecked = true;
            else
                AssocOfficerRadio.IsChecked = true;
            OfficerTypeRadio_Click(null, null);

            OfficeCombo.SelectedValue = ItemData.OfficeRec.OfficeId;
            MemberCombo.SelectedValue = ItemData.MemberRec.MemberId;

            SelectedYear.Value = ItemData.Year;
            //FromDate = officerRec.FromDate;
            //ToDate = officerRec.ToDate;

        }

        private void NewYearSelected(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            var yr = Convert.ToInt32(SelectedYear.Value);
            if (yr >= 1915 && yr < 1961)
            {
                FromMonth.SelectedIndex = 4;
                ToMonth.SelectedIndex = 3;
                //FromDate = DateTime.Parse($"05-01-{yr}");
                //ToDate = DateTime.Parse($"04-30-{yr + 1}");
            }

            if (yr < 1961 || yr >= 2100) return;

            //FromDate = DateTime.Parse($"01-01-{yr}");
            //ToDate = DateTime.Parse($"12-31-{yr}");
            FromMonth.SelectedIndex = 0;
            ToMonth.SelectedIndex = 11;

        }

        private void RefreshFromDates(int selYear)
        {
            FromDateOptions = new List<string>();
            for (var i = 1; i <= 12; i++)
            {
                FromDateOptions.Add($"{i}/{selYear}");
            }

        }

        private void FilterCheckBoxOnClick(object sender, RoutedEventArgs e)
        {
            LoadOfficeRecords();
        }
        private void OfficerTypeRadio_Click(object sender, RoutedEventArgs e)
        {
            _selectedOfficeType = DistrictOfficerRadio.IsChecked == true ? DistrictOnly : AssocOnly;
            LoadOfficeRecords();
        }
        private void LoadOfficeRecords()
        {
            _presenter.LoadOfficesForaType(_selectedOfficeType, FilterCurrent.IsChecked == true);
            if (OfficeRecs.Any())
                OfficeCombo.SelectedIndex = 0;
        }

        private void OfficeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMemberRecords();
        }

        private void LoadMemberRecords()
        {
            if (OfficeCombo.SelectedValue is null) return;
            var officeId = (int)OfficeCombo.SelectedValue;
            if (officeId == 15 || officeId > 20)
                _presenter.LoadCurrentMembers();
            else
                _presenter.LoadActiveMembers();
            if (MemberRecs.Count() > 0)
                MemberCombo.SelectedIndex = 0;
        }



        private void DatePickerOnPreviewTextInput(object sender, TextCompositionEventArgs eventArgs)
        {
            eventArgs.Handled = eventArgs.Text.Any(
                character => !char.IsDigit(character) && character != '/' && character != '-');
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void SelectedYearChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {

        }

        private void FromDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OfficerControlOnAddItem(object sender, RoutedEventArgs e)
        {

        }

        private void OfficerControlOnSaveItem(object sender, RoutedEventArgs e)
        {

        }

        private void OfficerControlOnRemoveItem(object sender, RoutedEventArgs e)
        {

        }

        private void OfficerControlOnEditItem(object sender, RoutedEventArgs e)
        {
            IsEditing = true;
        }

        private void OfficerControlOnCancelItem(object sender, RoutedEventArgs e)
        {

        }

    }
}
