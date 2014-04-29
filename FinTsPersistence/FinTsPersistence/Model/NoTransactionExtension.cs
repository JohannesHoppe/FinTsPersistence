namespace FinTsPersistence.Model
{
    public static class NoTransactionExtension
    {
        public static bool IsNoTransaction(this Transaction transaction)
        {
            return transaction.TransactionId == NoTransaction.NoTransactionId;
        }
    }
}
