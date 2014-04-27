using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTsPersistence.Actions
{
    public static class Arguments
    {
        public const string FromDate = "-fromdate";
        public const string ContactFile = "-contactfile";

        /// <summary>
        /// ISO standard date format which is yyyy-MM-dd
        /// </summary>
        public static string ToIsoDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }


    }
}
