using System.Data.Entity;
using FinTsPersistence.Code;

namespace FinTsPersistence.Model
{
    public interface ITransactionContext
    {
        IDbSet<Transaction> Transactions { get; set; }

        IDbSet<TransactionLog> TransactionLogs { get; set; }

        int SaveChanges();
    }
}