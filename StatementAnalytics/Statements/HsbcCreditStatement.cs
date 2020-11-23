using System;
using System.Linq;
using System.Text.RegularExpressions;
using StatementAnalytics.Helpers;
using StatementAnalytics.Transactions;

namespace StatementAnalytics.Statements
{
    public class HsbcCreditStatement : Statement
    {
        private const string StatementDatePattern = @"Statement Date\s*?(\d{2}\s?\D{1,9}\s\d{4})";

        private const string InterestPattern = @"(TOTAL INTEREST CHARGED ON THIS STATEMENT)\D+(\d{1,3}\.\d{2})";
        
        public HsbcCreditStatement(string allLinesAsOne, string filePath = "Not Given", bool statementDateOnly = false)
        {
            var allLines = allLinesAsOne.Split('\n');
            Bank = "HSBC";
            FilePath = filePath;

            var interest = 0.0;

            for (var i = 0; i < allLines.Length; i++)
            {
                var line = allLines[i];

                //Recognises the line to be a transaction and creates a new transaction object
                if (HsbcCreditTransaction.IsMatch(line))
                {
                    Transactions.Add(new HsbcCreditTransaction(line));
                }

                //Reads previous balance, rounding to 2d.p. from the summary
                else if (line.Contains("Previous Balance"))
                {
                    var amount = Regex.Replace(line.Substring(17), ",", string.Empty);
                    PreviousBalance = -Helper.GetCurrency(amount);
                }

                //Reads the new balance, rounding to 2d.p. from the summary
                else if (line.Contains("New Balance"))
                {
                    var amount = Regex.Replace(line.Substring(12), ",", string.Empty);
                    NewBalance = -Helper.GetCurrency(amount);
                }

                //Recognises the line to be a declaration of the statement date and that we haven't already assigned it
                else if (Regex.IsMatch(line, StatementDatePattern) && StatementDate == DateTime.MinValue)
                {
                    var matches = Regex.Match(line, StatementDatePattern);
                    StatementDate = DateTime.Parse(Helper.ConvertToShortDate(matches.Groups[1].ToString()));

                    if (statementDateOnly)
                        return;
                }

                //Recognises this is where the interest would be 
                //but formatting means we sometimes have the actual amount on the previous line
                else if (Regex.IsMatch(line, "TOTAL INTEREST CHARGED ON THIS STATEMENT"))
                {
                    if (Regex.IsMatch(line, InterestPattern))
                    {
                        var matches = Regex.Match(line, InterestPattern);
                        interest = Helper.GetCurrency(matches.Groups[2].ToString());
                    }
                    else
                    {
                        interest = Helper.GetCurrency(allLines[i - 1]);
                    }
                }
            }

            if (interest != 0.0)
                Transactions.Add(new HsbcCreditTransaction(StatementDate, interest));

            Transactions = Transactions.OrderBy(t => t.DateReceivedByUs).ToList();
        }
    }
}