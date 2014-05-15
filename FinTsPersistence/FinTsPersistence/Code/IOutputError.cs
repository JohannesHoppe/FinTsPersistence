namespace FinTsPersistence.Code
{
    /// <summary>
    /// Wrapper for the standard error output stream.
    /// </summary>
    public interface IOutputError
    {
        /// <summary>
        /// Writes a string followed by a line terminator to the text string or stream.
        /// </summary>
        /// <value>The string to write. If value is null, only the line terminator is written.</value>
        void Write(string value);

        /// <summary>
        /// Writes a formatted string and a new line to the text string or stream, using the same semantics as the System.String.Format(System.String,System.Object) method.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The object to format and write.</param>
        void Write(string format, object arg0);

        /// <summary>
        /// Writes a formatted string and a new line to the text string or stream, using the same semantics as the System.String.Format(System.String,System.Object) method.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An object array that contains zero or more objects to format and write.</param>
        void Write(string format, object[] arg);
    }
}