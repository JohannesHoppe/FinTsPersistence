using System.Data.Entity;

namespace FinTsPersistence.Model
{
    public interface ITransactionContext
    {
        IDbSet<Transaction> Transactions { get; set; }

        int SaveChanges();
    }
}