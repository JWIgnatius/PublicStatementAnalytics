using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StatementAnalytics.Statements;
using StatementAnalytics.Transactions;
using StatementAnalyticsDesktop.DataAccess;

namespace StatementAnalyticsDesktop.UI.Data.Repositories
{
    public class StatementRepository : GenericRepository<Statement, StatementAnalyticsDbContext>, IStatementRepository
    {
        public StatementRepository(StatementAnalyticsDbContext context) : base(context)
        {
        }

        public override async Task<Statement> GetByIdAsync(int statementId)
        {
            return await Context.Statements.Include(s => s.Transactions).SingleAsync(s => s.Id == statementId);
        }

        public void RemoveTransaction(Transaction model)
        {
            Context.Transactions.Remove(model);
        }
    }
}