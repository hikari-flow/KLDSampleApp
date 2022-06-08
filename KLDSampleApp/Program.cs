using System;

namespace KLDSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input Path: ");
            string inputPath = Console.ReadLine();

            Console.Write("Output Path: ");
            string outputPath = Console.ReadLine();

            Console.WriteLine($"{inputPath}\n{outputPath}");
        }
    }
}
