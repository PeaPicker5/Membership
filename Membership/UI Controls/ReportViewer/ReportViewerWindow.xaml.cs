using System;
using System.Windows;

namespace Membership.UI_Controls.ReportViewer
{
    public partial class ReportViewerWindow 
    {
        public ReportViewerWindow()
        {
            InitializeComponent();
            ReportViewer.Load += ReportViewer_Load;
        }
        private bool _isReportViewerLoaded;

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                var reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
                var dataset = new MembershipDataSet();

                dataset.BeginInit();

                reportDataSource1.Name = "CurrentDues"; //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dataset.VIEW_CurrentDues;
                ReportViewer.LocalReport.DataSources.Add(reportDataSource1);
                ReportViewer.LocalReport.ReportEmbeddedResource = "DuesWarning.rdlc";

                dataset.EndInit();

                //fill data into dataSet
                var salesOrderDetailTableAdapter = new MembershipDataSetTableAdapters.VIEW_CurrentDuesTableAdapter();
                salesOrderDetailTableAdapter.ClearBeforeFill = true;
                salesOrderDetailTableAdapter.Fill(dataset.VIEW_CurrentDues);

                ReportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }
    }
}
