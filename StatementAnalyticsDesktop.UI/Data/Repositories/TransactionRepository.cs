using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StatementAnalytics.Transactions;
using StatementAnalyticsDesktop.DataAccess;

namespace StatementAnalyticsDesktop.UI.Data.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction, StatementAnalyticsDbContext>, ITransactionRepository
    {
        public TransactionRepository(StatementAnalyticsDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Transaction>> GetByDetailsAsync(string details = null, string startDate = null, string endDate = null,
            double? startAmount = double.MinValue, double? endAmount = double.MaxValue)
        {
            details ??= string.Empty;
            startAmount ??= double.MinValue;
            endAmount ??= double.MaxValue;
            var startDateTime = DateTime.MinValue;
            var endDateTime = DateTime.MaxValue;

            if (!string.IsNullOrEmpty(startDate))
                DateTime.TryParse(startDate, out startDateTime);

            if (!string.IsNullOrEmpty(endDate))
                DateTime.TryParse(endDate, out endDateTime);

            return await Context.Transactions.Where(
                t => t.Details.Contains(details) && t.TransactionDate >= startDateTime && t.TransactionDate <= endDateTime
                && t.Amount >= startAmount && t.Amount <= endAmount
            ).OrderBy(t => t.TransactionDate).ToListAsync();
        }
    }
}

