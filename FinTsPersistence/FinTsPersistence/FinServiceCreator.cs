using System;
using System.Collections.Specialized;
using Subsembly.FinTS;

namespace FinTsPersistence
{
    /// <summary>
    /// Als nächstes ermitteln wir das Konto für die gewünschte Bankverbindung.
    /// </summary>
    public class FinServiceCreator
    {
        public static FinService GetFinService(FinContact contact, FinDialog dialog, StringDictionary arguments)
        {
            if (contact == null)
            {
                throw new ArgumentNullException("contact");
            } 

            string acctBankCode = arguments["-acctbankcode"] ?? contact.BankCode;
            if (acctBankCode == null)
            {
                throw new ArgumentException("Bankleitzahl zu Konto fehlt!");
            }

            if (acctBankCode.Length != 8 || !FinUtil.IsDigits(acctBankCode))
            {
                throw new ArgumentException("Bankleitzahl zu Konto ist ungültig!");
            }

            string acctNo = arguments["-acctno"];
            if (acctNo == null)
            {
                throw new ArgumentException("Kontonummer fehlt!");
            }

            if (acctNo == "" || acctNo.Length > 30)
            {
                throw new ArgumentException("Kontonummer ist ungültig!");
            }

            string acctCurrency = arguments["-acctcurrency"];
            if (acctCurrency == null)
            {
                acctCurrency = "EUR";
            }
            else if (acctCurrency.Length != 3 || !FinUtil.IsUpperAscii(acctCurrency))
            {
                throw new ArgumentException("Kontowährung ist ungültig!");
            }

            // Die Bankverbindung ist jetzt vollständig spezifiziert und wir können ein
            // FinService Objekt dafür anlegen.
            FinService aService = dialog != null ?
                new FinService(dialog, acctBankCode, acctNo, acctCurrency) :
                new FinService(contact, acctBankCode, acctNo, acctCurrency);

            return aService;
        }
    }
}
