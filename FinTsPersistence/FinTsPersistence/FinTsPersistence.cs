using System;
using System.Collections.Specialized;
using System.IO;
using FinTsPersistence.Interfaces;
using Subsembly.FinTS;

namespace FinTsPersistence
{
    /// <summary>
    /// Modified version of Subsembly FinCmd - with currently only one action: persist
    /// FinTsPersistence {action} -{argname1} {argvalue1} ...
    /// </summary>
    public class FinTsPersistence : IFinTsPersistence
    {
        private readonly IActionFactory actionFactory;
        private readonly ITanSourceFactory tanSourceFactory;

        public FinTsPersistence(IActionFactory actionFactory, ITanSourceFactory tanSourceFactory)
        {
            this.actionFactory = actionFactory;
            this.tanSourceFactory = tanSourceFactory;
        }

        public int DoAction(string action, StringDictionary arguments)
        {
            // In jedem Fall wird die PIN oder der Dialogkontext zur Fortführung benötigt.
            string pin = arguments["-pin"];
            string resume = arguments["-resume"];

            // Optional kann eine TAN oder eine TAN-Liste mitgegeben werden.
            // Wird beides nicht mitgegeben, so wird die TAN auf der Kommandozeile abgefragt.
            ITanSource tanSource = tanSourceFactory.GetTanSource(arguments);

            // Optional kann eine Datei für den HBCI-Trace angegeben werden.
            string traceFile = arguments["-trace"];

            // Wird der Schalter -suspend angegeben, so wird nach der Aktion keine
            // Dialogbeendigung durchgeführt, sondern einfach der Zustand in die angegebene Datei gespeichert.
            string suspend = arguments["-suspend"];

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

            int nResult = -2;

            try
            {
                service.ClearDocket();

                if (action == "sync")
                {
                    if (!service.Synchronize(pin))
                    {
                        nResult = -3;
                        goto _done;
                    }
                }
                else
                {
                    // Mit dem FinService und dem IAction die gewünschte Aktion durchführen.
                    if (!service.Online)
                    {
                        if (!service.LogOn(pin))
                        {
                            nResult = -3;
                            goto _done;
                        }
                    } 

                    if (service.Online) {

                        nResult = cmd.Execute(service, tanSource);

                        if (suspend != null)
                        {
                            service.Dialog.SaveAs(suspend);
                        }
                        else
                        {
                            service.LogOff();
                        }
                    }
                }

            _done:

                // Falls der Bankkontakt aus einer Datei geladen wurde, so muss diese nun noch
                // gespeichert werden, damit auch alle am Bankkontakt erfolgten Änderungen
                // erhalten bleiben.
                string sContactFile = arguments["-contactfile"];
                if (sContactFile != null)
                {
                    contact.SaveAs(sContactFile);
                }

                // Wurde eine Tracedatei angegeben, so wird der komplette HBCI Trace in diese
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
                Console.Error.WriteLine(service.Docket);

                if (nResult != -3)
                {
                    string sResponseData = cmd.GetResponseData(service);
                    if (sResponseData != null)
                    {
                        Console.WriteLine(sResponseData);
                    }
                }
            }
            catch (Exception x)
            {
                Console.Error.WriteLine(service.Trace);
                Console.Error.WriteLine(x.ToString());
            }
            finally
            {
                service.Dispose();
            }

            return nResult;
        }  
    }
}
