using System.Collections.Specialized;
using FinTsPersistence.Code;
using Subsembly.FinTS;
using Subsembly.Swift;

namespace FinTsPersistence.Actions
{
    public class ActionRemittDebit : ActionBase
    {
        public const string ActionName = "remitt";

        private FinRemitt m_aRemitt;

        public ActionRemittDebit(IInputOutput io) : base(io) { }

        protected override bool OnParse(string action, StringDictionary arguments)
        {
            string sPayeeName = arguments["-payeename"];
            if (string.IsNullOrEmpty(sPayeeName))
            {
                Io.Error.Write("Parameter -payeename fehlt!");
                return false;
            }

            string sPayeeAcctNo = arguments["-payeeacctno"];
            if (string.IsNullOrEmpty(sPayeeAcctNo))
            {
                Io.Error.Write("Parameter -payeeacctno fehlt!");
                return false;
            }

            string sPayeeBankCode = arguments["-payeebankcode"];
            if (string.IsNullOrEmpty(sPayeeBankCode))
            {
                Io.Error.Write("Parameter -payeebankcode fehlt!");
                return false;
            }

            decimal dAmount = 0M;
            try
            {
                dAmount = SwiftAmt.Parse(arguments["-amount"]);
            }
            catch { /* IGNORE */ }
            if (dAmount == 0M)
            {
                Io.Error.Write("Parameter -amount fehlt oder fehlerhaft!");
                return false;
            }

            string sTextKey = arguments["-textkey"];
            string sTextKeyExt = arguments["-textkeyext"];
            string[] vsPurpose = arguments["-purpose"].Split('|');

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
