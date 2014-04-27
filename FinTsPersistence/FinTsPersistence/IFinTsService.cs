using System.Collections.Specialized;
using FinTsPersistence.Actions.Result;

namespace FinTsPersistence
{
    public interface IFinTsService
    {
        ActionResult DoAction(string action, StringDictionary arguments);
    }
}