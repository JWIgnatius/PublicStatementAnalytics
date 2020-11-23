using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StatementAnalytics.Transactions;

namespace StatementAnalytics.Statements
{
    public class Statement
    {
        [Required]
        public int Id { get; set; }

        public string FilePath { get; set; }

        [Required]
        [MaxLength(50)]
        public string Bank { get; set; }

        public double PreviousBalance { get; set; }

        public double NewBalance { get; set; }

        [Required]
        public DateTime StatementDate { get; set; } = DateTime.MinValue;

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public void DisplayTransactions()
        {
            foreach (var transaction in Transactions)
                transaction.Display();
        }

        public void Display()
        {
            Console.WriteLine($"\nStatement of {StatementDate}");
            Console.WriteLine($"Bank: {Bank}");
            Console.WriteLine($"Previous Balance: £{PreviousBalance}");
            Console.WriteLine($"New Balance: £{NewBalance}");
            Console.WriteLine("Transactions:");
            
            DisplayTransactions();
        }

        public string GetStatementSummaryCsv() => $"{StatementDate},{PreviousBalance},{NewBalance}";
    }
}