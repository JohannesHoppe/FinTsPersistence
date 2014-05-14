namespace FinTsPersistence.Code
{
    /// <summary>
    /// Loggs with Severity level info.
    /// </summary>
    public class DbLoggerInfo : IOutputInfo
    {
        private readonly DbLogger logger;

        public DbLoggerInfo(DbLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Writes  a string to the DB.
        /// </summary>
        /// <value>The string to write.</value>
        public void WriteLine(string value)
        {
            logger.WriteToDB(value, MessageSeverity.Info);
        }
    }
}