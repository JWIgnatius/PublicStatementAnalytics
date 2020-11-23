using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using StatementAnalyser.Summaries;
using StatementAnalytics.Statements;
using StatementAnalytics.Transactions;
using static System.String;

namespace StatementAnalytics.Helpers
{
    public static partial class Helper
    {
        public static List<Statement> GetAllStatementsFromFolder(string path, bool datesOnly = false)
        {
            var allStatementsPdf = Directory.GetFiles(path, "*.pdf");

            var allStatements = new List<Statement>();

            foreach (var file in allStatementsPdf)
            {
                var statement = GetStatement(file, datesOnly);
                allStatements.Add(statement);
            }

            return allStatements;
        }

        public static List<Transaction> GetAllTransactions(List<Statement> allStatements)
        {
            var allTransactions = new List<Transaction>();
            foreach (var statement in allStatements)
            {
                foreach (var transaction in statement.Transactions)
                {
                    allTransactions.Add(transaction);
                }
            }
            return allTransactions.OrderBy(t => t.TransactionDate).ToList();
        }
        
        public static Statement GetStatement(string filePath, bool datesOnly = false)
        {
            var text = Empty;
            var pdfReader = new PdfDocument(new PdfReader(filePath));

            for (var i = 1; i <= pdfReader.GetNumberOfPages(); i++)
            {
                text += PdfTextExtractor.GetTextFromPage(pdfReader.GetPage(i));
            }

            // Office for Barclays, I chose this to be the most unique way to tell which statement is which
            // Assume it's a HSBC statement otherwise
            // ToDo: Find a way to determine HSBC statements 
            if (text.Contains("Registered Office: 1 Churchill Place, London E14 5HP"))
                return new BarclaysStatement(text, filePath, datesOnly);
            
            return new HsbcCreditStatement(text, filePath, datesOnly);
        }

        public static List<BalanceSummary> GetBalanceCsvAsList(List<Statement> allStatements)
        {
            var balanceSummary = new List<BalanceSummary>();


            var splitStatements = allStatements.GroupBy(s => s.Bank, s => s).ToList();

            foreach (var statementType in splitStatements)
            {
                var totalBalance = 0.0;

                for (int i = 0; i < statementType.Count(); i++)
                {
                    var statement = statementType.ElementAt(i);

                    if (i == 0)
                    {
                        totalBalance = statement.PreviousBalance;
                    }

                    foreach (var transaction in statement.Transactions)
                    {
                        if (transaction.Credited == true)
                            totalBalance += transaction.Amount;
                        else
                            totalBalance -= transaction.Amount;

                        balanceSummary.Add(new BalanceSummary(transaction, totalBalance));
                    }
                }
            }

            return balanceSummary;
        }
        
        public static bool StatementsAddUpCorrectly(List<Statement> allStatements)
        {
            var balanceSummary = GetBalanceCsvAsList(allStatements);

            var splitStatements = allStatements.GroupBy(s => s.Bank, s => s).ToList();

            foreach (var statementType in splitStatements)
            {
                Console.WriteLine($"Checking for {statementType.Key} Statements");
                foreach (var statement in statementType)
                {
                    var lastTransaction = new BalanceSummary();
                    foreach (var line in balanceSummary.Where(l => l.Bank == statementType.Key))
                    {
                        if (line.DateReceivedByUs <= statement.StatementDate)
                        {
                            lastTransaction = line;
                        }
                        else
                        {
                            if (lastTransaction.Total != statement.NewBalance)
                            {
                                Console.WriteLine($"Statement: {statement.FilePath}\n" +
                                                  $"Transaction date: {lastTransaction.DateReceivedByUs} Statement Date: {statement.StatementDate}\n" +
                                                  $"Total to date: {lastTransaction.Total} Statement New Balance: {statement.NewBalance}\n\n");

                                return false;
                            }
                            break;
                        }
                    }
                }
            }
            return true;
        }
    }
}