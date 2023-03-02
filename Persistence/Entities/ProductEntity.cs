using Contracts;
using Domain.Agregates.ProductAgregate;
using System;
using System.Collections.Generic;

namespace Persistence.Entities
{
    public class ProductEntity : ValueObject, IEntity
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public double Price { get; set; }

        public ICollection<ProductCategoryEntity> Categories { get; set; }

        public ProductEntity()
        {
            Id = Guid.NewGuid().ToString();
            Title = string.Empty;
            Description = string.Empty;
            ImgUrl = string.Empty;
            Price = 0;
            Categories = new List<ProductCategoryEntity>();
        }

        public ProductEntity(Product product)
        {
            Id = product.Id;
            Title = product.Title;
            Description = product.Description;
            ImgUrl = product.ImgUrl;
            Price = product.Price;
            Categories = new List<ProductCategoryEntity>();
        }

        public Guid Update(Product product)
        {
            Title = product.Title;
            Description = product.Description;
            ImgUrl = product.ImgUrl;
            Price = product.Price;

            return default;
        }

        public Guid Update(ProductEntity product)
        {
            Title = product.Title;
            Description = product.Description;
            ImgUrl = product.ImgUrl;
            Price = product.Price;

            return default;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Title;
            yield return Description;
            yield return ImgUrl;
            yield return Price;
        }
    }
}
