using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTsPersistence.Model
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ITransactionContext context;

        public TransactionRepository(ITransactionContext context)
        {
            this.context = context;
        }

        public void SaveTransactions(IEnumerable<Transaction> transactions)
        {
            foreach (var t in transactions)
            {
                context.Transactions.Add(t);
            }

            context.SaveChanges();
        }

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

        public IEnumerable<Transaction> GetLastTransactions(int amountOfDays)
        {
            return new List<Transaction>();
        }
    }
}
