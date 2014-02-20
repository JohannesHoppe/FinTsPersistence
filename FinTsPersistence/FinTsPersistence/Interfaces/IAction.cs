using System.Collections.Specialized;
using Subsembly.FinTS;

namespace FinTsPersistence.Interfaces
{
    public interface IAction
    {
        bool Parse(string sAction, StringDictionary vsArgsDict);

        int Execute(FinService aService, IFinCmdTanSource aTanSource);

        string GetResponseData(FinService aService);
    }
}
