using FinTsPersistence.Actions.Result;

namespace FinTsPersistence.Model
{
    /// <summary>
    /// Holds all relevant information about one transaction as well as properties to store it in a database
    /// </summary>
    public class Transaction : FinTsTransaction
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Example how to add own fields to the table
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Example how to add own fields to the table
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// Example how to add own fields to the table
        /// </summary>
        public SpecialCase SpecialCaseId { get; set; }
    }
}
