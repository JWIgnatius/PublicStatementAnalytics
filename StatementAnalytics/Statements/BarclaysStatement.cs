using System;
using System.Linq;
using System.Text.RegularExpressions;
using StatementAnalytics.Transactions;
using StatementAnalytics.Helpers;

namespace StatementAnalytics.Statements
{
    public class BarclaysStatement : Statement
    {
        private const string StatementDatePattern = @"to (\d{2}/\d{2}/\d{4})";

        public BarclaysStatement(string allLinesAsOne, string filePath = "Not Given", bool statementDateOnly = false)
        {
            var splitLines = allLinesAsOne.Split('\n');

            Bank = "Barclays";
            FilePath = filePath;

            // These will be needed to calculate new and previous balance
            string latestTransactionLine = null;
            string oldestTransactionLine = null;

            for (var i = 0; i < splitLines.Length; i++)
            {
                var line = splitLines[i];

                if (BarclaysTransaction.IsMatch(line))
                {
                    // Barclays transactions are in reverse chronological order
                    // Therefore our first transaction in the newest
                    if (latestTransactionLine == null)
                        latestTransactionLine = line;

                    var newTransaction = new BarclaysTransaction(line);
                    if (newTransaction.Details == "Not Found")
                        newTransaction.Details = Helper.CatchRetailersRemoveCommas(splitLines[i + 1].Trim());

                    Transactions.Add(newTransaction);

                    // Assigning this every time so we will eventually have the last(= oldest)
                    oldestTransactionLine = line;
                }
                else if (Regex.IsMatch(line, StatementDatePattern) && StatementDate == DateTime.MinValue)
                {
                    var statementDate = Regex.Match(line, StatementDatePattern);
                    StatementDate =
                        DateTime.Parse(Helper.ParseSlashToDash(statementDate.Groups[1].ToString()));

                    if (statementDateOnly)
                        return;
                }
            }

            // I want transactions to be listed in order of date
            Transactions = Transactions.OrderBy(t => t.TransactionDate).ToList();

            // New balance is the balance listed in the latest transaction line
            NewBalance = BarclaysTransaction.GetBalance(latestTransactionLine);

            // Previous balance calculated by taking the oldest transaction amount from the balance listed
            // Overall summaries are not shown by Barclays statements
            PreviousBalance = BarclaysTransaction.GetBalance(oldestTransactionLine)
                                        - new BarclaysTransaction(oldestTransactionLine).Amount;
        }
    }
}