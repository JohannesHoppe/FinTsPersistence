using System;

namespace FinTsPersistence.Model
{
    /// <summary>
    /// NULL-Object for Transaction
    /// Checks against properties fo that object must always result to false!
    /// </summary>
    public class NoTransaction : Transaction
    {
        public const int NoTransactionId = -1;

        public NoTransaction()
        {
            TransactionId = NoTransactionId;
            EntryDate = DateTime.MinValue;
            ValueDate = DateTime.MinValue;
        }
    }
}
