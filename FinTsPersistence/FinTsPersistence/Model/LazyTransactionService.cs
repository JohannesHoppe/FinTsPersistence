using System.Collections.Specialized;
using FinTsPersistence.Actions.Result;

namespace FinTsPersistence.Model
{
    /// <summary>
    /// This is the first attempt to synchronize to database
    /// The LAZY transaction service tries to avoid duplicate-checking by only recognizing transactions from the last day
    /// This strategy assumes that historic data never changes again!
    /// </summary>
    public class LazyTransactionService : ITransactionService
    {
        public LazyTransactionService(IFinTsService finTsService)
        {
            
        }

        public ActionResult DoPersistence(StringDictionary argument)
        {
            throw new System.NotImplementedException();
        }
    }
}
