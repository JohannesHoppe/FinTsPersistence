using Subsembly.FinTS;

namespace FinTsPersistence.Interfaces
{
    public interface ITanSource
    {
        string GetTan(FinService aService);
    }
}
