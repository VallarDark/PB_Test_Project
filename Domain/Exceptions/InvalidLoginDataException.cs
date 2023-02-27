using System;

namespace Domain.Exceptions
{
    public class InvalidLoginDataException : Exception
    {
        public InvalidLoginDataException(string message) : base(message)
        {
        }
    }
}
