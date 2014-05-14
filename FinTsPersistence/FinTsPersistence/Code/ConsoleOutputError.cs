namespace FinTsPersistence.Code
{
    /// <summary>
    /// Wrapper for the standard error output stream.
    /// </summary>
    public class ConsoleOutputError : IOutputError
    {
        /// <summary>
        /// Writes a string followed by a line terminator to the text string or stream.
        /// </summary>
        /// <value>The string to write. If value is null, only the line terminator is written.</value>
        public void WriteLine(string value)
        {
            System.Console.Error.WriteLine(value);
        }

        /// <summary>
        /// Writes a formatted string and a new line to the text string or stream, using the same semantics as the System.String.Format(System.String,System.Object) method.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="arg0">The object to format and write.</param>
        public void WriteLine(string format, object arg0)
        {
            System.Console.Error.WriteLine(format, arg0);
        }
    }
}