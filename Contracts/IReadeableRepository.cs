using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IReadeableRepository<T> where T : class, IEntityBase
    {
        Task<T?> GetById(string id);

        Task<T?> Get(Expression<Func<T, bool>>? predicate = null);

        Task<PaginatedCollectionBase<T>> GetAll(int pageNumber, int itemsPerPage, Expression<Func<T, bool>>? predicate = null);

        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? predicate = null);
    }
}
