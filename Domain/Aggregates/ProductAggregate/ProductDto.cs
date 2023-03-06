using System.Collections.Generic;

namespace Domain.Aggregates.ProductAggregate
{
    public class ProductDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public double Price { get; set; }

        public ICollection<ProductCategoryDto> Categories { get; set; }

        public int CyclicDepth { get; set; }
    }
}
