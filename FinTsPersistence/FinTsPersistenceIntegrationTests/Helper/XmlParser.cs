using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FintTsPersistenceIntegrationTests.Helper
{
    /// <summary>
    /// Parses some XML content to the given type T
    /// </summary>
    public static class XmlParser
    {
        /// <summary>
        /// Parses some XML content to the given type T
        /// </summary>
        public static T ParseXml<T>(this string xmlContent) where T : class
        {
            var reader = XmlReader.Create(xmlContent.Trim().ToMemoryStream(), 
                new XmlReaderSettings
                    {
                        ConformanceLevel = ConformanceLevel.Document
                    });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }

        /// <summary>
        /// String to Stream
        /// </summary>
        private static MemoryStream ToMemoryStream(this string content)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
