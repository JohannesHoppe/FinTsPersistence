// --------------------------------------------------------------------------------------------
//	FinCmdXml.cs
//	Subsembly FinTS API
//	Copyright © 2004-2006 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using Subsembly.FinTS;
using Subsembly.Swift;

namespace FinCmd
{
	public class FinCmdXml : FinCmdBase
	{
		XmlDocument						m_aXmlDocument;
		FinTransmogrifierRepository		m_aSyntax;

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
				Console.Error.WriteLine("XML Datei {0} nicht gefunden!", sFileName);
				return false;
			}

			m_aXmlDocument = new XmlDocument();
			m_aXmlDocument.Load(sFileName);

			m_aSyntax = _GetSyntax();

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		protected override FinOrder OnCreateOrder(FinService aService)
		{
			// Egal was der Aufrufer eingestellt hat. Als Auftragskonto stellen wir das Konto
			// der Bankverbindung ein.

			XmlElement xmlOrderingCustAcct = m_aXmlDocument.DocumentElement["OrderingCustAcct"];
			if (xmlOrderingCustAcct == null)
			{
				xmlOrderingCustAcct = m_aXmlDocument.CreateElement("OrderingCustAcct");
				m_aXmlDocument.DocumentElement.PrependChild(xmlOrderingCustAcct);
			}

			FinAcct aAcct = aService.GetAcct();
			XmlElement xml;

			xml = m_aXmlDocument.CreateElement("AcctNo");
			xml.InnerText = aAcct.AcctNo;
			xmlOrderingCustAcct.AppendChild(xml);

			if (aAcct.SubAcctCharacteristic != null)
			{
				xml = m_aXmlDocument.CreateElement("SubAcctCharacteristic");
				xml.InnerText = aAcct.SubAcctCharacteristic;
				xmlOrderingCustAcct.AppendChild(xml);
			}

			XmlElement xmlBankID = m_aXmlDocument.CreateElement("BankID");
			xmlOrderingCustAcct.AppendChild(xmlBankID);

			xml = m_aXmlDocument.CreateElement("CountryCode");
			xml.InnerText = aAcct.CountryCode;
			xmlBankID.AppendChild(xml);

			xml = m_aXmlDocument.CreateElement("BankCode");
			xml.InnerText = aAcct.BankCode;
			xmlBankID.AppendChild(xml);

			//

			FinXmlOrderBuilder aXmlOrderBuilder = new FinXmlOrderBuilder(aService.Contact, m_aSyntax);
			return aXmlOrderBuilder.Build(m_aXmlDocument.DocumentElement);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>

		protected override string OnGetResponseData(FinService aService, FinOrder aOrder)
		{
			FinXmlOrder aXmlOrder = aOrder as FinXmlOrder;

			if (aXmlOrder == null)
			{
				return null;
			}

			XmlDocument aResponseXml = aXmlOrder.ResponseXml;
			if (aResponseXml == null)
			{
				return null;
			}

			using (StringWriter aStringWriter = new StringWriter())
			{
				XmlTextWriter aXmlWriter = new XmlTextWriter(aStringWriter);
				aXmlWriter.Formatting = Formatting.Indented;
				aResponseXml.WriteContentTo(aXmlWriter);
				aXmlWriter.Flush();
				aStringWriter.Flush();
				return aStringWriter.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>

		private FinTransmogrifierRepository _GetSyntax()
		{
			FinTransmogrifierRepository aSyntax = new FinTransmogrifierRepository();

			Assembly aThisAssembly = Assembly.GetExecutingAssembly();
			using (Stream aStream = aThisAssembly.GetManifestResourceStream("Subsembly.syntax.xml"))
			{
				aSyntax.Load(aStream);
				aStream.Close();
			}

			return aSyntax;
		}
	}
}
