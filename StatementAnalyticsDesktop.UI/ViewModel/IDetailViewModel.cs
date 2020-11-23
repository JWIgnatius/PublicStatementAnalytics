using System.Threading.Tasks;

namespace StatementAnalyticsDesktop.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? statementId);
        
        bool HasChanges { get; }
    }
}