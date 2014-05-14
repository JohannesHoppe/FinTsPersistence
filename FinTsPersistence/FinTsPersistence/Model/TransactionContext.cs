using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FinTsPersistence.Code;

namespace FinTsPersistence.Model
{
    /// <summary>
    /// A mockable data context (EF Code First)
    /// </summary>
    public class TransactionContext : DbContext, ITransactionContext
    {
        public TransactionContext() 
            : base("TransactionContext")
        {
        }

        public IDbSet<Transaction> Transactions { get; set; }

        public IDbSet<TransactionLog> TransactionLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
