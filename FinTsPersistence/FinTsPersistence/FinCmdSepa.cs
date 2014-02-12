// --------------------------------------------------------------------------------------------
//	FinCmdSepa.cs
//	Subsembly FinTS API
//	Copyright © 2004-2008 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;

using Subsembly.FinTS;
using Subsembly.Sepa;
using Subsembly.Swift;

namespace FinTsPersistence
{
	public class FinCmdSepa : FinCmdBase
	{
		SepaDocument m_aSepaDoc;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		protected override bool OnParse(string sAction, StringDictionary vsArgsDict)
		{
			string sFileName = vsArgsDict["-xmlfile"];
			if (sFileName == null)
			{
				Console.Error.WriteLine("Argument -xmlfile muss angegeben werden!");
				return false;
			}
			if (!File.Exists(sFileName))
			{
				Console.Error.WriteLine("SEPA Datei {0} nicht gefunden!", sFileName);
				return false;
			}

			m_aSepaDoc = SepaDocument.NewDocument(sFileName);

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		protected override FinOrder OnCreateOrder(FinService aService)
		{
			FinAcct aAcct = aService.GetAcct();

			// Depending on the type of the SEPA document a suitable HBCI segment type is
			// determined. This is done with the help of a very convenient property of the
			// SepaDocument class.

			FinSepaOrderBuilder aSepaOrderBuilder = null;

			switch (m_aSepaDoc.HbciSegmentType)
			{
			case "HKCCS":
				aSepaOrderBuilder = new FinSepaSingRemittBuilder(aService.Contact);
				break;
			case "HKCSE":
				aSepaOrderBuilder = new FinSepaSubmitPostdatedSingRemittBuilder(aService.Contact);
				break;
			case "HKCCM":
				aSepaOrderBuilder = new FinSepaMultRemittBuilder(aService.Contact);
				break;
			case "HKCME":
				aSepaOrderBuilder = new FinSepaSubmitPostdatedMultRemittBuilder(aService.Contact);
				break;
			case "HKDSE":
				aSepaOrderBuilder = new FinSepaSubmitPostdatedSingDirDebBuilder(aService.Contact);
				break;
			case "HKDSC":
				aSepaOrderBuilder = new FinSepaSubmitPostdatedCor1SingDirDebBuilder(aService.Contact);
				break;
			case "HKBSE":
				aSepaOrderBuilder = new FinSepaSubmitPostdatedBusinessSingDirDebBuilder(aService.Contact);
				break;
			case "HKDME":
				aSepaOrderBuilder = new FinSepaSubmitPostdatedMultDirDebBuilder(aService.Contact);
				break;
			case "HKDMC":
				aSepaOrderBuilder = new FinSepaSubmitPostdatedCor1MultDirDebBuilder(aService.Contact);
				break;
			case "HKBME":
				aSepaOrderBuilder = new FinSepaSubmitPostdatedBusinessMultDirDebBuilder(aService.Contact);
				break;
			}

			// If no suitable segment type could be determined, or the required segment type is
			// not supported by the bank, then bail out.

			if (aSepaOrderBuilder == null)
			{
				Console.Error.WriteLine("Keine HBCI-Auftragsart für SEPA-Dokument bekannt!");
				return null;
			}
			if (!aSepaOrderBuilder.IsSupported)
			{
				Console.Error.WriteLine("HBCI-Auftragsart " + m_aSepaDoc.HbciSegmentType + " von Bank nicht unterstützt!");
				return null;
			}

			if (aSepaOrderBuilder.FindSepaFormat(m_aSepaDoc.MessageInfo.PainIdentifier) == null)
			{
				Console.Error.WriteLine("SEPA-Format " + m_aSepaDoc.MessageInfo.PainIdentifier + " von Bank nicht unterstützt!");
				return null;
			}

			//

			return aSepaOrderBuilder.Build(aAcct, m_aSepaDoc);
		}
	}
}