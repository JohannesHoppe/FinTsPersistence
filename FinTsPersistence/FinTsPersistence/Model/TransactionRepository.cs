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

        public void GetLastTransactions(int amountOfDays)
        {
            
        }
    }
}
