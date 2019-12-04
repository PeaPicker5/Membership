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

namespace Membership.UI_Controls.Offices
{

    public sealed partial class OfficerRecords : IOfficerRecordsView, INotifyPropertyChanged
    {

        private readonly OfficerRecordsPresenter _presenter;

        public IEnumerable<Officer> OfficerRecs
        {
            get { return (IEnumerable<Officer>)GetValue(OfficerRecsProperty); }
            set { SetValue(OfficerRecsProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty OfficerRecsProperty =
            DependencyProperty.Register("OfficerRecs",
                typeof(IEnumerable<Officer>),typeof(OfficerRecords));


        #region Dependency Properties

        public bool ShowingByYear
        {
            get { return (bool)GetValue(ShowingByYearProperty); }
            set { SetValue(ShowingByYearProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty ShowingByYearProperty =
            DependencyProperty.Register("ShowingByYear", typeof(bool),
                typeof(OfficerRecords));

        public IEnumerable<int> YearsOnFile
        {
            get => (IEnumerable<int>)GetValue(YearsOnFileProperty);
            private set
            {
                SetValue(YearsOnFileProperty, value); OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty YearsOnFileProperty =
            DependencyProperty.Register("YearsOnFile", typeof(IEnumerable<int>),
                typeof(OfficerRecords));

        public IEnumerable<Office> OfficesOnFile
        {
            get { return (IEnumerable<Office>)GetValue(OfficesOnFileProperty); }
            set { SetValue(OfficesOnFileProperty, value); }
        }
        public static readonly DependencyProperty OfficesOnFileProperty =
            DependencyProperty.Register("OfficesOnFile", typeof(IEnumerable<Office>),
                typeof(OfficerRecords));


        #endregion

        public OfficerRecords()
        {
            YearsOnFile = new ObservableCollection<int>();
            OfficesOnFile = new ObservableCollection<Office>();
            InitializeComponent();
            _presenter = new OfficerRecordsPresenter(this);
        }

        public void Load()
        {
            ShowingByYear = true;
            YearsOnFile = _presenter.LoadYearsOnFile();
            if (YearSelectionCombo.Items.Count > 0)
            {
                YearSelectionCombo.SelectedIndex = 0;
                OfficeFromCombo.SelectedIndex = 0;
                OfficeBackToCombo.SelectedIndex = YearsOnFile.Count() - 1;
            }
            OfficesOnFile = _presenter.LoadOfficesOnFile();
            if (OfficeSelectionCombo.Items.Count > 0)
                OfficeSelectionCombo.SelectedIndex = 0;
        }
        private void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OfficersByYearControl.Load(Convert.ToInt32(YearSelectionCombo.SelectedValue));
        }

        private void OfficeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadOfficersForaTitle();
        }

        private void ReloadOfficersForaTitle()
        {
            var officeId = Convert.ToInt32(OfficeSelectionCombo.SelectedValue);
            var fromYear = Convert.ToInt32(OfficeFromCombo.SelectedValue);
            var backToYear = Convert.ToInt32(OfficeBackToCombo.SelectedValue);
            _presenter.LoadOfficersForaTitle(officeId, fromYear, backToYear);
        }

        #region Button Click Events
        private void RecordsByYearOnClick(object sender, RoutedEventArgs e)
        {
            ShowingByYear = true;
        }
        private void RecordsByOfficeOnClick(object sender, RoutedEventArgs e)
        {
            ShowingByYear = false;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OfficeBackToYearChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShowingByYear) return;
            if (OfficeBackToCombo.SelectedIndex < OfficeFromCombo.SelectedIndex)
                OfficeBackToCombo.SelectedIndex = OfficeFromCombo.SelectedIndex;
            ReloadOfficersForaTitle();
        }

        private void OfficeFromYearChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShowingByYear) return;
            if (OfficeFromCombo.SelectedIndex > OfficeBackToCombo.SelectedIndex)
                OfficeFromCombo.SelectedIndex = OfficeBackToCombo.SelectedIndex;
            ReloadOfficersForaTitle();
        }
    }
}
