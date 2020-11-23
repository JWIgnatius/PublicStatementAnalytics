using System;
using System.Collections.Generic;
using StatementAnalytics.Transactions;

namespace StatementAnalytics.MenuHelpers
{
    public static class Menu
    {
        public static bool AskToDisplayTransactions()
        {
            Console.WriteLine("Would you like to see all of your result transactions?");

            while (true)
            {
                var answer = Console.ReadLine();
                var parsedAnswer = ParseAnswerToInt(answer);

                switch (parsedAnswer)
                {
                    case 0:
                        return true;
                    case 1:
                        return false;
                    default:
                        Console.WriteLine($"Sorry, I didn't recognise {answer}");
                        break;
                }
            }
        }

        public static bool AskToAddTransactions()
        {
            Console.WriteLine("\nWould you like to add the previous results to your result set?");

            while (true)
            {
                var answer = Console.ReadLine();

                var parsedAnswer = ParseAnswerToInt(answer);

                switch (parsedAnswer)
                {
                    case 0:
                        return true;
                    case 1:
                        return false;
                    default:
                        Console.WriteLine($"Sorry I don't recognise {answer} as a reply, please try with (y/n) input");
                        break;
                }
            }
        }

        // Parsing the answer in a single place
        private static int ParseAnswerToInt(string answer)
        {
            answer = answer.ToLower();

            if (answer == "y" || answer == "yes" || answer == "yeah")
                return 0;

            else if (answer == "n" || answer == "no" || answer == "nope")
                return 1;

            return 2;
        }

        public static void Display(List<Transaction> transactions, bool isNewTransactions = false)
        {
            var newOrAll = string.Empty;
            newOrAll = isNewTransactions ? "your new" : "all of your";

            Console.WriteLine($"\nHere are {newOrAll} transactions");

            foreach (var transaction in transactions)
                transaction.Display();
        }

        public static void DisplayOptions()
        {
            Console.WriteLine("\n*******\nMENU\n*******\n" +
                // $"Type '{GetShortcut(MenuOptions.WantSummary)}' to see a summary of all your statements\n" + // NOT IMPLEMENTED
                $"Type '{GetShortcut(MenuOptions.ViewSavedTransactions)}' to see your saved transactions\n" +
                $"Type '{GetShortcut(MenuOptions.WantTotal)}' to see total spend at each vendor\n" +
                $"Type '{GetShortcut(MenuOptions.FindTransactions)}' to find transactions\n" +
                $"Type '{GetShortcut(MenuOptions.FindTransactionsByDate)}' to find transactions by date\n" +
                $"Type '{GetShortcut(MenuOptions.FindStatementsFrom)}' to find statements from a date\n" +
                $"Type '{GetShortcut(MenuOptions.ClearTransactions)}' to clear your returned transactions\n" +
                $"Type '{GetShortcut(MenuOptions.Quit)}' to close the app\n");
        }

        // Here you configure what shortcuts you should use for each option
        public static string GetShortcut(MenuOptions options)
        {
            switch (options)
            {
                case MenuOptions.ViewSavedTransactions:
                    return "VST";
                
                case MenuOptions.WantSummary:
                    return "WS";

                case MenuOptions.WantTotal:
                    return "WT";

                case MenuOptions.FindTransactions:
                    return "FT";

                case MenuOptions.Quit:
                    return "Q";

                case MenuOptions.FindTransactionsByDate:
                    return "FTBD";

                case MenuOptions.FindStatementsFrom:
                    return "FSFD";

                case MenuOptions.ClearTransactions:
                    return "CT";

                case MenuOptions.Default:
                    return "oops";

                default:
                    return "Oops";
            }
        }

        public static MenuOptions ParseInput(string option)
        {
            option = option.ToLower();
            
            if (option == GetShortcut(MenuOptions.WantSummary).ToLower())
                return MenuOptions.WantSummary;
            
            if (option == GetShortcut(MenuOptions.WantTotal).ToLower())
                return MenuOptions.WantTotal;
            
            if (option == GetShortcut(MenuOptions.FindTransactions).ToLower())
                return MenuOptions.FindTransactions;

            if (option == GetShortcut(MenuOptions.Quit).ToLower())
                return MenuOptions.Quit;

            if (option == GetShortcut(MenuOptions.FindTransactionsByDate).ToLower())
                return MenuOptions.FindTransactionsByDate;

            if (option == GetShortcut(MenuOptions.FindStatementsFrom).ToLower())
                return MenuOptions.FindStatementsFrom;

            if (option == GetShortcut(MenuOptions.ClearTransactions).ToLower())
                return MenuOptions.ClearTransactions;
            
            if (option == GetShortcut(MenuOptions.ViewSavedTransactions).ToLower())
                return MenuOptions.ViewSavedTransactions;

            return MenuOptions.Default;
        }
    }

    public enum MenuOptions
    {
        ViewSavedTransactions,
        FindTransactionsByDate,
        WantSummary,
        FindTransactions,
        WantTotal,
        FindStatementsFrom,
        ClearTransactions,
        Quit,
        Default
    }
}
