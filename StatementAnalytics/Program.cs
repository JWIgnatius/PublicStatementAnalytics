using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StatementAnalytics.Helpers;
using StatementAnalytics.Statements;
using StatementAnalytics.Transactions;
using StatementAnalytics.MenuHelpers;

namespace StatementAnalytics
{
    class Program
    {
        private static void Main(string[] args)
        {
            const string path = @"C:\Users\User\Documents\Bank Statements\Both";

            var allStatements = Helper.GetAllStatementsFromFolder(path);

            var allTransactions = Helper.GetAllTransactions(allStatements);

            // We should exit here if the statements do not add up correctly
            if (!Helper.StatementsAddUpCorrectly(allStatements))
                return;

            Console.WriteLine("Welcome to the app");

            bool exit = false;
            var resultTransactions = new List<Transaction>();

            Console.WriteLine("\nWhat would you like to do?\n" +
                "Note that we will add all results to your existing results search, use the clear function to clear results");

            while (!exit)
            {
                Menu.DisplayOptions();

                var option = Console.ReadLine();

                var menuEnum = Menu.ParseInput(option);

                var newTransactions = new List<Transaction>();

                switch (menuEnum)
                {
                    case MenuOptions.ViewSavedTransactions:
                        Menu.Display(resultTransactions);
                        break;
                    
                    case MenuOptions.WantTotal:
                        var sortType = string.Empty;

                        while (sortType != "t" && sortType != "a")
                        {
                            Console.WriteLine("Would you like vendors sorted by total amount (t) or alphabetically (a)?");
                            sortType = Console.ReadLine()?.ToLower();
                            if (sortType == "t" || sortType == "a")
                                DisplayVendorsOrderedByTotalSpend(allStatements, sortType);

                            else
                                Console.WriteLine($"{sortType} isn't an option");
                        }
                        break;

                    case MenuOptions.FindTransactions:
                        newTransactions = AskForTransactions(allTransactions);
                        break;

                    case MenuOptions.FindTransactionsByDate:
                        newTransactions = AskForTransactionsBetweenTwoDates(allTransactions);
                        break;

                    case MenuOptions.Quit:
                        Console.WriteLine("Thanks for trying the app");
                        exit = true;
                        break;

                    case MenuOptions.FindStatementsFrom:
                        AskForStatementsFromADate(allStatements);
                        break;

                    case MenuOptions.ClearTransactions:
                        Console.WriteLine($"Clearing all {resultTransactions.Count} transactions");
                        resultTransactions.Clear();
                        break;

                    default:
                        Console.WriteLine("Sorry I didn't recognise that");
                        break;
                }

                // If we did an operation which didn't return transactions then continue and provide the menu again
                // Otherwise we should ask if they want to save their transactions to a results set
                if (!newTransactions.Any()) continue;
                
                Menu.Display(newTransactions, true);

                if (Menu.AskToAddTransactions())
                    resultTransactions.AddRange(newTransactions);

                resultTransactions = resultTransactions.OrderBy(t => t.TransactionDate).ToList();

                if (Menu.AskToDisplayTransactions())
                    Menu.Display(resultTransactions, true);
            }
        }

        private static void AskForStatementsFromADate(List<Statement> allStatements)
        {
            while (true)
            {
                Console.WriteLine("Enter a date for which you would like the statement");

                var filter = Console.ReadLine()?.Split(',');

                if (filter?.Length != 1)
                {
                    Console.WriteLine($"Sorry, I detected {filter?.Length} dates, please note that dates are separated by commas");
                    continue;
                }

                var dateIsCorrect = DateTime.TryParse(filter[0], out var filterDate);

                if (!dateIsCorrect)
                {
                    Console.WriteLine("Sorry I didn't recognise the date");
                    continue;
                }

                Console.WriteLine("Finding your statement(s)");
                var filteredStatements = allStatements
                    .Where(s => filterDate <= s.StatementDate && s.StatementDate <= filterDate.AddMonths(1))
                    .ToList();

                foreach (var statement in filteredStatements)
                {
                    Console.WriteLine("Please press enter to print the statement");
                    Console.ReadLine();
                    statement.Display();
                }

                return;
            }
        }

        private static List<Transaction> AskForTransactionsBetweenTwoDates(IEnumerable<Transaction> allTransactions)
        {
            Console.WriteLine("When do you want them from and to? (format is start date,end date)");

            while (true)
            {
                var filter = Console.ReadLine()?.Split(',');

                if (filter?.Length != 2)
                {
                    Console.WriteLine($"Sorry, I detected {filter?.Length} dates, please note that dates are separated by commas");
                    return new List<Transaction>();
                }

                var startDateCorrect = DateTime.TryParse(filter[0], out var startDate);
                var endDateCorrect = DateTime.TryParse(filter[1], out var endDate);

                if (startDateCorrect && endDateCorrect)
                    return allTransactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).ToList();
                
                if (!startDateCorrect && !endDateCorrect)
                    Console.WriteLine($"{filter[0]} and {filter[0]} are not recognised date formats");
                else if (!startDateCorrect)
                    Console.WriteLine($"{filter[0]} is not a recognised date format, {filter[1]} is though");
                else
                    Console.WriteLine($"{filter[1]} is not a recognised date format, {filter[0]} is though");
            }
        }
        
        private static void DisplayVendorsOrderedByTotalSpend(List<Statement> allStatements, string filterType = "t")
        {
            var allTransactions = Helper.GetAllTransactions(allStatements);

            var vendorsAndTransactions = allTransactions.GroupBy(t => t.Details.ToUpper(), t => t)
                .Select(g => new
                {
                    Vendor = g.First().Details,
                    Amount = Math.Round(g.Sum(t => t.Amount), 2)
                });

            if (filterType == "a")
                vendorsAndTransactions = vendorsAndTransactions.OrderBy(g => g.Vendor);

            else if (filterType == "t")
                vendorsAndTransactions = vendorsAndTransactions.OrderBy(g => g.Amount);

            foreach (var group in vendorsAndTransactions)
            {
                Console.WriteLine($"{group.Vendor}\t{group.Amount}");
            }
        }

        private static List<Transaction> AskForTransactions(List<Transaction> allTransactions)
        {
            Console.WriteLine("Who's transactions do you want?");
            var filter = Console.ReadLine();

            return FilterTransactions(allTransactions, filter);
        }

        private static List<Transaction> FilterTransactions(List<Transaction> allTransactions, string filter)
            => allTransactions.Where(t => t.Details.ToUpper().Contains(filter.ToUpper())).ToList();
    }
}
