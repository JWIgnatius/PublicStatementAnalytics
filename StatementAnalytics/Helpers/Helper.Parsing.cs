using System;
using System.Text.RegularExpressions;
using StatementAnalytics.Statements;

namespace StatementAnalytics.Helpers
{
    public static partial class Helper
    {
        /// <summary>
        /// Given a statement we can return a string which uniquely identifies that statement as 'Bank Month Year'
        /// Returns an empty string otherwise
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public static string GetIdentifier(Statement statement)
        {
            var monthAsInt = statement.StatementDate.Month;
            var year = statement.StatementDate.Year;
            var bank = statement.Bank;

            return monthAsInt switch
            {
                1 => bank + " Jan " + year,
                2 => bank + " Feb " + year,
                3 => bank + " Mar " + year,
                4 => bank + " Apr " + year,
                5 => bank + " May " + year,
                6 => bank + " Jun " + year,
                7 => bank + " Jul " + year,
                8 => bank + " Aug " + year,
                9 => bank + " Sep " + year,
                10 => bank + " Oct " + year,
                11 => bank + " Nov " + year,
                12 => bank + " Dec " + year,
                _ => ""
            };
        }

        public static string ParseSlashToDash(string date)
        {
            var correctDate = Regex.Replace(date, @"/", "-");

            if (correctDate.Contains("/01/"))
                correctDate = Regex.Replace(correctDate, @"/01/", "-Jan-");

            else if (correctDate.Contains("/02/"))
                correctDate = Regex.Replace(correctDate, @"/02/", "-Feb-");

            else if (correctDate.Contains("/03/"))
                correctDate = Regex.Replace(correctDate, @"/03/", "-Mar-");

            else if (correctDate.Contains("/04/"))
                correctDate = Regex.Replace(correctDate, @"/04/", "-Apr-");

            else if (correctDate.Contains("/05/"))
                correctDate = Regex.Replace(correctDate, @"/05/", "-May-");

            else if (correctDate.Contains("/06/"))
                correctDate = Regex.Replace(correctDate, @"/06/", "-Jun-");

            else if (correctDate.Contains("/07/"))
                correctDate = Regex.Replace(correctDate, @"/07/", "-Jul-");

            else if (correctDate.Contains("/08/"))
                correctDate = Regex.Replace(correctDate, @"/08/", "-Aug-");

            else if (correctDate.Contains("/09/"))
                correctDate = Regex.Replace(correctDate, @"/09/", "-Sep-");

            else if (correctDate.Contains("/10/"))
                correctDate = Regex.Replace(correctDate, @"/10/", "-Oct-");

            else if (correctDate.Contains("/11/"))
                correctDate = Regex.Replace(correctDate, @"/11/", "-Nov-");

            else if (correctDate.Contains("/12/"))
                correctDate = Regex.Replace(correctDate, @"/12/", "-Dec-");

            var suffix = Regex.Match(correctDate, @"20(\d{2})");
            correctDate = Regex.Replace(correctDate, @"20\d{2}", suffix.Groups[1].ToString());

            return correctDate;
        }

        public static string ConvertToShortDate(string longDate)
        {
            var shortDate = Regex.Replace(longDate, @"\s", "-");

            shortDate = Regex.Replace(shortDate, "January", "Jan");
            shortDate = Regex.Replace(shortDate, "February", "Feb");
            shortDate = Regex.Replace(shortDate, "March", "Mar");
            shortDate = Regex.Replace(shortDate, "April", "Apr");
            shortDate = Regex.Replace(shortDate, "June", "Jun");
            shortDate = Regex.Replace(shortDate, "July", "Jul");
            shortDate = Regex.Replace(shortDate, "August", "Aug");
            shortDate = Regex.Replace(shortDate, "September", "Sep");
            shortDate = Regex.Replace(shortDate, "October", "Oct");
            shortDate = Regex.Replace(shortDate, "November", "Nov");
            shortDate = Regex.Replace(shortDate, "December", "Dec");

            var suffix = Regex.Match(shortDate, @"20(\d{2})");
            shortDate = Regex.Replace(shortDate, @"20\d{2}", suffix.Groups[1].ToString());

            return shortDate;
        }

        public static double GetCurrency(string amount) => Math.Round(double.Parse(Regex.Replace(amount, ",", string.Empty)), 2);

        /// <summary>
        /// This has a list of all common retailers or retailers that like to add random strings to their transaction details
        /// Easiest way to group all transactions for vendor summaries is by swapping the strings out
        /// </summary>
        /// <param name="details">Should take in your transaction details</param>
        /// <returns>The details which should make the most sense</returns>
        public static string CatchRetailersRemoveCommas(string details)
        {
            var upperCaseDetails = details.ToUpper().Trim();

            upperCaseDetails = Regex.Replace(upperCaseDetails, ",", " ");

            if (upperCaseDetails.Contains("AMAZON") || upperCaseDetails.Contains("AMZN MKTP"))
                return "AMAZON.CO.UK";
            if (upperCaseDetails.Contains("PAYPAL"))
                return "PAYPAL";
            if (upperCaseDetails.Contains("WILKO"))
                return "WILKO";
            if (upperCaseDetails.Contains("ALDI"))
                return "ALDI";
            if (upperCaseDetails.Contains("MORRISON"))
                return "MORRISONS";
            if (upperCaseDetails.Contains("UBER"))
            {
                if (upperCaseDetails.Contains("EATS"))
                    return "UBER EATS";
                return "UBER TRIP";
            }
            if (upperCaseDetails.Contains("TURTLE BAY"))
                return "TURTLE BAY";
            if (upperCaseDetails.Contains("TESCO"))
                return "TESCO";
            if (upperCaseDetails.Contains("SUPERDRUG"))
                return "SUPERDRUG";
            if (upperCaseDetails.Contains("SAINSBURY"))
                return "SAINSBURY'S";
            if (upperCaseDetails.Contains("NEXT"))
                return "NEXT";
            if (upperCaseDetails.Contains("MCDONALD"))
                return "MCDONALDS";
            if (upperCaseDetails.Contains("MILDMAY"))
                return "MILDMAY COLOURS";
            if (upperCaseDetails.Contains("MICROSOFT"))
                return "MICROSOFT";
            if (upperCaseDetails.Contains("MARKS&SPENCER") || upperCaseDetails.Contains("M&S"))
                return "M&S";
            if (upperCaseDetails.Contains("LIDL"))
                return "LIDL";
            if (upperCaseDetails.Contains("HOTEL CHOCOLAT"))
                return "HOTEL CHOCOLAT";
            if (upperCaseDetails.Contains("DEBENHAMS"))
                return "DEBENHAMS";
            if (upperCaseDetails.Contains("CLINTON"))
                return "CLINTONS";
            if (upperCaseDetails.Contains("COSTA"))
                return "COSTA";
            if (upperCaseDetails.Contains("BOOTS"))
                return "BOOTS";
            if (upperCaseDetails.Contains("TFL"))
                return "TFL TRAVEL";
            if (upperCaseDetails.Contains("ASOS"))
                return "ASOS";
            if (upperCaseDetails.Contains("ACTURIS"))
                return "ACTURIS";
            if (upperCaseDetails.Contains("TRAYPORT"))
                return "TRAYPORT";
            if (upperCaseDetails.Contains("DOMINOS"))
                return "DOMINOS PIZZA";
            if (upperCaseDetails.Contains("PIZZAHUT") || upperCaseDetails.Contains("PIZZA HUT"))
                return "PIZZA HUT";
            if (upperCaseDetails.Contains("PAPAJOHN"))
                return "PAPA JOHNS PIZZA";
            return upperCaseDetails;
        }
    }
}
