using System;
using FinTsPersistence.Interfaces;
using Subsembly.FinTS;

namespace FinTsPersistence.TanSources
{
    /// <summary>
    /// Prompts for tan on command line
    /// </summary>
    public class TanByPrompt : ITanSource
    {
        public string GetTan(FinService aService)
        {
            FinTanProcessParameters aTanProc = aService.TanProcess;
            FinChallengeInfo aChallengeInfo = aService.ChallengeInfo;

            if ((aTanProc != null) && (aChallengeInfo != null))
            {
                Console.Write(aTanProc.ChallengeLabel + ": ");
                Console.WriteLine(aChallengeInfo.Challenge);
            }

            Console.Write("TAN: ");
            Console.Out.Flush();

            string sTAN = Console.ReadLine();
            return !string.IsNullOrEmpty(sTAN) ? sTAN : null;
        }
    }
}
