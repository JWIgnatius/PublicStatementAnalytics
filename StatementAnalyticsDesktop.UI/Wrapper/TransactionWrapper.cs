using System;
using StatementAnalytics.Transactions;

namespace StatementAnalyticsDesktop.UI.Wrapper
{
    public class TransactionWrapper : ModelWrapper<Transaction>
    {
        public TransactionWrapper(Transaction model) : base(model)
        {
        }

        public DateTime TransactionDate
        {
            get { return GetValue<DateTime>(); }
            set{ SetValue(value); }
        }

        public string Details
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool Credited
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public double Amount
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }
    }
}
