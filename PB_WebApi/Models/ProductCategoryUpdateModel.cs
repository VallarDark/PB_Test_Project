using Domain.Agregates.ProductAgregate;
using System.ComponentModel.DataAnnotations;

namespace PB_WebApi.Models
{
    public class ProductCategoryUpdateModel
    {
        [MinLength(ProductCategory.MIN_LENGTH)]
        [MaxLength(ProductCategory.MAX_LENGTH)]
        public string Name { get; set; }

        [MinLength(ProductCategory.MIN_LENGTH)]
        [MaxLength(ProductCategory.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }
    }
}
