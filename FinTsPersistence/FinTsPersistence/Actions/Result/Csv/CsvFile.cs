using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.IO;

namespace FinTsPersistence.Actions.Result.Csv
{
    public class CsvFile : IDisposable
    {
        char m_chComma = DEFAULTCOMMA;
        char m_chQuote = DEFAULTQUOTE;
        int m_nMaxValues = DEFAULTMAXVALUES;
        Encoding m_aEncoding = Encoding.GetEncoding(1252);
        TextReader m_aReader;
        TextWriter m_aWriter;
        int m_nLine;
        Hashtable m_vHeader;
        CsvParser m_aParser;

        /// <summary>
        /// The default comma character that is used when a new <see cref="CsvFile"/> instance
        /// is created through its default constructor.
        /// </summary>
        public const char DEFAULTCOMMA = ';';

        /// <summary>
        /// The default quote character that is used when a new <see cref="CsvFile"/> instance
        /// is created through its default constructor.
        /// </summary>
        public const char DEFAULTQUOTE = '"';

        /// <summary>
        /// The default size of the <see cref="CsvValues"/> when a new <see cref="CsvFile"/>
        /// instance is created through its default constructor.
        /// </summary>
        public const int DEFAULTMAXVALUES = 256;

        public CsvFile()
        {
        }

        /// <summary>
        /// Simplified constructor. By default quotes are used only when needed.
        /// <see cref="CsvFile(char,char,int,bool)"/>.
        /// </summary>
        public CsvFile(char chComma, char chQuote, int nMaxValues)
            : this(chComma, chQuote, nMaxValues, false)
        {
        }

        /// <summary>
        /// Extended constructor for creating a new CsvFile object.
        /// </summary>
        /// <param name="chComma">
        /// Character to be used as the logical comma in the CSV file. This usually is either
        /// a semicolon ';' or a real comma ','.
        /// </param>
        /// <param name="chQuote">
        /// Character to be used to quote strings. This are usually ordinary double quotes '"'.
        /// </param>
        /// <param name="nMaxValues">
        /// The max number of values on a single line in the CSV file. When the CSV file is
        /// written, each line will have exactly this number of values. When the CSV file is
        /// read, a line may have more values, but only the first nMaxValues are returned.
        /// </param>
        /// <param name="fAlwaysUseQuotes">
        /// See the documentation for the <see cref="AlwaysUseQuotes"/> property for details.
        /// </param>
        public CsvFile(char chComma, char chQuote, int nMaxValues, bool fAlwaysUseQuotes)
        {
            Debug.Assert(nMaxValues > 0);

            m_chComma = chComma;
            m_chQuote = chQuote;
            m_nMaxValues = nMaxValues;
            AlwaysUseQuotes = fAlwaysUseQuotes;
            // m_aEncoding = Encoding.GetEncoding(1252);
            // m_aReader = null;
            // m_aWriter = null;
            // m_nLine = 0;
            // m_vHeader = null;
        }

        void IDisposable.Dispose()
        {
            Close();
        }

        public char Comma
        {
            get
            {
                return m_chComma;
            }
            set
            {
                m_chComma = value;
            }
        }

        public char Quote
        {
            get
            {
                return m_chQuote;
            }
            set
            {
                m_chQuote = value;
            }
        }

        /// <summary>
        /// The number of values on a single line in the CSV file.
        /// </summary>
        public int MaxValues
        {
            get
            {
                return m_nMaxValues;
            }
            set
            {
                m_nMaxValues = value;
            }
        }

        public Encoding Encoding
        {
            get
            {
                return m_aEncoding;
            }
            set
            {
                m_aEncoding = value;
            }
        }

        /// <summary>
        /// When this property is set to true, then quotes are used to write
        /// each value and each header to the CSV file. When it is set to
        /// false, then quotes are used only when needed.
        /// </summary>
        public bool AlwaysUseQuotes { get; set; }

        public void Open(string sFilename)
        {
            Debug.Assert(sFilename != null);
            Debug.Assert(sFilename.Length > 0);
            Debug.Assert(m_aReader == null);
            Debug.Assert(m_aWriter == null);

            Open(new StreamReader(sFilename, m_aEncoding));
        }

        public void Open(TextReader aReader)
        {
            m_aParser = new CsvParser(this);
            m_aReader = aReader;
            m_nLine = 0;
        }

        public void Create(string sFilename)
        {
            Debug.Assert(sFilename != null);
            Debug.Assert(sFilename.Length > 0);
            Debug.Assert(m_aReader == null);
            Debug.Assert(m_aWriter == null);

            m_aWriter = new StreamWriter(sFilename, false, m_aEncoding)
                            {
                                NewLine = "\r\n"
                            };

            // TODO: To produce a consistent result on all platforms we enforce usage of the
            // Windows CRLF sequence, perhaps overriding the single LF line terminator which
            // is the default on other platforms.

            m_nLine = 0;
        }

        public void Create(TextWriter aWriter)
        {
            m_aWriter = aWriter;
            m_nLine = 0;
        }

        public CsvValues CreateValues()
        {
            return new CsvValues(this, m_nMaxValues);
        }

        /// <summary>
        /// Returns the positional index of a named column.
        /// </summary>
        /// <param name="sColName"></param>
        /// <returns>
        /// If the column name is not known, then -1 is returned. Otherwise the index
        /// of the column is returned.
        /// </returns>
        /// <remarks>
        /// This method works only after reading or writing a header with
        /// <see cref="ReadHeader"/> or <see cref="WriteHeader"/>.
        /// </remarks>
        public int IndexOf(string sColName)
        {
            Debug.Assert(sColName != null);
            Debug.Assert(sColName != "");

            if (m_vHeader != null)
            {
                object aValue = m_vHeader[sColName];
                if (aValue != null)
                {
                    return (int)aValue;
                }
                return -1;
            }
            return -1;
        }

        public void SetHeader(CsvValues aCsvHeader)
        {
            m_nMaxValues = aCsvHeader.ActualValues;
            _BuildHeaderHashtable(aCsvHeader);
        }

        /// <summary>
        /// Successfully reading the CSV header creates an internal hashtable that associates
        /// the column indices with their column names. In addition, the
        /// <see cref="MaxValues"/> property is adjusted to the actual number of values that
        /// were found in the header line.
        /// </summary>
        public CsvValues ReadHeader()
        {
            Debug.Assert(m_aReader != null);
            Debug.Assert(m_aWriter == null);

            CsvValues aCsvHeader = new CsvValues(this, m_nMaxValues);
            int nMaxValues = _ReadValues(aCsvHeader);
            if (nMaxValues <= 0)
            {
                throw new FormatException("Csv");
            }

            SetHeader(aCsvHeader);

            return aCsvHeader;
        }

        public CsvValues ReadLine()
        {
            Debug.Assert(m_aReader != null);
            Debug.Assert(m_aWriter == null);

            CsvValues aCsv = new CsvValues(this, m_nMaxValues);
            int nValueCount = _ReadValues(aCsv);
            return nValueCount < 0 ? null : aCsv;
        }

        /// <summary>
        /// Writing the CSV header creates an internal hashtable that associates the column
        /// indices with their column names.
        /// </summary>
        public void WriteHeader(CsvValues aCsvHeader)
        {
            Debug.Assert(m_aReader == null);
            Debug.Assert(m_aWriter != null);
            Debug.Assert(aCsvHeader.File == this);

            _BuildHeaderHashtable(aCsvHeader);
            string sLine = aCsvHeader.ToString(m_chComma, m_chQuote);
            m_aWriter.WriteLine(sLine);
        }

        public void WriteLine(CsvValues aCsv)
        {
            Debug.Assert(m_aReader == null);
            Debug.Assert(m_aWriter != null);
            Debug.Assert(aCsv.File == this);

            string sLine = aCsv.ToString(m_chComma, m_chQuote);
            m_aWriter.WriteLine(sLine);
        }

        public void Close()
        {
            if (m_aReader != null)
            {
                m_aReader.Close();
                m_aReader = null;
            }
            if (m_aWriter != null)
            {
                m_aWriter.Close();
                m_aWriter = null;
            }
        }

        /// <summary>
        /// The current line number.
        /// </summary>

        public int Line
        {
            get
            {
                return m_nLine;
            }
        }

        private void _BuildHeaderHashtable(CsvValues aCsvHeader)
        {
            m_vHeader = new Hashtable();
            for (int i = 0; i < aCsvHeader.MaxValues; ++i)
            {
                string sColName = aCsvHeader[i];
                if (!string.IsNullOrEmpty(sColName))
                {
                    // Before we add the column name, we check whether the same column name
                    // was already used. If so, then we generate an unique column name by
                    // appending a number.

                    string sBaseName = sColName;
                    int n = 2;

                    while (m_vHeader.ContainsKey(sColName))
                    {
                        sColName = sBaseName + n.ToString(CultureInfo.InvariantCulture);
                        ++n;
                    }

                    // Now we have a unique column name and thus can safely add the header
                    // entry.

                    m_vHeader.Add(sColName, i);
                }
            }
        }

        private int _ReadValues(CsvValues vValues)
        {
            // Read one CSV line from the StreamReader. If we encounter the end of the
            // StreamReader, then -1 is returned. Only if a non-empty line was read, then we
            // update the line number.

            int nValueCount = m_aParser.ReadLine(m_aReader, vValues);
            if (nValueCount > 0)
            {
                ++m_nLine;
            }

            return nValueCount;
        }
    }
}
