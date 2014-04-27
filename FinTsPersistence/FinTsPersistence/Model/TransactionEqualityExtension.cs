using System.Globalization;

namespace FinTsPersistence.Model
{
    /// <summary>
    /// Very critical aspect of the overall software
    /// This class checks if a transaction is equivalent to another one.
    /// </summary>
    /// <remarks>
    /// The check takes into account that there are reports in the internet that banks sometimes change existing payment purposes and/or do not calculate the balance correctly.
    /// </remarks>
    public static class TransactionEqualityExtension
    {
        public static bool IsVeryEqualTo(this Transaction trans1, Transaction trans2)
        {
            if (trans1.Value !=
                trans2.Value)
            {
                return false;
            }

            if (trans1.EntryDate !=
                trans2.EntryDate)
            {
                return false;
            }

            if (trans1.ValueDate !=
                trans2.ValueDate)
            {
                return false;
            }

            if (trans1.Name.Simplyfy() !=
                trans2.Name.Simplyfy())
            {
                return false;
            }

            if (trans1.PaymentPurpose.Simplyfy() !=
                trans2.PaymentPurpose.Simplyfy())
            {
                return false;
            }

            return true;
        }

        private static string Simplyfy(this string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            return text
                .Trim()
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("ß", "S")
                .ToUpper(CultureInfo.InvariantCulture)
                .Replace("Ü", "U")
                .Replace("Ä", "A")
                .Replace("Ö", "O")
                .Replace("SS", "S")
                .Replace("UE", "U")
                .Replace("AE", "A")
                .Replace("OE", "O");

        }
    }
}
