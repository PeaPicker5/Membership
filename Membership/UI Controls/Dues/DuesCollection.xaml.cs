using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Membership.Core.DataModels;
using Membership.Core.Presenters;
using Membership.Properties;
using CheckBox = System.Windows.Controls.CheckBox;

namespace Membership.UI_Controls.Dues
{

    public sealed partial class DuesCollection : IDuesRecordView, INotifyPropertyChanged
    {

        private readonly DuesPresenter _presenter;
        private bool _showUnpaidOnly;

        #region Dependency Properties



        private ICollection<DuesRecord> AllDuesRecs
        {
            get { return (ICollection<DuesRecord>)GetValue(AllDuesRecsProperty); }
            set { SetValue(AllDuesRecsProperty, value); }
        }
        public static readonly DependencyProperty AllDuesRecsProperty =
            DependencyProperty.Register("AllDuesRecs", typeof(ICollection<DuesRecord>),
                typeof(DuesCollection));

        public ICollection<DuesRecord> DuesRecs
        {
            get => (ICollection<DuesRecord>)GetValue(DuesRecsProperty);
            set
            {
                SetValue(DuesRecsProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty DuesRecsProperty =
            DependencyProperty.Register("DuesRecs", typeof(ICollection<DuesRecord>),
                typeof(DuesCollection));

        public int RunningCount
        {
            get => (int)GetValue(RunningCountProperty);
            set { SetValue(RunningCountProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty RunningCountProperty =
            DependencyProperty.Register("RunningCount", typeof(int),
                typeof(DuesCollection));

        public float RunningAmount  
        {
            get => (float)GetValue(RunningAmountProperty);
            set { SetValue(RunningAmountProperty, value); OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty RunningAmountProperty =
            DependencyProperty.Register("RunningAmount", typeof(float),
                typeof(DuesCollection));


        public DuesRecord SelectedDuesRecord
        {
            get { return (DuesRecord)GetValue(SelectedDuesRecordProperty); }
            set { SetValue(SelectedDuesRecordProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty SelectedDuesRecordProperty =
            DependencyProperty.Register("SelectedDuesRecord", typeof(DuesRecord),
                typeof(DuesCollection));



        #endregion

        public DuesCollection()
        {
            DuesRecs = new ObservableCollection<DuesRecord>();
            InitializeComponent();
            _presenter = new DuesPresenter(this);
        }

        public void LoadMembers()
        {
            _presenter.MembersCurrentlyOweDues();
            AllDuesRecs = DuesRecs;
        }

        private void RecordDuesButtonOnClick(object sender, RoutedEventArgs e)
        {
            _presenter.UpdateDuesRecords();
            LoadMembers();
            UpdateDuesRecsGrid();
        }

        private void DisplayUnpaidMembersOnly(object sender, RoutedEventArgs e)
        {
            _showUnpaidOnly = ((CheckBox)sender).IsChecked==true;

            UpdateDuesRecsGrid();
        }

        private void CollectionGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var gridRow = (DataGrid) sender; 
            if (gridRow == null) return;
            var selectedRow = gridRow.SelectedItem; 
            if (selectedRow == null) return;
            if (((DuesRecord) selectedRow).Year > 0) return;

            var selectedMemberId = ((DuesRecord)selectedRow).MemberRec.MemberId;
            TogglePaymentRecord(selectedMemberId);
        }

        private void IsPaidBox_OnUnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                e.Handled = true;
                var cBox = (CheckBox) sender;
                cBox.Checked -= IsPaidBox_OnChecked;

                var yearPaid = Convert.ToInt32(cBox.Tag);
                if (yearPaid > 0)
                {
                    cBox.Checked -= IsPaidBox_OnChecked;
                    cBox.IsChecked = true;
                    cBox.Checked += IsPaidBox_OnChecked;
                    return;
                }
                UpdateDuesRecsGrid();
//                cBox.Checked += IsPaidBox_OnChecked;
            }
            catch (Exception ex) { }
        }

        private void IsPaidBox_OnChecked(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
         //   UpdateDuesRecsGrid();
        }

        private void TogglePaymentRecord(Guid selectedMemberId)
        {
            var duesRec = DuesRecs.FirstOrDefault(r => r.MemberRec.MemberId == selectedMemberId);
            if (duesRec == null) return;
            duesRec.IsPaid = !duesRec.IsPaid;
            UpdateDuesRecsGrid();
        }
        private void UpdateDuesRecsGrid()
        {
            
            RunningCount = DuesRecs.Count(c => c.IsPaid && c.Year == 0);
            RunningAmount = DuesRecs.Where(c => c.IsPaid && c.Year == 0).Sum(rec => rec.Amount);
            DuesRecs = _showUnpaidOnly
                ? AllDuesRecs.Where(y => y.Year == 0).ToList()
                : AllDuesRecs.ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
