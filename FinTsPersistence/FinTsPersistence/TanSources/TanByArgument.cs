using FinTsPersistence.Interfaces;
using Subsembly.FinTS;

namespace FinTsPersistence.TanSources
{
    /// <summary>
    /// Just holds a tan string directly
    /// </summary>
    public class TanByArgument : ITanSource
    {
        readonly string tan;

        public TanByArgument(string tan)
        {
            this.tan = tan;
        }

        public string GetTan(FinService aService)
        {
            return tan;
        }
    }
}
