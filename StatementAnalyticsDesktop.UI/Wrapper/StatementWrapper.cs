using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StatementAnalytics.Statements;
using StatementAnalytics.Transactions;

namespace StatementAnalyticsDesktop.UI.Wrapper
{

    public class StatementWrapper : ModelWrapper<Statement>
    {
        public StatementWrapper(Statement model) : base(model)
        {
        }

        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Bank
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime StatementDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public double PreviousBalance
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public double NewBalance
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                var value = GetValue<List<Transaction>>();
                return new ObservableCollection<Transaction>(value);
            }
            set { SetValue(value); }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Bank):
                    if (!(string.Equals(Bank, "Hsbc", StringComparison.OrdinalIgnoreCase) 
                          || string.Equals(Bank, "Barclays", StringComparison.OrdinalIgnoreCase)))
                        yield return "I only support HSBC and Barclays";
                    break;
            }
        }
    }
}
