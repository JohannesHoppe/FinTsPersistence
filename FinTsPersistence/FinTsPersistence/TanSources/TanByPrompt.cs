using System;
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
            FinTanProcessParameters tanProcess = aService.TanProcess;
            FinChallengeInfo aChallengeInfo = aService.ChallengeInfo;

            if ((tanProcess != null) && (aChallengeInfo != null))
            {
                Console.Write(tanProcess.ChallengeLabel + ": ");
                Console.WriteLine(aChallengeInfo.Challenge);
            }

            Console.Write("TAN: ");
            Console.Out.Flush();

            string sTAN = Console.ReadLine();
            return !string.IsNullOrEmpty(sTAN) ? sTAN : null;
        }
    }
}
