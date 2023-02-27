using Contracts;
using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Domain.Agregates.ProductAgregate
{
    public interface IProductService
    {
        Task<ProductCategory> CreateCategory(ProductCategory category);

        Task<Product> CreateProduct(Product product);

        Task<ProductCategory> UpdateCategory(ProductCategory category);

        Task<Product> UpdateProduct(Product product);

        Task<Unit> RemoveCategory(string id);

        Task<Unit> RemoveProduct(string id);

        Task<PaginatedCollectionBase<Product>> GetProducts(int pageNumber);

        Task<PaginatedCollectionBase<ProductCategory>> GetProductCategories(int pageNumber);
    }
}
