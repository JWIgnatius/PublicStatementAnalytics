using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using StatementAnalyticsDesktop.UI.Event;
using StatementAnalyticsDesktop.UI.View.Services;

namespace StatementAnalyticsDesktop.UI.ViewModel
{
    public class StatementWindowViewModel : ViewModelBase, IWindowViewModel
    {
        private IDetailViewModel _detailViewModel;
        private Func<IStatementDetailViewModel> _statementDetailViewModelCreator;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        public StatementWindowViewModel(INavigationViewModel navigationViewModel, Func<IStatementDetailViewModel> statementDetailViewModelCreator,
            IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            NavigationViewModel = navigationViewModel;
            _statementDetailViewModelCreator = statementDetailViewModelCreator;

            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            _eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            CreateNewStatementCommand = new DelegateCommand(OnCreateNewStatementExecute);
        }


        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get => _detailViewModel;
            private set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateNewStatementCommand { get; }

        private void OnCreateNewStatementExecute()
        {
            OnOpenDetailView(null);
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You will lose changes if you navigate away.", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            switch (args.ViewModelName)
            {
                case nameof(StatementDetailViewModel):
                    DetailViewModel = _statementDetailViewModelCreator();
                    break;
            }
            
            await DetailViewModel.LoadAsync(args.Id);
        }
    }
}
