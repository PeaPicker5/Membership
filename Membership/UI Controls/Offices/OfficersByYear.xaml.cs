using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.Offices
{

    public sealed partial class OfficersByYear : IOfficersByYearView, INotifyPropertyChanged
    {
        private readonly OfficersByYearPresenter _presenter;
        public int SelectedYear { get; set; }

        #region Dependency Properties for officer types
        public IEnumerable<Officer> Commissioners
        {
            get { return (IEnumerable<Officer>)GetValue(CommissionersProperty); }
            set { SetValue(CommissionersProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty CommissionersProperty =
            DependencyProperty.Register("Commissioners", 
                typeof(IEnumerable<Officer>), typeof(OfficersByYear));

        public IEnumerable<Officer> LineOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(LineOfficersProperty); }
            set { SetValue(LineOfficersProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty LineOfficersProperty =
            DependencyProperty.Register("LineOfficers",
                typeof(IEnumerable<Officer>), typeof(OfficersByYear));

        public IEnumerable<Officer> OtherDistrictOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(OtherDistrictOfficersProperty); }
            set { SetValue(OtherDistrictOfficersProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty OtherDistrictOfficersProperty =
            DependencyProperty.Register("OtherDistrictOfficers",
                typeof(IEnumerable<Officer>), typeof(OfficersByYear));

        public IEnumerable<Officer> BoardOfDirectors
        {
            get { return (IEnumerable<Officer>)GetValue(BoardOfDirectorsProperty); }
            set { SetValue(BoardOfDirectorsProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty BoardOfDirectorsProperty =
            DependencyProperty.Register("BoardOfDirectors",
                typeof(IEnumerable<Officer>), typeof(OfficersByYear));

        public IEnumerable<Officer> TableOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(TableOfficersProperty); }
            set { SetValue(TableOfficersProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty TableOfficersProperty =
            DependencyProperty.Register("TableOfficers",
                typeof(IEnumerable<Officer>), typeof(OfficersByYear));

        public IEnumerable<Officer> OtherAssocOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(OtherAssocOfficersProperty); }
            set { SetValue(OtherAssocOfficersProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty OtherAssocOfficersProperty =
            DependencyProperty.Register("OtherAssocOfficers",
                typeof(IEnumerable<Officer>), typeof(OfficersByYear));
        #endregion
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
                typeof(OfficersByYear));

        public OfficersByYear()
        {
            InitializeComponent();
            _presenter = new OfficersByYearPresenter(this);
        }

        public void LoadYearsOnFile()
        {
            YearsOnFile = _presenter.LoadYearsOnFile();
            if (YearSelectionCombo.Items.Count > 0)
            {
                YearSelectionCombo.SelectedIndex = 0;
            }
            LoadOfficerRecords();
        }

        private void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedYear = Convert.ToInt32(YearSelectionCombo.SelectedValue);
            if (SelectedYear == 0) return;
            LoadOfficerRecords();
        }
        private void LoadOfficerRecords()
        {
            _presenter.LoadOfficersForaYear(SelectedYear);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
