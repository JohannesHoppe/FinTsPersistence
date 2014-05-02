using System.Collections.Generic;

namespace FinTsPersistence.Actions.Result
{
    public interface IResponseData
    {
        /// <summary>
        /// (Lazy) Formatted Data from all actions except 'ActionPersist'
        /// </summary>
        string Formatted { get; set; }

        /// <summary>
        /// Data from action 'ActionPersist'
        /// </summary>
        List<FinTsTransaction> Transactions { get; set; }
    }
}