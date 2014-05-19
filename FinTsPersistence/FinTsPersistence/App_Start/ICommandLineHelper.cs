using System;
using System.Collections.Specialized;
using FinTsPersistence.Actions.Result;

namespace FinTsPersistence
{
    public interface ICommandLineHelper
    {
        /// <summary>
        /// At least one parameters must be given, and the number of parameters must be uneven
        /// (because action has no value)
        /// </summary>
        /// <param name="vsArgs">args from Main</param>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        void CheckAmountOfParameters(string[] vsArgs);

        /// <summary>
        /// In jedem Fall wird die PIN oder der Dialogkontext zur Fortführung benötigt.
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        void CheckForPinOrResume(StringDictionary arguments);

        /// <summary>
        /// Get the action and all trailing args/argvalue pairs
        /// </summary>
        /// <param name="vsArgs">args from Main</param>
        ExtractedArguments ExtractArguments(string[] vsArgs);

        /// <summary>
        /// Displays a help text on standard error output stream
        /// </summary>
        void ShowUsage();

        void DisplayShortException(Exception ex);
        void DisplayActionException(ActionException ex);
        void DisplayException(Exception ex);
    }
}