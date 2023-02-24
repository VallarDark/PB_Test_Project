using System;

namespace Contracts
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct,
        AllowMultiple = false)]
    public class RepositoryTypeAttribute : Attribute
    {
        public RepositoryType Type { get; set; }

        public RepositoryTypeAttribute(RepositoryType type)
        {
            Type = type;
        }
    }
}
