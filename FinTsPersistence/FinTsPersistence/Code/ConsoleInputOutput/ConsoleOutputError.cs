﻿namespace FinTsPersistence.Code.ConsoleInputOutput
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
        public void Write(string value)
        {
            System.Console.Error.WriteLine(value);
        }

        /// <summary>
        /// Writes a formatted string and a new line to the text string or stream, using the same semantics as the System.String.Format(System.String,System.Object) method.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The object to format and write.</param>
        public void Write(string format, object arg0)
        {
            System.Console.Error.WriteLine(format, arg0);
        }

        /// <summary>
        /// Writes a formatted string and a new line to the text string or stream, using the same semantics as the System.String.Format(System.String,System.Object) method.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An object array that contains zero or more objects to format and write.</param>
        public void Write(string format, object[] arg)
        {
            System.Console.Error.WriteLine(format, arg);
        }
    }
}