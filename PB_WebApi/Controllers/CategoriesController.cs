using Contracts;
using Domain.Agregates.ProductAgregate;
using Domain.Agregates.UserAgregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PB_WebApi.Authorization;
using PB_WebApi.Models;

namespace PB_WebApi.Controllers
{
    /// <summary>
    /// Product categories CRUD controller and adding/removing product from category 
    /// </summary>

    [EnableCors("CrudPolicy")]
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriesController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="productService">Product service</param>

        public CategoriesController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get paginated collection of categories
        /// </summary>
        /// <param name="pageNumber">Number of page</param>
        /// <returns>Paginated collection of categories</returns>

        [HttpGet("page/{pageNumber}"), Authorize]
        public async Task<PaginatedCollectionBase<ProductCategory>> GetCategories(
            int pageNumber)
        {
            return await _productService.GetProductCategories(pageNumber);
        }

        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="data">Category creation data</param>   
        /// <returns>New category</returns>

        [RequedRoleAuthorize(UserRoleType.Admin)]
        [HttpPost, Authorize]
        public async Task<ProductCategory> CreateCategory(
            [FromBody] ProductCategoryCreationModel data)
        {
            var creationData = new ProductCategoryChangeDto()
            {
                Description = data.Description,
                Name = data.Name
            };

            return await _productService.CreateCategory(creationData);
        }

        /// <summary>
        /// Update category by id
        /// </summary>
        /// <param name="data">Category updating data</param>   
        /// <param name="id">Category id</param>   
        /// <returns>Updated category</returns>

        [RequedRoleAuthorize(UserRoleType.Admin)]
        [HttpPut("{id}"), Authorize]
        public async Task<ProductCategory> UpdateCategory(
            string id,
            [FromBody] ProductCategoryUpdateModel data)
        {
            var updatingData = new ProductCategoryChangeDto()
            {
                Id = id,
                Description = data.Description,
                Name = data.Name
            };

            return await _productService.UpdateCategory(updatingData);
        }

        /// <summary>
        /// Delete category by id
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>HTTP OK</returns>

        [RequedRoleAuthorize(UserRoleType.Admin)]
        [HttpDelete("{id}"), Authorize]
        public async Task<IResult> Delete(string id)
        {
            await _productService.RemoveCategory(id);

            return Results.Ok();
        }

        /// <summary>
        /// Add existing product to category by id
        /// </summary>
        /// <param name="categoryId">Category id</param>   
        /// <param name="productId">ProductId id</param>   
        /// <returns>HTTP OK</returns>

        [RequedRoleAuthorize(UserRoleType.Admin)]
        [HttpPost("{categoryId}/addProduct"), Authorize]
        public async Task<IResult> AddProductToCategory(
            string categoryId,
            [FromBody] string productId)
        {
            await _productService.AddProductToCategory(productId, categoryId);

            return Results.Ok();
        }

        /// <summary>
        /// Remove existing product from category by id
        /// </summary>
        /// <param name="categoryId">Category id</param>   
        /// <param name="productId">ProductId id</param>   
        /// <returns>HTTP OK</returns>

        [RequedRoleAuthorize(UserRoleType.Admin)]
        [HttpPost("{categoryId}/removeProduct"), Authorize]
        public async Task<IResult> RemoveProductFromCategory(
            string categoryId,
            [FromBody] string productId)
        {
            await _productService.RemoveProductFromCategory(productId, categoryId);

            return Results.Ok();
        }
    }
}
