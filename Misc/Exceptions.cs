using System;

namespace BFL
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException() {}
        
        public InvalidTypeException(string received):base($"Invalid type, received: {received}") {}
    }
}