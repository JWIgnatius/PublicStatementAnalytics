using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using StatementAnalytics.Helpers;
using StatementAnalytics.Statements;
using StatementAnalytics.Transactions;
using StatementAnalyticsDesktop.UI.Data.Repositories;
using StatementAnalyticsDesktop.UI.View.Services;
using StatementAnalyticsDesktop.UI.Wrapper;

namespace StatementAnalyticsDesktop.UI.ViewModel
{
    public class StatementDetailViewModel : DetailViewModelBase, IStatementDetailViewModel
    {
        private IStatementRepository _statementRepository;
        private IMessageDialogService _messageDialogService;
        private StatementWrapper _statement;
        private TransactionWrapper _selectedTransaction;

        public StatementDetailViewModel(IStatementRepository dataService,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        :base(eventAggregator)
        {
            _statementRepository = dataService;
            _messageDialogService = messageDialogService;
            
            AddTransactionCommand = new DelegateCommand(OnAddTransactionExecute);
            RemoveTransactionCommand = new DelegateCommand(OnRemoveTransactionExecute, OnRemoveTransactionCanExecute);

            Transactions = new ObservableCollection<TransactionWrapper>();
        }

        public override async Task LoadAsync(int? statementId)
        {
            var statement = statementId.HasValue
                ? await _statementRepository.GetByIdAsync(statementId.Value)
                : CreateNewStatement();

            InitialiseStatement(statement);

            InitialiseTransactions(statement.Transactions);
        }

        public ICommand AddTransactionCommand { get; }

        public ICommand RemoveTransactionCommand { get; }

        public StatementWrapper Statement
        {
            get { return _statement; }
            private set
            {
                _statement = value;
                OnPropertyChanged();
            }
        }

        public TransactionWrapper SelectedTransaction
        {
            get { return _selectedTransaction; }
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged();
                ((DelegateCommand) RemoveTransactionCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<TransactionWrapper> Transactions { get; }

        private Statement CreateNewStatement()
        {
            var statement = new Statement {Id = 0};

            _statementRepository.Add(statement);

            return statement;
        }

        protected override async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog(
                $"Do you really want to delete statement {Helper.GetIdentifier(Statement.Model)}", "Question");

            if (result == MessageDialogResult.OK)
            {
                _statementRepository.Remove(Statement.Model);
                await _statementRepository.SaveAsync();
                RaiseDetailDeletedEvent(Statement.Id);
            }
        }

        protected override async void OnSaveExecute()
        {
            await _statementRepository.SaveAsync();
            HasChanges = _statementRepository.HasChanges();
            RaiseDetailSavedEvent(Statement.Id, Helper.GetIdentifier(Statement.Model));
        }

        protected override bool OnSaveCanExecute()
        {
            return Statement != null
                   && !Statement.HasErrors
                   && Transactions.All(t => !t.HasErrors)
                   && HasChanges;
        }

        private bool OnRemoveTransactionCanExecute()
        {
            return SelectedTransaction != null;
        }

        private void OnRemoveTransactionExecute()
        {
            SelectedTransaction.PropertyChanged -= TransactionWrapper_PropertyChanged;
            _statementRepository.RemoveTransaction(SelectedTransaction.Model);
            Transactions.Remove(SelectedTransaction);
            SelectedTransaction = null;
            HasChanges = _statementRepository.HasChanges();
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddTransactionExecute()
        {
            var newTransaction = new TransactionWrapper(new Transaction());
            newTransaction.PropertyChanged += TransactionWrapper_PropertyChanged;
            Transactions.Add(newTransaction);
            Statement.Model.Transactions.Add(newTransaction.Model);

            // Trigger validations by setting values here
            newTransaction.TransactionDate = Statement.StatementDate;
            newTransaction.Details = "";
            newTransaction.Credited = false;

            newTransaction.Amount = 0.0;
        }

        private void TransactionWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
                HasChanges = _statementRepository.HasChanges();

            if (e.PropertyName == nameof(TransactionWrapper.HasErrors))
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
        }

        private void InitialiseStatement(Statement statement)
        {
            Statement = new StatementWrapper(statement);

            Statement.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _statementRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Statement.HasErrors))
                {
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
                }
            };

            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();

            // Trick to get validations to work immediately - Statement only has no id if it hasn't been added to db yet
            if (statement.Id == 0)
            {
                Statement.Bank = "";
                Statement.NewBalance = 0.0;
                Statement.PreviousBalance = 0.0;
                Statement.StatementDate = DateTime.Now;
            }
        }

        private void InitialiseTransactions(ICollection<Transaction> transactions)
        {
            foreach (var wrapper in Transactions)
            {
                wrapper.PropertyChanged -= TransactionWrapper_PropertyChanged;
            }

            Transactions.Clear();

            foreach (var transaction in transactions)
            {
                var wrapper = new TransactionWrapper(transaction);
                Transactions.Add(wrapper);
                wrapper.PropertyChanged += TransactionWrapper_PropertyChanged;
            }
        }
    }
}