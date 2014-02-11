// --------------------------------------------------------------------------------------------
// Subsembly.Csv.CsvParser.cs
// Copyright © 2004-2012 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Subsembly.Csv
{
	/// <summary>
	/// 
	/// </summary>

	public class CsvParser
	{
		CsvFile m_aCsvFile;

		enum ParseState
		{
			StartValue,
			QuotedValue,
			QuoteInQuotedValue,
			VanillaValue,
			EndQuotedValue,
			Done,
		}

		StringBuilder m_sb;
		ParseState m_nState;
		int m_nValueIndex;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aCsvFile"></param>

		public CsvParser(CsvFile aCsvFile)
		{
			m_aCsvFile = aCsvFile;
		}

		/// <summary>
		/// Reads the values of a single CSV formatted record into a given
		/// <see cref="CsvValues"/> container.
		/// </summary>
		/// <param name="vValues">
		/// Destination that will receive the record data. All values in this
		/// instance are overwritten. Missing values are overwritten with String.Empty
		/// references.
		/// </param>
		/// <returns>
		/// If a record was read, then the number of values that were actually found is
		/// returned. For an empty line zero is returned. If an immediate end of file was
		/// encountered, then -1 is returned.
		/// </returns>

		public int ReadLine(TextReader aTextReader, CsvValues vValues)
		{
			if ((aTextReader == null) || (vValues == null))
			{
				throw new ArgumentNullException();
			}

			this.Reset();
			bool fMore = false;

			do
			{
				string sLine = aTextReader.ReadLine();
				if (sLine == null)
				{
					if (fMore)
					{
						throw new FormatException("CSV truncated");
					}
					return -1;
				}

				fMore = !this.ParseLine(sLine, vValues);
			}
			while (fMore);

			// Store the number of actual values that have been read.

			vValues.InternalSetActualValues(m_nValueIndex);

			// After splitting up all values from the given CSV line, the final nValueIndex
			// tells us the number of values that we actually extracted from the line. If
			// the values array expects more data, then it is filled with empty string values.

			for (int i = m_nValueIndex; i < m_aCsvFile.MaxValues; ++i)
			{
				vValues[i] = String.Empty;
			}

			//

			return m_nValueIndex;
		}

		/// <summary>
		/// 
		/// </summary>

		internal void Reset()
		{
			m_sb = new StringBuilder(1024);
			m_nState = ParseState.StartValue;
			m_nValueIndex = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sLine"></param>
		/// <param name="vValues"></param>
		/// <returns>
		/// If the final state after parsing the line indicates the end of the record,
		/// then true is returned.
		/// </returns>

		internal bool ParseLine(string sLine, CsvValues vValues)
		{
			int nMaxValues = m_aCsvFile.MaxValues;
			char chComma = m_aCsvFile.Comma;
			char chQuote = m_aCsvFile.Quote;

			int nLineLength = sLine.Length;
			for (int i = 0; i <= nLineLength; ++i)
			{
				int ch = (i == nLineLength) ? -1 : sLine[i];

				// Process character according to state machine.

				switch (m_nState)
				{

				// StartValue - This is the initial state which is reentered every time when
				// we await another field value.

				case ParseState.StartValue:
					if (ch == chQuote)
					{
						m_nState = ParseState.QuotedValue;
					}
					else if (ch == ' ')
					{
						// m_nState = ParseState.StartValue;
					}
					else if (ch == chComma)
					{
						if (m_nValueIndex < nMaxValues)
						{
							vValues[m_nValueIndex] = String.Empty;
						}
						++m_nValueIndex;
						// m_nState = ParseState.StartValue;
					}
					else if (ch == -1)
					{
						m_nState = ParseState.Done;
					}
					else
					{
						m_sb.Append((char)ch);
						m_nState = ParseState.VanillaValue;
					}
					break;

				// VanillaValue - The characters of an unquoted value are collected until a
				// comma or the end of the line is encountered.

				case ParseState.VanillaValue:
					if ((ch == chComma) || (ch == -1))
					{
						if (m_nValueIndex < nMaxValues)
						{
							vValues[m_nValueIndex] = m_sb.ToString().TrimEnd(' ');
						}
						++m_nValueIndex;
						m_sb.Length = 0;
						m_nState = (ch == -1) ? ParseState.Done : ParseState.StartValue;
					}
					else
					{
						m_sb.Append((char)ch);
						// m_nState = ParseState.VanillaValue;
					}
					break;

				// QuotedValue - The characters of a quoted value are collected. If another
				// quote is encountered it could be a quoted quote or the value could end.

				case ParseState.QuotedValue:
					if (ch == chQuote)
					{
						m_nState = ParseState.QuoteInQuotedValue;
					}
					else if (ch == -1)
					{
						m_sb.Append(Environment.NewLine);
						// m_nState = ParseState.QuotedValue;
					}
					else
					{
						m_sb.Append((char)ch);
						// m_nState = ParseState.QuotedValue;
					}
					break;

				// QuoteInQuotedValue - A quote within a quoted value was encountered. We must
				// now find another quote, or a comma delimiter, or the end of the line.

				case ParseState.QuoteInQuotedValue:
					if (ch == chQuote)
					{
						m_sb.Append((char)ch);
						m_nState = ParseState.QuotedValue;
					}
					else if ((ch == chComma) || (ch == -1))
					{
						if (m_nValueIndex < nMaxValues)
						{
							vValues[m_nValueIndex] = m_sb.ToString();
						}
						++m_nValueIndex;
						m_sb.Length = 0;
						m_nState = (ch == -1) ? ParseState.Done : ParseState.StartValue;
					}
					else if (ch == ' ')
					{
						// This is either malformed, or there really is whitespace after the
						// closing quote.

						if (m_nValueIndex < nMaxValues)
						{
							vValues[m_nValueIndex] = m_sb.ToString();
						}
						++m_nValueIndex;
						m_sb.Length = 0;
						m_nState = ParseState.EndQuotedValue;
					}
					else
					{
						// This is an invalid structure. Within a quoted field, all quotes
						// must be doubled. Anyway we try to recover gracefully by simply
						// accepting the quote and the subsequent character.

						m_sb.Append(chQuote);
						m_sb.Append((char)ch);
						m_nState = ParseState.QuotedValue;
					}
					break;

				case ParseState.EndQuotedValue:
					if (ch == chComma)
					{
						m_nState = ParseState.StartValue;
					}
					else if (ch == -1)
					{
						m_nState = ParseState.Done;
					}
					else if (ch != ' ')
					{
						// This is an invalid structure. Only whitespace may appear until the
						// next field. We try to recover by backing up and continuing the
						// previous field. If multiple whitespaces occur after the quote, then
						// they will be collapsed into a single whitespace.

						Debug.Assert(m_nValueIndex > 0);
						Debug.Assert(m_sb.Length == 0);

						--m_nValueIndex;
						m_sb.Append(vValues[m_nValueIndex]);
						m_sb.Append(chQuote);
						m_sb.Append(' ');
						m_sb.Append((char)ch);
						m_nState = ParseState.QuotedValue;
					}
					break;
				}
			}

			//

			return m_nState == ParseState.Done;
		}
	}
}
