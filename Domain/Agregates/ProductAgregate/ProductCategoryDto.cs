using System.Collections.Generic;

namespace Domain.Agregates.ProductAgregate
{
    public class ProductCategoryDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ProductDto> Products { get; set; }
    }
}
