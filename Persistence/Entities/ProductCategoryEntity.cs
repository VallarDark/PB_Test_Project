using Contracts;
using Domain.Agregates.ProductAgregate;
using System;
using System.Collections.Generic;

namespace Persistence.Entities
{
    public class ProductCategoryEntity : ValueObject, IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ProductEntity> Products { get; set; }

        public ProductCategoryEntity()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Description = string.Empty;
            Products = new List<ProductEntity>();
        }

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

        public Guid Update(ProductCategoryEntity category)
        {
            Name = category.Name;
            Description = category.Description;

            return default;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Description;
        }
    }
}
