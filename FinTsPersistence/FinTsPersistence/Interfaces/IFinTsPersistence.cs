using System.Collections.Specialized;
using FinTsPersistence.Actions;

namespace FinTsPersistence.Interfaces
{
    public interface IFinTsPersistence
    {
        ActionResult DoAction(string action, StringDictionary arguments);
    }
}