using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KLDSampleApp
{
    public static class FileAnalyzer
    {
        private static Dictionary<string, string> FileTypes = ParseFileTypesList($"{Path.Join(GetSolutionDirectory(), "FileTypes.txt")}");
        
        /// <summary>
        ///     Reads the bytes of the file pointed in <paramref name="path"/> to get the magic number.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>The detected extension of <paramref name="path"/> (ie. "jpg" or "pdf") in all uppercase.</returns>
        public static string GetFileType(string path)
        {
            byte[] buffer;

            using (var filesStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using var binaryReader = new BinaryReader(filesStream);
                buffer = binaryReader.ReadBytes(4);
            }

            string fileSignature = BitConverter.ToString(buffer).Replace("-", string.Empty);

            if (!string.IsNullOrWhiteSpace(fileSignature))
            {
                foreach (var fileType in FileTypes)
                {
                    if (fileType.Value.Equals(fileSignature.Substring(0, fileType.Value.Length)))
                    {
                        return fileType.Key.ToUpper();
                    }
                }
            }

            return "Unknown";
        }

        /// <summary>
        ///     Calculates the MD5 of the file in the given <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns>A string of the file's MD5 hash.</returns>
        public static string CalculateMD5(string path)
        {
            using var md5 = MD5.Create();
            using var fileStream = File.OpenRead(path);
            var hash = md5.ComputeHash(fileStream);

            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        ///     Parses a list of file types into a Dictionary where the key is the extension and value is the signature. Every line must follow the format "extension:signature".
        /// </summary>
        /// <param name="path">Path to the text file.</param>
        /// <returns></returns>
        private static Dictionary<string, string> ParseFileTypesList(string path)
        {
            var fileTypes = new Dictionary<string, string>();

            foreach (var line in File.ReadLines(path))
            {
                string[] split = line.Split(":");
                fileTypes.Add(split[0], split[1]);
            }

            return fileTypes;
        }

        private static string GetSolutionDirectory() => new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
    }
}
