using System.Collections.Generic;

namespace KLDSampleApp
{
    public interface IInputRetriever
    {
        void GetUserInput(IDictionary<string, IUserInput> userInput);
    }
}
