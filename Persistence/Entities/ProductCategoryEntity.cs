using Contracts;
using System;
using System.Collections.Generic;

namespace Domain.Agregates.ProductAgregate
{
    public class ProductCategoryEntity : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ProductEntity> Products { get; set; }

        public ProductCategoryEntity(ProductCategory category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
            Products = new List<ProductEntity>();
        }

        public Guid Update(ProductCategory category)
        {
            Name = category.Name;
            Description = category.Description;

            return default;
        }
    }
}
