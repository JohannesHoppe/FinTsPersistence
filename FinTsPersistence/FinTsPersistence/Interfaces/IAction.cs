using System.Collections.Specialized;
using FinTsPersistence.Actions.Result;
using Subsembly.FinTS;

namespace FinTsPersistence.Interfaces
{
    public interface IAction
    {
        bool Parse(string action, StringDictionary arguments);

        ActionResult Execute(FinService service, ITanSource tanSource);

        ResponseData GetResponseData(FinService aService);
    }
}
