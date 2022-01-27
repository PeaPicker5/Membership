using System.Data;
using Microsoft.Reporting.WinForms;

namespace Membership.Core.Repositories
{
    public interface IReportDataRepository
    {
        DataTable GetRecordSet(string query);

    }
}
