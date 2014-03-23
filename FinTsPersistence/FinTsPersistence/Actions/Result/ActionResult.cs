namespace FinTsPersistence.Actions.Result
{
    /// <summary>
    /// All recevied information regarding one action
    /// </summary>
    public class ActionResult
    {
        public ActionResult(Status status, bool success = false, int orderStatusCode = 0)
        {
            Status = status;
            Success = success;
            OrderStatusCode = orderStatusCode;
        }

        /// <summary>
        /// Indicates a problem that happened even if all input data was set up correctly
        /// (otherwise an ArgumentException would have been happening before) 
        /// </summary>
        public Status Status { private set; get; }

        /// <summary>
        /// Indicates of the action was executed successfull
        /// </summary>
        public bool Success { private set; get; }

        /// <summary>
        /// Als Rückgabewert wird der höchste Rückmeldecode aus dem HIRMS genommen.
        /// Wurde kein HIRMS übermittelt wird als Rückgabewert 0 eingesetzt.
        /// </summary>
        public int OrderStatusCode { private set; get; }
    }
}
