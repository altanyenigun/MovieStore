using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStoreApi.Services.Logger
{
    public class ConsoleLogger : ILoggerService
    {

        public void LogError(string message)
        {
            throw new NotImplementedException();
        }

        public void LogRequest(string message)
        {
            Console.WriteLine("\n[{0}] -----LogRequest-----  \n{1}\n",DateTime.Now,message);
        }

        public void LogResponse(string message)
        {
            Console.WriteLine("\n[{0}] -----LogResponse-----  \n{1}\n",DateTime.Now,message);
        }
    }
}