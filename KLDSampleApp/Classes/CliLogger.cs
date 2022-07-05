using System;

namespace KLDSampleApp
{
    public class CliLogger : ILogger
    {
        public void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}\n");
            Console.ResetColor();
        }
    }
}
