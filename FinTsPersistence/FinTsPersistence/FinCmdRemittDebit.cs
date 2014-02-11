// --------------------------------------------------------------------------------------------
//	FinCmdRemittDebit.cs
//	Subsembly FinTS API
//	Copyright © 2004-2014 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;

using Subsembly.FinTS;
using Subsembly.Swift;

namespace FinCmd
{
	/// <summary>
	/// 
	/// </summary>

	public class FinCmdRemittDebit : FinCmdBase
	{
		string m_sAction;
		FinRemitt m_aRemitt;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		protected override bool OnParse(string sAction, StringDictionary vsArgsDict)
		{
			m_sAction = sAction;

			string sPayeeName = vsArgsDict["-payeename"];
			if ((sPayeeName == null) || (sPayeeName == ""))
			{
				Console.Error.WriteLine("Parameter -payeename fehlt!");
				return false;
			}

			string sPayeeAcctNo = vsArgsDict["-payeeacctno"];
			if ((sPayeeAcctNo == null) || (sPayeeAcctNo == ""))
			{
				Console.Error.WriteLine("Parameter -payeeacctno fehlt!");
				return false;
			}

			string sPayeeBankCode = vsArgsDict["-payeebankcode"];
			if ((sPayeeBankCode == null) || (sPayeeBankCode == ""))
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

			m_aRemitt = new FinRemitt();
			m_aRemitt.PayeePayerAcct = new FinAcct(sPayeeAcctNo, "280", sPayeeBankCode);
			m_aRemitt.PayeePayerName1 = sPayeeName;
			m_aRemitt.Amount = new SwiftAmt(dAmount, "EUR");
			m_aRemitt.TextKey = sTextKey;
			m_aRemitt.TextKeyExt = sTextKeyExt;
			m_aRemitt.PaymtPurpose = vsPurpose;

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		protected override FinOrder OnCreateOrder(FinService aService)
		{
			return aService.CreateRemitt(m_aRemitt);
		}
	}
}
