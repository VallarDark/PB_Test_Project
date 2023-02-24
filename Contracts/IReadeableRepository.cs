using System;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IReadeableRepository<T> where T : EntityBase
    {
        T? GetById(string id, Expression<Func<T, bool>>? predicate = null);

        T? Get(Expression<Func<T, bool>>? predicate = null);

        PaginatedCollectionBase<T> GetAll(int pageNumber, int itemsPerPage, Func<bool, T>? predicate = null);
    }
}
