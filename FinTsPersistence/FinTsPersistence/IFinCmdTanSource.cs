// --------------------------------------------------------------------------------------------
//	IFinCmdTanSource.cs
//	Subsembly FinTS API
//	Copyright © 2004-2011 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using Subsembly.FinTS;

namespace FinTsPersistence
{
	/// <summary>
	/// 
	/// </summary>

	public interface IFinCmdTanSource
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		string GetTan(FinService aService);
	}
}
