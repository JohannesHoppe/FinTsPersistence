using System;
using System.Collections;
using System.IO;
using System.Linq;
using FinTsPersistence.Code;
using Subsembly.FinTS;

namespace FinTsPersistence.TanSources
{
    /// <summary>
    /// Uses a tan list file to get the right TAN
    /// </summary>
    public class TanByList : ITanSource
    {
        private readonly ArrayList m_vList = new ArrayList();
        private readonly IConsole consoleX;

        public TanByList(IConsole consoleX)
        {
            this.consoleX = consoleX;
        }

        class Tan
        {
            public readonly int Index;
            public readonly string TAN;

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

                Index = Int32.Parse(sIndex);
                TAN = sTAN;
            }
        };

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

                    if (FindTan(aTan.Index) != null)
                    {
                        throw new ApplicationException("Doppelter Index in TAN-Datei!");
                    }

                    m_vList.Add(aTan);
                }

                aReader.Close();
            }

            return m_vList.Count;
        }

        public string FindTan(int nIndex)
        {
            return (from Tan aTan in m_vList where aTan.Index == nIndex select aTan.TAN).FirstOrDefault();
        }

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

            string sChallenge = aChallengeInfo.Challenge;
            int nIndex = GetIndexFromChallenge(sChallenge);
            if (nIndex >= 0)
            {
                string sTAN = FindTan(nIndex);
                if (sTAN != null)
                {
                    return sTAN;
                }
            }

            consoleX.WriteLine("Keine TAN für " + sChallenge + "in TAN-Datei gefunden!");
            return null;
        }

        internal static int GetIndexFromChallenge(string sChallenge)
        {
            if (sChallenge == null)
            {
                throw new ArgumentNullException();
            }

            // We extract the very first sequence of digits from the given challenge, 
            // assuming that this is the index number of the requested TAN.

            int nSeqNo = -1;

            foreach (char ch in sChallenge)
            {
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
