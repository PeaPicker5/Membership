using System;
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
using CheckBox = System.Windows.Controls.CheckBox;

namespace Membership.UI_Controls.LOSAP
{

    public sealed partial class LosapCredit : ILosapRecordView, INotifyPropertyChanged
    {

        private readonly LosapRecordPresenter _presenter;
        private bool _showActiveOnly;

        #region Dependency Properties
        private ICollection<LosapRecord> AllLosapRecs
        {
            get { return (ICollection<LosapRecord>)GetValue(AllLosapRecsProperty); }
            set { SetValue(AllLosapRecsProperty, value); }
        }
        public static readonly DependencyProperty AllLosapRecsProperty =
            DependencyProperty.Register("AllLosapRecs", typeof(ICollection<LosapRecord>),
                typeof(LosapCredit));

        public ICollection<LosapRecord> LosapRecs
        {
            get => (ICollection<LosapRecord>)GetValue(LosapRecsProperty);
            set
            {
                SetValue(LosapRecsProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty LosapRecsProperty =
            DependencyProperty.Register("LosapRecs", typeof(ICollection<LosapRecord>),
                typeof(LosapCredit));

        public int RunningCount
        {
            get => (int)GetValue(RunningCountProperty);
            set { SetValue(RunningCountProperty, value); OnPropertyChanged();}
        }
        public static readonly DependencyProperty RunningCountProperty =
            DependencyProperty.Register("RunningCount", typeof(int),
                typeof(LosapCredit));

        public float RunningAmount  
        {
            get => (float)GetValue(RunningAmountProperty);
            set { SetValue(RunningAmountProperty, value); OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty RunningAmountProperty =
            DependencyProperty.Register("RunningAmount", typeof(float),
                typeof(LosapCredit));


        public LosapRecord SelectedLosapRecord
        {
            get { return (LosapRecord)GetValue(SelectedLosapRecordProperty); }
            set { SetValue(SelectedLosapRecordProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty SelectedLosapRecordProperty =
            DependencyProperty.Register("SelectedLosapRecord", typeof(LosapRecord),
                typeof(LosapCredit));


        #endregion

        public LosapCredit()
        {
            LosapRecs = new ObservableCollection<LosapRecord>();
            InitializeComponent();
            _presenter = new LosapRecordPresenter(this);
        }

        public void LoadMembers()
        {
            _presenter.GetCurrentMembers();
            AllLosapRecs = LosapRecs;
        }

        private void RecordLosapButtonOnClick(object sender, RoutedEventArgs e)
        {
            _presenter.InsertLosapRecords();
            LoadMembers();
            UpdateLosapRecsGrid();
        }

        private void DisplayActiveMembersOnly(object sender, RoutedEventArgs e)
        {
            _showActiveOnly = ((CheckBox)sender).IsChecked==true;

            UpdateLosapRecsGrid();
        }

        private void AssignmentGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var gridRow = (DataGrid) sender;
            var selectedRow = gridRow?.SelectedItem; 
            if (selectedRow == null) return;
            if (((LosapRecord) selectedRow).Year > 0) return;

            var selectedMemberId = ((LosapRecord)selectedRow).MemberRec.MemberId;
            TogglePaymentRecord(selectedMemberId);
        }

        private void TogglePaymentRecord(Guid selectedMemberId)
        {
            var losapRec = LosapRecs.FirstOrDefault(r => r.MemberRec.MemberId == selectedMemberId);
            if (losapRec == null) return;
            losapRec.HasCredit = !losapRec.HasCredit;
            UpdateLosapRecsGrid();
        }
        private void UpdateLosapRecsGrid()
        {
            RunningCount = LosapRecs.Count(c => c.HasCredit && c.Year == 0);
            LosapRecs = _showActiveOnly
                ? AllLosapRecs.Where(y => y.Year == 0).ToList()
                : AllLosapRecs.ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
