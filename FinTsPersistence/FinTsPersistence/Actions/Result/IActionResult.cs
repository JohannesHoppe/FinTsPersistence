namespace FinTsPersistence.Actions.Result
{
    /// <summary>
    /// All received information regarding one action
    /// </summary>
    public interface IActionResult
    {
        /// <summary>
        /// Indicates a problem that happened even if all input data was set up correctly
        /// (otherwise an ArgumentException would have been happening before) 
        /// </summary>
        Status Status { get; }

        /// <summary>
        /// Als Rückgabewert wird der höchste Rückmeldecode aus dem HIRMS genommen.
        /// Wurde kein HIRMS übermittelt wird als Rückgabewert 0 eingesetzt.
        /// </summary>
        int OrderStatusCode { get; }

        /// <summary>
        /// Data from the actions
        /// </summary>
        ResponseData Response { set; get; }
    }
}