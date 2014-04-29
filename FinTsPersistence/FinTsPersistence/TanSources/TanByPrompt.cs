using System;
using FinTsPersistence.Code;
using Subsembly.FinTS;

namespace FinTsPersistence.TanSources
{
    /// <summary>
    /// Prompts for tan on command line
    /// </summary>
    public class TanByPrompt : ITanSource
    {
        private readonly IConsoleX consoleXX;

        public TanByPrompt(IConsoleX consoleXX)
        {
            this.consoleXX = consoleXX;
        }

        public string GetTan(FinService aService)
        {
            FinTanProcessParameters tanProcess = aService.TanProcess;
            FinChallengeInfo aChallengeInfo = aService.ChallengeInfo;

            if ((tanProcess != null) && (aChallengeInfo != null))
            {
                consoleXX.Write(tanProcess.ChallengeLabel + ": ");
                consoleXX.WriteLine(aChallengeInfo.Challenge);
            }

            consoleXX.Write("TAN: ");
            consoleXX.Out.Flush();

            string sTAN = consoleXX.ReadLine();
            return !string.IsNullOrEmpty(sTAN) ? sTAN : null;
        }
    }
}
