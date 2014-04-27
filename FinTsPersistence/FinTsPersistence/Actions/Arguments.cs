using System;

namespace FinTsPersistence.Actions
{
    public static class Arguments
    {
        public const string FromDate = "-fromdate";
        public const string ContactFile = "-contactfile";
        public const string Pin = "-pin";
        public const string Resume = "-resume";
        public const string AcctNo = "-acctno";
        public const string AcctBankCode = "-acctbankcode";
        public const string Format = "-format";
        public const string Trace = "-trace";
        public const string Suspend = "-suspend";

        /// <summary>
        /// ISO standard date format which is yyyy-MM-dd
        /// </summary>
        public static string ToIsoDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}
