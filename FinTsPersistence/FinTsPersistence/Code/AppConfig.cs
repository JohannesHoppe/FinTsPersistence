using System;

namespace FinTsPersistence.Code
{
    /// <summary>
    /// Should be called at the very first beginning
    /// </summary>
    public class AppConfig
    {
        private static bool AlreadySetup;

        /// <summary>
        /// Searches for suitable app.config file an applies its path to the AppDomain
        /// Will look up into the current directory and will go up recursively, if not found. 
        /// </summary>
        public static string Setup()
        {
            if (AlreadySetup)
            {
                return null;
            }

            string foundConfigurationFile = FolderSearch.DefaultConfigurationDirectoryName()
                .FindFileUpwards(FolderSearch.ConfigurationFileName());

            string message = null;

            if (foundConfigurationFile != null &&
                foundConfigurationFile != FolderSearch.DefaultConfigurationFile)
            {
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", foundConfigurationFile);
                message = "Using non-default APP_CONFIG_FILE: " + foundConfigurationFile;
            }

            AlreadySetup = true;
            return message;
        }
    }
}
