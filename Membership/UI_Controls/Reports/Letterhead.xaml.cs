using Membership.Core.Reports.Presenters;
using Membership.UI_Controls.ReportViewer;

namespace Membership.UI_Controls.Reports
{
    public partial class Letterhead : ReportUserControlBase
    {
        public Letterhead()
        {
            InitializeComponent();
            Loaded += (sender, args) => UpdateTheReport();
        }

        protected override ReportViewerWindow Viewer => ReportControl;
    }
}
