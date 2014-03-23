using System.Collections.Specialized;
using FinTsPersistence.Actions;
using Subsembly.FinTS;

namespace FinTsPersistence.Interfaces
{
    public interface IAction
    {
        bool Parse(string action, StringDictionary arguments);

        ActionResult Execute(FinService service, ITanSource tanSource);

        string GetResponseData(FinService aService);
    }
}
