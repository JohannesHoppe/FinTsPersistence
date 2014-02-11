// --------------------------------------------------------------------------------------------
//	FinCmdTan.cs
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

	public class FinCmdTan : IFinCmdTanSource
	{
		string m_sTAN;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sTAN"></param>

		public FinCmdTan(string sTAN)
		{
			m_sTAN = sTAN;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		public string GetTan(FinService aService)
		{
			return m_sTAN;
		}
	}
}
