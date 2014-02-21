using System;
using System.Collections.Specialized;
using FinTsPersistence.Interfaces;
using Subsembly.FinTS;

namespace FinTsPersistence.Actions
{
    public abstract class ActionBase : IAction
    {
        FinOrder order;

        public bool Parse(string action, StringDictionary arguments)
        {
            return OnParse(action, arguments);
        }

        public virtual int Execute(FinService service, ITanSource tanSource)
        {
            order = OnCreateOrder(service);
            if (order == null)
            {
                return -1;
            }

            FinServiceResult result = service.SendOrder(order);

            if (result == FinServiceResult.NeedTan)
            {
                string tan = tanSource.GetTan(service);
                if (tan == null)
                {
                    return -1;
                }

                result = service.SendTan(tan);
            }

            if (result == FinServiceResult.NeedTanMediaName)
            {
                Console.WriteLine("Bezeichnung des TAN-Mediums erforderlich!");
                FinTanMedia[] vTanMedias = service.TanMedias;
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

            if (result == FinServiceResult.Fatal)
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

        protected abstract bool OnParse(string action, StringDictionary arguments);

        protected abstract FinOrder OnCreateOrder(FinService aService);

        protected virtual string OnGetResponseData(FinService service, FinOrder order)
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
