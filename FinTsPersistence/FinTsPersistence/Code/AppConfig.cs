using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTsPersistence.Code
{
    public static class AppConfig
    {
        /// <summary>
        /// Searches for suitable app.config file an applies its path to the AppDomain
        /// Will look up into the current directory and will go up recursively, if not found. 
        /// </summary>
        public static void Setup()
        {
            string currentExecutingDirectory = FolderSearch.CurrentExecutingDirectory();
            currentExecutingDirectory.FindFileUpwards("FinTsPersistence.exe.config");
            
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", @"C:\Temp\test.config");

        }
    }
}
