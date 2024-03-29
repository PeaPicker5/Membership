﻿using System;
using System.Collections.Generic;
using System.Windows;
using Membership.Core.DataModels;
using Membership.Core.Officers.DataModels;
using Membership.Core.Officers.Presenters;

namespace Membership.UI_Controls.Offices
{
    public partial class OfficesHeld : IOfficeHistoryView
    {
        public IEnumerable<Officer> OfficerRecs
        {
            get { return (IEnumerable<Officer>)GetValue(OfficerRecsProperty); }
            set { SetValue(OfficerRecsProperty, value); }
        }
        public static readonly DependencyProperty OfficerRecsProperty =
            DependencyProperty.Register("OfficerRecs", typeof(IEnumerable<Officer>), 
                typeof(OfficesHeld));


        private readonly OfficerPresenter _presenter;

        public OfficesHeld()
        {
            InitializeComponent();
            _presenter = new OfficerPresenter(this);
        }

        public void LoadOfficeRecords(Guid memberId)
        {
            _presenter.GetByMemberId(memberId);
        }

    }
}
