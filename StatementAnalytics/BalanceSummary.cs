using System;
using StatementAnalytics.Transactions;

namespace StatementAnalyser.Summaries
{
    public class BalanceSummary
    {
        public BalanceSummary()
        {
        }

        public BalanceSummary(Transaction transaction = null, double total = 0)
        {
            DateReceivedByUs = transaction.DateReceivedByUs;
            TransactionDate = transaction.TransactionDate;
            Total = Math.Round(total, 2);
            Amount = transaction.Amount;
            Bank = transaction.Bank;
        }

        public string Bank;

        public DateTime DateReceivedByUs = DateTime.MinValue;

        public DateTime TransactionDate = DateTime.MinValue;

        public double Total;

        public double Amount;

        public string GetCsvString()
        {
            return $"{DateReceivedByUs.ToShortDateString()},{Total}";
        }
    }
}
