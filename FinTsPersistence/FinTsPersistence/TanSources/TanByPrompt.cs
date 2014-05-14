using FinTsPersistence.Code;
using Subsembly.FinTS;

namespace FinTsPersistence.TanSources
{
    /// <summary>
    /// Prompts for tan on command line
    /// </summary>
    public class TanByPrompt : ITanSource
    {
        private readonly IInputOutput io;

        public TanByPrompt(IInputOutput io)
        {
            this.io = io;
        }

        public string GetTan(FinService aService)
        {
            FinTanProcessParameters tanProcess = aService.TanProcess;
            FinChallengeInfo aChallengeInfo = aService.ChallengeInfo;

            if ((tanProcess != null) && (aChallengeInfo != null))
            {
                io.WriteLine(tanProcess.ChallengeLabel + ": ");
                io.WriteLine(aChallengeInfo.Challenge);
            }

            io.WriteLine("TAN:");

            string sTAN = io.ReadLine();
            return !string.IsNullOrEmpty(sTAN) ? sTAN : null;
        }
    }
}
