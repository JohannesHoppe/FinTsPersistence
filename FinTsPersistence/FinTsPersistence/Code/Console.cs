namespace FinTsPersistence.Code
{
    /// <summary>
    /// Wrapper to make Standard-Console Unit-Testable
    /// </summary>
    public class Console : IInputOutput
    {
        public Console()
        {
            Error = new ConsoleOutputError();
            Info = new ConsoleOutputInfo();
        }

        /// <summary>
        /// Gets the standard error output stream.
        /// </summary>
        public IOutputError Error { get; private set; }

        /// <summary>
        /// Gets a special output stream.
        /// </summary>
        public IOutputInfo Info { get; private set; }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLine(string value) 
        {
            System.Console.WriteLine(value);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        public string ReadLine()
        {
            return System.Console.ReadLine();
        }
    }
}
