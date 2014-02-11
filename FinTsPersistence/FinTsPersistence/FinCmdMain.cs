// --------------------------------------------------------------------------------------------
//	FinCmdMain.cs
//	Subsembly FinTS API
//	Copyright © 2004-2014 Subsembly GmbH
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

using Subsembly.FinTS;

namespace FinCmd
{
	/// <summary>
	/// 
	/// </summary>

	class FinCmdMain
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static int Main(string[] vsArgs)
		{
			// Mindestens ein Argument muss vorhanden sein und die Anzahl der Argumente muss
			// ungerade sein.

			if ((vsArgs.Length == 0) || ((vsArgs.Length & 0x0001) == 0))
			{
				_Usage();
				return -1;
			}

			// Die gewünschte Aktion und die zugehörigen Parameter werden aus der Kommandozeile
			// extrahiert.

			string sAction = vsArgs[0];
			StringDictionary vsArgsDict = new StringDictionary();

			for (int i = 1; i < vsArgs.Length; i += 2)
			{
				string sArgName = vsArgs[i];
				string sArgValue = vsArgs[i + 1];

				vsArgsDict.Add(sArgName, sArgValue);
			}

			// Aktion ausführen.

			int nResult = -1;

			FinCmdMain aMain = new FinCmdMain();

			try
			{
				nResult = aMain.DoAction(sAction, vsArgsDict);
			}
			catch (Exception x)
			{
				Console.Error.WriteLine("Exception " + x.Message + "!");
				Console.Error.WriteLine(x.ToString());
				Debug.Fail(x.Message);
			}
#if DEBUG
			Console.ReadLine();
#endif
			return nResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		internal int DoAction(string sAction, StringDictionary vsArgsDict)
		{
			// In jedem Fall wird die PIN oder der Dialogkontext zur Fortführung benötigt.

			string sPIN = vsArgsDict["-pin"];
			string sResume = vsArgsDict["-resume"];

			if ((sPIN == null) && (sResume == null))
			{
				Console.Error.WriteLine("PIN fehlt!");
				return -1;
			}

			// Optional kann eine TAN oder eine TAN-Liste mitgegeben werden. Wird beides nicht
			// mitgegeben, so wird die TAN auf der Kommandozeile abgefragt.

			IFinCmdTanSource aTanSource = null;

			string sTAN = vsArgsDict["-tan"];
			if (sTAN != null)
			{
				aTanSource = new FinCmdTan(sTAN);
			}

			string sTanList = vsArgsDict["-tanlist"];
			if (sTanList != null)
			{
				if (aTanSource != null)
				{
					Console.Error.WriteLine("TAN und TANLIST dürfen nicht gleichzeitig angegeben werden!");
					return -1;
				}

				FinCmdTanList aTanList = new FinCmdTanList();
				aTanList.LoadTanList(sTanList);
				aTanSource = aTanList;
			}

			if (aTanSource == null)
			{
				aTanSource = new FinCmdTanPrompt();
			}

			// Optional kann eine Datei für den HBCI-Trace angegeben werden.

			string sTraceFile = vsArgsDict["-trace"];

			// Wird der Schalter -suspend angegeben, so wird nach der Aktion keine
			// Dialogbeendigung durchgeführt, sondern einfach der Zustand in die angegebene
			// Datei gespeichert.

			string sSuspend = vsArgsDict["-suspend"];

			// Die IFinCmd Implementierung für die gewünschte Aktion erstellen.

			IFinCmd aCmd = _GetCmd(sAction);
			if (aCmd == null)
			{
				_Usage();
				Console.Error.WriteLine("Aktion {0} nicht bekannt!", sAction);
				return -1;
			}

			//

			if (!aCmd.Parse(sAction, vsArgsDict))
			{
				return -1;
			}

			// Zuerst ermitteln wir den Bankkontakt aus den übergebenen Parametern. Danach
			// erstellen wir damit einen FinService.

			FinContact aContact = null;
			FinDialog aDialog = null;
			FinService aService = null;

			if (sResume != null)
			{
				aDialog = new FinDialog();
				aDialog.Load(sResume);
				aContact = aDialog.Contact;
			}
			else
			{
				aContact = _GetContact(vsArgsDict);
				if (aContact == null)
				{
					return -1;
				}
			}

			aService = _GetService(aContact, aDialog, vsArgsDict);
			if (aService == null)
			{
				return -1;
			}

			//

			int nResult = -2;

			try
			{
				aService.ClearDocket();

				//

				if (sAction == "sync")
				{
					if (!aService.Synchronize(sPIN))
					{
						nResult = -3;
						goto _done;
					}
				}
				else
				{
					// Mit dem FinService und dem IFinCmd die gewünschte Aktion durchführen.

					if (!aService.Online)
					{
						if (!aService.LogOn(sPIN))
						{
#if FINQA
						FinCmdTestcase aCmdTestcase = aCmd as FinCmdTestcase;
						if (aCmdTestcase != null)
						{
							string s = aCmdTestcase.GetLogOnResponseData(aService);
							if (s != null)
							{
								Console.WriteLine(s);
							}
						}
#endif
							nResult = -3;
							goto _done;
						}
					}

					//

					if (aService.Online)
					{
						nResult = aCmd.Execute(aService, aTanSource);

						if (sSuspend != null)
						{
							aService.Dialog.SaveAs(sSuspend);
						}
						else
						{
							aService.LogOff();
						}
					}
				}

			_done:

				// Falls der Bankkontakt aus einer Datei geladen wurde, so muss diese nun noch
				// gespeichert werden, damit auch alle am Bankkontakt erfolgten Änderungen
				// erhalten bleiben.

				string sContactFile = vsArgsDict["-contactfile"];
				if (sContactFile != null)
				{
					aContact.SaveAs(sContactFile);
				}

				// Wurde eine Tracedatei angegeben, so wird der komplette HBCI Trace in diese
				// Datei geschrieben.

				if (sTraceFile != null)
				{
					StreamWriter sw = File.CreateText(sTraceFile);
					sw.Write(aService.Trace);
					sw.Close();
				}

				// Auftrag ausgeführt. Zuerst geben wir den gesammelten Laufzettel aus,
				// danach die Antwortdaten, sofern welche vorhanden sind. Der Laufzettel wird
				// auf den Error-Kanal ausgegeben, damit er von der Antwortdaten leichter
				// getrennt werden kann.

				Console.Error.WriteLine(aService.Docket);

				if (nResult != -3)
				{
					string sResponseData = aCmd.GetResponseData(aService);
					if (sResponseData != null)
					{
						Console.WriteLine(sResponseData);
					}
				}
			}
			catch (Exception x)
			{
				Console.Error.WriteLine(aService.Trace);
				Console.Error.WriteLine(x.ToString());
			}
			finally
			{
				aService.Dispose();
				aService = null;
			}

			return nResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		static FinContact _GetContact(StringDictionary vsArgsDict)
		{
			FinContact aContact = null;

			string sContactFile = vsArgsDict["-contactfile"];
			string sContactName = vsArgsDict["-contactname"];

			if (sContactFile != null)
			{
				if (!File.Exists(sContactFile))
				{
					Console.Error.WriteLine("Datei {0} nicht gefunden!", sContactFile);
					return null;
				}
				aContact = new FinContact();
				aContact.Load(sContactFile);
			}
			else if (sContactName != null)
			{
				aContact = FinContactFolder.Default.FindContact(sContactName);
				if (aContact == null)
				{
					Console.Error.WriteLine("Bankkontakt {0} nicht gefunden!", sContactName);
					return null;
				}
			}
			else
			{
				string sCommAddress = vsArgsDict["-commaddress"];
				string sFinTSVersion = vsArgsDict["-fintsversion"];
				string sTanProcedure = vsArgsDict["-tanprocedure"];
				string sTanMediaName = vsArgsDict["-tanmedianame"];
				string sBankCode = vsArgsDict["-bankcode"];
				string sUserID = vsArgsDict["-userid"];
				string sCustID = vsArgsDict["-custid"];

				if ((sCommAddress == null) || (sBankCode == null) || (sUserID == null))
				{
					Console.Error.WriteLine("Bankkontakt nicht vollständig spezifiziert!");
					return null;
				}

				int nFinTSVersion = (sFinTSVersion != null) ? Int32.Parse(sFinTSVersion) : 300;

				aContact = new FinContact(sCommAddress, nFinTSVersion, sTanProcedure,
					sBankCode, sUserID);

				if (sTanMediaName != null)
				{
					aContact.TanMediaName = sTanMediaName;
				}
				if (sCustID != null)
				{
					aContact.DefaultCustID = sCustID;
				}
			}

			return aContact;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aContact"></param>
		/// <param name="aDialog"></param>
		/// <param name="vsArgsDict"></param>
		/// <returns></returns>

		static FinService _GetService(
			FinContact aContact,
			FinDialog aDialog,
			StringDictionary vsArgsDict)
		{
			Debug.Assert(aContact != null);

			// Als nächstes ermitteln wir das Konto für die gewünschte Bankverbindung.

			string sAcctBankCode = vsArgsDict["-acctbankcode"];
			string sAcctNo = vsArgsDict["-acctno"];
			string sAcctCurrency = vsArgsDict["-acctcurrency"];

			if (sAcctBankCode == null)
			{
				sAcctBankCode = aContact.BankCode;
			}
			if (sAcctBankCode == null)
			{
				Console.Error.WriteLine("Bankleitzahl zu Konto fehlt!");
				return null;
			}
			if ((sAcctBankCode.Length != 8) || !FinUtil.IsDigits(sAcctBankCode))
			{
				Console.Error.WriteLine("Bankleitzahl zu Konto ist ungültig!");
				return null;
			}

			if (sAcctNo == null)
			{
				Console.Error.WriteLine("Kontonummer fehlt!");
				return null;
			}
			if ((sAcctNo == "") || (sAcctNo.Length > 30))
			{
				Console.Error.WriteLine("Kontonummer ist ungültig!");
				return null;
			}

			if (sAcctCurrency == null)
			{
				sAcctCurrency = "EUR";
			}
			else if ((sAcctCurrency.Length != 3) || !FinUtil.IsUpperAscii(sAcctCurrency))
			{
				Console.Error.WriteLine("Kontowährung ist ungültig!");
				return null;
			}

			// Die Bankverbindung ist jetzt vollständig spezifiziert und wir können ein
			// FinService Objekt dafür anlegen.

			FinService aService;

			if (aDialog != null)
			{
				aService = new FinService(aDialog, sAcctBankCode, sAcctNo, sAcctCurrency);
			}
			else
			{
				aService = new FinService(aContact, sAcctBankCode, sAcctNo, sAcctCurrency);
			}

#if FINQA
			aService.ProductName = "Notarnet";
#endif

			return aService;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sAction"></param>
		/// <returns></returns>

		static IFinCmd _GetCmd(string sAction)
		{
			IFinCmd aCmd = null;

			switch (sAction)
			{
			case "balance":
				aCmd = new FinCmdBalance();
				break;
			case "statement":
				aCmd = new FinCmdStatement();
				break;
			case "remitt":
				aCmd = new FinCmdRemittDebit();
				break;
			case "xml":
				aCmd = new FinCmdXml();
				break;
			case "sepa":
				aCmd = new FinCmdSepa();
				break;
			case "sync":
				aCmd = new FinCmdSync();
				break;
#if FINQA
			case "testcase":
				aCmd = new FinCmdTestcase();
				break;
#endif
			}

			return aCmd;
		}

		/// <summary>
		/// 
		/// </summary>

		static void _Usage()
		{
			Assembly aThisAssembly = Assembly.GetExecutingAssembly();
			Stream aStream = aThisAssembly.GetManifestResourceStream("Subsembly.FinCmdUsage.txt");
			StreamReader aReader = new StreamReader(aStream, Encoding.UTF8);
			string sUsage = aReader.ReadToEnd();
			aReader.Close();
			aStream.Close();

			Console.Error.WriteLine(sUsage);
		}
	}
}
