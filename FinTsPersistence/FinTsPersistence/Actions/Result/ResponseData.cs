using System.Collections.Generic;

namespace FinTsPersistence.Actions.Result
{
    public class ResponseData
    {
        public ResponseData()
        {
            Transactions = new List<FintTsTransaction>();  
        }

        /// <summary>
        /// (Lazy) Formatted Data from all actions except 'ActionPersist'
        /// </summary>
        public string Formatted { get; set; }

        /// <summary>
        /// Data from action 'ActionPersist'
        /// </summary>
        public List<FintTsTransaction> Transactions { get; set; }           
    }
}
