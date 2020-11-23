using Microsoft.EntityFrameworkCore;
using StatementAnalytics.Statements;
using StatementAnalytics.Transactions;

namespace StatementAnalyticsDesktop.DataAccess
{
    public class StatementAnalyticsDbContext : DbContext
    {
        public StatementAnalyticsDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=..\..\..\StatementAnalytics.db");
        }

        public DbSet<Statement> Statements { get; set; }

        public DbSet<Transaction> Transactions { get; set; } 
    }
}
