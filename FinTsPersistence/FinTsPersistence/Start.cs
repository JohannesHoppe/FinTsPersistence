using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using FinTsPersistence.Bootstrap;
using FinTsPersistence.Interfaces;
using Subsembly.FinTS;

namespace FinTsPersistence
{   
    /// <summary>
    /// Modified version of Subsembly FinCmd - with currently only one action: persist
    /// FinTsPersistence {action} -{argname1} {argvalue1} ...
    /// </summary>
    public class Start
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>   
        public static int Main(string[] vsArgs)
        {
            int result = -1;

            try
            {
                CommandLineHelper.CheckAmountOfParameters(vsArgs);

                var extractedArguments = CommandLineHelper.ExtractArguments(vsArgs);
                CommandLineHelper.CheckForPinOrResume(extractedArguments.Arguments);

                var finTsPersistence = ContainerConfig.Resolve<IFinTsPersistence>();

                result = finTsPersistence.DoAction(extractedArguments.Action, extractedArguments.Arguments);
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine(ex.Message);
                CommandLineHelper.ShowUsage();
            }     
            catch (Exception ex)
            {
                CommandLineHelper.DisplayException(ex);
            }

            CommandLineHelper.WaitForEnterOnDebug();
            return result;
        }
    }
}
