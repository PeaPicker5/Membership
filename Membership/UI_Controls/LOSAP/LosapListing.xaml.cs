using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Membership.Core.LOSAP.DataModels;
using Membership.Core.LOSAP.Presenters;
using Membership.Properties;
using ILosapListingView = Membership.Core.LOSAP.Presenters.ILosapListingView;

namespace Membership.UI_Controls.LOSAP
{

    public partial class LosapListing : ILosapListingView, INotifyPropertyChanged
    {
        private readonly LosapListingPresenter _presenter;

        public ICollection<LosapRecord> LosapRecs
        {
            get => (ICollection<LosapRecord>)GetValue(LosapRecsProperty);
            set
            {
                SetValue(LosapRecsProperty, value); OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty LosapRecsProperty =
            DependencyProperty.Register("LosapRecs", typeof(ICollection<LosapRecord>), typeof(LosapListing));



        public LosapListing()
        {
            LosapRecs = new ObservableCollection<LosapRecord>();
            InitializeComponent();
            _presenter = new LosapListingPresenter(this);
            Loaded += (sender, args) =>
            {
                LoadMembers();
            };
        }

        public void LoadMembers()
        {
            _presenter.LoadMemberRecords();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
