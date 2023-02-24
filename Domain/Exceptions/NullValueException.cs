using System;

namespace Domain.Exceptions
{
    public class NullValueException : Exception
    {
        public NullValueException(string message) : base(message)
        {
        }
    }
}
