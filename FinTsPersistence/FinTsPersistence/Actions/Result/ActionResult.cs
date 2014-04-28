namespace FinTsPersistence.Actions.Result
{
    /// <summary>
    /// All received information regarding one action
    /// </summary>
    public class ActionResult
    {
        public ActionResult(Status status, int orderStatusCode = 0)
        {
            Status = status;
            OrderStatusCode = orderStatusCode;
        }

        /// <summary>
        /// Indicates a problem that happened even if all input data was set up correctly
        /// (otherwise an ArgumentException would have been happening before) 
        /// </summary>
        public Status Status { private set; get; }

        /// <summary>
        /// Als Rückgabewert wird der höchste Rückmeldecode aus dem HIRMS genommen.
        /// Wurde kein HIRMS übermittelt wird als Rückgabewert 0 eingesetzt.
        /// </summary>
        public int OrderStatusCode { private set; get; }

        /// <summary>
        /// Data from the actions
        /// </summary>
        public ResponseData Response { set; get; }
    }
}
