using System.Collections.Specialized;

namespace FinTsPersistence.TanSources
{
    public interface ITanSourceFactory
    {
        ITanSource GetTanSource(StringDictionary arguments);
    }
}