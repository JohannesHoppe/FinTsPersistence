using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FintTsPersistenceIntegrationTests.Helper
{
    public static class IntegrationTestData
    {
        public static string GetContacfileLocation()
        {
            string projectFolder = GetProjectFolder();
            string contactfileLocation = projectFolder + @"\IntegrationTestData\Contactfile.xml";

            if (!File.Exists(contactfileLocation))
            {
                throw new ArgumentException(
                    "Integration Test can't run! No Contactfile.xml found. " +
                    "Copy 'IntegrationTestData-Example' to 'IntegrationTestData' and change the example files to real ones.");
            }
                    
            return contactfileLocation;
        }

        public static string GetCmdArgumentsLocation()
        {
            string projectFolder = GetProjectFolder();
            string cmdArgumentsLocation = projectFolder + @"\IntegrationTestData\CmdArguments.xml";

            if (!File.Exists(cmdArgumentsLocation))
            {
                throw new ArgumentException(
                    "Integration Test can't run! No CmdArguments.txt found. " +
                    "Copy 'IntegrationTestData-Example' to 'IntegrationTestData' and change the example files to real ones.");
            }

            return cmdArgumentsLocation;
        }

        public static CmdArguments GetCmdArguments()
        {
            string xmlContent = File.ReadAllText(GetCmdArgumentsLocation());
            return xmlContent.ParseXml<CmdArguments>();
        }

        /// <summary>
        /// Retrieves the folder which is two steps higher then current (this should be the project folder)
        /// </summary>
        private static string GetProjectFolder()
        {
            string codeBaseLocalPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            string currentExecutingDirectory = Path.GetDirectoryName(codeBaseLocalPath);
            return currentExecutingDirectory.ParentFolder().ParentFolder();
        }

        /// <summary>
        /// Removes last part of the directory (goes one up)
        /// </summary>
        internal static string ParentFolder(this string path)
        {
            if (!path.Contains(@"\"))
            {
                return path;
            }

            List<string> parts = path.Split(new[] { @"\" }, StringSplitOptions.None).ToList();
            parts.RemoveAt(parts.Count() - 1);
            return string.Join(@"\", parts.ToArray());
        }
    }
}
