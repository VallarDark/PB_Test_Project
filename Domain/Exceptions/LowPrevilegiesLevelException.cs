using System;

namespace Domain.Exceptions
{
    public class LowPrivilegesLevelException : Exception
    {
        public LowPrivilegesLevelException(string message) : base(message)
        {
        }
    }
}
