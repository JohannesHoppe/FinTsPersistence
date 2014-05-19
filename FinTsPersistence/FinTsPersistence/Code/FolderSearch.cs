using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FinTsPersistence.Code
{
    public static class FolderSearch
    {
        private const int MaxLevelOfRecursion = 6;
        private static string _configurationFileName;

        public static string DefaultConfigurationFile
        {
            get
            {
                return _configurationFileName ??
                       (_configurationFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            }
        }

        public static string ConfigurationFileName()
        {
            return Path.GetFileName(DefaultConfigurationFile);
        }

        public static string DefaultConfigurationDirectoryName()
        {
            return Path.GetDirectoryName(DefaultConfigurationFile);
        }

        public static string FindFileUpwards(this string curentPath, string fileName)
        {
            return curentPath.GetFileIfExists(fileName) ?? 
                FindFileUpwards(curentPath, fileName, 0);
        }

        private static string FindFileUpwards(this string curentPath, string fileName, int currentLevel)
        {
            if (curentPath == null)
            {
                return null;
            }

            if (currentLevel >= MaxLevelOfRecursion)
            {
                return null;
            }

            string matchingFile = curentPath.GetFileIfExists(fileName);
            return matchingFile ?? curentPath.RemoveLastPart().FindFileUpwards(fileName, currentLevel + 1);
        }

        public static string GetFileIfExists(this string curentPath, string fileName)
        {
            if (File.Exists(curentPath + "\\" + fileName))
            {
                return curentPath + "\\" + fileName;
            }

            return null;
        }

        private static string RemoveLastPart(this string path)
        {
            if (!path.Contains(@"\"))
            {
                return null;
            }

            List<string> parts = path.Split(new[] {@"\"}, StringSplitOptions.None).ToList();
            parts.RemoveAt(parts.Count() - 1);
            return string.Join(@"\", parts.ToArray());
        }
    }
}
