using System;

namespace RetailCrm
{
    public class CurlException : Exception
    {
        public CurlException()
        {
        }

        public CurlException(string message)
            : base(message)
        {
        }

        public CurlException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
