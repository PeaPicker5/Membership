using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.LOSAP.Presenters;
using Membership.Properties;

namespace Membership.UI_Controls.LOSAP
{
    /// <summary>
    /// LOSAP credit entry. Pick a year, double-click members to credit or un-credit them,
    /// then APPLY to save. Rows with unsaved changes are highlighted.
    /// </summary>
    public sealed partial class LosapCredit : ILosapRecordView, INotifyPropertyChanged
    {
        private readonly LosapRecordPresenter _presenter;
        private int _loadedYear;

        #region Dependency Properties

        public ICollection<LosapRecord> LosapRecs
        {
            get => (ICollection<LosapRecord>)GetValue(LosapRecsProperty);
            set
            {
                SetValue(LosapRecsProperty, value);
                OnPropertyChanged();
                UpdateCounts();
            }
        }
        public static readonly DependencyProperty LosapRecsProperty =
            DependencyProperty.Register("LosapRecs", typeof(ICollection<LosapRecord>),
                typeof(LosapCredit));

        public IList<int> SelectableYears
        {
            get => (IList<int>)GetValue(SelectableYearsProperty);
            set => SetValue(SelectableYearsProperty, value);
        }
        public static readonly DependencyProperty SelectableYearsProperty =
            DependencyProperty.Register("SelectableYears", typeof(IList<int>), typeof(LosapCredit));

        public int CreditedCount
        {
            get => (int)GetValue(CreditedCountProperty);
            set => SetValue(CreditedCountProperty, value);
        }
        public static readonly DependencyProperty CreditedCountProperty =
            DependencyProperty.Register("CreditedCount", typeof(int), typeof(LosapCredit));

        public int PendingCount
        {
            get => (int)GetValue(PendingCountProperty);
            set
            {
                SetValue(PendingCountProperty, value);
                OnPropertyChanged(nameof(HasPendingChanges));
            }
        }
        public static readonly DependencyProperty PendingCountProperty =
            DependencyProperty.Register("PendingCount", typeof(int), typeof(LosapCredit));

        public bool HasPendingChanges => PendingCount > 0;

        #endregion

        public LosapCredit()
        {
            LosapRecs = new ObservableCollection<LosapRecord>();
            InitializeComponent();
            _presenter = new LosapRecordPresenter(this);
        }

        /// <summary>Called by MainWindow when the screen opens: fill the years and load last year.</summary>
        public void LoadMembers()
        {
            SelectableYears = _presenter.GetSelectableYears();
            YearCombo.SelectedItem = LosapRecordPresenter.DefaultYear;   // triggers LoadSelectedYear
        }

        private void YearCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(YearCombo.SelectedItem is int year) || year == _loadedYear) return;

            if (HasPendingChanges &&
                MessageBox.Show($"You have {PendingCount} unsaved LOSAP change(s) for {_loadedYear}. Discard them?",
                    "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            {
                YearCombo.SelectedItem = _loadedYear;   // stay on the year being edited
                return;
            }

            LoadSelectedYear(year);
        }

        private void LoadSelectedYear(int year)
        {
            _loadedYear = year;
            _presenter.LoadYear(year);
        }

        private void RecordLosapButtonOnClick(object sender, RoutedEventArgs e)
        {
            _presenter.SaveChanges(_loadedYear);
            LoadSelectedYear(_loadedYear);   // reload so totals reflect what was saved
        }

        private void AssignmentGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(((DataGrid)sender).SelectedItem is LosapRecord losapRec)) return;
            losapRec.HasCredit = !losapRec.HasCredit;
            UpdateCounts();
        }

        private void UpdateCounts()
        {
            CreditedCount = LosapRecs?.Count(r => r.HasCredit) ?? 0;
            PendingCount = LosapRecs?.Count(r => r.IsChanged) ?? 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
