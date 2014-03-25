using Subsembly.FinTS;

namespace FinTsPersistence.TanSources
{
    public interface ITanSource
    {
        string GetTan(FinService aService);
    }
}
