using System.Collections.Generic;

namespace FinTsPersistence.Model
{
    public interface ITransactionRepository
    {
        void SaveTransactions(IEnumerable<Transaction> transactions);
        IEnumerable<Transaction> GetLastTransactions(int amountOfDays);
    }
}