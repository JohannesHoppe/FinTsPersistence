// --------------------------------------------------------------------------------------------
//	FinCmdTanList.cs
//	Subsembly FinTS API
//	Copyright © 2004-2011 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;

using Subsembly.FinTS;

namespace FinTsPersistence
{
	/// <summary>
	/// 
	/// </summary>

	public class FinCmdTanList : IFinCmdTanSource
	{
		ArrayList m_vList = new ArrayList();

		/// <summary>
		/// 
		/// </summary>

		class Tan
		{
			public int Index;
			public string TAN;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="sTanLine"></param>

			public Tan(string sTanLine)
			{
				string[] sTokens = sTanLine.Split(',');
				if ((sTokens == null) || (sTokens.Length != 2))
				{
					throw new ApplicationException("Ungültige Zeile in TAN-Datei!");
				}

				string sIndex = sTokens[0].Trim();
				string sTAN = sTokens[1].Trim();

				if (sIndex == "")
				{
					throw new ApplicationException("Fehlender Index in Zeile in TAN-Datei!");
				}
				if (sTAN == "")
				{
					throw new ApplicationException("Fehlende TAN in Zeile in TAN-Datei!");
				}

				this.Index = Int32.Parse(sIndex);
				this.TAN = sTAN;
			}
		};

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sFileName"></param>

		public int LoadTanList(string sFileName)
		{
			m_vList.Clear();

			using (TextReader aReader = File.OpenText(sFileName))
			{
				for (; ; )
				{
					string sTanLine = aReader.ReadLine();
					if (sTanLine == null)
					{
						break;
					}

					sTanLine = sTanLine.Trim();
					if (sTanLine == "")
					{
						continue;
					}

					Tan aTan = new Tan(sTanLine);

					if (this.FindTan(aTan.Index) != null)
					{
						throw new ApplicationException("Doppelter Index in TAN-Datei!");
					}

					m_vList.Add(aTan);
				}

				aReader.Close();
			}

			return m_vList.Count;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sChallenge"></param>
		/// <returns></returns>

		public string FindTan(int nIndex)
		{
			foreach (Tan aTan in m_vList)
			{
				if (aTan.Index == nIndex)
				{
					return aTan.TAN;
				}
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		public string GetTan(FinService aService)
		{
			if (m_vList.Count == 0)
			{
				throw new InvalidOperationException("Keine TAN-Nummern in Liste!");
			}

			FinTanProcessParameters aTanProc = aService.TanProcess;
			FinChallengeInfo aChallengeInfo = aService.ChallengeInfo;

			// If this is not a 2-step TAN procedure then we always just return the very first
			// TAN of the list.

			if ((aTanProc == null) || (aChallengeInfo == null))
			{
				Tan aTan = (Tan)m_vList[0];
				return aTan.TAN;
			}

			//

			string sChallenge = aChallengeInfo.Challenge;
			int nIndex = FinCmdTanList.GetIndexFromChallenge(sChallenge);
			if (nIndex >= 0)
			{
				string sTAN = this.FindTan(nIndex);
				if (sTAN != null)
				{
					return sTAN;
				}
			}

			Console.WriteLine("Keine TAN für " + sChallenge + "in TAN-Datei gefunden!");
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sChallenge"></param>
		/// <returns></returns>

		internal static int GetIndexFromChallenge(string sChallenge)
		{
			if (sChallenge == null)
			{
				throw new ArgumentNullException();
			}

			// We extract the very first sequence of digits from the given challenge, 
			// assuming that this is the index number of the requested TAN.

			int nSeqNo = -1;

			for (int i = 0; i < sChallenge.Length; ++i)
			{
				char ch = sChallenge[i];
				if ((ch >= '0') && (ch <= '9'))
				{
					if (nSeqNo < 0)
					{
						nSeqNo = (ch - '0');
					}
					else
					{
						nSeqNo = (nSeqNo * 10) + (ch - '0');
					}
				}
				else
				{
					if (nSeqNo >= 0)
					{
						break;
					}
				}
			}

			return nSeqNo;
		}
	}
}
