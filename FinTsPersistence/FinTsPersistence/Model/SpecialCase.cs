namespace FinTsPersistence.Model
{
    /// <summary>
    /// Example how to add an enum
    /// </summary>
    public enum SpecialCase
    {
        /// <summary>
        /// There is now special case
        /// </summary>
        Default,

        /// <summary>
        /// This transaction should be ignored
        /// </summary>
        Ignore,

        /// <summary>
        /// This transaction should be reviewed again later
        /// </summary>
        Clarify
    }
}
