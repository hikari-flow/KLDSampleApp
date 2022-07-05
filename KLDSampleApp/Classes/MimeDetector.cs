using System.Collections.Generic;

namespace KLDSampleApp
{
    class MimeDetector
    {
        private readonly CsvGenerator _csvGenerator;
        private readonly IInputRetriever _inputRetriever;
        private readonly ILogger _logger;

        public MimeDetector(IInputRetriever inputRetriever, ILogger logger)
        {
            this._inputRetriever = inputRetriever; // is using "this." standard and recommended? or do we not use it?
            this._logger = logger;
            this._csvGenerator = new CsvGenerator(this._logger);
        }

        public void Run(Dictionary<string, IUserInput> userInput)
        {
            this._inputRetriever.GetUserInput(userInput);
            this._csvGenerator.Generate((FilePath)userInput["Input Path"], (FilePath)userInput["Output Path"], new List<string> { "JPG", "PDF" });
        }
    }
}
