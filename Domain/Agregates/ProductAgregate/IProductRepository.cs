using Contracts;

namespace Domain.Agregates.ProductAgregate
{
    public interface IProductRepository : IRepository<Product>, IResolvable
    {
    }
}
