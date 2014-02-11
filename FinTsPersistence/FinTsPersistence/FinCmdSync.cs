// --------------------------------------------------------------------------------------------
//	FinCmdSync.cs
//	Subsembly FinTS API
//	Copyright © 2013 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

using Subsembly.FinTS;
using Subsembly.Swift;

namespace FinCmd
{
	/// <summary>
	/// Empty pseuso-action.
	/// </summary>

	public class FinCmdSync : FinCmdBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		protected override bool OnParse(string sAction, StringDictionary vsArgsDict)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		protected override FinOrder OnCreateOrder(FinService aService)
		{
			return null;
		}
	}
}