using Contracts;

namespace Domain.Aggregates.ProductAggregate
{
    public interface IProductRepository : IRepository<Product>, IResolvable
    {
    }
}
