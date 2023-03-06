using Contracts;

namespace Domain.Aggregates.ProductAggregate
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>, IResolvable
    {
    }
}
