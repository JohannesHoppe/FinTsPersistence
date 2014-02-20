using System;
using System.Collections.Specialized;
using Subsembly.FinTS;
using Subsembly.Swift;

namespace FinTsPersistence.Actions
{
    public class ActionRemittDebit : ActionBase
    {
        public const string ActionName = "remitt";

        FinRemitt m_aRemitt;

        protected override bool OnParse(string sAction, StringDictionary vsArgsDict)
        {
            string sPayeeName = vsArgsDict["-payeename"];
            if (string.IsNullOrEmpty(sPayeeName))
            {
                Console.Error.WriteLine("Parameter -payeename fehlt!");
                return false;
            }

            string sPayeeAcctNo = vsArgsDict["-payeeacctno"];
            if (string.IsNullOrEmpty(sPayeeAcctNo))
            {
                Console.Error.WriteLine("Parameter -payeeacctno fehlt!");
                return false;
            }

            string sPayeeBankCode = vsArgsDict["-payeebankcode"];
            if (string.IsNullOrEmpty(sPayeeBankCode))
            {
                Console.Error.WriteLine("Parameter -payeebankcode fehlt!");
                return false;
            }

            decimal dAmount = 0M;
            try
            {
                dAmount = SwiftAmt.Parse(vsArgsDict["-amount"]);
            }
            catch { /* IGNORE */ }
            if (dAmount == 0M)
            {
                Console.Error.WriteLine("Parameter -amount fehlt oder fehlerhaft!");
                return false;
            }

            string sTextKey = vsArgsDict["-textkey"];
            string sTextKeyExt = vsArgsDict["-textkeyext"];
            string[] vsPurpose = vsArgsDict["-purpose"].Split('|');

            if (sTextKey == null)
            {
                sTextKey = "51";
            }

            m_aRemitt = new FinRemitt
                            {
                                PayeePayerAcct = new FinAcct(sPayeeAcctNo, "280", sPayeeBankCode),
                                PayeePayerName1 = sPayeeName,
                                Amount = new SwiftAmt(dAmount, "EUR"),
                                TextKey = sTextKey,
                                TextKeyExt = sTextKeyExt,
                                PaymtPurpose = vsPurpose
                            };

            return true;
        }

        protected override FinOrder OnCreateOrder(FinService aService)
        {
            return aService.CreateRemitt(m_aRemitt);
        }
    }
}
