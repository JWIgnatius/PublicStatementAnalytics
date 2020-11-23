using System;
using System.Text.RegularExpressions;
using StatementAnalytics.Helpers;

namespace StatementAnalytics.Transactions
{
    public class HsbcCreditTransaction : Transaction
    {
        private const string Pattern =
            @"(" + Date + @")\s(" + Date + @")\s*(\){3})?\s*(.*?)\s(" + AmountPattern + @")(CR)?";

        private const string Date = @"\d{2}\s.{3}\s\d{2}";

        private const string AmountPattern = @"\d{1,3}\.\d{2}";

        public static bool IsMatch(string input) => Regex.IsMatch(input, Pattern);

        // This constructor will create a transaction to represent the added interest
        public HsbcCreditTransaction(DateTime date, double interest)
        {
            DateReceivedByUs = date;
            TransactionDate = date;
            Contactless = false;
            Details = "INTEREST";
            Amount = interest;
        }
        
        public HsbcCreditTransaction(string line)
        {
            var match = Regex.Match(line, Pattern);

            DateReceivedByUs = DateTime.Parse(Regex.Replace(match.Groups[1].ToString(), @"\s", "-"));

            TransactionDate = DateTime.Parse(Regex.Replace(match.Groups[2].ToString(), @"\s", "-"));

            Contactless = match.Groups[3].ToString() == ")))";

            Bank = "HSBC Credit";

            //Hardcoded transformations for the biggest retailers
            //Hopefully in the future I wil find a better way to sort this problem out
            Details = Helper.CatchRetailersRemoveCommas(match.Groups[4].ToString());

            Amount = Helper.GetCurrency(match.Groups[5].ToString());

            Credited = match.Groups[6].ToString() == "CR";
        }
    }
}