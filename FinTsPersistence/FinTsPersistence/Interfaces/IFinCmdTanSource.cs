using Subsembly.FinTS;

namespace FinTsPersistence.Interfaces
{
    public interface IFinCmdTanSource
    {
        string GetTan(FinService aService);
    }
}
