using System;
using System.Collections.Generic;
using System.IO;
using StatementAnalytics.Statements;

namespace StatementAnalytics.Helpers
{
    public static partial class Helper
    {
        public static void CreateStatementSummaryCsv(string path, List<Statement> allStatements)
        {
            var newCsvPath = path + @"/StatementSummary.csv";

            if (File.Exists(newCsvPath))
            {
                Console.WriteLine("Deleting Statement Summary Csv File");
                File.Delete(newCsvPath);
            }

            using (var csvFile = File.AppendText(newCsvPath))
            {
                csvFile.WriteLine("Statement date,Previous Balance,New Balance,Principal Balance");

                foreach (var statement in allStatements)
                {
                    csvFile.WriteLine(statement.GetStatementSummaryCsv());
                }
            }

            Console.WriteLine("Created Statement Summary Csv File");
        }

        public static void CreateBalanceCsv(string pathToFolder, List<Statement> allStatements)
        {
            var newCsvPath = pathToFolder + @"/TotalBalance.csv";

            if (File.Exists(newCsvPath))
            {
                Console.WriteLine("Deleting Balance Csv File");
                File.Delete(newCsvPath);
            }

            var balanceSummary = Helper.GetBalanceCsvAsList(allStatements);
            using (var csvFile = File.AppendText(newCsvPath))
            {
                csvFile.WriteLine("Date Received by Us,Cumulative Balance");

                foreach (var line in balanceSummary)
                {
                    csvFile.WriteLine(line.GetCsvString());
                }
            }

            Console.WriteLine("Created Balance Summary Csv File");
        }

        public static void CreateTransactionCsv(string pathToFolder, List<Statement> allStatements)
        {
            var newCsvPath = pathToFolder + @"/TransactionSummary.csv";

            if (File.Exists(newCsvPath))
            {
                Console.WriteLine("Deleting Statements Csv File");
                File.Delete(newCsvPath);
            }

            using (var csvFile = File.AppendText(newCsvPath))
            {
                csvFile.WriteLine("Date received by us,Transaction date,Details,Amount");

                foreach (var statement in allStatements)
                {
                    foreach (var transaction in statement.Transactions)
                    {
                        csvFile.WriteLine(transaction.GetCsvString());
                    }
                }
            }

            Console.WriteLine("Created Transactions Csv File");
        }
    }
}