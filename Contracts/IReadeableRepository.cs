using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IReadeableRepository<T> where T : class, IEntity
    {
        Task<T?> GetById(
            string id,
            bool addInnerItems = false);

        Task<T?> Get(
            Expression<Func<T, bool>>? predicate = null,
            bool addInnerItems = false);

        Task<PaginatedCollectionBase<T>> GetPage(
            int pageNumber,
            int itemsPerPage,
            Expression<Func<T, bool>>? predicate = null,
            bool addInnerItems = false);

        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>>? predicate = null,
            bool addInnerItems = false);
    }
}
