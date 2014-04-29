namespace FinTsPersistence.Code
{
    /// <summary>
    /// Made Unit-Testable
    /// </summary>
    public interface IConsoleX
    {
        /// <summary>
        /// Gets the standard error output stream.
        /// </summary>
        IConsoleError Error { get; }

        /// <summary>
        /// Gets the standard output stream.
        /// </summary>
        IConsoleOut Out { get; }

        /// <summary>
        /// Writes the specified string value to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void Write(string value);

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void WriteLine(string value);

        /// <summary>
        /// Reads the next line of characters from the standard input stream.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        string ReadLine();
    }
}