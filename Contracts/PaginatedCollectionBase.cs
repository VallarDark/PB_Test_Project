using System.Collections.Generic;

namespace Contracts
{
    public abstract class PaginatedCollectionBase<T>
    {
        public IEnumerable<T> Items { get; set; }

        public bool DoesNextPageExists { get; set; }

        public PaginatedCollectionBase(IEnumerable<T> items, bool doesNextPageExists)
        {
            Items = items;
            DoesNextPageExists = doesNextPageExists;
        }
    }
}
