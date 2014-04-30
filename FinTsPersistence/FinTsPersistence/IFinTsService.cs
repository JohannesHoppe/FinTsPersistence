using System.Collections.Specialized;
using FinTsPersistence.Actions.Result;

namespace FinTsPersistence
{
    public interface IFinTsService
    {
        IActionResult DoAction(string action, StringDictionary arguments);
    }
}