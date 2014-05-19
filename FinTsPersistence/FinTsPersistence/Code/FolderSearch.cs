using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FinTsPersistence.Code
{
    public static class FolderSearch
    {
        private const int MaxLevelOfRecursion = 6;

        public static string CurrentExecutingDirectory()
        {
            string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            return Path.GetDirectoryName(filePath);
        }

        public static string FindFileUpwards(this string curentPath, string fileName)
        {
            return FindFileUpwards(curentPath, fileName, 0);
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

            string matchingFile = curentPath.FindFile(fileName);
            return matchingFile ?? curentPath.RemoveLastPart().FindFileUpwards(fileName, currentLevel + 1);
        }

        public static string FindFile(this string curentPath, string fileName)
        {
            string[] files = Directory.GetFiles(curentPath);
            return files.FirstOrDefault(f => f == fileName);
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
