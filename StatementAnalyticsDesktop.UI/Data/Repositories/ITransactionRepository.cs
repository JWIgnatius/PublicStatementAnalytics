using System.Collections.Generic;
using System.Threading.Tasks;
using StatementAnalytics.Transactions;

namespace StatementAnalyticsDesktop.UI.Data.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByDetailsAsync(string details = null, string startDate = null, string endDate = null,
            double? startAmount = double.MinValue, double? endAmount = double.MaxValue);
    }
}