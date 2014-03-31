using System;

namespace FinTsPersistence.Model
{
    /// <summary>
    /// NULL-Object for Transaction
    /// Checks against properties for that object must should result to false!
    /// </summary>
    public class NoTransaction : Transaction
    {
        public const int NoTransactionId = -1;

        public NoTransaction()
        {
            TransactionId = NoTransactionId;
            EntryDate = DateTime.MaxValue;
            ValueDate = DateTime.MaxValue;
        }
    }
}
