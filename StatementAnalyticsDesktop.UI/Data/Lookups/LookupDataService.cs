using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StatementAnalytics;
using StatementAnalytics.Helpers;
using StatementAnalyticsDesktop.DataAccess;

namespace StatementAnalyticsDesktop.UI.Data.Lookups
{
    public class LookupDataService : ILookupDataService
    {
        private readonly Func<StatementAnalyticsDbContext> _contextCreator;

        public LookupDataService(Func<StatementAnalyticsDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetStatementLookupAsync()
        {
            using var ctx = _contextCreator();
            return await ctx.Statements.Select(s => new LookupItem
            {
                Id = s.Id,
                DisplayMember = Helper.GetIdentifier(s),
                StatementDate = s.StatementDate
            }).ToListAsync();
        }
    }
}
