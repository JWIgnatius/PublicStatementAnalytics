using System.Threading.Tasks;

namespace StatementAnalyticsDesktop.UI.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int statementId);
        Task SaveAsync();
        bool HasChanges();
        void Add(T statement);
        void Remove(T model);
    }
}