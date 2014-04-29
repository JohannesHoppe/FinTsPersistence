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
        private readonly IConsole consoleX;

        public TanByPrompt(IConsole consoleX)
        {
            this.consoleX = consoleX;
        }

        public string GetTan(FinService aService)
        {
            FinTanProcessParameters tanProcess = aService.TanProcess;
            FinChallengeInfo aChallengeInfo = aService.ChallengeInfo;

            if ((tanProcess != null) && (aChallengeInfo != null))
            {
                consoleX.Write(tanProcess.ChallengeLabel + ": ");
                consoleX.WriteLine(aChallengeInfo.Challenge);
            }

            consoleX.Write("TAN: ");
            consoleX.Out.Flush();

            string sTAN = consoleX.ReadLine();
            return !string.IsNullOrEmpty(sTAN) ? sTAN : null;
        }
    }
}
