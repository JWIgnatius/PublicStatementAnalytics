using System;
using System.Text.RegularExpressions;
using StatementAnalytics.Helpers;

namespace StatementAnalytics.Transactions
{
    public class BarclaysTransaction : Transaction
    {
        private const string Pattern = "(" + Date + @")\s(.*?)(" + AmountPattern + @")\s*?(" + AmountPattern + ")";

        private const string Date = @"\d{2}/\d{2}/\d{4}";

        private const string AmountPattern = @"-?£\d*?.\d*?\.\d{2}";
        
        public static bool IsMatch(string line) => Regex.IsMatch(line, Pattern);
        
        public BarclaysTransaction(string line)
        {
            var match = Regex.Match(line, Pattern);

            var parsedDate = DateTime.Parse(Helper.ParseSlashToDash(match.Groups[1].ToString()));
            
            TransactionDate = parsedDate;

            DateReceivedByUs = parsedDate;

            Bank = "Barclays Debit";

            var detailsString = match.Groups[2].ToString();
            if (string.IsNullOrWhiteSpace(detailsString) || string.IsNullOrEmpty(detailsString))
                Details = "Not Found";
            else
                Details = Helper.CatchRetailersRemoveCommas(detailsString);

            var amount = Regex.Replace(match.Groups[3].ToString(), "£", string.Empty);
            Amount = Helper.GetCurrency(amount);

            if (Amount < 0)
            {
                Credited = false;
                Amount = -Amount;
            }
            else
                Credited = true;

            var balance = Regex.Replace(match.Groups[4].ToString(), "£", string.Empty);
            Balance = Helper.GetCurrency(balance);
        }
        
        public static double GetBalance(string line)
        {
            var match = Regex.Match(line, Pattern);

            var balance = Regex.Replace(match.Groups[4].ToString(), "£", string.Empty);

            return Helper.GetCurrency(balance);
        }
    }
}