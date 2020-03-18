using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.Offices
{
    public sealed partial class OfficeHistory : IOfficeHistoryView, INotifyPropertyChanged
    {
        private readonly OfficeHistoryPresenter _presenter;

        #region Dependency Properties
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
                typeof(OfficeHistory));

        public IEnumerable<Office> OfficesOnFile
        {
            get { return (IEnumerable<Office>)GetValue(OfficesOnFileProperty); }
            set { SetValue(OfficesOnFileProperty, value); }
        }
        public static readonly DependencyProperty OfficesOnFileProperty =
            DependencyProperty.Register("OfficesOnFile", typeof(IEnumerable<Office>),
                typeof(OfficeHistory));

        public IEnumerable<Officer> OfficerRecs
        {
            get { return (IEnumerable<Officer>)GetValue(OfficerRecsProperty); }
            set { SetValue(OfficerRecsProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty OfficerRecsProperty =
            DependencyProperty.Register("OfficerRecs", typeof(IEnumerable<Officer>),
                typeof(OfficeHistory));


        public Color AltColor => Color.Red;

        public Color AlternatingColor
        {
            get { return (Color)GetValue(AlternatingColorProperty); }
            set { SetValue(AlternatingColorProperty, value); }
        }
        public static readonly DependencyProperty AlternatingColorProperty =
            DependencyProperty.Register("AlternatingColor", typeof(Color),
                typeof(OfficeHistory));


        #endregion

        public OfficeHistory()
        {
            YearsOnFile = new ObservableCollection<int>();
            OfficesOnFile = new ObservableCollection<Office>();
            InitializeComponent();
            _presenter = new OfficeHistoryPresenter(this);
        }

        public void LoadRecords()
        {
            OfficesOnFile = _presenter.LoadOfficesOnFile();
            if (OfficeSelectionCombo.Items.Count > 0)
                OfficeSelectionCombo.SelectedIndex = 0;

            YearsOnFile = _presenter.LoadYearsOnFile();
            if (OfficeFromCombo.Items.Count > 0)
            {
                OfficeBackToCombo.SelectedIndex = YearsOnFile.Count() - 1;
                OfficeFromCombo.SelectedIndex = 0;
            }
        }

        private void LoadOfficersForaTitle()
        {
            var officeId = Convert.ToInt32(OfficeSelectionCombo.SelectedValue);
            var fromYear = Convert.ToInt32(OfficeFromCombo.SelectedValue);
            var backToYear = Convert.ToInt32(OfficeBackToCombo.SelectedValue);
            if (officeId <= 0 || fromYear <= 0 || backToYear <= 0) return;

            _presenter.LoadOfficersForaTitle(officeId, fromYear, backToYear);
        }

        private void OfficeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadOfficersForaTitle();
        }
        private void OfficeBackToYearChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OfficeBackToCombo.SelectedIndex < OfficeFromCombo.SelectedIndex)
                OfficeBackToCombo.SelectedIndex = OfficeFromCombo.SelectedIndex;
            LoadOfficersForaTitle();
        }
        private void OfficeFromYearChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OfficeFromCombo.SelectedIndex > OfficeBackToCombo.SelectedIndex)
                OfficeFromCombo.SelectedIndex = OfficeBackToCombo.SelectedIndex;
            LoadOfficersForaTitle();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
