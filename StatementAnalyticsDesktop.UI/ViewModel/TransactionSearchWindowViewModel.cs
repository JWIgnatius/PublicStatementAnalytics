using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using StatementAnalytics.Transactions;
using StatementAnalyticsDesktop.UI.Data.Repositories;

namespace StatementAnalyticsDesktop.UI.ViewModel
{
    public class TransactionSearchWindowViewModel : ViewModelBase, IWindowViewModel
    {
        private ITransactionRepository _transactionRepository;
        private IEventAggregator _eventAggregator;
        private string _detailFilter = string.Empty;
        private string _startDateFilter;
        private string _endDateFilter;
        private double? _startAmountFilter = double.MinValue;
        private double? _endAmountFilter = double.MaxValue;
        private double _totalAmount;
        private ObservableCollection<Transaction> _filteredTransactions;

        public TransactionSearchWindowViewModel(ITransactionRepository dataService, IEventAggregator eventAggregator)
        {
            _transactionRepository = dataService;
            _eventAggregator = eventAggregator;

            FilteredTransactions = new ObservableCollection<Transaction>();
        }

        public string DetailFilter
        {
            get { return _detailFilter; }
            set
            {
                _detailFilter = value?.ToUpper();
                OnPropertyChanged();
                UpdateTransactions().Wait();
            }
        }

        public string StartDateFilter
        {
            get { return _startDateFilter; }
            set
            {
                _startDateFilter = value;
                OnPropertyChanged();
                UpdateTransactions().Wait();
            }
        }

        public string EndDateFilter
        {
            get { return _endDateFilter; }
            set
            {
                _endDateFilter = value;
                OnPropertyChanged();
                UpdateTransactions().Wait();
            }
        }

        public double? StartAmountFilter
        {
            get { return _startAmountFilter; }
            set
            {
                _startAmountFilter = value;
                OnPropertyChanged();
                UpdateTransactions().Wait();
            }
        }

        public double? EndAmountFilter
        {
            get { return _endAmountFilter; }
            set
            {
                _endAmountFilter = value;
                OnPropertyChanged();
                UpdateTransactions().Wait();
            }
        }

        public double TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Transaction> FilteredTransactions
        {
            get { return _filteredTransactions; }
            set
            {
                _filteredTransactions = value;
                OnPropertyChanged();
                TotalAmount = Math.Round(_filteredTransactions.Sum(t => t.Amount), 2);
            }
        }

        public async Task LoadAsync()
        {
            await UpdateTransactions();
        }

        public async Task UpdateTransactions()
        {
            var filteredTransactions = await _transactionRepository.GetByDetailsAsync(DetailFilter, StartDateFilter, EndDateFilter, StartAmountFilter, EndAmountFilter);

            var observableTransactions = new ObservableCollection<Transaction>();

            foreach (var transaction in filteredTransactions)
                observableTransactions.Add(transaction);

            FilteredTransactions = observableTransactions;
        }
    }
}
