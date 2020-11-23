using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using StatementAnalyticsDesktop.UI.Data.Lookups;
using StatementAnalyticsDesktop.UI.Event;

namespace StatementAnalyticsDesktop.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly ILookupDataService _lookupRepository;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(ILookupDataService lookupDataService, IEventAggregator eventAggregator)
        {
            _lookupRepository = lookupDataService;
            _eventAggregator = eventAggregator;
            Statements = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(StatementDetailViewModel):
                    var statement = Statements.SingleOrDefault(s => s.Id == args.Id);
                    if (statement != null)
                    {
                        Statements.Remove(statement);
                    }
                    break;
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(StatementDetailViewModel):
                    var lookupItem = Statements.SingleOrDefault(l => l.Id == args.Id);

                    if(lookupItem == null)
                    {
                        Statements.Add(new NavigationItemViewModel(
                            args.Id, args.DisplayMember, nameof(StatementDetailViewModel), _eventAggregator));
                    }
                    else
                    {
                        lookupItem.DisplayMember = args.DisplayMember;
                    }
                    break;
            }
        }

        public async Task LoadAsync()
        {
            var lookups = await _lookupRepository.GetStatementLookupAsync();

            lookups = lookups.OrderBy(l => l.StatementDate);

            Statements.Clear();

            foreach (var item in lookups)
            {
                Statements.Add(new NavigationItemViewModel(
                    item.Id, item.DisplayMember, nameof(StatementDetailViewModel), _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Statements { get; set; }
    }
}
