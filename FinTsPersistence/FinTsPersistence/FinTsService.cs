using System;
using System.Collections.Specialized;
using System.IO;
using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;
using FinTsPersistence.Code;
using FinTsPersistence.TanSources;
using Subsembly.FinTS;

namespace FinTsPersistence
{
    /// <summary>
    /// Uses Subsembly.FinTS to receive online-banking data
    /// </summary>
    public class FinTsService : IFinTsService
    {
        private readonly IActionFactory actionFactory;
        private readonly ITanSourceFactory tanSourceFactory;
        private readonly IConsole consoleX;

        public FinTsService(IActionFactory actionFactory, ITanSourceFactory tanSourceFactory, IConsole consoleX)
        {
            this.actionFactory = actionFactory;
            this.tanSourceFactory = tanSourceFactory;
            this.consoleX = consoleX;
        }

        public ActionResult DoAction(string action, StringDictionary arguments)
        {
            // In jedem Fall wird die PIN oder der Dialogkontext zur Fortführung benötigt.
            string pin = arguments[Arguments.Pin];
            string resume = arguments[Arguments.Resume];

            // Optional kann eine TAN oder eine TAN-Liste mitgegeben werden.
            // Wird beides nicht mitgegeben, so wird die TAN auf der Kommandozeile abgefragt.
            ITanSource tanSource = tanSourceFactory.GetTanSource(arguments);

            // Optional kann eine Datei für den HBCI-FinTsTrace angegeben werden.
            string traceFile = arguments[Arguments.Trace];

            // Wird der Schalter -suspend angegeben, so wird nach der Aktion keine
            // Dialogbeendigung durchgeführt, sondern einfach der Zustand in die angegebene Datei gespeichert.
            string suspend = arguments[Arguments.Suspend];

            // Die IAction Implementierung für die gewünschte Aktion erstellen.
            IAction cmd = actionFactory.GetAction(action);
            if (!cmd.Parse(action, arguments))
            {
                throw new ArgumentException("Wrong arguments for the given action!");
            }

            FinContact contact;
            FinDialog dialog = null;

            if (resume != null)
            {
                dialog = new FinDialog();
                dialog.Load(resume);
                contact = dialog.Contact;
            }
            else
            {
                contact = FinContactCreator.GetFinContact(arguments);
            }

            FinService service = FinServiceCreator.GetFinService(contact, dialog, arguments);
            ActionResult result = new ActionResult(Status.Unknown);

            try
            {
                service.ClearDocket();

                if (!service.Online)
                {
                    // Hinweis: eine erforderliche Synchronisierung wird von LogOn immer automatisch durchführt.
                    if (!service.LogOn(pin))
                    {
                        result = new ActionResult(Status.CouldNotLogOn);
                        goto _done;
                    }
                } 

                if (service.Online) {

                    result = cmd.Execute(service, tanSource);

                    if (suspend != null)
                    {
                        service.Dialog.SaveAs(suspend);
                    }
                    else
                    {
                        service.LogOff();
                    }
                }

                _done:

                // Falls der Bankkontakt aus einer Datei geladen wurde, so muss diese nun noch
                // gespeichert werden, damit auch alle am Bankkontakt erfolgten Änderungen
                // erhalten bleiben.
                string sContactFile = arguments[Arguments.ContactFile];
                if (sContactFile != null)
                {
                    contact.SaveAs(sContactFile);
                }

                // Wurde eine Tracedatei angegeben, so wird der komplette HBCI FinTsTrace in diese
                // Datei geschrieben.
                if (traceFile != null)
                {
                    StreamWriter sw = File.CreateText(traceFile);
                    sw.Write(service.Trace);
                    sw.Close();
                }

                // Auftrag ausgeführt. Zuerst geben wir den gesammelten Laufzettel aus,
                // danach die Antwortdaten, sofern welche vorhanden sind. Der Laufzettel wird
                // auf den Error-Kanal ausgegeben, damit er von der Antwortdaten leichter
                // getrennt werden kann.
                consoleX.Error.WriteLine(service.Docket);

                if (result.Status != Status.CouldNotLogOn)
                {
                    ResponseData responseData = cmd.GetResponseData(service);
                    if (responseData.Formatted != null)
                    {
                        consoleX.WriteLine(responseData.Formatted);
                    }
                    result.Response = responseData;
                }
            }
            catch (Exception x)
            {
                throw new ActionException(x, service.Trace);
            }
            finally
            {
                service.Dispose();
            }

            return result;
        }  
    }
}
