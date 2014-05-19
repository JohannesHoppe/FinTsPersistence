using System;
using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;
using FinTsPersistence.Code;
using FinTsPersistence.Model;
using CC = FinTsPersistence.ContainerConfig;

namespace FinTsPersistence
{   
    /// <summary>
    /// Modified version of Subsembly FinCmd - with one extr action: persist
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
            var message = AppConfig.Setup();
            if (message != null)
            {
                CC.Resolve<IInputOutput>().Write(message);
            }

            int returnCode = -1;
            var commandLineHelper = CC.Resolve<ICommandLineHelper>();

            try
            {
                IActionResult result = DoAction(args);
                returnCode = result.OrderStatusCode;
            }
            catch (ArgumentException ex)
            {
                commandLineHelper.DisplayShortException(ex);
                commandLineHelper.ShowUsage();
            }
            catch (ActionException ex)
            {
                commandLineHelper.DisplayActionException(ex);
            }  
            catch (Exception ex)
            {
                commandLineHelper.DisplayException(ex);
            }

            return returnCode;
        }

        /// <summary>
        /// Entry point for embedding the library
        /// Does some in-before checks against the given data
        /// </summary>
        public static IActionResult DoAction(string[] args)
        {
            var message = AppConfig.Setup();
            if (message != null) {
                CC.Resolve<IInputOutput>().Write(message);
            }

            var commandLineHelper = CC.Resolve<ICommandLineHelper>();
            commandLineHelper.CheckAmountOfParameters(args);

            var extractedArguments = commandLineHelper.ExtractArguments(args);
            commandLineHelper.CheckForPinOrResume(extractedArguments.Arguments);

            if (extractedArguments.Action == ActionPersist.ActionName)
            {
                var transactionService = CC.Resolve<ITransactionService>();
                return transactionService.DoPersistence(extractedArguments.Arguments);
            }
            
            // 'old' FinCMD code without any special workflow
            var finTsService = CC.Resolve<IFinTsService>();
            return finTsService.DoAction(extractedArguments.Action, extractedArguments.Arguments);
        }
    }
}
