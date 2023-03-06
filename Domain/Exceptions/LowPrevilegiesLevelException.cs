using System;

namespace Domain.Exceptions
{
    public class LowPrevilegesLevelException : Exception
    {
        public LowPrevilegesLevelException(string message) : base(message)
        {
        }
    }
}
