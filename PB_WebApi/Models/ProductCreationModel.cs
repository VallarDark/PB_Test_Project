using Domain.Agregates.ProductAgregate;
using System.ComponentModel.DataAnnotations;

namespace PB_WebApi.Models
{
    public class ProductCreationModel
    {
        [Required]
        [MinLength(Product.MIN_LENGTH)]
        [MaxLength(Product.MAX_LENGTH)]
        public string Title { get; set; }

        [Required]
        [MinLength(Product.MIN_LENGTH)]
        [MaxLength(Product.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        [Required]
        [MinLength(Product.MIN_LENGTH)]
        [MaxLength(Product.MAX_URL_LENGTH)]
        public string ImgUrl { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
