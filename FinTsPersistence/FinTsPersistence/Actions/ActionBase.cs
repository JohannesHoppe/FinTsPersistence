using System;
using System.Collections.Specialized;
using FinTsPersistence.Interfaces;
using Subsembly.FinTS;

namespace FinTsPersistence.Actions
{
    public abstract class ActionBase : IAction
    {
        FinOrder order;

        public bool Parse(string sAction, StringDictionary vsArgsDict)
        {
            return OnParse(sAction, vsArgsDict);
        }

        public virtual int Execute(FinService aService, ITanSource aTanSource)
        {
            order = OnCreateOrder(aService);
            if (order == null)
            {
                return -1;
            }

            //

            FinServiceResult nRes = aService.SendOrder(order);

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
            if (order.StatusSegment != null)
            {
                int nIndex = order.StatusSegment.FindMax();
                nResult = order.StatusSegment.GetStatusCode(nIndex);
            }

            return nResult;
        }

        public virtual string GetResponseData(FinService aService)
        {
            return OnGetResponseData(aService, order);
        }

        protected abstract bool OnParse(string sAction, StringDictionary vsArgsDict);

        protected abstract FinOrder OnCreateOrder(FinService aService);

        protected virtual string OnGetResponseData(FinService aService, FinOrder aOrder)
        {
            return null;
        }

        public virtual bool GoOnline
        {
            get { return true; }
        }

        public virtual bool DoSync
        {
            get { return false; }
        }
    }
}
