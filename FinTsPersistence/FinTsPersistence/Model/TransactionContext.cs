using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FinTsPersistence.Model
{
    public class TransactionContext : DbContext
    {

        public TransactionContext()
            : base("TransactionContext")
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
