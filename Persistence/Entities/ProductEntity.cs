﻿using Contracts;
using System;
using System.Collections.Generic;

namespace Domain.Agregates.ProductAgregate
{
    public class ProductEntity : IEntity
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public ICollection<ProductCategoryEntity> Categories { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public double Price { get; set; }

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
    }
}