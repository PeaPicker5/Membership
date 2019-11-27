using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Membership.Core.DataModels;
using Membership.Core.Presenters;

namespace Membership.UI_Controls.Offices
{
    public partial class CurrentOfficers : IOfficeHeldView
    {


        public CurrentOfficers()
        {
            InitializeComponent();
        }

        public IEnumerable<OfficeHeld> Officers { get; set; }

        private void OfficeHeldGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void  LoadOfficers()
        {

        }

        public IEnumerable<OfficeHeld> OfficerRecs { get; set; }
    }
}
