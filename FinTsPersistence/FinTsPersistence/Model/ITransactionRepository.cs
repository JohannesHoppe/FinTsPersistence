using System.Collections.Generic;

namespace FinTsPersistence.Model
{
    public interface ITransactionRepository
    {
        void SaveTransactions(IEnumerable<Transaction> transactions);

        Transaction GetLastTransaction();

        IEnumerable<Transaction> GetLastTransactions(int amountOfDays);
    }
}