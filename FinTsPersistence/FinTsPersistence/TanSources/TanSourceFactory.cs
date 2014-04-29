using System.Collections.Specialized;
using FinTsPersistence.Code;

namespace FinTsPersistence.TanSources
{
    /// <summary>
    /// Resolves the best source the receive the required TAN, fallback is a prompt on command line!
    /// </summary>
    public class TanSourceFactory : ITanSourceFactory
    {
        public const string SourceName = "sourceName";

        private readonly IConsole consoleX;

        public TanSourceFactory(IConsole consoleX)
        {
            this.consoleX = consoleX;
        }

        public ITanSource GetTanSource(StringDictionary arguments)
        {
            string tan = arguments["-tan"];
            if (tan != null)
            {
                return new TanByArgument(tan);
            }

            // TAN und TANLIST sollten nicht gleichzeitig angegeben werden!
            // (TANLIST wird ignoriert)
            string tanlist = arguments["-tanlist"];
            if (tanlist != null)
            {
                TanByList aTanByList = new TanByList(consoleX);
                aTanByList.LoadTanList(tanlist);
                return aTanByList;
            }

            return new TanByPrompt(consoleX);
        }
    }
}
