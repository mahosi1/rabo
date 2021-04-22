using System;
using System.Collections.Generic;
using System.Linq;

namespace rabo
{
    class Program
    {
        static void Main(string[] args)
        {

            var customerTransactions = new List<Transaction>
            {

                new Transaction // not over 50 so it doesn't count
                {
                    Amount = 50, PurchaseDate = new DateTime(2020, 1, 1)
                },
                new Transaction // counts
                {
                    Amount = 120, PurchaseDate = new DateTime(2020, 2, 1)
                },
                new Transaction // counts
                {
                    Amount = 160, PurchaseDate = new DateTime(2020, 3, 1)
                },
                new Transaction // outside 3 month period
                {
                    Amount = 120, PurchaseDate = new DateTime(2020, 4, 1)
                }
            };

            var period = new DateTime(2020, 1, 1);
            IEnumerable<Transaction> transactions = 
                customerTransactions.Where(x =>
                x.PurchaseDate.Subtract(period).Days >= 0 && x.PurchaseDate.Subtract(period).Days <= 90).ToArray();
            var reward = CalculateRewardPerCustomer(transactions);
            Console.Out.WriteLine($"reward = {reward} for period {period}");


        }

        static int CalculateRewardPerCustomer(IEnumerable<Transaction> transactions)
        {
            int rewards = 0;
            foreach (Transaction transaction in transactions)
            {
                if (transaction.Amount > 50)
                {
                    rewards += 50;
                }

                if (transaction.Amount > 100)
                {
                    rewards += (int) (transaction.Amount - 100) * 2;
                }
            }
            return rewards;
        }

        // this one works too, no branching
        static int CalculateRewardPerCustomer2(IEnumerable<Transaction> transactions)
        {
            int rewards = 0;
            foreach (Transaction transaction in transactions)
            {
                rewards += (int)(Math.Max(transaction.Amount - 100, 0) + Math.Max(transaction.Amount - 50, 0));
            }
            return rewards;
        }
    }


    class Transaction
    {
        public DateTime PurchaseDate { get; set; }
        public double Amount { get; set; }

    }
}
