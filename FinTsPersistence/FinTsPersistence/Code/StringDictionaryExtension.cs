using System.Collections.Specialized;

namespace FinTsPersistence.Code
{
    public static class StringDictionaryExtension
    {
        public static StringDictionary NewCopy(this StringDictionary olddict)
        {
            var newdict = new StringDictionary();
            foreach (string key in olddict.Keys)
            {
                newdict.Add(key, olddict[key]);
            }
            return newdict;
        }
    }
}
