using System.Data;

namespace Membership.Core.Reports.Repositories
{
    public interface IReportDataRepository
    {
        DataTable GetRecordSet(string query);

    }
}
