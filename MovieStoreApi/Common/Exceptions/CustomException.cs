using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStoreApi.Common.Exceptions
{
    // base class to use to create CustomError
    public class CustomException : Exception
    {
        public int ErrorCode { get; }
        public string Error { get; }

        public CustomException(int errorCode, string error, string message) : base(message)
        {
            ErrorCode = errorCode;
            Error = error;
        }
    }
    // Creating special error definitions for error situations that may occur in the flow
    public static class CustomExceptions
    {

    }
}