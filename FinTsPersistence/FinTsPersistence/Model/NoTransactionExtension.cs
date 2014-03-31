using System;

namespace FinTsPersistence.Model
{
    public static class NoTransactionExtension
    {
        public static bool IsNoTransaction(this Transaction transaction)
        {
            Console.WriteLine("{0} == {1}", transaction.TransactionId, NoTransaction.NoTransactionId);
            return transaction.TransactionId == NoTransaction.NoTransactionId;
        }
    }
}
