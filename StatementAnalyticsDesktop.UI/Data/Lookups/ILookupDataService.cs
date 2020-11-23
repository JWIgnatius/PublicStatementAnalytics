using System.Collections.Generic;
using System.Threading.Tasks;
using StatementAnalytics;

namespace StatementAnalyticsDesktop.UI.Data.Lookups
{
    public interface ILookupDataService
    {
        Task<IEnumerable<LookupItem>> GetStatementLookupAsync();
    }
}