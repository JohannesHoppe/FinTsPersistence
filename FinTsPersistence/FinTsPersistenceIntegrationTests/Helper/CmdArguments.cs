using System.Xml.Serialization;

namespace FinTsPersistenceIntegrationTests.Helper
{
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class CmdArguments
    {
        public string Pin { get; set; }
        public string Acctno { get; set; }
        public string Acctbankcode { get; set; }
    }
}