using Contracts;

namespace Domain.Agregates.ProductAgregate
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>, IResolvable
    {
    }
}
