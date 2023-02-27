using System;

namespace Domain.Exceptions
{
    public class LowPrevilegiesLevelException : Exception
    {
        public LowPrevilegiesLevelException(string message) : base(message)
        {
        }
    }
}
