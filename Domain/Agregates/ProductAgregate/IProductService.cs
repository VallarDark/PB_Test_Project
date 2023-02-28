using Contracts;
using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Domain.Agregates.ProductAgregate
{
    public interface IProductService
    {
        RepositoryType RepositoryType { get; set; }

        Task<ProductCategory> CreateCategory(ProductCategoryChangeDto category);

        Task<Product> CreateProduct(ProductChangeDto product);

        Task<ProductCategory> UpdateCategory(ProductCategoryChangeDto category);

        Task<Product> UpdateProduct(ProductChangeDto product);

        Task<Unit> RemoveCategory(string id);

        Task<Unit> RemoveProduct(string id);

        Task<Unit> AddProductToCategory(string productId, string categoryId);

        Task<Unit> RemoveProductFromCategory(string productId, string categoryId);

        Task<PaginatedCollectionBase<Product>> GetProducts(int pageNumber);

        Task<PaginatedCollectionBase<ProductCategory>> GetProductCategories(int pageNumber);
    }
}
