using System;
using System.Collections.Generic;
using System.Linq;

namespace FinTsPersistence.Model
{
    /// <summary>
    /// Stores and retrieves data from DB
    /// 
    /// Requirements:
    /// The transactions to save MUST be ordered by EntryDate!! No checks are done.
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ITransactionContext context;

        public TransactionRepository(ITransactionContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Saves all given transactions
        /// </summary>
        public void SaveTransactions(IEnumerable<Transaction> transactions)
        {
            foreach (var t in transactions)
            {
                context.Transactions.Add(t);
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Returns transactions with the highest TransactionId
        /// </summary>
        public Transaction GetLastTransaction()
        {
            if (!context.Transactions.Any())
            {
                return new NoTransaction();
            }

            return (from t in context.Transactions
                    orderby t.TransactionId descending
                    select t).FirstOrDefault();
        }

        /// <summary>
        /// Gets last transactions by entrydate
        /// </summary>
        public IEnumerable<Transaction> GetLastTransactions(int amountOfDays)
        {
            var lastTransaction = GetLastTransaction();
            if (lastTransaction.IsNoTransaction())
            {
                return new List<Transaction>();
            }

            DateTime daysBefore = lastTransaction.EntryDate.Date.AddDays(-amountOfDays);

            return (from t in context.Transactions
                    orderby t.TransactionId ascending
                    where t.EntryDate >= daysBefore
                    select t).ToList();
        }
    }
}
