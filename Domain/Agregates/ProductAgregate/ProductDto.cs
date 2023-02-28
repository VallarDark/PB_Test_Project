using System.Collections.Generic;

namespace Domain.Agregates.ProductAgregate
{
    public class ProductDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public ICollection<ProductCategoryDto> Categories { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public double Price { get; set; }
    }
}
