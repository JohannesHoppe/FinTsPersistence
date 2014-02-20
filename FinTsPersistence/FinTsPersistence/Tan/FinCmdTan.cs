using FinTsPersistence.Interfaces;
using Subsembly.FinTS;

namespace FinTsPersistence.Tan
{
    public class FinCmdTan : IFinCmdTanSource
    {
        readonly string m_sTAN;

        public FinCmdTan(string sTAN)
        {
            m_sTAN = sTAN;
        }

        public string GetTan(FinService aService)
        {
            return m_sTAN;
        }
    }
}
