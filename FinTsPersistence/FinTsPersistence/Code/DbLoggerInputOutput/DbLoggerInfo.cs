using System;
using System.Globalization;

namespace FinTsPersistence.Code.DbLoggerInputOutput
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
        public void Write(string value)
        {
            logger.WriteToDB(value, MessageSeverity.Info);
        }

        /// <summary>
        /// Writes a formatted string to the DB, using the same semantics as the System.String.Format(System.String,System.Object) method.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The object to format and write.</param>
        public void Write(string format, object arg0)
        {
            logger.WriteToDB(
                String.Format(CultureInfo.InvariantCulture, format, arg0), MessageSeverity.Info);
        }

        /// <summary>
        /// Writes a formatted string to the DB, using the same semantics as the System.String.Format(System.String,System.Object) method.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An object array that contains zero or more objects to format and write.</param>
        public void Write(string format, object[] arg)
        {
            logger.WriteToDB(
                String.Format(CultureInfo.InvariantCulture, format, arg), MessageSeverity.Info);
        }
    }
}