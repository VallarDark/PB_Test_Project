using System;

namespace Domain.Exceptions
{
    public class ItemNotExistsException : Exception
    {
        public ItemNotExistsException(string message) : base(message)
        {
        }
    }
}
