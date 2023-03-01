using Contracts;
using System.Collections.Generic;

namespace Persistence
{
    internal class PaginatedCollection<T> : PaginatedCollectionBase<T>
    {
        public PaginatedCollection(IEnumerable<T> items, bool doesNextPageExists)
            : base(items, doesNextPageExists)
        {
        }
    }
}
