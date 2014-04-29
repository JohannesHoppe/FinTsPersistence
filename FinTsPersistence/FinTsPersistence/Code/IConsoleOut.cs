namespace FinTsPersistence.Code
{
    /// <summary>
    /// Wrapper for the standard output stream.
    /// </summary>
    public interface IConsoleOut
    {
        /// <summary>
        /// Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.
        /// </summary>
        void Flush();
    }
}