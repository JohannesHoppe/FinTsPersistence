using System;
using FinTsPersistence.Actions.Result;
using FinTsPersistence.App_Start;

namespace FinTsPersistence
{   
    /// <summary>
    /// Modified version of Subsembly FinCmd - with currently only one action: persist
    /// FinTsPersistence {action} -{argname1} {argvalue1} ...
    /// </summary>
    /// <remarks>
    /// 'sync' was removed because of this API doc:
    /// - FinService.Synchronize: Synchronisiert die internen Zustandsdaten des Bankzugangs mit dem Banksystem.
    /// - Diese Methode muss grundsätzlich niemals aufgerufen werden, da die Methode Subsembly.FinTS.FinService.LogOn(System.String) eine erforderliche Synchronisierung immer automatisch durchführt.
    /// </remarks>
    public class Start
    {
        /// <summary>
        /// The main entry point for the command-line application.
        /// </summary>
        /// <return>Return Code: -1 for an exception, otherwise the internal status code</return>   
        public static int Main(string[] args)
        {
            int returnCode = -1;

            try
            {
                ActionResult result = DoAction(args);
                returnCode = result.OrderStatusCode;
            }
            catch (ArgumentException ex)
            {
                CommandLineHelper.DisplayShortException(ex);
                CommandLineHelper.ShowUsage();
            }
            catch (ActionException ex)
            {
                CommandLineHelper.DisplayActionException(ex);
            }  
            catch (Exception ex)
            {
                CommandLineHelper.DisplayException(ex);
            }

            CommandLineHelper.WaitForEnterOnDebug();
            return returnCode;
        }

        /// <summary>
        /// Entry point for embedding the library
        /// </summary>
        public static ActionResult DoAction(string[] args)
        {
            CommandLineHelper.CheckAmountOfParameters(args);

            var extractedArguments = CommandLineHelper.ExtractArguments(args);
            CommandLineHelper.CheckForPinOrResume(extractedArguments.Arguments);

            var finTsPersistence = ContainerConfig.Resolve<IFinTsPersistence>();

            return finTsPersistence.DoAction(extractedArguments.Action, extractedArguments.Arguments);
        }
    }
}
