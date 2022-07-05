using System;
using System.Collections.Generic;
using System.IO;

namespace KLDSampleApp
{
    class CsvGenerator
    {
        private readonly ILogger _logger;

        public CsvGenerator(ILogger logger) => this._logger = logger;

        public void Generate(FilePath inputPath, FilePath outputPath, List<string> filter = null)
        {
            var searchOption = inputPath.ContainsFlag("-r") ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            try
            {
                var files = Directory.EnumerateFiles(inputPath.Value, "*", searchOption);
                var lines = new List<string>();

                foreach (string file in files)
                {
                    string fileType = FileAnalyzer.GetFileType(file);

                    if (filter == null || filter.Contains(fileType))
                    {
                        lines.Add($"\"{file}\",\"{fileType}\",\"{FileAnalyzer.CalculateMD5(file)}\"");
                    }
                }

                lines.Sort();
                lines.Insert(0, @"""File Path"",""Detected File Type"",""MD5""");

                File.WriteAllLines(outputPath.IsFile() ? outputPath.Value : outputPath.Value + "output.csv", lines);
            }
            catch (Exception e)
            {
                this._logger.LogError(e.ToString());
            }
        }
    }
}
