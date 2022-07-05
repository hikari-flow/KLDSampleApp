using System.Collections.Generic;

namespace KLDSampleApp
{
    interface IInputRetriever
    {
        /// <summary>
        /// <paramref name="userInput"/> is the list of all input values that need to be retrieved from user.
        /// </summary>
        /// <param name="userInput">List of all input values that need to be retrieved from user</param>
        void GetUserInput(Dictionary<string, IUserInput> userInput);
    }
}
