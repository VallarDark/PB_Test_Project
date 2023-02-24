using System;

namespace Domain.Exceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
