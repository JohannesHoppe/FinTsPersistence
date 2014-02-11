// --------------------------------------------------------------------------------------------
//	FinCmdBalance.cs
//	Subsembly FinTS API
//	Copyright © 2004-2006 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

using Subsembly.FinTS;
using Subsembly.Swift;

namespace FinCmd
{
	public class FinCmdBalance : FinCmdBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		protected override bool OnParse(string sAction, StringDictionary vsArgsDict)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		protected override FinOrder OnCreateOrder(FinService aService)
		{
			return aService.CreateBalance(false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>

		protected override string OnGetResponseData(FinService aService, FinOrder aOrder)
		{
			FinAcctBal aAcctBal = aOrder as FinAcctBal;

			if ((aAcctBal == null) || (aAcctBal.AcctBals == null))
			{
				return null;
			}

			StringBuilder sb = new StringBuilder(2000);

			sb.Append("BalanceType;BankCode;AcctNo;Date;Currency;Value");
			sb.Append(Environment.NewLine);

			foreach (FinAcctBalResp aBal in aAcctBal.AcctBals)
			{
				_AppendBalance(aBal, sb);
			}

			return sb.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aBal"></param>
		/// <param name="sb"></param>

		private void _AppendBalance(FinAcctBalResp aBal, StringBuilder sb)
		{
			FinAcct aAcct = aBal.Account;
			SwiftBalance aCurrentBal = aBal.CurrentBal;
			SwiftBalance aPendingBal = aBal.IncludingPendingTransBal;

			if (aCurrentBal != null)
			{
				sb.AppendFormat("BOOKED;{0};{1};{2};{3};{4}",
					aAcct.BankCode,
					aAcct.AcctNo,
					aCurrentBal.Date.ToString(SwiftDateFormat.StandardDate),
					aCurrentBal.Currency,
					SwiftAmt.Format(aCurrentBal.DecValue, ',', 2));
				sb.Append(Environment.NewLine);
			}

			if (aPendingBal != null)
			{
				sb.AppendFormat("FORWARD;{0};{1};{2};{3};{4}",
					aAcct.BankCode,
					aAcct.AcctNo,
					aPendingBal.Date.ToString(SwiftDateFormat.StandardDate),
					aPendingBal.Currency,
					SwiftAmt.Format(aPendingBal.DecValue, ',', 2));
				sb.Append(Environment.NewLine);
			}
		}
	}
}