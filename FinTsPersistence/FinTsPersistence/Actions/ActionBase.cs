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

        public virtual ActionResult Execute(FinService service, ITanSource tanSource)
        {
            order = OnCreateOrder(service);
            if (order == null)
            {
                return new ActionResult(Status.CouldNotCreateOrder);
            }

            FinServiceResult result = service.SendOrder(order);

            if (result == FinServiceResult.NeedTan)
            {
                string tan = tanSource.GetTan(service);
                if (tan == null)
                {
                    return new ActionResult(Status.CouldNotCreateOrder);
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

                return new ActionResult(Status.NeedTanMediaName);
            }

            if (result == FinServiceResult.Fatal)
            {
                return new ActionResult(Status.FatalResult);
            }

            // Als Rückgabewert wird der höchste Rückmeldecode aus dem HIRMS genommen.
            // Wurde kein HIRMS übermittelt wird als Rückgabewert 0 eingesetzt.
            int orderStatusCode = 0;
            if (order.StatusSegment != null)
            {
                int nIndex = order.StatusSegment.FindMax();
                orderStatusCode = order.StatusSegment.GetStatusCode(nIndex);
            }

            if (result == FinServiceResult.Error)
            {
                return new ActionResult(Status.ErrorResult, false, orderStatusCode);
            }

            if (result == FinServiceResult.Success)
            {
                return new ActionResult(Status.Success, true, orderStatusCode);
            }

            throw new Exception("The enum FinServiceResult has been changed! " +
                                "Known states: NeedTan, NeedTanMediaName, Fatal, Error, Success " +
                                "New: " + result);
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
    }
}
