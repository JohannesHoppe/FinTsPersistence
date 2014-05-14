using System;

namespace FinTsPersistence.Code
{
    public class TransactionLog
    {
        public int TransactionLogId { get; set; }

        public MessageSeverity Severity { get; set; }

        public string Message { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
