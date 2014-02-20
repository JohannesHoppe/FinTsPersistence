using System;
using FinTsPersistence.Interfaces;
using Subsembly.FinTS;

namespace FinTsPersistence.Tan
{
    public class FinCmdTanPrompt : IFinCmdTanSource
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
