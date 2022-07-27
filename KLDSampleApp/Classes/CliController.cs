using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLDSampleApp
{
    public class CliController : IInputRetriever
    {
        /// <summary>
        ///     Loop through list of required input from <paramref name="userInput"/> and request user input via CLI.
        /// </summary>
        /// <param name="userInput">List of all input values that need to be retrieved from user</param>
        public void GetUserInput(IDictionary<string, IUserInput> userInput)
        {
            foreach (var item in userInput)
            {
                bool isValid = false;

                while (isValid == false)
                {
                    Display($"{item.Key}: ", ConsoleColor.Blue);

                    try
                    {
                        item.Value.Value = Console.ReadLine();

                        if (item.Value is FilePath path)
                        {
                            HandleFilePaths(path);
                        }
                    }
                    catch (Exception e)
                    {
                        DisplayError(e.ToString());
                        continue;
                    }

                    isValid = true;
                }

                Console.WriteLine(); // newline for aesthetics
            }
        }

        /// <summary>
        ///     Turns console text to desired <paramref name="color"/>, displays message, then resets text color back to default. Doesn't write a newline at the end.
        /// </summary>
        /// <param name="message">Message to be displayed.</param>
        /// <param name="color">Desired color to display message in (defaults to white).</param>
        private static void Display(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write($"{message}");
            Console.ResetColor();
        }

        /// <summary>
        ///     Turns console text to red, displays message, then resets text color back to default.
        /// </summary>
        /// <param name="message">Error message to be displayed</param>
        /// <param name="color"></param>
        private static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}\n");
            Console.ResetColor();
        }

        /// <summary>
        ///     Deals with the special properties of FilePath input types.
        /// </summary>
        /// <param name="path">The FilePath object to be handled.</param>
        private static void HandleFilePaths(FilePath path)
        {
            // If path can accept flags, request user input
            if (path.AcceptsFlags())
            {
                foreach (string flag in path.GetAcceptedFlags())
                {
                    bool isYesOrNo = false;

                    while (isYesOrNo == false)
                    {
                        Display($"- {path.GetAcceptedFlagDescription(flag)}? (y/n): ", ConsoleColor.Blue);
                        string yesOrNo = Console.ReadLine().ToLower().Trim();

                        if (!yesOrNo.Equals("y") && !yesOrNo.Equals("yes") && !yesOrNo.Equals("n") && !yesOrNo.Equals("no"))
                        {
                            DisplayError($"Invalid Input");
                            continue;
                        }

                        isYesOrNo = true;

                        if (yesOrNo.Equals("y") || yesOrNo.Equals("yes"))
                        {
                            path.AddFlag(flag);
                        }
                    }
                }
            }
        }
    }
}
