using System;

namespace Domain.Exceptions
{
    public class WrongOperationException : Exception
    {
        public WrongOperationException(string message) : base(message)
        {
        }
    }
}
