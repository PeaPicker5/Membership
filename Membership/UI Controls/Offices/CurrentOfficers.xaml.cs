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

        public IEnumerable<Officer> Officers { get; set; }

        private void OfficeHeldGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void  LoadOfficers()
        {

        }

        public IEnumerable<Officer> OfficerRecs { get; set; }
    }
}
