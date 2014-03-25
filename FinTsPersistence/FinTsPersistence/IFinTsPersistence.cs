using System.Collections.Specialized;
using FinTsPersistence.Actions.Result;

namespace FinTsPersistence
{
    public interface IFinTsPersistence
    {
        ActionResult DoAction(string action, StringDictionary arguments);
    }
}