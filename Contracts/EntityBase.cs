using System;

namespace Contracts
{
    public abstract class EntityBase
    {
        public string Id { get; set; }

        public EntityBase()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
