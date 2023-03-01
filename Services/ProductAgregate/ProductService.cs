using Contracts;
using Domain.Agregates.ProductAgregate;
using Microsoft.FSharp.Core;
using System;
using System.Threading.Tasks;

namespace Services.ProductAgregate
{
    public class ProductService : ResolvableServiceBase, IProductService
    {
        public ProductService(
            IRepositoryResolver repositoryResolver) : base(repositoryResolver)
        {
        }

        public Task<Unit> AddProductToCategory(string productId, string categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductCategory> CreateCategory(ProductCategoryChangeDto category)
        {
            throw new NotImplementedException();
        }

        public Task<Product> CreateProduct(ProductChangeDto product)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<ProductCategory>> GetProductCategories(int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedCollectionBase<Product>> GetProducts(int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> RemoveCategory(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> RemoveProduct(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> RemoveProductFromCategory(string productId, string categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductCategory> UpdateCategory(ProductCategoryChangeDto category)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProduct(ProductChangeDto product)
        {
            throw new NotImplementedException();
        }
    }
}
