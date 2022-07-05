using System.Collections.Generic;

namespace KLDSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var userInput = new Dictionary<string, IUserInput>
            {
                { "Input Path", new FilePath(PathConstraint.IsDirectory, new Dictionary<string, string>{ { "-r", "Include all subdirectories" } }) },
                { "Output Path", new FilePath() }
            };

            var mimeDetector = new MimeDetector(new CliController(), new CliLogger());
            mimeDetector.Run(userInput);
        }
    }
}
