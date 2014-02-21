using System.Collections.Specialized;

namespace FinTsPersistence.Interfaces
{
    public interface ITanSourceFactory
    {
        ITanSource GetTanSource(StringDictionary arguments);
    }
}