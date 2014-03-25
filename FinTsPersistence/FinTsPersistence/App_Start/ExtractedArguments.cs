using System.Collections.Specialized;

namespace FinTsPersistence.App_Start
{
    /// <summary>
    /// Holds one argument and an various number of args/argvalue pairs
    /// </summary>
    public class ExtractedArguments
    {
        public ExtractedArguments()
        {
            Arguments = new StringDictionary();
        }

        public string Action { get; set; }

        public StringDictionary Arguments { get; set; }
    }
}
