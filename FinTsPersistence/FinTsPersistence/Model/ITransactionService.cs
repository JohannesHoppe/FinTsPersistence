using System.Collections.Specialized;
using FinTsPersistence.Actions.Result;

namespace FinTsPersistence.Model
{
    public interface ITransactionService
    {
        ActionResult DoPersistence(StringDictionary argument);
    }
}