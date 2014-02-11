// --------------------------------------------------------------------------------------------
//	FinCmdStatement.cs
//	Subsembly FinTS API
//	Copyright © 2004-2006 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

using Subsembly.Csv;
using Subsembly.FinTS;
using Subsembly.Swift;

namespace FinCmd
{
	public class FinCmdStatement : FinCmdBase
	{
		enum OutputFormat
		{
			CSV,
			MT940,
			MT942,
			CSV942,
		};

		SwiftDate		m_tFromDate = SwiftDate.NullDate;
		OutputFormat	m_nFormat = OutputFormat.CSV;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		protected override bool OnParse(string sAction, StringDictionary vsArgsDict)
		{
			string sFromDate = vsArgsDict["-fromdate"];
			if (sFromDate != null)
			{
				m_tFromDate = SwiftDate.Parse(sFromDate, SwiftDateFormat.StandardDate);
			}

			string sFormat = vsArgsDict["-format"];
			switch (sFormat)
			{
			case "csv":
				m_nFormat = OutputFormat.CSV;
				break;
			case "mt940":
				m_nFormat = OutputFormat.MT940;
				break;
			case "mt942":
				m_nFormat = OutputFormat.MT942;
				break;
			case "csv942":
				m_nFormat = OutputFormat.CSV942;
				break;
			case null:
				break;
			default:
				return false;
			}

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		protected override FinOrder OnCreateOrder(FinService aService)
		{
			return aService.CreateDownloadStatement(m_tFromDate);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aOrder"></param>
		/// <returns></returns>

		protected override string OnGetResponseData(FinService aService, FinOrder aOrder)
		{
			FinAcctMvmtsSpecifiedPeriod aAcctMvmts = aOrder as FinAcctMvmtsSpecifiedPeriod;

			if (aAcctMvmts == null)
			{
				return null;
			}

			switch (m_nFormat)
			{
			case OutputFormat.CSV:
				return _GetCsvResponseData(aAcctMvmts);
			case OutputFormat.MT940:
				return _GetMT940ResponseData(aAcctMvmts);
			case OutputFormat.MT942:
				return _GetMT942ResponseData(aAcctMvmts);
			case OutputFormat.CSV942:
				return _GetCsv942ResponseData(aAcctMvmts);
			default:
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aAcctMvmts"></param>
		/// <returns></returns>

		private string _GetCsvResponseData(FinAcctMvmtsSpecifiedPeriod aAcctMvmts)
		{
			SwiftStatementReader aMT940 = aAcctMvmts.BookedTrans;
			if (aMT940 == null)
			{
				return null;
			}

			SwiftStatement aStmt = SwiftStatement.ReadMT940(aMT940, true);
			if (aStmt == null)
			{
				return null;
			}

			return _GetCsvStatementData(aStmt);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aAcctMvmts"></param>
		/// <returns></returns>

		private string _GetCsv942ResponseData(FinAcctMvmtsSpecifiedPeriod aAcctMvmts)
		{
			SwiftStatementReader aMT942 = aAcctMvmts.NonbookedTrans;
			if (aMT942 == null)
			{
				return null;
			}

			SwiftInterimTransRept aStmt = SwiftInterimTransRept.ReadMT942(aMT942, true);
			if (aStmt == null)
			{
				return null;
			}

			return _GetCsvStatementData(aStmt);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aStmt"></param>
		/// <returns></returns>

		private string _GetCsvStatementData(Swift9xxBase aStmt)
		{
			StringBuilder sb = new StringBuilder(20000);

			sb.Append("EntryDate;ValueDate;Value;AcctNo;BankCode;Name1;Name2;PaymtPurpose;EntryText;PrimaNotaNo;TranTypeIdCode;ZkaTranCode;TextKeyExt;BankRef;OwnerRef;SupplementaryDetails");
			sb.Append(Environment.NewLine);

			foreach (SwiftStatementLine aStmtLine in aStmt.StatementLines)
			{
				CsvValues aCsv = new CsvValues(16);

				if (!aStmtLine.EntryDate.IsNull)
				{
					aCsv[0] = aStmtLine.EntryDate.ToString(SwiftDateFormat.StandardDate);
				}
				if (!aStmtLine.ValueDate.IsNull)
				{
					aCsv[1] = aStmtLine.ValueDate.ToString(SwiftDateFormat.StandardDate);
				}

				aCsv[2] = SwiftAmt.Format(aStmtLine.DecValue, ',', 2);
				aCsv[3] = aStmtLine.PayeePayerAcctNo;
				aCsv[4] = aStmtLine.PayeePayerBankCode;
				aCsv[5] = aStmtLine.PayeePayerName1;
				aCsv[6] = aStmtLine.PayeePayerName2;

				string[] vsPaymtPurpose = aStmtLine.PaymtPurpose;
				if (vsPaymtPurpose != null)
				{
					aCsv[7] = String.Join("|", vsPaymtPurpose);
				}

				aCsv[8] = aStmtLine.EntryText;
				aCsv[9] = aStmtLine.PrimaNotaNo;
				aCsv[10] = aStmtLine.TranTypeIdCode;
				aCsv[11] = aStmtLine.ZkaTranCode;
				aCsv[12] = aStmtLine.TextKeyExt;
				aCsv[13] = aStmtLine.BankRef;
				aCsv[14] = aStmtLine.OwnerRef;
				aCsv[15] = aStmtLine.SupplementaryDetails;

				sb.Append(aCsv.ToString());
				sb.Append(Environment.NewLine);
			}

			return sb.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aAcctMvmts"></param>
		/// <returns></returns>

		private string _GetMT940ResponseData(FinAcctMvmtsSpecifiedPeriod aAcctMvmts)
		{
			FinByteBuffer aBufMT940 = aAcctMvmts.BookedTransBuffer;
			if (aBufMT940 == null)
			{
				return null;
			}

			return Encoding.GetEncoding(1252).GetString(
				aBufMT940.Bytes, aBufMT940.Offset, aBufMT940.Length);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aAcctMvmts"></param>
		/// <returns></returns>

		private string _GetMT942ResponseData(FinAcctMvmtsSpecifiedPeriod aAcctMvmts)
		{
			FinByteBuffer aBufMT942 = aAcctMvmts.NonbookedTransBuffer;
			if (aBufMT942 == null)
			{
				return null;
			}

			return Encoding.GetEncoding(1252).GetString(
				aBufMT942.Bytes, aBufMT942.Offset, aBufMT942.Length);
		}
	}
}