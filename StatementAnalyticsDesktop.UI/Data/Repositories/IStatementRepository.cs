using StatementAnalytics.Statements;
using StatementAnalytics.Transactions;

namespace StatementAnalyticsDesktop.UI.Data.Repositories
{
    public interface IStatementRepository :IGenericRepository<Statement>
    {
        void RemoveTransaction(Transaction model);
    }
}
