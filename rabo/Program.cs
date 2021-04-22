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

                new Transaction 
                {
                    Amount = 50.1, PurchaseDate = new DateTime(2020, 1, 1), CustomerId = 1
                },
                new Transaction 
                {
                    Amount = 120, PurchaseDate = new DateTime(2020, 2, 1), CustomerId = 2
                },
                new Transaction 
                {
                    Amount = 160, PurchaseDate = new DateTime(2020, 3, 1), CustomerId = 2
                },
                new Transaction
                {
                    Amount = 180, PurchaseDate = new DateTime(2020, 4, 1), CustomerId = 3
                }
            };

            var period = new DateTime(2020, 1, 1);
            IEnumerable<Transaction> threeMonthTransactions = 
                customerTransactions.Where(x =>
                x.PurchaseDate.Subtract(period).Days >= 0 && x.PurchaseDate.Subtract(period).Days <= 90).ToArray();

            var groupByCustomerId = threeMonthTransactions.GroupBy(x => x.CustomerId);
            foreach (IGrouping<int, Transaction> grouping in groupByCustomerId)
            {
                Console.Out.WriteLine($"Customer {grouping.Key}");
                Transaction[] trans = grouping.ToArray();
                foreach (var transactionsByMonth in trans.GroupBy(x => new { x.PurchaseDate.Month, x.PurchaseDate.Year }))
                {
                    int reward = CalculateRewardPerCustomer(transactionsByMonth.ToArray());
                    Console.Out.WriteLine($"\t\t for month/year {transactionsByMonth.Key.Month}/{transactionsByMonth.Key.Year} = {reward}");
                }

                var totalReward = CalculateRewardPerCustomer(trans);
                Console.Out.WriteLine($"\t\t\t total reward = {totalReward} ");
            }
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
        public int CustomerId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double Amount { get; set; }

    }
}
