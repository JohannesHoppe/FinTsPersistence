using System.Collections.Specialized;
using FinTsPersistence.Actions.Result;
using FinTsPersistence.TanSources;
using Subsembly.FinTS;

namespace FinTsPersistence.Actions
{
    public interface IAction
    {
        bool Parse(string action, StringDictionary arguments);

        IActionResult Execute(FinService service, ITanSource tanSource);

        ResponseData GetResponseData(FinService aService);
    }
}
