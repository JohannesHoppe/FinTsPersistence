using System;
using System.Collections.Specialized;
using System.IO;
using FinTsPersistence.Actions;
using Subsembly.FinTS;

namespace FinTsPersistence
{
    public class FinContactCreator
    {
        public static FinContact GetFinContact(StringDictionary arguments)
        {
            string contactFile = arguments[Arguments.ContactFile];
            if (contactFile != null)
            {
                return GetContactByFile(contactFile);
            }

            string contactName = arguments["-contactname"];
            if (contactName != null)
            {
                return GetContactByName(contactName);
            }

            return GetContactByArguments(arguments);
        }

        private static FinContact GetContactByArguments(StringDictionary arguments)
        {
            string commAddress = arguments["-commaddress"];
            string fintsVersion = arguments["-fintsversion"];
            string tanProcedure = arguments["-tanprocedure"];
            string tanMediaName = arguments["-tanmedianame"];
            string bankCode = arguments["-bankcode"];
            string userId = arguments["-userid"];
            string custId = arguments["-custid"];

            if (commAddress == null || bankCode == null || userId == null)
            {
                throw new ArgumentException("Bankkontakt nicht vollständig spezifiziert!");
            }

            int finTsVersion = fintsVersion != null ? Int32.Parse(fintsVersion) : 300;

            FinContact contact = new FinContact(commAddress, finTsVersion, tanProcedure, bankCode, userId);

            if (tanMediaName != null)
            {
                contact.TanMediaName = tanMediaName;
            }

            if (custId != null)
            {
                contact.DefaultCustID = custId;
            }

            return contact;
        }

        private static FinContact GetContactByName(string contactName)
        {
            FinContact contact = FinContactFolder.Default.FindContact(contactName);
            if (contact == null)
            {
                throw new ArgumentException(String.Format("Bankkontakt {0} nicht gefunden!", contactName));
            }
            return contact;
        }

        private static FinContact GetContactByFile(string contactFile)
        {
            if (!File.Exists(contactFile))
            {
                throw new ArgumentException(String.Format("Datei {0} nicht gefunden!", contactFile));
            }
            FinContact contact = new FinContact();
            contact.Load(contactFile);
            return contact;
        }
    }
}
