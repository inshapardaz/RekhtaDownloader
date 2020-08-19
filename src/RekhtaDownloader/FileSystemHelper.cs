using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    internal static class FileSystemHelper
    {
        public static void EnsureEmptyDirectory(this string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        public static void CreateIfDirectoryDoesNotExists(this string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void MakeSureFileDoesNotExist(this string path)
        {
            if (File.Exists(path)) File.Delete(path);
        }

        public static string ToSafeFilename(this string filename)
        {
            return Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public static void TryDeleteDirectory(this string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch
            {
            }
        }

        public static void TryDeleteFile(this string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }
}