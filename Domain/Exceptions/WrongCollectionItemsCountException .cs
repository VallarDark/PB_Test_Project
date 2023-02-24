using System;

namespace Domain.Exceptions
{
    public class WrongCollectionItemsCountException : Exception
    {
        public WrongCollectionItemsCountException(string message) : base(message)
        {
        }
    }
}
