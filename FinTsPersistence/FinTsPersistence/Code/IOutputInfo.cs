namespace FinTsPersistence.Code
{
    /// <summary>
    /// Wrapper for the standard error output stream.
    /// </summary>
    public interface IOutputInfo
    {
        /// <summary>
        /// Writes a string followed by a line terminator to the text string or stream.
        /// </summary>
        /// <value>The string to write. If value is null, only the line terminator is written.</value>
        void WriteLine(string value);
    }
}