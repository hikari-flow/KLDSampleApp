using System.Collections.Generic;

namespace KLDSampleApp
{
    public class MimeDetector
    {
        private readonly CsvGenerator _csvGenerator;
        private readonly IInputRetriever _inputRetriever;
        private readonly ILogger _logger;

        public MimeDetector(IInputRetriever inputRetriever, ILogger logger)
        {
            _inputRetriever = inputRetriever;
            _logger = logger;
            _csvGenerator = new CsvGenerator(_logger);
        }

        public void Run(IDictionary<string, IUserInput> userInput)
        {
            _inputRetriever.GetUserInput(userInput);
            _csvGenerator.Generate((FilePath)userInput["Input Path"], (FilePath)userInput["Output Path"], new List<string> { "JPG", "PDF" });
        }
    }
}
