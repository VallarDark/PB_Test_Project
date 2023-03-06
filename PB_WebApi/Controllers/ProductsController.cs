using Domain.Aggregates.ProductAggregate;
using Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PB_WebApi.Authorization;
using PresentationModels.Models;

namespace PB_WebApi.Controllers
{
    /// <summary>
    /// Products CRUD controller
    /// </summary>

    [EnableCors("CrudPolicy")]
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="productService">Product service</param>

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get paginated collection of products
        /// </summary>
        /// <param name="pageNumber">Number of page</param>
        /// <returns>Paginated collection of products</returns>

        [HttpGet("page/{pageNumber}"), Authorize]
        public async Task<IActionResult> GetProducts(int pageNumber)
        {
            return Ok(await _productService.GetProducts(pageNumber));
        }

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="data">Product creation data</param>   
        /// <returns>New product</returns>

        [RequiredRoleAuthorize(UserRoleType.Admin)]
        [HttpPost, Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreationModel data)
        {
            var creationData = new ProductChangeDto()
            {
                Description = data.Description,
                ImgUrl = data.ImgUrl,
                Price = data.Price,
                Title = data.Title
            };

            return Ok(await _productService.CreateProduct(creationData));
        }

        /// <summary>
        /// Update product by id
        /// </summary>
        /// <param name="data">Product updating data</param>   
        /// <param name="id">Product id</param>   
        /// <returns>Updated product</returns>

        [RequiredRoleAuthorize(UserRoleType.Admin)]
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateProduct(
            string id,
            [FromBody] ProductUpdateModel data)
        {
            var updatingData = new ProductChangeDto()
            {
                Id = id,
                Description = data.Description,
                ImgUrl = data.ImgUrl,
                Price = data.Price,
                Title = data.Title
            };

            return Ok(await _productService.UpdateProduct(updatingData));
        }

        /// <summary>
        /// Delete product by id
        /// </summary>
        /// <param name="id">Product id</param>

        [RequiredRoleAuthorize(UserRoleType.Admin)]
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await _productService.RemoveProduct(id);

            return Ok("Product deleted");
        }
    }
}
