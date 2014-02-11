// --------------------------------------------------------------------------------------------
//	IFinCmd.cs
//	Subsembly FinTS API
//	Copyright © 2004-2011 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;

using Subsembly.FinTS;

namespace FinCmd
{
	/// <summary>
	/// 
	/// </summary>

	public interface IFinCmd
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		bool Parse(string sAction, StringDictionary vsArgsDict);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <param name="aTanSource"></param>
		/// <returns></returns>

		int Execute(FinService aService, IFinCmdTanSource aTanSource);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		string GetResponseData(FinService aService);
	}
}
