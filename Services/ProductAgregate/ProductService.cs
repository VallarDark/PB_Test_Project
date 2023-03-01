using Contracts;
using Domain.Agregates.ProductAgregate;
using Domain.Agregates.UserAgregate;
using Domain.Exceptions;
using Domain.Utils;
using Microsoft.FSharp.Core;
using System.Threading.Tasks;

namespace Services.ProductAgregate
{
    public class ProductService : ResolvableServiceBase, IProductService
    {
        private IProductRepository? productRepository =>
            _RepositoryResolver?.GetRepository<IProductRepository, Product>(RepositoryType)
            as IProductRepository;

        private IProductCategoryRepository? productCategoryRepository =>
            _RepositoryResolver?.GetRepository<IProductCategoryRepository, ProductCategory>(
                RepositoryType)
            as IProductCategoryRepository;

        private readonly IUserService _userService;

        public ProductService(
            IRepositoryResolver repositoryResolver, IUserService userService)
            : base(repositoryResolver)
        {
            _userService = userService;
        }

        public async Task<ProductCategory> CreateCategory(ProductCategoryChangeDto category)
        {
            if (!_userService.DoesUserHavePermission(UserRoleType.Admin))
            {
                throw new LowPrevilegiesLevelException(DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR);
            }

            EnsuredUtils.EnsureNotNull(
                productCategoryRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productCategoryRepository)));

            var existedItem = await productCategoryRepository
                .Get(c => c.Name == category.Name);

            if (existedItem != null)
            {
                throw new ItemAlreadyExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_NOT_EXISTS_ERROR, nameof(ProductCategory)));
            }

            var newItem = new ProductCategory(category.Name, category.Description);

            await productCategoryRepository.Create(newItem);

            return newItem;
        }

        public async Task<Product> CreateProduct(ProductChangeDto product)
        {
            if (!_userService.DoesUserHavePermission(UserRoleType.Admin))
            {
                throw new LowPrevilegiesLevelException(DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR);
            }

            EnsuredUtils.EnsureNotNull(
                productRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productRepository)));

            var existedItem = await productRepository
                .Get(c => c.Title == product.Title);

            if (existedItem != null)
            {
                throw new ItemAlreadyExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_NOT_EXISTS_ERROR, nameof(ProductCategory)));
            }

            var newItem = new Product(
                product.Title,
                product.Description,
                product.ImgUrl,
                product.Price);

            await productRepository.Create(newItem);

            return newItem;
        }

        public async Task<PaginatedCollectionBase<ProductCategory>> GetProductCategories(
            int pageNumber)
        {
            EnsuredUtils.EnsureNotNull(
                productCategoryRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productCategoryRepository)));

            return await productCategoryRepository.GetPage(
                pageNumber,
                ITEMS_PER_PAGE,
                addInnerItems: true);
        }

        public async Task<PaginatedCollectionBase<Product>> GetProducts(int pageNumber)
        {
            EnsuredUtils.EnsureNotNull(
                productRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productRepository)));

            return await productRepository.GetPage(
                pageNumber,
                ITEMS_PER_PAGE,
                addInnerItems: true);
        }

        public async Task<Unit> RemoveCategory(string id)
        {
            if (!_userService.DoesUserHavePermission(UserRoleType.Admin))
            {
                throw new LowPrevilegiesLevelException(DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR);
            }

            EnsuredUtils.EnsureNotNull(
                productCategoryRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productCategoryRepository)));

            await productCategoryRepository.Delete(id);

            return default;
        }

        public async Task<Unit> RemoveProduct(string id)
        {
            if (!_userService.DoesUserHavePermission(UserRoleType.Admin))
            {
                throw new LowPrevilegiesLevelException(DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR);
            }

            EnsuredUtils.EnsureNotNull(
                productRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productRepository)));

            await productRepository.Delete(id);

            return default;
        }

        public async Task<ProductCategory> UpdateCategory(ProductCategoryChangeDto category)
        {
            if (!_userService.DoesUserHavePermission(UserRoleType.Admin))
            {
                throw new LowPrevilegiesLevelException(DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR);
            }

            EnsuredUtils.EnsureNotNull(
                productCategoryRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productCategoryRepository)));

            var existedItem = await productCategoryRepository
                .Get(c => c.Id == category.Id);

            if (existedItem == null)
            {
                throw new ItemNotExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_EXISTS_ERROR, nameof(ProductCategory)));
            }

            if (existedItem.Name != category.Name)
            {
                existedItem.ChangeName(category.Name);
            }

            if (existedItem.Description != category.Description)
            {
                existedItem.ChangeDescription(category.Description);
            }

            await productCategoryRepository.Update(existedItem);

            return existedItem;
        }

        public async Task<Product> UpdateProduct(ProductChangeDto product)
        {
            if (!_userService.DoesUserHavePermission(UserRoleType.Admin))
            {
                throw new LowPrevilegiesLevelException(DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR);
            }

            EnsuredUtils.EnsureNotNull(
                productRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productRepository)));

            var existedItem = await productRepository
                .Get(c => c.Id == product.Id);

            if (existedItem == null)
            {
                throw new ItemNotExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_EXISTS_ERROR, nameof(Product)));
            }

            if (existedItem.Title != product.Title)
            {
                existedItem.ChangeTitle(product.Title);
            }

            if (existedItem.Price != product.Price)
            {
                existedItem.ChangePrice(product.Price);
            }

            if (existedItem.Description != product.Description)
            {
                existedItem.ChangeDescription(product.Description);
            }

            if (existedItem.ImgUrl != product.ImgUrl)
            {
                existedItem.ChangeImage(product.ImgUrl);
            }

            await productRepository.Update(existedItem);

            return existedItem;
        }

        public async Task<Unit> AddProductToCategory(string productId, string categoryId)
        {
            return await ProductCategoryInteractionInner(
                productId,
                categoryId,
                ActionType.Add);
        }

        public async Task<Unit> RemoveProductFromCategory(string productId, string categoryId)
        {
            return await ProductCategoryInteractionInner(
                productId,
                categoryId,
                ActionType.Remove);
        }

        private enum ActionType
        {
            Add,
            Remove
        }

        private async Task<Unit> ProductCategoryInteractionInner(string productId, string categoryId, ActionType action)
        {
            if (!_userService.DoesUserHavePermission(UserRoleType.Admin))
            {
                throw new LowPrevilegiesLevelException(DEFAULT_LOW_PREVILEGIES_LEVEL_ERROR);
            }

            EnsuredUtils.EnsureNotNull(
                productRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productRepository)));

            EnsuredUtils.EnsureNotNull(
                productCategoryRepository,
                string.Format(REPOSITORY_DOES_NOT_EXISTS, nameof(productCategoryRepository)));

            var existedProduct = await productRepository
                .Get(c => c.Id == productId);

            if (existedProduct == null)
            {
                throw new ItemNotExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_EXISTS_ERROR, nameof(Product)));
            }

            var existedCategory = await productCategoryRepository
                .Get(c => c.Id == categoryId);

            if (existedCategory == null)
            {
                throw new ItemNotExistsException(
                    string.Format(DEFAULT_ITEM_SHOULD_EXISTS_ERROR, nameof(ProductCategory)));
            }

            switch (action)
            {
                case ActionType.Add:
                    {
                        existedCategory.AddProduct(existedProduct);
                    }
                    break;
                case ActionType.Remove:
                    {
                        existedCategory.RemoveProduct(existedProduct);
                    }
                    break;
                default:
                    break;
            }

            return await productCategoryRepository.Update(existedCategory);
        }
    }
}
