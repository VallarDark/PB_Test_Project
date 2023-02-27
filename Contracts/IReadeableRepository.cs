using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IReadeableRepository<T> where T : EntityBase
    {
        Task<T?> GetById(string id, Expression<Func<T, bool>>? predicate = null);

        Task<T?> Get(Expression<Func<T, bool>>? predicate = null);

        Task<PaginatedCollectionBase<T>> GetAll(int pageNumber, int itemsPerPage, Func<bool, T>? predicate = null);

        Task<PaginatedCollectionBase<T>> GetAll(Func<bool, T>? predicate = null);
    }
}
