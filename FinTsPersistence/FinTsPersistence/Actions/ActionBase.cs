using System;
using System.Collections.Specialized;
using FinTsPersistence.Interfaces;
using Subsembly.FinTS;

namespace FinTsPersistence.Actions
{
    public abstract class ActionBase : IAction
    {
        FinOrder m_aOrder;

        public bool Parse(string sAction, StringDictionary vsArgsDict)
        {
            return OnParse(sAction, vsArgsDict);
        }

        public virtual int Execute(FinService aService, ITanSource aTanSource)
        {
            m_aOrder = OnCreateOrder(aService);
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
                    foreach (FinTanMedia t in vTanMedias)
                    {
                        Console.WriteLine("-tanmedianame \"" + t.TanMediaName + "\"");
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

        public virtual string GetResponseData(FinService aService)
        {
            return OnGetResponseData(aService, m_aOrder);
        }

        protected abstract bool OnParse(string sAction, StringDictionary vsArgsDict);

        protected abstract FinOrder OnCreateOrder(FinService aService);

        protected virtual string OnGetResponseData(FinService aService, FinOrder aOrder)
        {
            return null;
        }
    }
}
