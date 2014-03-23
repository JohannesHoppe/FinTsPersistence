using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using Subsembly.FinTS;

namespace FinTsPersistence.Actions
{
    public class ActionXml : ActionBase
    {
        public const string ActionName = "xml";

        XmlDocument                        m_aXmlDocument;
        FinTransmogrifierRepository        m_aSyntax;

        protected override bool OnParse(string action, StringDictionary arguments)
        {
            string sFileName = arguments["-xmlfile"];
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

        protected override FinOrder OnCreateOrder(FinService aService)
        {
            // Egal was der Aufrufer eingestellt hat. Als Auftragskonto stellen wir das Konto
            // der Bankverbindung ein.

            Debug.Assert(m_aXmlDocument.DocumentElement != null, "m_aXmlDocument.DocumentElement != null");
            XmlElement xmlOrderingCustAcct = m_aXmlDocument.DocumentElement["OrderingCustAcct"];
            if (xmlOrderingCustAcct == null)
            {
                xmlOrderingCustAcct = m_aXmlDocument.CreateElement("OrderingCustAcct");
                Debug.Assert(m_aXmlDocument.DocumentElement != null, "m_aXmlDocument.DocumentElement != null");
                m_aXmlDocument.DocumentElement.PrependChild(xmlOrderingCustAcct);
            }

            FinAcct aAcct = aService.GetAcct();

            XmlElement xml = m_aXmlDocument.CreateElement("AcctNo");
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

            FinXmlOrderBuilder aXmlOrderBuilder = new FinXmlOrderBuilder(aService.Contact, m_aSyntax);
            return aXmlOrderBuilder.Build(m_aXmlDocument.DocumentElement);
        }

        protected override string OnGetResponseData(FinService service, FinOrder order)
        {
            FinXmlOrder aXmlOrder = order as FinXmlOrder;

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
                XmlTextWriter aXmlWriter = new XmlTextWriter(aStringWriter)
                                               {
                                                   Formatting = Formatting.Indented
                                               };
                aResponseXml.WriteContentTo(aXmlWriter);
                aXmlWriter.Flush();
                aStringWriter.Flush();
                return aStringWriter.ToString();
            }
        }

        private FinTransmogrifierRepository _GetSyntax()
        {
            FinTransmogrifierRepository aSyntax = new FinTransmogrifierRepository();

            Assembly aThisAssembly = Assembly.GetExecutingAssembly();
            using (Stream aStream = aThisAssembly.GetManifestResourceStream("Subsembly.syntax.xml"))
            {
                aSyntax.Load(aStream);
                Debug.Assert(aStream != null, "aStream != null");
                aStream.Close();
            }

            return aSyntax;
        }
    }
}
