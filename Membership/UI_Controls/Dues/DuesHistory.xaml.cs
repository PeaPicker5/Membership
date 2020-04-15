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

namespace Membership.UI_Controls.Dues
{

    public partial class DuesHistory : IDuesRecordView, INotifyPropertyChanged
    {

        public IEnumerable<int> YearsOnFile
        {
            get => (IEnumerable<int>)GetValue(YearsOnFileProperty);
            private set
            {
                SetValue(YearsOnFileProperty, value); OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty YearsOnFileProperty =
            DependencyProperty.Register("YearsOnFile", typeof(IEnumerable<int>), typeof(DuesHistory));

        public ICollection<DuesRecord> DuesRecs
        {
            get => (ICollection<DuesRecord>)GetValue(DuesRecsProperty);
            set
            {
                SetValue(DuesRecsProperty, value); OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty DuesRecsProperty =
            DependencyProperty.Register("DuesRecs", typeof(ICollection<DuesRecord>), typeof(DuesHistory));


        private readonly DuesPresenter _presenter;

        public DuesHistory()
        {
            DuesRecs = new ObservableCollection<DuesRecord>();
            YearsOnFile = new ObservableCollection<int>();
            InitializeComponent();
            _presenter = new DuesPresenter(this);
        }

        public void LoadMembers()
        {
            YearsOnFile = _presenter.LoadYearsOnFile();
            if (YearsOnFile.Any()) YearSelectionCombo.SelectedIndex = 0;
        }
        private void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DuesRecs = _presenter.GetByYear((int)YearSelectionCombo.SelectedItem)
                                        .OrderBy(name => name.MemberRec.LastName)
                                        .ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
