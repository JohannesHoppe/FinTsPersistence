using System.Collections.Specialized;
using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;

namespace FinTsPersistence.Interfaces
{
    public interface IFinTsPersistence
    {
        ActionResult DoAction(string action, StringDictionary arguments);
    }
}