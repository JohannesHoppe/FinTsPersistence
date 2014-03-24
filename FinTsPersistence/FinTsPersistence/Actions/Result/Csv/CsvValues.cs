using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FinTsPersistence.Actions.Result.Csv
{
    /// <summary>
    /// Contains the CSV values of a single CSV line.
    /// </summary>
    /// <remarks>
    /// Instance of this class must be created through <see cref="CsvFile.CreateValues"/>.
    /// </remarks>
    public class CsvValues
    {
        readonly CsvFile m_aCsvFile;
        readonly string[] m_vValues;
        int m_nActualValues;

        public CsvValues()
        {
            m_aCsvFile = null;
            m_vValues = new string[CsvFile.DEFAULTMAXVALUES];
            m_nActualValues = 0;
        }

        public CsvValues(int nMaxValues)
        {
            m_aCsvFile = null;
            m_vValues = new string[nMaxValues];
            m_nActualValues = 0;
        }

        internal CsvValues(CsvFile aCsvFile, int nMaxValues)
        {
            m_aCsvFile = aCsvFile;
            m_vValues = new string[nMaxValues];
            m_nActualValues = 0;
        }

        public CsvFile File
        {
            get
            {
                return m_aCsvFile;
            }
        }

        public int MaxValues
        {
            get
            {
                return m_vValues.Length;
            }
        }

        public int ActualValues
        {
            get
            {
                return m_nActualValues;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return m_vValues.All(string.IsNullOrEmpty);
            }
        }

        /// <summary>
        /// Random access to values by their positional index.
        /// </summary>

        public string this[int nIndex]
        {
            get
            {
                if ((nIndex < 0) || (nIndex >= m_vValues.Length))
                {
                    throw new IndexOutOfRangeException("CsvValues");
                }

                return m_vValues[nIndex];
            }
            set
            {
                if ((nIndex < 0) || (nIndex >= m_vValues.Length))
                {
                    throw new IndexOutOfRangeException("CsvValues");
                }

                m_vValues[nIndex] = value;
            }
        }

        /// <summary>
        /// Random access to values by their name.
        /// </summary>
        /// <remarks>
        /// This method works only if a CSV header has been previously read or written
        /// with the associated <see cref="CsvFile"/> class.
        /// </remarks>

        public string this[string sColName]
        {
            get
            {
                if (sColName == null)
                {
                    throw new ArgumentNullException("sColName", "ArgumentNullException: sColName");
                }
                if (sColName == "")
                {
                    throw new ArgumentException("sColName", "sColName");
                }
                if (m_aCsvFile == null)
                {
                    throw new InvalidOperationException("m_aCsvFile");
                }

                int nIndex = m_aCsvFile.IndexOf(sColName);
                return nIndex < 0 ? null : m_vValues[nIndex];
            }
            set
            {
                if (sColName == null)
                {
                    throw new ArgumentNullException("sColName", "ArgumentNullException: sColName");
                }
                if (sColName == "")
                {
                    throw new ArgumentException("sColName", "sColName");
                }
                if (m_aCsvFile == null)
                {
                    throw new InvalidOperationException("m_aCsvFile");
                }

                int nIndex = m_aCsvFile.IndexOf(sColName);
                if (nIndex < 0)
                {
                    throw new ApplicationException("CsvValues");
                }
                m_vValues[nIndex] = value;
            }
        }

        public int FindValue(string sValue)
        {
            for (int i = 0; i < m_nActualValues; ++i)
            {
                if (m_vValues[i] == sValue)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Clear()
        {
            for (int i = 0; i < m_vValues.Length; ++i)
            {
                m_vValues[i] = null;
            }
            m_nActualValues = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chComma">
        /// Character that shall be used as the logical comma in the given CSV line. This
        /// usually is either a semicolon ';' or a real comma ','.
        /// </param>
        /// <param name="chQuote">
        /// Character that shall be used to quote fields in the given CSV line. This are
        /// usually ordinary double quotes '"'.
        /// </param>
        /// <returns></returns>
        public string ToString(char chComma, char chQuote)
        {
            StringBuilder sb = new StringBuilder(1024);

            // Use quotes only when needen and not explicity stated by the CsvFile object.
            bool fAlwaysUseQuotes = m_aCsvFile != null && m_aCsvFile.AlwaysUseQuotes;

            for (int i = 0; i < m_vValues.Length; ++i)
            {
                if (i > 0)
                {
                    sb.Append(chComma);
                }

                string sValue = m_vValues[i];
                if (sValue != null)
                {
                    // Check whether the value requires quoting. If so, then we
                    // must process the value in a very special way.

                    if (fAlwaysUseQuotes ||
                        sValue.StartsWith(" ") ||
                        sValue.EndsWith(" ") ||
                        (sValue.IndexOf(chComma) >= 0) ||
                        (sValue.IndexOf(chQuote) >= 0) ||
                        (sValue.IndexOf('\n') >= 0))
                    {
                        sb.Append(chQuote);

                        int nValueLength = sValue.Length;
                        for (int j = 0; j < nValueLength; ++j)
                        {
                            char ch = sValue[j];
                            if (ch == chQuote)
                            {
                                sb.Append(chQuote);
                                sb.Append(chQuote);
                            }
                            else
                            {
                                sb.Append(ch);
                            }
                        }

                        sb.Append(chQuote);
                    }
                    else
                    {
                        sb.Append(sValue);
                    }
                } 
                else if (fAlwaysUseQuotes) // sValue is null here
                {
                    // Write the null value as an empty quote:
                    // (starting quote and ending quote)
                    sb.Append(chQuote);
                    sb.Append(chQuote);
                }
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString(CsvFile.DEFAULTCOMMA, CsvFile.DEFAULTQUOTE);
        }

        internal void InternalSetActualValues(int nActualValues)
        {
            Debug.Assert(nActualValues >= 0);
            Debug.Assert(nActualValues <= m_vValues.Length);

            m_nActualValues = nActualValues;
        }
    }
}
