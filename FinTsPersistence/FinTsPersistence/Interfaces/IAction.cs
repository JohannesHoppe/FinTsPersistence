using System.Collections.Specialized;
using Subsembly.FinTS;

namespace FinTsPersistence.Interfaces
{
    public interface IAction
    {
        bool Parse(string action, StringDictionary arguments);

        int Execute(FinService service, ITanSource tanSource);

        string GetResponseData(FinService aService);

        bool GoOnline { get; }

        bool DoSync { get; }
    }
}
