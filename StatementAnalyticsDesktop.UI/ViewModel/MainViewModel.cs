using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using StatementAnalyticsDesktop.UI.Data.Repositories;
using StatementAnalyticsDesktop.UI.Event;
using StatementAnalyticsDesktop.UI.View.Services;

namespace StatementAnalyticsDesktop.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IWindowViewModel _currentWindow;
        private IEventAggregator _eventAggregator;

        public MainViewModel(INavigationViewModel navigationViewModel, Func<IStatementDetailViewModel> statementDetailViewModelCreator
            , IEventAggregator eventAggregator, IMessageDialogService messageDialogService, ITransactionRepository dataService)
        {
            _eventAggregator = eventAggregator;

            StatementWindow = new StatementWindowViewModel(navigationViewModel, statementDetailViewModelCreator,
                                            eventAggregator, messageDialogService);

            TransactionSearchWindow = new TransactionSearchWindowViewModel(dataService, eventAggregator);

            ShowTransactionSearchWindowCommand = new DelegateCommand(OnShowTransactionSearchWindowExecute);
            ShowStatementWindowCommand = new DelegateCommand(OnShowStatementWindowExecute);
            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);

            CurrentWindow = StatementWindow;
        }

        public IWindowViewModel CurrentWindow
        {
            get { return _currentWindow; }
            set
            {
                _currentWindow = value;
                OnPropertyChanged();
                _currentWindow.LoadAsync();
            }
        }

        public StatementWindowViewModel StatementWindow;

        public TransactionSearchWindowViewModel TransactionSearchWindow;

        public async Task LoadAsync()
        {
            await CurrentWindow.LoadAsync();
        }

        public ICommand ShowTransactionSearchWindowCommand { get; }

        public ICommand CreateNewDetailCommand { get; }

        public ICommand ShowStatementWindowCommand { get; }

        private void OnShowTransactionSearchWindowExecute()
        {
            CurrentWindow = TransactionSearchWindow;
        }

        private void OnShowStatementWindowExecute()
        {
            CurrentWindow = StatementWindow;
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            CurrentWindow = StatementWindow;
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(new OpenDetailViewEventArgs{ViewModelName = viewModelType.Name});
        }
    }
}
