using System;
using System.ComponentModel.DataAnnotations;

namespace StatementAnalytics.Transactions
{
    public class Transaction
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int StatementId { get; set; }

        public string Bank { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.MinValue;
        
        [Required]
        public DateTime DateReceivedByUs { get; set; } = DateTime.MinValue;

        [Required]
        public string Details { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The Amount must be positive")]
        public double Amount { get; set; }

        [Required]
        public bool Credited { get; set; }

        public double Balance { get; set; }

        public bool Contactless { get; set; }

        public virtual void Display()
        {
            Console.WriteLine($"{Bank}\t{TransactionDate.ToShortDateString()}\t{Details}\t£{Amount}");
        }

        public virtual string GetCsvString()
        {
            return $"{Bank},{TransactionDate},{Details},{Amount}";
        }
    }
}