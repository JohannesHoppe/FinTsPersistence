using System;

namespace FinTsPersistence.Code
{
    /// <summary>
    /// Wrapper to make Standard-Console Unit-Testable
    /// </summary>
    public class ConsoleX : IConsoleX
    {
        public ConsoleX()
        {
            Error = new ConsoleXError();
            Out = new ConsoleXOut();
        }

        /// <summary>
        /// Gets the standard error output stream.
        /// </summary>
        public IConsoleError Error { get; private set; }

        /// <summary>
        /// Gets the standard output stream.
        /// </summary>
        public IConsoleOut Out { get; private set; }

        /// <summary>
        /// Writes the specified string value to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(string value)
        {
            Console.Write(value);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLine(string value) 
        {
            Console.WriteLine(value);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
