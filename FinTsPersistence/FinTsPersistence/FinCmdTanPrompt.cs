// --------------------------------------------------------------------------------------------
//	FinCmdTanPrompt.cs
//	Subsembly FinTS API
//	Copyright © 2004-2011 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using Subsembly.FinTS;

namespace FinCmd
{
	/// <summary>
	/// 
	/// </summary>

	public class FinCmdTanPrompt : IFinCmdTanSource
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		public string GetTan(FinService aService)
		{
			FinContact aContact = aService.Contact;
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
			if ((sTAN != null) && (sTAN != ""))
			{
				return sTAN;
			}
			else
			{
				return null;
			}
		}
	}
}
