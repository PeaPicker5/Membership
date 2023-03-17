using System;
using System.Collections.Generic;
using Membership.Core.Members.DataModels;
using Membership.Core.Members.Presenters;
using Microsoft.Reporting.WinForms;

namespace Membership.UI_Controls.Reports
{

    public partial class MemberDetailsReport : IMemberView
    {

        private readonly MemberInfoPresenter _presenter;

        public MemberDetailsReport()
        {
            InitializeComponent();
            _presenter = new MemberInfoPresenter(this);
            UpdateTheReport();
        }

        private void UpdateTheReport()
        {
            ReportControl.ReportName = "MemberDetails"; 
            ReportControl.ReportParams = new List<ReportParameter>();
            ReportControl.LoadReport();
        }

        public ICollection<Tuple<int, string>> MemberTypeLookups { get; set; }
        public ICollection<Tuple<Guid, string>> SponsorLookups { get; set; }
        public ICollection<MemberRemoval> RemovalCodes { get; set; }
    }
}
