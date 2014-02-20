using System.Collections.Specialized;

namespace FinTsPersistence.Interfaces
{
    public interface IFinTsPersistence
    {
        int DoAction(string sAction, StringDictionary vsArgsDict);
    }
}