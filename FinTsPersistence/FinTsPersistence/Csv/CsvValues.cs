// --------------------------------------------------------------------------------------------
// Subsembly.Csv.CsvValues.cs
// Copyright © 2004-2011 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
//using System.Collections;
using System.Diagnostics;
using System.Text;

namespace Subsembly.Csv
{
	/// <summary>
	/// Contains the CSV values of a single CSV line.
	/// </summary>
	/// <remarks>
	/// Instance of this class must be created through <see cref="CsvFile.CreateValues"/>.
	/// </remarks>

	public class CsvValues
	{
		CsvFile m_aCsvFile;
		string[] m_vValues;
		int m_nActualValues;

		/// <summary>
		/// 
		/// </summary>

		public CsvValues()
		{
			m_aCsvFile = null;
			m_vValues = new string[CsvFile.DEFAULTMAXVALUES];
			m_nActualValues = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nMaxValues"></param>

		public CsvValues(int nMaxValues)
		{
			m_aCsvFile = null;
			m_vValues = new string[nMaxValues];
			m_nActualValues = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aCsvFile"></param>
		/// <param name="nMaxValues"></param>

		internal CsvValues(CsvFile aCsvFile, int nMaxValues)
		{
			m_aCsvFile = aCsvFile;
			m_vValues = new string[nMaxValues];
			m_nActualValues = 0;
		}

		/// <summary>
		/// 
		/// </summary>

		public CsvFile File
		{
			get
			{
				return m_aCsvFile;
			}
		}

		/// <summary>
		/// 
		/// </summary>

		public int MaxValues
		{
			get
			{
				return m_vValues.Length;
			}
		}

		/// <summary>
		/// 
		/// </summary>

		public int ActualValues
		{
			get
			{
				return m_nActualValues;
			}
		}

		/// <summary>
		/// 
		/// </summary>

		public bool IsEmpty
		{
			get
			{
				for (int i = 0; i < m_vValues.Length; ++i)
				{
					if ((m_vValues[i] != null) && (m_vValues[i] != ""))
					{
						return false;
					}
				}
				return true;
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
					throw new ArgumentNullException("sColName", "CsvValues");
				}
				if (sColName == "")
				{
					throw new ArgumentException("sColName", "CsvValues");
				}
				if (m_aCsvFile == null)
				{
					throw new InvalidOperationException("CsvValues");
				}

				int nIndex = m_aCsvFile.IndexOf(sColName);
				if (nIndex < 0)
				{
					return null;
				}
				else
				{
					return m_vValues[nIndex];
				}
			}
			set
			{
				if (sColName == null)
				{
					throw new ArgumentNullException("sColName", "CsvValues");
				}
				if (sColName == "")
				{
					throw new ArgumentException("sColName", "CsvValues");
				}
				if (m_aCsvFile == null)
				{
					throw new InvalidOperationException("CsvValues");
				}

				int nIndex = m_aCsvFile.IndexOf(sColName);
				if (nIndex < 0)
				{
					throw new ApplicationException("CsvValues");
				}
				m_vValues[nIndex] = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sValue"></param>
		/// <returns></returns>

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

		/// <summary>
		/// 
		/// </summary>

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

			// Use quotes only when needen and not explicity stated by the
			// CsvFile object.
			bool fAlwaysUseQuotes = m_aCsvFile == null ?
				false : m_aCsvFile.AlwaysUseQuotes;

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

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>

		public override string ToString()
		{
			return this.ToString(CsvFile.DEFAULTCOMMA, CsvFile.DEFAULTQUOTE);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nActualValues"></param>

		internal void InternalSetActualValues(int nActualValues)
		{
			Debug.Assert(nActualValues >= 0);
			Debug.Assert(nActualValues <= m_vValues.Length);

			m_nActualValues = nActualValues;
		}
	}
}
