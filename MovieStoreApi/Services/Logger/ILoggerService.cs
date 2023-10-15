using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStoreApi.Services.Logger
{
    public interface ILoggerService
    {
        public void LogRequest(string message);
        public void LogResponse(string message);
        public void LogError(string message);
    }
}