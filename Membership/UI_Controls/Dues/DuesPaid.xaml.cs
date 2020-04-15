using System;
using System.Collections.Generic;
using System.Windows;
using Membership.Core.DataModels;
using Membership.Core.Presenters;

namespace Membership.UI_Controls.Dues
{
    public partial class DuesPaid : IDuesRecordView
    {
        public ICollection<DuesRecord> DuesRecs
        {
            get { return (ICollection<DuesRecord>)GetValue(OfficerRecsProperty); }
            set { SetValue(OfficerRecsProperty, value); }
        }
        public static readonly DependencyProperty OfficerRecsProperty =
            DependencyProperty.Register("DuesRecs", typeof(ICollection<DuesRecord>), 
                typeof(DuesPaid));

        private readonly DuesPresenter _presenter;

        public DuesPaid()
        {
            InitializeComponent();
            _presenter = new DuesPresenter(this);
        }

        public void LoadDuesPaidRecords(Guid memberId)
        {
            _presenter.GetByMemberId(memberId);
        }

    }
}
