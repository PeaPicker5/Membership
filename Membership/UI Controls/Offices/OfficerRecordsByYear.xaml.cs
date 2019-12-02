using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.Offices
{

    public sealed partial class OfficerRecordsByYear : IOfficerRecordsByYearView, INotifyPropertyChanged
    {
        private readonly OfficerRecordsByYearPresenter _presenter;

        #region Dependency Properties for officer types
        public IEnumerable<Officer> Commissioners
        {
            get { return (IEnumerable<Officer>)GetValue(CommissionersProperty); }
            set { SetValue(CommissionersProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty CommissionersProperty =
            DependencyProperty.Register("Commissioners", 
                typeof(IEnumerable<Officer>), typeof(OfficerRecordsByYear));

        public IEnumerable<Officer> LineOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(LineOfficersProperty); }
            set { SetValue(LineOfficersProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty LineOfficersProperty =
            DependencyProperty.Register("LineOfficers",
                typeof(IEnumerable<Officer>), typeof(OfficerRecordsByYear));

        public IEnumerable<Officer> OtherDistrictOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(OtherDistrictOfficersProperty); }
            set { SetValue(OtherDistrictOfficersProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty OtherDistrictOfficersProperty =
            DependencyProperty.Register("OtherDistrictOfficers",
                typeof(IEnumerable<Officer>), typeof(OfficerRecordsByYear));

        public IEnumerable<Officer> BoardOfDirectors
        {
            get { return (IEnumerable<Officer>)GetValue(BoardOfDirectorsProperty); }
            set { SetValue(BoardOfDirectorsProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty BoardOfDirectorsProperty =
            DependencyProperty.Register("BoardOfDirectors",
                typeof(IEnumerable<Officer>), typeof(OfficerRecordsByYear));

        public IEnumerable<Officer> TableOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(TableOfficersProperty); }
            set { SetValue(TableOfficersProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty TableOfficersProperty =
            DependencyProperty.Register("TableOfficers",
                typeof(IEnumerable<Officer>), typeof(OfficerRecordsByYear));

        public IEnumerable<Officer> OtherAssocOfficers
        {
            get { return (IEnumerable<Officer>)GetValue(OtherAssocOfficersProperty); }
            set { SetValue(OtherAssocOfficersProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty OtherAssocOfficersProperty =
            DependencyProperty.Register("OtherAssocOfficers",
                typeof(IEnumerable<Officer>), typeof(OfficerRecordsByYear));
        #endregion

        public OfficerRecordsByYear()
        {
            InitializeComponent();
            _presenter = new OfficerRecordsByYearPresenter(this);
        }

        public void Load(int year)
        {
            if (year == 0) return;
            _presenter.LoadOfficersForaYear(year);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
