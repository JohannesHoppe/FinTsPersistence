using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;
using FinTsPersistence.Code;

namespace FinTsPersistence
{
    public class CommandLineHelper : ICommandLineHelper
    {
        private readonly IInputOutput io;

        public CommandLineHelper(IInputOutput io)
        {
            this.io = io;
        }

        /// <summary>
        /// At least one parameters must be given, and the number of parameters must be uneven
        /// (because action has no value)
        /// </summary>
        /// <param name="vsArgs">args from Main</param>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        public void CheckAmountOfParameters(string[] vsArgs)
        {
            if ((vsArgs.Length == 0) || ((vsArgs.Length & 0x0001) == 0))
            {
                throw new ArgumentException("At least one command line parameter must be given, and the number of parameters must be uneven");
            }
        }

        /// <summary>
        /// In jedem Fall wird die PIN oder der Dialogkontext zur Fortführung benötigt.
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        public void CheckForPinOrResume(StringDictionary arguments)
        {
            string pin = arguments[Arguments.Pin];
            string resume = arguments[Arguments.Resume];

            if ((pin == null) && (resume == null))
            {
                throw new ArgumentException("PIN is missing (or resume)!");
            }
        }

        /// <summary>
        /// Get the action and all trailing args/argvalue pairs
        /// </summary>
        /// <param name="vsArgs">args from Main</param>
        public ExtractedArguments ExtractArguments(string[] vsArgs)
        {
            string sAction = vsArgs[0];
            StringDictionary vsArgsDict = new StringDictionary();

            for (int i = 1; i < vsArgs.Length; i += 2)
            {
                string sArgName = vsArgs[i];
                string sArgValue = vsArgs[i + 1];

                vsArgsDict.Add(sArgName, sArgValue);
            }

            return new ExtractedArguments
                {
                    Action = sAction,
                    Arguments = vsArgsDict
                };
        }

        /// <summary>
        /// Displays a help text on standard error output stream
        /// </summary>
        public void ShowUsage()
        {
            Assembly aThisAssembly = Assembly.GetExecutingAssembly();
            Stream aStream = aThisAssembly.GetManifestResourceStream("FinTsPersistence.FinTsPersistenceUsage.txt");
            Debug.Assert(aStream != null, "aStream != null");
            StreamReader aReader = new StreamReader(aStream, Encoding.UTF8);
            string sUsage = aReader.ReadToEnd();
            aReader.Close();
            aStream.Close();

            io.Error.Write(sUsage);
        }

        public void DisplayShortException(Exception ex)
        {
            io.Error.Write("Exception: {0}", ex.GetType());
            io.Error.Write("Message: {0}", ex.Message);
            io.Error.Write("-----------------------------");
        }

        public void DisplayActionException(ActionException ex)
        {
            io.Error.Write("Exception: {0}", ex.GetType());
            io.Error.Write("Message: {0}", ex.Message);
            io.Error.Write("-----------------------------");
            io.Error.Write(ex.ToString());
            io.Error.Write("-----------------------------");

            if (ex.InnerException != null)
            {
                io.Error.Write("Inner Exception: {0}", ex.InnerException.GetType());
                io.Error.Write("Inner Message: {0}", ex.InnerException.Message);
            }
            io.Error.Write("-----------------------------");
            io.Error.Write("FinTS Trace:");
        }

        public void DisplayException(Exception ex)
        {
            io.Error.Write("Exception: {0}", ex.GetType());
            io.Error.Write("Message: {0}", ex.Message);
            io.Error.Write("-----------------------------");
            io.Error.Write(ex.ToString());
            io.Error.Write("-----------------------------");
        }

        public void WaitForEnterOnDebug()
        {
            #if DEBUG
            io.Write("Please press enter to exit.");
            io.Read();
            #endif
        }
    }
}
