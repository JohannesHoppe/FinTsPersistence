// --------------------------------------------------------------------------------------------
//	FinCmdBase.cs
//	Subsembly FinTS API
//	Copyright © 2004-2013 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;

using Subsembly.FinTS;

namespace FinCmd
{
	public abstract class FinCmdBase : IFinCmd
	{
		FinOrder m_aOrder;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		public bool Parse(string sAction, StringDictionary vsArgsDict)
		{
			return this.OnParse(sAction, vsArgsDict);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <param name="aTanSource"></param>
		/// <returns></returns>

		public virtual int Execute(FinService aService, IFinCmdTanSource aTanSource)
		{
			m_aOrder = this.OnCreateOrder(aService);
			if (m_aOrder == null)
			{
				return -1;
			}

			//

			FinServiceResult nRes = aService.SendOrder(m_aOrder);

			if (nRes == FinServiceResult.NeedTan)
			{
				string sTAN = aTanSource.GetTan(aService);
				if (sTAN == null)
				{
					return -1;
				}

				nRes = aService.SendTan(sTAN);
			}

			if (nRes == FinServiceResult.NeedTanMediaName)
			{
				Console.WriteLine("Bezeichnung des TAN-Mediums erforderlich!");
				FinTanMedia[] vTanMedias = aService.TanMedias;
				if (vTanMedias != null)
				{
					Console.WriteLine("Bitte geben Sie einen der folgenden Parameter an:");
					for (int i = 0; i < vTanMedias.Length; ++i)
					{
						Console.WriteLine("-tanmedianame \"" + vTanMedias[i].TanMediaName + "\"");
					}
				}

				return -1;
			}

			if (nRes == FinServiceResult.Fatal)
			{
				return -1;
			}


			// Als Rückgabewert wird der höchste Rückmeldecode aus dem HIRMS genommen.
			// Wurde kein HIRMS übermittelt wird als Rückgabewert 0 eingesetzt.

			int nResult = 0;
			if (m_aOrder.StatusSegment != null)
			{
				int nIndex = m_aOrder.StatusSegment.FindMax();
				nResult = m_aOrder.StatusSegment.GetStatusCode(nIndex);
			}

			return nResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		public virtual string GetResponseData(FinService aService)
		{
			return this.OnGetResponseData(aService, m_aOrder);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		protected abstract bool OnParse(string sAction, StringDictionary vsArgsDict);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aService"></param>
		/// <returns></returns>

		protected abstract FinOrder OnCreateOrder(FinService aService);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aOrder"></param>
		/// <returns></returns>

		protected virtual string OnGetResponseData(FinService aService, FinOrder aOrder)
		{
			return null;
		}
	}
}
