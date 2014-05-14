using System;
using FinTsPersistence.Model;

namespace FinTsPersistence.Code
{
    /// <summary>
    /// Non-Interactive replacement for the console, loggs everything to DB
    /// Is not capable to do a ReadLine
    /// Default MessageSeverity: Notice 
    /// </summary>
    public class DbLogger : IInputOutput
    {
        private readonly ITransactionContext context;

        public DbLogger(ITransactionContext context)
        {
            this.context = context;

            Error = new DbLoggerError(this);
            Info = new DbLoggerInfo(this);
        }
        
        public IOutputError Error { get; private set; }

        public IOutputInfo Info { get; private set; }

        public void WriteLine(string value)
        {
            WriteToDB(value);
        }

        public string ReadLine()
        {
            WriteToDB("DbLogger is not capable to do a ReadLine!", MessageSeverity.Error);
            throw new NotImplementedException();
        }

        internal void WriteToDB(string message, MessageSeverity severity = MessageSeverity.Notice)
        {
            context.TransactionLogs.Add(new TransactionLog
            {
                Severity = severity,
                Message = message,
                TimeStamp = DateTime.Now
            });
            context.SaveChanges();
        }
    }
}
